var AtoZPlugin = (function () {
    var ITEM_REQUEST_BATCH_COUNT = 100;
    var DEFAULT_TOP = 100;

    var _containerElement;
    var _element;
    var _contentElement;
    var _appId;
    var _config;
    var _baseUri;
    var _isAtoZInitialize = false;
    var _isInfiniteScrollEnabled = false;
    var _shouldReinstateInfiniteScroll = false;
    var _alphabet = "#ABCDEFGHIJKLMNOPQRSTUVWXYZ".split("");
    var _subCharacters = "1234567890".split("");
    var _requestId = 0;
    var _initStepRegister = 0;

    var _templates = [{
        "Slug": "atoz-listing-template",
        "Template": null,
    },{
        "Slug": "atoz-listing-placeholder-template",
        "Template": null,
    },{
        "Slug": "atoz-letter-template",
        "Template": null,
    },{
        "Slug": "atoz-loading-template",
        "Template": null,
    }];

    var _self = {
        Initialize: Initialize
    };

    function Initialize(containerElement, applicationId, settings) {
        //Process the incoming div
        if (containerElement == null || containerElement == undefined) {
            console.log("Container element is a required parameter.");
            return;
        }
        _containerElement = containerElement;

        //Process the incoming application ID
        if (applicationId == null || applicationId == undefined) {
            console.log("ApplicationId is a required parameter.");
            return;
        }
        _appId = applicationId;

        //Initialize base areas.
        InitializeContainer();
        InitializeConfiguration();

        //Register html4 mode, use #, if the mode is inline mode.
        if (_config.inlineMode) window.History.options.html4Mode = true;
        window.History.init();
        ItializeHistory();

        //Determine the base paths.
        _baseUri = GetBasePath();               //Set the base URI which is important for routing throughout the application.
        _currentPath = GetProcessedPathName();  //Get the base path for processing going forward.
        _currentHash = window.location.hash;

        //Route the request
        RouteRequest();
    }

    function ItializeHistory() {
        History.Adapter.bind(window, 'statechange', function () { // Note: We are using statechange instead of popstate
            //Process the current state.
            var State = History.getState(); // Note: We are using History.getState() instead of event.state

            var uriParser = document.createElement('a');
            uriParser.href = State.url;

            var uri;
            if (_config.inlineMode) {
                if (State.url.indexOf("?") > -1) {

                    //Trim the full URL and get just the hash.
                    uri = State.url.substring(State.url.indexOf("?") + 1, State.url.length);
                    if (uri.indexOf("/") != 0) uri = "/" + uri;

                    uri = CorrectUriForIneractiveContent(uri);
                } else uri = GetProcessedPathName();
            } else {
                var uriParser = document.createElement('a');
                uriParser.href = State.url;

                //Hack for different browsers.  IE does not include the starting slush but
                //Chrome does include the slash.
                if (uriParser.pathname.charAt(0) != "/") uri = "/" + uriParser.pathname;
                else uri = uriParser.pathname;

                uri = GetProcessedPathName(uri);
            }

            //This is needed because if you on a page with a # and you click refresh both the Initialize method,
            //which is executed on document ready, and the bound History Adapter will execute and subsequently call
            //route request twice.  This doesn't necessarily impact the page but it will do everything twice.
            if (_currentPath != uri || _currentHash != window.location.hash) {
                _currentPath = uri;
                _currentHREF = window.location.href + _currentPath;
                RouteRequest();
            }
        });
    }

    function InitializeContainer() {
        _containerElement.addClass("plugin-container");

        var baseHTML = "<div id='SW-AtoZElement' class='hidden-initial'></div>";
        baseHTML += "<div id='SW-ContentElement' class='hidden-initial'></div>";

        _containerElement.html(baseHTML);

        _element = $("#SW-AtoZElement");
        _contentElement = $("#SW-ContentElement");

    }

    function InitializeConfiguration() {
        //Define the default config
        _config = {
            baseApiPath: "https://api.kramesstaywell.com",
            templatePlath: null,
            contentDisplayPath: "/Content/{{BucketSlug}}/{{ContentSlug}}",
            inlineMode: true    //There is no option to override this yet.
        };

        _config.baseApiPath = GetConfigValueIfSet("data-base-api-path", _config.baseApiPath);
        _config.templatePath = GetConfigValueIfSet("data-template-path", _config.templatePath);
        _config.contentDisplayPath = GetConfigValueIfSet("data-content-display-path", _config.contentDisplayPath);
    }

    function InitializeView() {
        GetTemplate("atoz-initialization-template", function (source) {
            var template = Handlebars.compile(source);
            var html = template();
            _element.html(html);

            var activeTabId = $(".tab-pane.active").attr("id");

            InitializeAtoZForEachTab();
        });
    }

    function InitializeAtoZForEachTab() {
        _initStepRegister = _templates.length;

        //Couldn't run this in a loop because IE wouldn't update the object in a global
        //array from within an anonymous function
        GetTemplate(_templates[0].Slug, function (source) {
            _templates[0].Template = Handlebars.compile(source);
            _initStepRegister--;
            FinishInitializationIfReady();
        });

        GetTemplate(_templates[1].Slug, function (source) {
            _templates[1].Template = Handlebars.compile(source);
            _initStepRegister--;
            FinishInitializationIfReady();
        });

        GetTemplate(_templates[2].Slug, function (source) {
            _templates[2].Template = Handlebars.compile(source);
            _initStepRegister--;
            FinishInitializationIfReady();
        });

        GetTemplate(_templates[3].Slug, function (source) {
            _templates[3].Template = Handlebars.compile(source);
            _initStepRegister--;
            FinishInitializationIfReady();
        });
    }

    function GetTemplateFromSlug(slug) {
        for (var i = 0; i < _templates.length; i++) {
            if (_templates[i].Slug == slug) return _templates[i].Template;
        }
        return null;
    }

    function RouteRequest() {
        //Define the URL Paths
        var loadAtoZ = /^\//;                                   //Get: /
        var loadContent = /^\/Content\/[\w-]+\/[\w-]+\/?$/;     //Get: /Content/{BucketSlug}/{ContentSlug}

        //Route the path to the proper processes
        if (loadContent.test(_currentPath)) {
            if (_isInfiniteScrollEnabled) {
                _shouldReinstateInfiniteScroll = true;
                DisableInfiniteScroll();
            }

            _element.hide();
            _contentElement.html("");
            _contentElement.show();

            var contentSlugs = GetContentSlugs();
            GetContent(contentSlugs.BucketSlug, contentSlugs.ContentSlug, ShowContent);
        } else if (loadAtoZ.test(_currentPath)) {
            _contentElement.hide();
            _element.show();

            if (!_isAtoZInitialize) InitializeView();
            else if (_shouldReinstateInfiniteScroll) EnableInfiniteScroll();    //Only enable if it was previously and everything is already initialized.
        } else {
            console.log("Route not found: " + _currentPath);
        }

    }

    function RegisterInternalLinks() {
        RegisterLinksByCSS(".internalLink");
    }

    function RegisterContentLinks() {
        RegisterLinksByCSS(".sw-content-link");
    }

    function RegisterLinksByCSS(cssSelector) {
        $(cssSelector).unbind("click"); //Make sure we don't double bind the same listener.
        $(cssSelector).click(function (e) {
            e.preventDefault(); //Prevent the href from executing.  Pushing the state will take care of the change.
            var url = $(e.currentTarget).attr("href");
            if (_config.inlineMode) {
                url = url.substring(url.indexOf("/?"), url.length)
            }

            //Push the state change
            History.pushState(null, null, url);
            window.scrollTo(0, 0); //Scroll to top so that you can see the state change.
        });
    }

    function GetBasePath() {
        return window.location.pathname;
    }

    function GetProcessedPathName(uriToProcess) {
        if (_config.inlineMode) {
            var uri = window.location.hash;
            if (uri == "") {
                uri = "/";
            } else {
                uri = uri.substring(uri.indexOf("?") + 1, uri.length);
                if (uri.indexOf("/") != 0) uri = "/" + uri;
            }
            uri = CorrectUriForIneractiveContent(uri);
        } else {
            var uri;
            if (uriToProcess == undefined) uri = window.location.pathname;
            else uri = uriToProcess;

            var baseIndex = uri.indexOf(_baseUri);
            uri = uri.substring(baseIndex + _baseUri.length, uri.length);
        }
        return uri;
    }

    //Fix URL issues caused by the workarounds to get quizzes, risk assessments, and 
    //calclators to work.
    function CorrectUriForIneractiveContent(uri) {
        //Fix URL issues caused by the workarounds to get quizzes, risk assessments, and 
        //calclators to work.
        if (uri.indexOf("&?") > -1) {
            uri = uri.substring(0, uri.indexOf("&?"));
        }
        if (uri.charAt(uri.length - 1) == "&") uri = uri.substring(0, uri.length - 1);

        return uri;
    }

    function GetContentSlugs() {
        var path = _currentPath;

        var selectRegex = /^\/Content\/([\w-]+)\/([\w-]+)\/?$/;;
        var arr = selectRegex.exec(path);

        //The entire string will match and be added to the first array element.
        //The second element in the array represents the ([\w-).
        if (arr == null || arr == undefined || arr.length != 3) return "";
        var contentSlugs = {
            "BucketSlug": arr[1],
            "ContentSlug": arr[2]
        };
        return contentSlugs;
    }

    function FinishInitializationIfReady() {
        if (_initStepRegister == 0) {

            //Initialize A-Z for each tab
            var listingTemplate = GetTemplateFromSlug("atoz-listing-template");
            var atozHTML = listingTemplate(_alphabet);
            $(".atoz").html(atozHTML);

            //Initialize the placeholder results
            var listingPlaceholderTemplate = GetTemplateFromSlug("atoz-listing-placeholder-template");
            var listingPlaceholderHTML = listingPlaceholderTemplate(_alphabet);
            $(".sw-results-area").append(listingPlaceholderHTML);

            //Complete the initialization
            InitializeAtoZClick();
            InitializeTabChangeEvent();
            InitializeViewAllClick();

            //Initialize the listing of data.
            var activeTabId = $(".tab-pane.active").attr("id");
            $("#" + activeTabId + "-results .loading-indicator").show();
            InitializeListing(activeTabId);

            _isAtoZInitialize = true;
        }
    }

    function GetConfigValueIfSet(attributeName, defaultValue) {
        var value = _containerElement.attr(attributeName);
        if (value != null && value != undefined) return value;
        else return defaultValue;
    }

    function GetConfigValueBoolIfSet(attributeName, defaultValue) {
        var value = _containerElement.attr(attributeName);
        if (value != null && value != undefined) {
            if (value.toUpperCase() == "FALSE") return false;
            else return true;
        }
        else return defaultValue;
    }

    //---------------------------------//
    //---------AtoZ Section-----------//
    //-------------------------------//
    function InitializeTabChangeEvent() {
        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            ShowAllLoadedItems();
        });
    }

    function ShowAllLoadedItems() {
        //Reset the data
        DisableInfiniteScroll();
        $(".active-letter").removeClass("active-letter");
        $(".section-listing").hide();
        $(".view-all-items").hide();

        //Activate the new tab
        var activeTabId = $(".tab-pane.active").attr("id");
        $("#" + activeTabId + "-results").show();

        InitalizeLoadedLetters(activeTabId);

        if (!IsAlreadyInitialized(activeTabId)) {
            $("#" + activeTabId + "-results .loading-indicator").show();
            InitializeListing(activeTabId);
        } else if (IsMoreLettersToLoad(activeTabId)) {
            EnableInfiniteScroll();
        }   //All done, nothing to do!
    }

    function InitializeAtoZClick() {
        $(".atoz a").click(function (e) {
            DisableInfiniteScroll();
            _requestId++;               //Immediatly increment to prevent other requests from displaying.   

            var currentLetter = $(e.target).data("letter");
            var activeTabId = $(".tab-pane.active").attr("id");

            //Mark letter as active
            $(".active-letter").removeClass("active-letter");
            $(e.target).addClass("active-letter");

            //Show view all link
            $(".tab-pane.active .view-all-items").show();

            //Hide the general loading indicator incase it is still showing.
            $("#" + activeTabId + "-results .loading-indicator").hide();

            //Hide all letters under the active tab and then display only the letter that we want.
            $("#" + activeTabId + "-results .letter-listing").hide();

            var letter = $("#" + activeTabId + "-results [data-letter='" + currentLetter + "']");
            if (letter.data("isloaded") == true) {
                letter.show();
            } else {
                //Loading Template
                var template = GetTemplateFromSlug("atoz-loading-template");
                letter.html(template());
                letter.show();

                var requestState = {
                    TotaltemCount: 0,
                    CurrentLetter: currentLetter,
                    Buckets: $("[aria-controls='" + activeTabId + "']").data("buckets"),
                    ActiveTabId: activeTabId,
                    RequestId: _requestId
                };

                GetListByLetter(requestState, DisplayIndividualLetter)
            }

        });
    }

    function InitializeViewAllClick() {
        $(".view-all-items").click(function (e) {
            ShowAllLoadedItems();
        });
    }

    function DisplayIndividualLetter(requestState, letterResults) {
        DisplayLetter(requestState, letterResults);
    }

    function InitalizeLoadedLetters(activeTabId) {
        //Hide all letters currently showing.  We want to show all letters from # - Z in order.
        //The order is important because if someone loaded Z, by direclty clicking Z, we don't want to
        //show A, B, C and Z.
        $("#" + activeTabId + "-results .letter-listing").hide();
        ShowLoadedLetters(activeTabId);
    }

    function ShowLoadedLetters(activeTabId) {
        //Hide all letters currently showing.  We want to show all letters from # - Z in order.
        //The order is important because if someone loaded Z, by direclty clicking Z, we don't want to
        //show A, B, C and Z.
        var letters = $("#" + activeTabId + "-results .letter-listing");

        //Iterate over all letter and display letters that are loaded with content.  Stop
        //as soon as you get to a unloaded letter.
        for (var i = 0; i < letters.length; i++) {

            //If the data is loaded display it.  Otherwise stop everything.
            if ($(letters[i]).attr("data-isloaded") == "true") {

                //Only show if there are items to show in the letter.  Otherwise skip it.
                if ($(letters[i]).find(".listing li a").length > 0) {
                    $(letters[i]).show();
                }
            } else return;
        }
    }

    function InitializeListing(activeTabId) {
        var requestState = GetInitialRequest(activeTabId);
        ShowTopOfList(requestState);
    }

    function ShowTopOfList(requestState, letterResults) {
        var activeTabId = requestState.ActiveTabId;
        var requestId = requestState.RequestId;

        //Show results if present
        if (letterResults != undefined && letterResults != null) {
            DisplayLetter(requestState, letterResults);
            if (letterResults.length > 0) $("#" + activeTabId + "-results .loading-indicator").hide();

            //Show all letters that may have already been loaded in between if we are still
            //on the current active request.
            if (requestState.RequestId == _requestId) ShowLoadedLetters(activeTabId);

            //Update the requeststate
            requestState.TotaltemCount += letterResults.length;
            requestState = GetUdpatedRequest(requestState);
        }

        //If more content needs to load initially load more data.
        if (requestState != null) {

            //Mark the letter as loaded right away otherwise switching the tabs quickly
            //can kick off a bunch of uneeded web service requests.
            var letter = $("#" + activeTabId + "-results [data-letter='" + requestState.CurrentLetter + "']");
            letter.attr("data-isloaded", "true");

            //Start the load
            GetListByLetter(requestState, ShowTopOfList);
        }
        else {
            $("#InfiniteLoadingResults").hide();
            if (IsMoreLettersToLoad(activeTabId) && requestId == _requestId) EnableInfiniteScroll();
        }

    }

    function GetInitialRequest(activeTabId) {
        //Get the next letter to load
        var letter = GetNextUnloadedLetter(activeTabId);

        //Initialize the start of the load
        if (letter == null) requestState = null;
        else {
            _requestId++;
            requestState = {
                TotaltemCount: 0,
                CurrentLetter: letter,
                Buckets: $("[aria-controls='" + activeTabId + "']").data("buckets"),
                ActiveTabId: activeTabId,
                RequestId: _requestId
            };
        }

        return requestState;
    }

    function GetUdpatedRequest(requestState) {
        //Get the next letter to load
        var letter = GetNextUnloadedLetter(requestState.ActiveTabId);

        if (letter == null) {
            //Nothing left to load
            requestState = null;
        } else if (requestState.TotaltemCount < ITEM_REQUEST_BATCH_COUNT) {
            //Update the existing request
            requestState.CurrentLetter = letter;
        } else {
            //Nothing left to rquest at this time.
            requestState = null;
        }

        return requestState;
    }

    function IsAlreadyInitialized(activeTabId) {
        if (GetNextUnloadedLetter(activeTabId) == _alphabet[0]) return false;
        else return true;
    }

    function IsMoreLettersToLoad(activeTabId) {
        if (GetNextUnloadedLetter(activeTabId) != null) return true;
        else return false;
    }

    function GetNextUnloadedLetter(activeTabId) {
        var unloadedLetters = $("#" + activeTabId + "-results span[data-isloaded='false']");

        if (unloadedLetters == null && unloadedLetters.length > 0) return null;
        else return $(unloadedLetters[0]).data("letter");
    }

    function GetListByLetter(requestState, onSuccess) {
        GetEntireList(requestState, 0, [], onSuccess);
    }

    //function DisplayLetter(activeTabId, letter, items) {
    function DisplayLetter(requestState, items) {
        items.sort(CompareTitle);
        var results = { Letter: requestState.CurrentLetter, Items: items }

        var template = GetTemplateFromSlug("atoz-letter-template");
        var html = template(results);

        var letter = $("#" + requestState.ActiveTabId + "-results [data-letter='" + requestState.CurrentLetter + "']");
        letter.html(html);
        letter.attr("data-isloaded", "true");

        //Only display the results if the requestID equals the current job ID.
        //Otherwise something else happened more recently that should be displayed.
        if (requestState.RequestId == _requestId) {
            if (items.length > 0) {
                letter.show();
                RegisterContentLinks();
            }
        }
    }

    function CompareTitle(a, b) {
        if (a.Title < b.Title)
            return -1;
        if (a.Title > b.Title)
            return 1;
        return 0;
    }

    function EnableInfiniteScroll() {
        _isInfiniteScrollEnabled = true;
        $(window).scroll(function () {
            if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                DisableInfiniteScroll();

                $("#InfiniteLoadingResults").show();
                var activeTabId = $(".tab-pane.active").attr("id");
                InitializeListing(activeTabId);
            }
        });
    }

    function DisableInfiniteScroll() {
        _isInfiniteScrollEnabled = false;
        $(window).unbind('scroll');
    }

    //---------------------------------//
    //-------Show Content Section-----//
    //-------------------------------//
    function ShowContent(content) {
        if (content.Type == "StreamingMedia") {
            GetVideo(content.Bucket.Slug, content.Slug, ShowVideo);
        } else {
            if (content.Segments[0].Body.indexOf("<h2>" + content.Title + "</h2") > -1) content.Title = "";
            if (content.Segments[0].Body.indexOf("<h1>" + content.Title + "</h1") > -1) content.Title = "";

            GetTemplate("content-template", function (source) {
                var template = Handlebars.compile(source);
                if (_config.inlineMode) {
                    //if quiz, assesssment, or calculator
                    content.Segments[0].Body = PreProcessSource(content.Segments[0].Body);
                }

                var html = template(content);
                _contentElement.html(html);

                var internalLinks = _contentElement.find("a[data-bucket-slug]");
                for (var i = 0; i < internalLinks.length; i++) {
                    var internalLink = $(internalLinks[i]);
                    var bucketSlug = internalLink.attr("data-bucket-slug");
                    var contentSlug = internalLink.attr("data-content-slug");

                    var uri = GetContentPath(bucketSlug, contentSlug);
                    internalLink.attr("href", uri);
                    internalLink.addClass("internalLink");
                }

                RegisterInternalLinks();
            });
        }
    }

    function PreProcessSource(source) {
        var hash = window.location.hash;
        if (hash.indexOf("&?") > -1) {
            var customAnswers = hash.substring(hash.indexOf("&?") + 2, hash.length);
            source = source.replace("location.search.substring(1);", "'" + customAnswers + "';");
        }
        source = source.replace(/location.pathname/g, "'" + _baseUri + "#/?" + _currentPath + "&'");

        return source;
    }

    function ShowVideo(video) {
        GetTemplate("content-template", function (source) {
            var template = Handlebars.compile(source);
            var html = template(video);
            _contentElement.html(html);

            //Create the container
            var videoContainer = $("#VideoContainer");
            var flowPlayerConfig = {
                "poster": video.ImageUri,
                "key": "$170954793413711,$103737256684481,$333970518248932,$220529912050272",
                "clip": {
                    "sources": []
                }
            };

            //Fill in the URL information
            for (var i = 0; i < video.Formats.length; i++) {
                flowPlayerConfig.clip.sources.push({
                    "type": video.Formats[i].MimeType,
                    "src": video.Formats[i].Uri
                });
            }

            flowplayer(videoContainer.get()[0], flowPlayerConfig);
        });
    }

    //---------------------------------//
    //--Handelebars Content Section---//
    //-------------------------------//
    Handlebars.registerHelper("getContentPath", function (bucketSlug, contentSlug) {
        return GetContentPath(bucketSlug, contentSlug);
    });

    Handlebars.registerHelper('ifEqualTo', function (value1, value2, options) {
        var fnTrue = options.fn, fnFalse = options.inverse;
        if (value1 == value2) return fnTrue();
        else return fnFalse();
    });

    Handlebars.registerHelper('ifElseBlank', function (val, options) {
        var fnTrue = options.fn, fnFalse = options.inverse;
        if (val == undefined || val == "") return fnTrue();
        else return fnFalse();
    });

    Handlebars.registerHelper('ifNotNullOrEmpty', function (val, options) {
        var fnTrue = options.fn, fnFalse = options.inverse;
        if (val == undefined || val == null || val == "") return fnFalse();
        else return fnTrue();
    });

    Handlebars.registerHelper('ToShortDateString', function (date) {
        if (date != undefined && date != null && date != "") {
            var convertedDate = new Date(date);
            var month = convertedDate.getMonth() + 1
            var day = convertedDate.getDay();
            var year = convertedDate.getFullYear();
            var shortStartDate = month + "/" + day + "/" + year;

            return shortStartDate;
        }

        return null;
    });

    function GetContentPath(bucketSlug, contentSlug) {
        var contentModel = {
            "BucketSlug": bucketSlug,
            "ContentSlug": contentSlug
        };

        var pathTemplate = "";
        if (_config.inlineMode) pathTemplate = _baseUri + "#/?";
        pathTemplate += _config.contentDisplayPath;

        var template = Handlebars.compile(pathTemplate);

        return template(contentModel);
    }

    //---------------------------------//
    //-----API Request Section--------//
    //-------------------------------//
    function GetEntireList(requestState, skip, items, onSuccess) {
        if (items == undefined || items == null) items = [];

        if (requestState.CurrentLetter == "#") {
            GetEntireSpecialCharacterList(requestState, 0, skip, items, onSuccess);
            return;
        }

        $.ajax({
            type: "GET",
            url: "https://api.kramesstaywell.com/Content.jsonp?TitleStartsWith=" + requestState.CurrentLetter + "&Buckets=" + requestState.Buckets + "&IncludeDrafts=False&$skip=" + skip + "&$top=" + DEFAULT_TOP + "&applicationId=" + _appId,
            contentType: "application/json; charset=utf-8",
            dataType: "jsonp",
            success: function (msg) {

                //Combine the paged items
                items = items.concat(msg.Items);
                if (items.length < msg.Total) {
                    skip += DEFAULT_TOP;
                    GetEntireList(requestState, skip, items, onSuccess);
                }
                else onSuccess(requestState, items);
            },
            error: function (request, status, error) {
                console.log(error);
            }
        });
    }

    function GetEntireSpecialCharacterList(requestState, subCharacterIndex, skip, items, onSuccess) {
        var letter = _subCharacters[subCharacterIndex];

        $.ajax({
            type: "GET",
            url: "https://api.kramesstaywell.com/Content.jsonp?TitleStartsWith=" + letter + "&Buckets=" + requestState.Buckets + "&IncludeDrafts=False&$skip=" + skip + "&$top=" + DEFAULT_TOP + "&applicationId=" + _appId,
            contentType: "application/json; charset=utf-8",
            dataType: "jsonp",
            success: function (msg) {

                //Combine the paged items
                items = items.concat(msg.Items);

                if (items.length < msg.Total) {
                    skip += DEFAULT_TOP;
                    GetEntireSpecialCharacterList(requestState, subCharacterIndex, skip, items, onSuccess);
                }
                else if (subCharacterIndex < _subCharacters.length - 1) {
                    subCharacterIndex++;
                    GetEntireSpecialCharacterList(requestState, subCharacterIndex, 0, items, onSuccess);
                } else onSuccess(requestState, items);
            },
            error: function (request, status, error) {
                console.log(error);
            }
        });
    }

    function GetTemplate(slug, onSuccess) {

        if (_config.templatePath == null) {
            $.ajax({
                type: "GET",
                contentType: "application/json; charset=utf-8",
                url: _config.baseApiPath + "/Content/plugin-templates/" + slug + ".jsonp?IncludeBody=True&applicationId=" + _appId,
                dataType: "jsonp",
                cache: true,
                success: function (msg) {
                    onSuccess(msg.Segments[0].Body);
                },
                error: function (request, status, error) {
                    console.log("Error retrieving '" + slug + "': " + error);
                }
            });
        } else {
            uri = _config.templatePath + "/" + slug + ".html";
            $.ajax({
                url: uri,
                cache: false,
                success: function (data) {
                    onSuccess(data);
                },
                error: function (request, status, error) {
                    console.log("Error retrieving '" + uri + "': " + error);
                }
            });
        }
    }

    function GetVideo(bucketSlug, slug, onSuccess) {

        //Get the streaming media from the bucket slug and content slug
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: _config.baseApiPath + "/StreamingMedia/" + bucketSlug + "/" + slug + ".jsonp?IncludeBody=True&Draft=False&GetOriginal=False&editMode=True&applicationId=" + _appId,
            dataType: "jsonp",
            cache: false,
            success: function (msg) {
                onSuccess(msg);
            },
            error: function (request, status, error) {
                console.log(error);
            }
        });
    }

    function GetContent(bucketSlug, contentSlug, onSuccess) {

        //Get the streaming media from the bucket slug and content slug
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: _config.baseApiPath + "/Content/" + bucketSlug + "/" + contentSlug + ".jsonp?IncludeBody=True&applicationId=" + _appId,
            dataType: "jsonp",
            cache: false,
            success: function (msg) {
                onSuccess(msg);
            },
            error: function (request, status, error) {
                console.log(error);
            }
        });
    }

    return _self;

}());