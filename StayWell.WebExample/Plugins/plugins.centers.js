var CenterPlugin = (function () {
    var _categorySlug = "health-centers";
    var _category;
    var _jobCount = 0;
    var _activeCollection;
    var _containerElement;
    var _element;
    var _appId;
    var _config;
    var _baseUri;
    var _currentPath;
    var _currentHash;

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
        RegisterPartialTemplates();

        //Register html4 mode, use #, if the mode is inline mode.
        if(_config.inlineMode) window.History.options.html4Mode = true;
        window.History.init();
        ItializeHistory();

        //Determine the base paths.
        _baseUri = GetBasePath();           //Set the base URI which is important for routing throughout the application.
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
        var baseHTML = "<span id='LoadingCenters' class='loading-indicator'>Loading...<br /></span>";
        baseHTML += "<div id='PluginCenterElement'></div>";

        _containerElement.html(baseHTML);

        _element = $("#PluginCenterElement");
    }

    function InitializeConfiguration() {
        //Define the default config
        _config = {
            baseApiPath: "https://api.kramesstaywell.com",
            categorySlug: "health-centers",
            inlineMode: true,
            templatePlath: null,
            contentDisplayPath: "/Content/{{BucketSlug}}/{{ContentSlug}}"
        };

        _config.baseApiPath = GetConfigValueIfSet("data-base-api-path", _config.baseApiPath);
        _config.categorySlug = GetConfigValueIfSet("data-category-slug", _config.categorySlug);
        _config.inlineMode = GetConfigValueBoolIfSet("data-inline-mode", _config.inlineMode);
        _config.templatePath = GetConfigValueIfSet("data-template-path", _config.templatePath);
        _config.contentDisplayPath = GetConfigValueIfSet("data-content-display-path", _config.contentDisplayPath);
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

    Handlebars.registerHelper('ifVideos', function (topicName, options) {

        var fnTrue = options.fn, fnFalse = options.inverse;
        if (topicName.toUpperCase() == "VIDEOS") return fnTrue();
        else return fnFalse();
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

    Handlebars.registerHelper('shouldDisplayTopic', function (topicName, options) {
        var fnTrue = options.fn, fnFalse = options.inverse;

        if (topicName == undefined || topicName == null) return fnTrue();
        if (topicName.toUpperCase() == "VIDEOS") return fnFalse();
        if (topicName.toUpperCase() == "EXTRAS") return fnFalse();
        if (topicName.toUpperCase() == "MORE RESOURCES") return fnFalse();

        return fnTrue();
    });

    Handlebars.registerHelper('ifIsSubtopic', function (item, options) {
        var fnTrue = options.fn, fnFalse = options.inverse;
        if (item.Type == "Topic") return fnTrue();

        return fnFalse();
    });

    Handlebars.registerHelper("getPathCenter", function (model) {
        if (_config.inlineMode) return _baseUri + "#/?" + model.CategorySlug + "/" + model.CollectionSlug;//_baseUri + "/#!/" + model.CategorySlug + "/" + model.CollectionSlug;
        else return _baseUri + "/" + model.CategorySlug + "/" + model.CollectionSlug;

    });

    Handlebars.registerHelper("getCenterPath", function (categorySlug, collectionSlug) {
        if (_config.inlineMode) return _baseUri + "#/?" + categorySlug + "/" + collectionSlug;//_baseUri + "/#!/" + model.CategorySlug + "/" + model.CollectionSlug;
        else return _baseUri + "/" + categorySlug + "/" + collectionSlug;

    });

    Handlebars.registerHelper("getPath", function (parentSlug, slug) {
        var categorySlug = GetCategorySlug();
        var collectionSlug = GetCollectionSlug();

        if (_config.inlineMode) return _baseUri + "#/?" + categorySlug + "/" + collectionSlug + "/" + parentSlug + "/" + slug;
        else return _baseUri + "/" + categorySlug + "/" + collectionSlug + "/" + parentSlug + "/" + slug;
    });

    Handlebars.registerHelper("getTopicPath", function (slug) {
        var categorySlug = GetCategorySlug();
        var collectionSlug = GetCollectionSlug();
        var subTopicSlugs = GetSubTopicSlugs();

        var path
        if (_config.inlineMode) path = _baseUri + "#/?";
        else path = _baseUri;

        path += "/" + categorySlug + "/" + collectionSlug;
        for (var i = 0; i < subTopicSlugs.length; i++) {
            path += "/" + subTopicSlugs[i];
        }
        path += "/" + slug;

        return path;
    });

    Handlebars.registerHelper("getFullTopicPath", function (categorySlug, collectionSlug, topicSlug) {
        var path
        if (_config.inlineMode) path = _baseUri + "#/?";
        else path = _baseUri;

        path += "/" + categorySlug + "/" + collectionSlug + "/" + topicSlug;
        return path;
    });

    Handlebars.registerHelper("getContentPath", function (bucketSlug, contentSlug) {
        return GetContentPath(bucketSlug, contentSlug);
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

    Handlebars.registerHelper("moduloIf", function (index_count, mod, block) {
        if (parseInt(index_count) % (mod) === 0) {
            return block.fn(this);
        }
    });

    Handlebars.registerHelper("moduloIfLast", function (index_count, last, mod, block) {
        if ((parseInt(index_count) + 1) % (mod) === 0) {
            return block.fn(this);
        }

        if (last) {
            return block.fn(this);
        }
    });

    Handlebars.registerHelper("divide", function (index_count, mod) {
        return Math.floor(parseInt(index_count) / mod);
    });

    Handlebars.registerHelper('ifNotNullOrEmpty', function (val, options) {
        var fnTrue = options.fn, fnFalse = options.inverse;
        if (val == undefined || val==null || val == "") return fnFalse();
        else return fnTrue();
    });

    Handlebars.registerHelper('ifElseBlank', function (val, options) {
        var fnTrue = options.fn, fnFalse = options.inverse;
        if (val == undefined || val == "") return fnTrue();
        else return fnFalse();
    });

    Handlebars.registerHelper('ifGreaterThan', function (value1, value2, options) {
        var fnTrue = options.fn, fnFalse = options.inverse;
        if (parseInt(value1) > parseInt(value2)) return fnTrue();
        else return fnFalse();
    });

    Handlebars.registerHelper('ifLessThan', function (value1, value2, options) {
        var fnTrue = options.fn, fnFalse = options.inverse;
        if (parseInt(value1) < parseInt(value2)) return fnTrue();
        else return fnFalse();
    });

    Handlebars.registerHelper('ifEqualTo', function (value1, value2, options) {
        var fnTrue = options.fn, fnFalse = options.inverse;
        if (value1 == value2) return fnTrue();
        else return fnFalse();
    });

    function RegisterPartialTemplates() {
        //Note there may be a race condition here...  
        //GetTemplate("/plugins/centers/content-thumbnail-template.html", function (source) {
        GetTemplate("content-thumbnail-template", function (source) {
            Handlebars.registerPartial("contentThumbnailTemplate", source);
        });

        //GetTemplate("/plugins/centers/content-thumbnail-small-template.html", function (source) {
        GetTemplate("content-thumbnail-small-template", function (source) {
            Handlebars.registerPartial("contentThumbnailSmallTemplate", source);
        });
    }

    //------------------------------//
    //--------Page Routing---------//
    //----------------------------//
    function RouteRequest() {
        //Define the URL Paths
        var loadCenters = /^\/[\w-]+\/?$/;                            //Get: /{CategorySlug}
        var loadCentersExplorer = /^\/[\w-]+\/[\w-]+\/?$/;            //Get: /{CategorySlug}/{CollectionSlug}/
        var loadSubTopicExplorer = /^\/[\w-]+\/[\w-]+(\/[\w-]+)+\/?$/; //Get: /{CategorySlug}/{CollectionSlug}/{SubTopicSlug}
        var loadContent = /^\/Content\/[\w-]+\/[\w-]+\/?$/;

        //Route the path to the proper processes
        if (loadContent.test(_currentPath)) {
            _element.html("");
            $("#LoadingCenters").show();

            var contentSlugs = GetContentSlugs();
            GetContent(contentSlugs.BucketSlug, contentSlugs.ContentSlug, ShowContent);
        } else if (loadCenters.test(_currentPath)) {
            _element.html("");
            $("#LoadingCenters").show();

            //Get the slugs
            _activeCollection = null;

            var categorySlug = GetCategorySlug();
            DisplayCenters(categorySlug);

        } else if (loadCentersExplorer.test(_currentPath)) {
            var collectionSlug = GetCollectionSlug();
            
            if (_activeCollection != null && _activeCollection.Slug == collectionSlug) {
                //The collection was already requested so we don't need to
                //go back to the server.
                ShowCenter(_activeCollection);
            } else {
                //This must be a direct link or refresh. As a result
                //there is no reference to the collection so we need to go
                //get one.
                //Get the slugs
                var categorySlug = GetCategorySlug();

                $("#LoadingCenters").show();
                _element.html("");

                GetCollection(collectionSlug, ShowCenter);
            }
        } else if (loadSubTopicExplorer.test(_currentPath)) {

            if (_activeCollection != null) {
                //The collection was already requested so we don't need to
                //go back to the server.
                ShowSubTopic(_activeCollection);
            } else {
                //This must be a direct link or refresh. As a result
                //there is no reference to the collection so we need to go
                //get one.
                var categorySlug = GetCategorySlug();
                var collectionSlug = GetCollectionSlug();

                _element.html("");
                $("#LoadingCenters").show();

                GetCollection(collectionSlug, ShowSubTopic);
            }

        }
        else {
            console.log("Route not found: " + _currentPath);
        }

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

    function GetCategorySlug() {
        var path = _currentPath;

        var selectRegex = /^\/([\w-]+)\/?/;
        var arr = selectRegex.exec(path);

        //The entire string will match and be added to the first array element.
        //The second element in the array represents the ([\w-).
        if (arr == null || arr == undefined) return "";
        if (arr.length == 2) return arr[1];
        else return null;
    }

    function GetCollectionSlug(path) {
        var path = _currentPath;

        var selectRegex = /^\/[\w-]+\/([\w-]+)/;
        var arr = selectRegex.exec(path);

        //The entire string will match and be added to the first array element.
        //The second element in the array represents the ([\w-).
        if (arr == null || arr == undefined) return "";
        if (arr.length == 2) return arr[1];
        else return null;
    }

    function GetSubTopicSlugs(path) {
        var path = _currentPath;

        var selectRegex = /^\/[\w-]+\/[\w-]+((\/[\w-]+)+)\/?$/;
        var arr = selectRegex.exec(path);

        //The entire string will match and be added to the first array element.
        //The second element in the array represents the ([\w-).
        if (arr == undefined || arr == null || arr.length == 0) return [];

        var subTopicSplit = arr[1].split("/");
        var subTopics = [];
        for (var i = 0; i < subTopicSplit.length; i++) {
            if (subTopicSplit[i] != "") subTopics.push(subTopicSplit[i]);
        }

        if (subTopics.length > 0) return subTopics;
        else return null;

    }

    function GetBasePath() {
        var basePath = "";
        if (_config.inlineMode) basePath = window.location.pathname;
        else {
            var pathParts = window.location.pathname.split("/");
            for (var i = 0; i < pathParts.length; i++) {
                if (pathParts[i] != "") {
                    var category = GetCategoryBySlug(pathParts[i]);
                    if (category == null) basePath += "/" + pathParts[i];
                    else break;
                }
            }
        }

        return basePath;
    }

    function GetProcessedPathName(uriToProcess) {
        if (_config.inlineMode) {
            var uri = window.location.hash;
            if (uri == "") {
                uri = "/" + _config.categorySlug;
            } else {
                uri = uri.substring(uri.indexOf("?") + 1, uri.length);
                if(uri.indexOf("/")!=0) uri = "/" + uri;
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

    //------------------------------//
    //----Show list of Centers-----//
    //----------------------------//
    function DisplayCenters(categorySlug) {
        _categorySlug = categorySlug;
        _category = GetCategoryBySlug(_categorySlug);

        if (_category != null) {
            //Add placeholders for all centers
            //GetTemplate("/Plugins/Centers/center-listing-template.html", function (source) {
            GetTemplate("center-listing-template", function (source) {
                var template = Handlebars.compile(source);
                var html = template(_category);
                _element.html(html);

                //Load each center
                for (var i = 0; i < _category.Collections.length; i++) {
                    _jobCount++;
                    GetCollectionLite(_category.Collections[i].Slug, ShowCenterBlurb);
                }
            });
        }
    }

    function GetMappedCollection(categegorySlug, collectionSlug) {
        var category = GetCategoryBySlug(categegorySlug);

        for (var i = 0; i < category.Collections.length; i++) {
            if (category.Collections[i].Slug == collectionSlug) return category.Collections[i];
        }
        return null;
    }

    function GetCategoryBySlug(categegorySlug) {
        for (var i = 0; i < ContentCategoryList.Categories.length; i++) {
            if (ContentCategoryList.Categories[i].Slug == categegorySlug) return ContentCategoryList.Categories[i];
        }
        return null;
    }

    function ShowCenterBlurb(center) {
        _jobCount--;

        //Get the collection friendly information.
        var collectionInfo;
        for (var i = 0; i < _category.Collections.length; i++) {
            if (_category.Collections[i].Slug == center.Slug) collectionInfo = _category.Collections[i];
        }

        var collectionModel = {
            "ImageUri": center.ImageUri,
            "Description": truncateText(center.Description, 130),
            "Title": collectionInfo.Name,
            "Items": center.Items,
            "CategorySlug": GetCategorySlug(GetProcessedPathName()),
            "CollectionSlug": collectionInfo.Slug
        };

        //Build the center blurb.
        //GetTemplate("/Plugins/Centers/center-blurb-template.html", function (source) {
        GetTemplate("center-blurb-template", function (source) {

            var template = Handlebars.compile(source);
            var html = template(collectionModel);
            var centerDiv = $("#center-" + center.Slug);
            centerDiv.html(html);
            centerDiv.show();

            RegisterInternalLinks();
            if (_jobCount == 0) {
                $("#LoadingCenters").hide();
            }
        });
    }


    //------------------------//
    //----Show the Center----//
    //----------------------//
    function ShowCenter(collection) {
        _activeCollection = collection;
        collection = CorrectSubTopicImageUriBug(collection);;

        //GetTemplate("/Plugins/Centers/center-template.html", function (source) {
        GetTemplate("center-template", function (source) {
            var template = Handlebars.compile(source);
            var collectionView = CreateCollectionView(collection);

            //Swap out the name for the root collection because the name is not always super friendly.
            var mappedCollectionName = GetMappedCollectionName();
            if (mappedCollectionName != null) collectionView.Title = mappedCollectionName;

            var html = template(collectionView);
            //_element.html("Loaded");
            //return;

            _element.html(html);

            //Build the other sections
            //ShowVideoSection(collection);
            ShowMoreResourcesSection(collectionView);

            $("#LoadingCenters").hide();
            RegisterInternalLinks();
        });
    }

    function RegisterInternalLinks() {
        //Register links that should be internal state change link
        $(".internalLink").unbind("click"); //Make sure we don't double bind the same listener.
        $(".internalLink").click(function (e) {
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

    function GetMappedCollectionName() {
        var categorySlug = GetCategorySlug();
        var collectionSlug = GetCollectionSlug();

        var mappedCollection = GetMappedCollection(categorySlug, collectionSlug);
        if (mappedCollection != null) return mappedCollection.Name;
        else return null;
    }

    function CreateCollectionView(collection) {
        //Initialize the Root View
        var collectionView = GetCollectionViewFromTopic(collection);

        //Iterate over Items and Create Views for each
        for (var i = 0; i < collection.Items.length; i++) {

            //Add topics and\or articles to the list
            if (collection.Items[i].Type == "Topic") {

                //Createt the subtopic collection view.
                var subTopicCollectionView = CreateSubTopicCollectionView(collection, collection.Items[i]);
            }
            else collectionView.Articles.push(collection.Items[i])

            collectionView.Topics.push(subTopicCollectionView);
        }

        collectionView.GettingStarted = GetSectionContent(collection, ["Extras", "Getting Started"]);
        collectionView.Calculators = GetSectionContent(collection, ["More Resources", "Interactive Tools", "Calculators"], "Calculators");
        collectionView.Quizzes = GetSectionContent(collection, ["More Resources", "Interactive Tools", "Quizzes"], "Quizzes");
        collectionView.Podcasts = GetSectionContent(collection, ["More Resources", "Multimedia", "Podcasts"], "Podcasts");
        collectionView.Assessments = GetSectionContent(collection, ["More Resources", "Multimedia", "Assessments"], "Assessments");
        collectionView.Videos = GetSectionContent(collection, ["Videos"], "Videos");
        collectionView.Videos = PreProcessVideoThumbnails(collectionView.Videos);

        collectionView.Highlights = GetDailyHighlight(collectionView);

        return collectionView;
    }

    function CreateSubTopicCollectionView(collection, topic) {
        var subTopicCollectionView = GetCollectionViewFromTopic(topic);

        //Add either topics or articles to the lists
        if (topic.Items != null && topic.Items != undefined) {
            for (var c = 0; c < topic.Items.length; c++) {
                if (topic.Items[c].Type == "Topic") subTopicCollectionView.Topics.push(topic.Items[c]);
                else subTopicCollectionView.Articles.push(topic.Items[c]);
            }
        }

        //Get extra information for the SubtopicCollectionView
        subTopicCollectionView.Quizzes = GetSectionContent(collection, ["Extras", subTopicCollectionView.Title, "Section Features", "Quizzes"], "Quizzes");
        subTopicCollectionView.Calculators = GetSectionContent(collection, ["Extras", subTopicCollectionView.Title, "Section Features", "Calculators"], "Calculators");
        subTopicCollectionView.Calculators = GetSectionContent(collection, ["Extras", subTopicCollectionView.Title, "Section Features", "Assessments"], "Assessments");
        subTopicCollectionView.Podcasts = GetSectionContent(collection, ["Extras", subTopicCollectionView.Title, "Section Features - Multimedia", "Podcasts"], "Podcasts");
        subTopicCollectionView.Videos = GetSectionContent(collection, ["Extras", subTopicCollectionView.Title, "Section Features - Multimedia", "Videos"], "Videos");
        subTopicCollectionView.Videos = PreProcessVideoThumbnails(subTopicCollectionView.Videos);

        subTopicCollectionView.Highlights = GetHighlghtsByCount(subTopicCollectionView, 3);

        //Get articles from one level deeper in the mix
        for (var c = 0; c < subTopicCollectionView.Topics.length; c++) {
            if (subTopicCollectionView.Topics[c].Items != null) {
                for (var x = 0; x < subTopicCollectionView.Topics[c].Items.length; x++) {
                    if (subTopicCollectionView.Topics[c].Items[x].Type != "Topic") subTopicCollectionView.Articles.push(subTopicCollectionView.Topics[c].Items[x]);
                }
            }
        }

        return subTopicCollectionView;
    }

    function GetCollectionViewFromTopic(topic) {
        var collectionView = {
            "Title": topic.Title,
            "Slug": topic.Slug,
            "Topics": [],
            "Articles": [],
            "ImageUri": topic.ImageUri,
            "Description": topic.Description,
            "Highlights": [],
            "GettingStarted": [],
            "Podcasts": [],
            "Videos": [],
            "Quizzes": [],
            "Assessments": [],
            "Calculators": [],
            "News": [] //TODO: Can't pull specific news because dynamic collections are not in place yet.
        };
        return collectionView;
    }

    function GetHighlghtsByCount(collectionView, count) {
        var highlight = []

        highlight = highlight.concat(GetTopItems(collectionView.Videos, count - highlight.length));
        highlight = highlight.concat(GetTopItems(collectionView.Podcasts, count - highlight.length));
        highlight = highlight.concat(GetTopItems(collectionView.Assessments, count - highlight.length));
        highlight = highlight.concat(GetTopItems(collectionView.Quizzes, count - highlight.length));
        highlight = highlight.concat(GetTopItems(collectionView.Calculators, count - highlight.length));

        return highlight;
    }

    function GetTopItems(itemArray, count) {
        var items = [];

        //If array is less then count just return the array.
        if (itemArray.length <= count) items = itemArray;
        else {
            for (var i = 0; i < count; i++) {
                items.push(itemArray[i]);
            }
        }
        return itemArray;
    }

    function GetDailyHighlight(collectionView) {
        var dailyHighlight = [];

        var items = [];
        items = items.concat(collectionView.Calculators);
        items = items.concat(collectionView.Quizzes);
        items = items.concat(collectionView.Podcasts);
        items = items.concat(collectionView.Videos);
        items = items.concat(collectionView.Assessments);

        //Get constant index for an interactive tool.
        //Leverage the day of the year to get something
        //constant for an entire day.
        var dayOfYear = new Date().getDayOfYear();
        var totalCount = items.length;
        var itemIndex = dayOfYear % totalCount;

        dailyHighlight.push(items[itemIndex]);

        return dailyHighlight;
    }

    function GetSectionContent(topic, titlePath, typeName) {
        var rootTopic = GetSubTopicFromCollectionByTitle(topic, titlePath);

        if (rootTopic != null && rootTopic.Items != null && rootTopic.Items.length != 0) return GetAllArticles(rootTopic, typeName);
        else return [];
    }

    function GetAllArticles(topic, typeName) {
        var flattenedArticles = [];
        if (topic != null && topic.Items != null) {
            for (var i = 0; i < topic.Items.length; i++) {
                if (topic.Items[i].Type == "Topic") {
                    flattenedArticles = flattenedArticles.concat(GetAllArticles(topic.Items[i], typeName));
                }
                else {
                    var itemModel = CreateItemModel(topic.Items[i], typeName);
                    flattenedArticles.push(itemModel);
                }
            }
        }
        return flattenedArticles;
    }

    function PreProcessVideoThumbnails(items) {
        for (var i = 0; i < items.length; i++) {
            items[i].ImageUri = "/Images/imagePlaceholder.png";
        }
        return items;
    }

    function ShowSubTopic(collection) {
        _activeCollection = collection;
        collection = CorrectSubTopicImageUriBug(collection);

        //Find the topic in the collection
        var topicSlugs = GetSubTopicSlugs();
        var topic = GetSubTopicFromCollection(collection, topicSlugs);

        //Create the model
        var subTopicCollectionView = CreateSubTopicCollectionView(collection, topic)

        GetTemplate("center-sub-topic-template", function (source) {
            var template = Handlebars.compile(source);
            var html = template(subTopicCollectionView);
            _element.html(html);

            ShowMoreResourcesSection(subTopicCollectionView);
            RegisterInternalLinks();

            $("#LoadingCenters").hide();
        });
    }

    function GetSubTopicFromCollection(collection, topicSlugs) {
        var topic = collection;
        for (var i = 0; i < topicSlugs.length; i++) {

            var isFound = false;
            for (var c = 0; c < topic.Items.length; c++) {;
                if (topic.Items[c].Slug.toUpperCase() == topicSlugs[i].toUpperCase()) {
                    topic = topic.Items[c];
                    isFound = true;
                    break;
                }
            }

            if (!isFound) return null;
        }

        return topic;
    }

    function GetSubTopicFromCollectionByTitle(collection, titles) {
        var topic = collection;
        for (var i = 0; i < titles.length; i++) {

            var isFound = false;
            if (topic.Items != null && topic.Items != undefined) {

                for (var c = 0; c < topic.Items.length; c++) {

                    if (topic.Items[c].Title.toUpperCase() == titles[i].toUpperCase()) {
                        topic = topic.Items[c];
                        isFound = true;
                        break;
                    }
                }
            }

            if (!isFound) return null;
        }

        return topic;
    }

    //------------------------//
    //-----Video Section-----//
    //----------------------//
    function LoadVideo(bucketSlug, slug) {

        //Get the streaming media from the bucket slug and content slug
        $.ajax({
            type: "GET",
            url: "/api/streamingmedia/" + bucketSlug + "/" + slug,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var videoPanel = $("[data-slug='" + msg.Slug + "'][data-bucketslug='" + msg.Bucket.Slug + "']");

                var imageAsset = $(videoPanel).find("img");
                imageAsset.attr('src', msg.ImageUri);
                imageAsset.attr('alt', msg.Title);
                imageAsset.attr('title', msg.Title);
                $(videoPanel).find("a").attr('href', GetContentPath(msg.Bucket.Slug, msg.Slug));
                $(videoPanel).find(".placeholder-title").html(msg.Title);
                $(videoPanel).find(".placeholder-viewcount").html(msg.ViewCount);

                var duration = msg.Formats[0].RunningTime;
                if (duration != null && duration != "") {
                    duration = GetDurationForDisplay(duration);
                }
                $(videoPanel).find(".placeholder-duration").html(duration);
            },
            error: function (request, status, error) {
                console.log(error);
            }
        });
    }

    function GetDurationForDisplay(runningTime) {
        var seconds = runningTime % 60;
        var minutes = runningTime / 60;

        return Math.floor(minutes) + ":" + seconds;
    }

    function CorrectSubTopicImageUriBug(collection) {
        var imageUri = collection.ImageUri;
        if (imageUri == null) return collection;

        var firstPart = imageUri.substring(0, imageUri.lastIndexOf("/"));

        for (var i = 0; i < collection.Items.length; i++) {
            var subTopicImageUri = collection.Items[i].ImageUri;

            if (subTopicImageUri != null && subTopicImageUri != "") {
                var lastIndexOfSlash = subTopicImageUri.lastIndexOf("/");

                var lastPart = subTopicImageUri.substring(lastIndexOfSlash, subTopicImageUri.length);
                collection.Items[i].ImageUri = firstPart + lastPart;
            }
        }

        return collection;
    }

    //---------------------------------//
    //-----More Resources Section-----//
    //-------------------------------//
    function ShowMoreResourcesSection(collectionView) {
        //Get the model
        var interactiveResourcesModel = CreateIntearctiveResourcesModel(collectionView);

        //Display the model
        //GetTemplate("/Plugins/Centers/more-resources-template.html", function (source) {
        GetTemplate("center-more-resources-template", function (source) {
            var template = Handlebars.compile(source);
            var html = template(interactiveResourcesModel);
            $("#MoreResourcesContainer").html(html);

            LoadAllVideos(collectionView.Videos);   //Async load of videos
            RegisterMoreResourcesLinks();           //Register inline links
        });
    }

    function RegisterMoreResourcesLinks() {
        $(".interactive-filter").click(function (e) {
            var filterType = $(e.currentTarget).attr("data-interactivetype");
            $("#MoreResourcesSection").data("currentfilter", filterType);

            //Hide everything to start.
            $(".interactive-item").hide();

            //Show all items if it's expanded.  If it's not expanded only show the top 7.
            if ($("#MoreResourcesSection").data("isexpanded")) {
                if (filterType == "All") $(".interactive-item").show();
                else $(".interactive-item[data-interactivetype='" + filterType + "']").show();
            } else {
                var items;
                if (filterType == "All") items = $(".interactive-item");
                else items = $(".interactive-item[data-interactivetype='" + filterType + "']");

                for (var i = 0; i < 7; i++) {
                    $(items[i]).show();
                }

                if (items.length > 7) $(".more-resources-show-all").show();
                else $(".more-resources-show-all").hide();
            }
        });

        $(".more-resources-show-all").click(function (e) {
            $("#MoreResourcesSection").data("isexpanded", true);

            var filterType = $("#MoreResourcesSection").data("currentfilter");
            if (filterType == "All") {
                $(".interactive-item").show();
            } else {
                $(".interactive-item[data-interactivetype='" + filterType + "']").show();
            }

            $(e.currentTarget).hide();
        });
    }

    function CreateIntearctiveResourcesModel(collectionView) {
        var interactiveResourcesModel = {
            "Menu": [],
            "Items": []
        };

        //Build the Items
        var items = [];
        items = items.concat(CreateInteractiveModelItem("Videos", collectionView.Videos))
        items = items.concat(CreateInteractiveModelItem("Quizzes", collectionView.Quizzes));
        items = items.concat(CreateInteractiveModelItem("Podcasts", collectionView.Podcasts));
        items = items.concat(CreateInteractiveModelItem("Calculators", collectionView.Calculators));
        items = items.concat(CreateInteractiveModelItem("Assessments", collectionView.Assessments));

        //Build the Interactive Menu
        var interactiveMenu = [];
        if (items.length > 0) interactiveMenu.push({ "Title": "All", "Count": items.length });
        if (collectionView.Videos.length > 0) interactiveMenu.push({ "Title": "Videos", "Count": collectionView.Videos.length });
        if (collectionView.Quizzes.length > 0) interactiveMenu.push({ "Title": "Quizzes", "Count": collectionView.Quizzes.length });
        if (collectionView.Podcasts.length > 0) interactiveMenu.push({ "Title": "Podcasts", "Count": collectionView.Podcasts.length });
        if (collectionView.Calculators.length > 0) interactiveMenu.push({ "Title": "Calculators", "Count": collectionView.Calculators.length });
        if (collectionView.Assessments.length > 0) interactiveMenu.push({ "Title": "Assessments", "Count": collectionView.Assessments.length });

        //Pull it together
        interactiveResourcesModel.Items = items;
        interactiveResourcesModel.Menu = interactiveMenu;

        return interactiveResourcesModel;
    }

    function CreateInteractiveModelItem(typeName, items) {
        var interactiveContentModel = [];
        for (var i = 0; i < items.length; i++) {
            interactiveContentModel.push(CreateItemModel(items[i], typeName));
        }
        return interactiveContentModel;
    }

    function CreateItemModel(item, typeName) {
        var model = {
            "Type": item.Type,
            "TypeName": typeName,
            "Bucket": item.Bucket,
            "Slug": item.Slug,
            "Description": truncateText(item.Description, 140),
            "Title": item.Title,
            "ImageUri": item.ImageUri
        };

        return model;
    }

    function LoadAllVideos(videos) {
        for (var i = 0; i < videos.length; i++) {
            GetVideo(videos[i].Bucket.Slug, videos[i].Slug, function (item) {
                var videoPanel = $("[data-slug='" + item.Slug + "'][data-bucketslug='" + item.Bucket.Slug + "']");

                var imageAsset = $(videoPanel).find("img");
                imageAsset.attr('src', item.ImageUri);
            });
        }
    }

    //---------------------------------//
    //-------Show Content Section-----//
    //-------------------------------//
    function ShowContent(content) {
        if (content.Type == "StreamingMedia") {
            GetVideo(content.Bucket.Slug, content.Slug, ShowVideo);
        } else {
            $("#LoadingCenters").hide();

            if (content.Segments[0].Body.indexOf("<h2>" + content.Title + "</h2") > -1) content.Title = "";
            if (content.Segments[0].Body.indexOf("<h1>" + content.Title + "</h1") > -1) content.Title = "";

            GetTemplate("content-template", function (source) {
                var template = Handlebars.compile(source);
                if (_config.inlineMode) {
                    //if quiz, assesssment, or calculator
                    content.Segments[0].Body = PreProcessSource(content.Segments[0].Body);
                }

                var html = template(content);
                _element.html(html);

                var internalLinks = _element.find("a[data-bucket-slug]");
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
        $("#LoadingCenters").hide();

        GetTemplate("content-template", function (source) {
            var template = Handlebars.compile(source);
            var html = template(video);
            _element.html(html);

            //Create the container
            var videoContainer = $("#VideoContainer");
            var flowPlayerConfig = {
                "poster": video.ImageUri,
                "key": "$170954793413711,$103737256684481,$333970518248932,$220529912050272",
                "clip": {
                    "sources" : []
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

    var _dayOfYearOffsets = [
      0,
      31, // + 31 jan
      59, // + 28 feb *
      90, // + 31 mar
      120, // + 30 apr
      151, // + 31 may
      181, // + 30 jun
      212, // + 31 jul
      243, // + 31 aug
      273, // + 30 sep
      304, // + 31 oct
      334 // + 30 nov
    ];

    var _dayOfLeapYearOffsets = [
      0,
      31, // + 31 jan
      59, // + 29 feb *
      91, // + 31 mar
      121, // + 30 apr
      152, // + 31 may
      182, // + 30 jun
      213, // + 31 jul
      244, // + 31 aug
      274, // + 30 sep
      305, // + 31 oct
      335 // + 30 nov
    ];

    Date.prototype.isLeapYear = function () {
        yr = this.getFullYear();
        return !((yr % 4) || (!(yr % 100) && (yr % 400)));
    }

    Date.prototype.getDayOfYear = function () {
        var m = this.getMonth(),
          d = this.getDay(),
          ly = this.isLeapYear(),
          day = ly ? (_dayOfLeapYearOffsets[m] + d) : (_dayOfYearOffsets[m] + d);
        return day;
    }

    function truncateText(text, numChars) {
        if (text == null || text == undefined) return "";

        if (text.length >= numChars) {
            text = text.substring(0, numChars) + "...";
        }
        return text;
    }

    function GetCollectionLite(collectionSlug, onSuccess) {
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: _config.baseApiPath + "/Collections/" + collectionSlug + ".jsonp?includeChildren=True&recursive=False&includeContent=False&applicationId=" + _appId,
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

    function GetCollection(collectionSlug, onSuccess) {
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: _config.baseApiPath + "/Collections/" + collectionSlug + ".jsonp?includeChildren=True&recursive=True&includeContent=True&applicationId=" + _appId,
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
                    console.log(error);
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
                    console.log(error);
                }
            });
        }
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