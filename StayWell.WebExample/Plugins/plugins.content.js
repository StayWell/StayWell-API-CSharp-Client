var CenterPlugin = (function () {
    var _containerElement;
    var _element;
    var _appId;
    var _config;

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
    }

    function InitializeContainer() {
        _containerElement.addClass("plugin-container");
    }

    function InitializeConfiguration() {
        //Define the default config
        _config = {
            baseApiPath: "https://api.kramesstaywell.com",
            templatePlath: null,
            contentDisplayPath: "/Content/{{BucketSlug}}/{{ContentSlug}}"
        };

        _config.baseApiPath = GetConfigValueIfSet("data-base-api-path", _config.baseApiPath);
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
            });
        }
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