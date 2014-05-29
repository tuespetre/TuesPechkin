using System;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading;

namespace Pechkin
{
    /// <summary>
    /// Document settings object. Is used to supply document parameters to the converter.
    /// 
    /// You will find that there's no page body for conversion of pages that are in memory.
    /// Instead the page body is supplied into the <code>Convert</code> method of the converter.
    /// </summary>
    [Serializable]
    public class ObjectSettings
    {
        private string _tocUseDottedLines = "false"; // must be either "true" or "false"
        private string _tocCaption; // caption for table of content
        private string _tocCreateLinks = "true"; // should TOC entries link to the content
        private string _tocBackLinks = "false"; // create links from headings back to TOC
        private string _tocIndentation; // indentation value for leveling TOC entries, ex. "2em"
        private string _tocFontScale; // factor of font scaling for the deeper TOC level, ex. "0.8"

        private string _createToc = "false"; // create table of content in the document

        private string _includeInOutline; // include this document into outline and TOC generation
        private string _pagesCount; // count pages in this document for use in outline and TOC generation

        private string _tocXsl; // if it's not empty, this XSL stylesheet is used to convert XML outline into TOC, the page content is ignored

        private string _pageUri; // URL of filename of the page to convert, if "-" then input is read from stdin

        private string _useExternalLinks = "true"; // create external PDF links from external <a> tags in html
        private string _useLocalLinks = "true"; // create PDF links for local anchors in html
        private string _produceForms = "true"; // create PDF forms form html ones

        private string _loadUsername; // username to use in HTTPAuth
        private string _loadPassword; // password to use in HTTPAuth
        private string _loadJsDelay; // amount of time in ms to wait before printing the page
        private string _loadZoomFactor; // zoom factor, e.g. "2.2"
        private string _loadRepeatCustomHeaders = "true"; // repeat custom headers for every request
        private string _loadBlockLocalFileAccess = "false";
        private string _loadStopSlowScript = "true"; // kinda like in a browser
        #if DEBUG
        private string _loadDebugJavascript = "true"; // forward javascript warnings and errors into into warning callback
        private string _loadErrorHandling = "abort"; // how we handle objects that are failed to load. "abort" stops conversion, "skip" just omits the object from the output, "ignore" tries to deal with damaged object
        #else
        private string _loadDebugJavascript = "false"; // forward javascript warnings and errors into into warning callback
        private string _loadErrorHandling = "ignore"; // how we handle objects that are failed to load. "abort" stops conversion, "skip" just omits the object from the output, "ignore" tries to deal with damaged object
        #endif
        private string _loadProxy; // string describing proxy in format http://username:password@host:port, http can be changed to socks5, see http://madalgo.au.dk/~jakobt/wkhtmltoxdoc/wkhtmltopdf-0.9.9-doc.html

        private string _webPrintBackground = "false"; // print background image
        private string _webLoadImages = "true"; // load images in the document
        private string _webRunJavascript = "true"; // run javascript
        private string _webIntelligentShrinking = "true"; // fits more content in one page
        private string _webMinFontSize; // minimum font size in pt, e.g. "9"
        private string _webPrintMediaType = "true"; // use "print" media instead of "screen" media
        private string _webDefaultEncoding = "utf-8"; // encoding to use, if it's not specified properly
        private string _webUserStylesheetUri; // URL or filename for the user stylesheet
        private string _webEnablePlugins = "false"; // enable some sort of plugins, er...

        public HeaderSettings Header { get; set; }

        public FooterSettings Footer { get; set; }

        /// <summary>
        /// Use dotted lines between ToC entry and page number.
        /// </summary>
        /// <param name="useDottedLinesForToc"></param>
        /// <returns>config object</returns>
        [Obsolete("Library has no proper implementation for ToC creation in c bingins. This setting is supported, but it has no effect.")]
        public ObjectSettings SetTocLinkDottedLines(bool useDottedLinesForToc)
        {
            this._tocUseDottedLines = useDottedLinesForToc ? "true" : "false";

            return this;
        }

        [Obsolete("Library has no proper implementation for ToC creation in c bingins. This setting is supported, but it has no effect.")]
        public ObjectSettings SetTocCaption(string captionText)
        {
            this._tocCaption = captionText;

            return this;
        }

        /// <summary>
        /// Set whether we should create a link from each TOC entry to the place in the document it points to.
        /// </summary>
        /// <param name="createLinksToContent"></param>
        /// <returns>config object</returns>
        [Obsolete("Library has no proper implementation for ToC creation in c bingins. This setting is supported, but it has no effect.")]
        public ObjectSettings SetTocCreateLinks(bool createLinksToContent)
        {
            this._tocCreateLinks = createLinksToContent ? "true" : "false";

            return this;
        }

        /// <summary>
        /// Set whether we should create links from content back to the TOC.
        /// </summary>
        /// <param name="createLinksToToc"></param>
        /// <returns>config object</returns>
        [Obsolete("Library has no proper implementation for ToC creation in c bingins. This setting is supported, but it has no effect.")]
        public ObjectSettings SetTocCreateBackLinks(bool createLinksToToc)
        {
            this._tocBackLinks = createLinksToToc ? "true" : "false";

            return this;
        }

        /// <summary>
        /// Sets TOC indentation value for leveling the entries.
        /// </summary>
        /// <param name="indent">indent size in hundredth of inches</param>
        /// <returns>config object</returns>
        [Obsolete("Library has no proper implementation for ToC creation in c bingins. This setting is supported, but it has no effect.")]
        public ObjectSettings SetTocIndentation(int indent)
        {
            this._tocIndentation = (indent / 100.0).ToString("0.##", CultureInfo.InvariantCulture);

            return this;
        }

        /// <summary>
        /// Sets TOC indentation value for leveling the entries. Gives you more freedom.
        /// </summary>
        /// <param name="indent">indent size with the units, like "2em", or "1in", or "3pt"</param>
        /// <returns>config object</returns>
        [Obsolete("Library has no proper implementation for ToC creation in c bingins. This setting is supported, but it has no effect.")]
        public ObjectSettings SetTocIndentation(string indent)
        {
            this._tocIndentation = indent;

            return this;
        }

        /// <summary>
        /// Sets TOC font scale for deeper TOC level links.
        /// </summary>
        /// <param name="scale">scale factor, recommended to be &lt;= 1.0</param>
        /// <returns>config object</returns>
        [Obsolete("Library has no proper implementation for ToC creation in c bingins. This setting is supported, but it has no effect.")]
        public ObjectSettings SetTocFontScale(double scale)
        {
            this._tocFontScale = scale.ToString("0.##", CultureInfo.InvariantCulture);

            return this;
        }

        [Obsolete("Library has no proper implementation for ToC creation in c bingins. Turning this on will break the output.")]
        public ObjectSettings SetCreateToc(bool createToc)
        {
            // to fix this, see pdfcommanlineparser.cc:183 for the correct syntax
            this._createToc = createToc ? "true" : "false";

            Tracer.Warn(String.Format("T:{0} Table of content generation is turned on. The result may be not as expected", Thread.CurrentThread.Name));

            return this;
        }

        /// <summary>
        /// Specifies whether this document should be included in the outline
        /// </summary>
        /// <param name="includeDocumentInOutline"></param>
        /// <returns>config object</returns>
        public ObjectSettings SetIncludeInOutline(bool includeDocumentInOutline)
        {
            this._includeInOutline = includeDocumentInOutline ? "true" : "false";

            return this;
        }

        /// <summary>
        /// Specifies whether his document's pages are counted for the TOC and outline generation.
        /// </summary>
        /// <param name="affectPageCounts"></param>
        /// <returns>object config</returns>
        public ObjectSettings SetAffectPageCounts(bool affectPageCounts)
        {
            this._pagesCount = affectPageCounts ? "true" : "false";

            return this;
        }

        /// <summary>
        /// Sets TOC XSL stylesheet URL or filename. If it's not null, this stylesheet used with the outline XML file to form TOC and page content is ignored completely.
        /// </summary>
        /// <param name="tocXslStylesheetUri">uri of the TOC XSL stylesheet</param>
        /// <returns>config object</returns>
        public ObjectSettings SetTocXsl(string tocXslStylesheetUri)
        {
            this._tocXsl = tocXslStylesheetUri;

            return this;
        }

        /// <summary>
        /// Sets page URL or filename that will be converted to PDF. If you want to convert from string or resource, use appropriate method of the Converter instead.
        /// </summary>
        /// <param name="pageUri">URI of the page</param>
        /// <returns>config object</returns>
        public ObjectSettings SetPageUri(string pageUri)
        {
            this._pageUri = pageUri;

            return this;
        }

        public ObjectSettings SetCreateExternalLinks(bool createExternalPdfLinks)
        {
            this._useExternalLinks = createExternalPdfLinks ? "true" : "false";

            return this;
        }

        public ObjectSettings SetCreateInternalLinks(bool createInternalPdfLinks)
        {
            this._useLocalLinks = createInternalPdfLinks ? "true" : "false";

            return this;
        }

        /// <summary>
        /// Specify whether converter should produce PDF forms from HTML ones.
        /// </summary>
        /// <param name="createPdfForms"></param>
        /// <returns>config object</returns>
        public ObjectSettings SetCreateForms(bool createPdfForms)
        {
            this._produceForms = createPdfForms ? "true" : "false";

            return this;
        }

        /// <summary>
        /// Sets HTTP Auth username used to load the content
        /// </summary>
        /// <param name="username"></param>
        /// <returns>config object</returns>
        public ObjectSettings SetHttpUsername(string username)
        {
            this._loadUsername = username;

            return this;
        }

        /// <summary>
        /// Sets HTTP Auth password used to load the content
        /// </summary>
        /// <param name="password"></param>
        /// <returns>config object</returns>
        public ObjectSettings SetHttpPassword(string password)
        {
            this._loadPassword = password;

            return this;
        }

        /// <summary>
        /// Sets HTTP Auth parameters used to load the content
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>config object</returns>
        public ObjectSettings SetHttpAuth(string username, string password)
        {
            return this.SetHttpUsername(username).SetHttpPassword(password);
        }

        /// <summary>
        /// Sets render delay for the page. This timeout allows javascripts on the page to finish building the page.
        /// </summary>
        /// <param name="time">time of the delay in milliseconds</param>
        /// <returns>config object</returns>
        public ObjectSettings SetRenderDelay(int time)
        {
            this._loadJsDelay = time.ToString();

            return this;
        }

        public ObjectSettings SetZoomFactor(double factor)
        {
            this._loadZoomFactor = factor.ToString("0.##", CultureInfo.InvariantCulture);

            return this;
        }

        /// <summary>
        /// Specifies whether custom headers are sent only for the main page or for all the content we're requesting with the page.
        /// </summary>
        /// <param name="repeatCustomHeaders"></param>
        /// <returns>config object</returns>
        [Obsolete("Custom header functionality is not yet implemented in C bindings for the lib.")]
        public ObjectSettings SetRepeatCustomHeaders(bool repeatCustomHeaders)
        {
            this._loadRepeatCustomHeaders = repeatCustomHeaders ? "true" : "false";

            return this;
        }

        /// <summary>
        /// Specifies whether file:/// urls are allowed.
        /// </summary>
        /// <param name="allowLocalContent"></param>
        /// <returns>config object</returns>
        public ObjectSettings SetAllowLocalContent(bool allowLocalContent)
        {
            this._loadBlockLocalFileAccess = allowLocalContent ? "false" : "true";

            return this;
        }

        [Obsolete("Slow scripts are terminated regardless of this setting")]
        public ObjectSettings SetSlowScriptTermination(bool terminateSlowScripts)
        {
            this._loadStopSlowScript = terminateSlowScripts ? "true" : "false";

            return this;
        }

        /// <summary>
        /// Allows to activate Javascript debug mode. In that mode all JS errors and warnings are sent to the Warning event handler.
        /// </summary>
        /// <param name="debugJavascript"></param>
        /// <returns>config object</returns>
        public ObjectSettings SetJavascriptDebugMode(bool debugJavascript)
        {
            this._loadDebugJavascript = debugJavascript ? "true" : "false";

            return this;
        }

        public enum ContentErrorHandlingType
        {
            Abort,
            Skip,
            Ignore
        }

        /// <summary>
        /// Sets the content error handling policy. Abort stops converson on any error, Skip omits content with errors from output, Ignore tries to process errorneous content anyway.
        /// </summary>
        /// <param name="type">content error policy</param>
        /// <returns>config object</returns>
        public ObjectSettings SetErrorHandlingType(ContentErrorHandlingType type)
        {
            switch (type)
            {
                case ContentErrorHandlingType.Abort:
                    this._loadErrorHandling = "abort";
                    break;
                case ContentErrorHandlingType.Skip:
                    this._loadErrorHandling = "skip";
                    break;
                case ContentErrorHandlingType.Ignore:
                    this._loadErrorHandling = "ignore";
                    break;
            }

            return this;
        }

        /// <summary>
        /// Sets proxy string that specifies the proxy to run any loading operation through.
        /// 
        /// Protocol can be either "http" or "socks5". Refer to the http://madalgo.au.dk/~jakobt/wkhtmltoxdoc/wkhtmltopdf-0.9.9-doc.html for more details
        /// </summary>
        /// <param name="proxyString">String desribing a proxy in the format protocol://username:password@host:port</param>
        /// <returns>config object</returns>
        public ObjectSettings SetProxyString(string proxyString)
        {
            this._loadProxy = proxyString;

            return this;
        }

        public ObjectSettings SetPrintBackground(bool printBackground)
        {
            this._webPrintBackground = printBackground ? "true" : "false";

            return this;
        }

        public ObjectSettings SetLoadImages(bool loadImages)
        {
            this._webLoadImages = loadImages ? "true" : "false";

            return this;
        }

        public ObjectSettings SetRunJavascript(bool enableJavascript)
        {
            this._webRunJavascript = enableJavascript ? "true" : "false";

            return this;
        }

        /// <summary>
        /// Allows to enable intelligent shrinking. This feature allows to fit the page content onto the paper by width.
        /// </summary>
        /// <param name="enableIntelligentShrinking"></param>
        /// <returns>config object</returns>
        public ObjectSettings SetIntelligentShrinking(bool enableIntelligentShrinking)
        {
            this._webIntelligentShrinking = enableIntelligentShrinking ? "true" : "false";

            return this;
        }

        /// <summary>
        /// Sets minimum font size.
        /// </summary>
        /// <param name="minSize">size in pt</param>
        /// <returns>config object</returns>
        public ObjectSettings SetMinFontSize(double minSize)
        {
            this._webMinFontSize = minSize.ToString("0.##", CultureInfo.InvariantCulture);

            return this;
        }

        /// <summary>
        /// If set, converter uses "screen" media type instead of "print".
        /// </summary>
        /// <param name="useScreenMediaType"></param>
        /// <returns>config object</returns>
        public ObjectSettings SetScreenMediaType(bool useScreenMediaType)
        {
            this._webPrintMediaType = useScreenMediaType ? "false" : "true";

            return this;
        }

        /// <summary>
        /// Sets the fallback encoding for the conten that is used when no encoding is specified explicitly. Default is UTF-8.
        /// </summary>
        /// <param name="enc">fallback encoding</param>
        /// <returns>config object</returns>
        public ObjectSettings SetFallbackEncoding(Encoding enc)
        {
            this._webDefaultEncoding = enc.EncodingName;

            return this;
        }

        /// <summary>
        /// Sets the user stylesheet URL or filename.
        /// </summary>
        /// <param name="userStylesheet"></param>
        /// <returns>config object</returns>
        public ObjectSettings SetUserStylesheetUri(string userStylesheet)
        {
            this._webUserStylesheetUri = userStylesheet;

            return this;
        }

        [Obsolete("Documentation is very vague about what these plugins are and they have no meaning for us.")]
        public ObjectSettings SetEnablePlugins(bool enablePlugins)
        {
            this._webEnablePlugins = enablePlugins ? "true" : "false";

            return this;
        }

        internal void SetUpObjectConfig(IntPtr config)
        {
            Tracer.Trace(String.Format("T:{0} Setting up object config (many wkhtmltopdf_set_object_setting)", Thread.CurrentThread.Name));

            if (this._tocUseDottedLines != null)
            {
                PechkinStatic.SetObjectSetting(config, "toc.useDottedLines", this._tocUseDottedLines);
            }
            if (this._tocCaption != null)
            {
                PechkinStatic.SetObjectSetting(config, "toc.captionText", this._tocCaption);
            }
            if (this._tocCreateLinks != null)
            {
                PechkinStatic.SetObjectSetting(config, "toc.forwardLinks", this._tocCreateLinks);
            }
            if (this._tocBackLinks != null)
            {
                PechkinStatic.SetObjectSetting(config, "toc.backLinks", this._tocBackLinks);
            }
            if (this._tocIndentation != null)
            {
                PechkinStatic.SetObjectSetting(config, "toc.indentation", this._tocIndentation);
            }
            if (this._tocFontScale != null)
            {
                PechkinStatic.SetObjectSetting(config, "toc.fontScale", this._tocFontScale);
            }
            if (this._createToc != null)
            {
                PechkinStatic.SetObjectSetting(config, "isTableOfContent", this._createToc);
            }
            if (this._includeInOutline != null)
            {
                PechkinStatic.SetObjectSetting(config, "includeInOutline", this._includeInOutline);
            }
            if (this._pagesCount != null)
            {
                PechkinStatic.SetObjectSetting(config, "pagesCount", this._pagesCount);
            }
            if (this._tocXsl != null)
            {
                PechkinStatic.SetObjectSetting(config, "tocXsl", this._tocXsl);
            }
            if (this._pageUri != null)
            {
                PechkinStatic.SetObjectSetting(config, "page", this._pageUri);
            }
            if (this._useExternalLinks != null)
            {
                PechkinStatic.SetObjectSetting(config, "useExternalLinks", this._useExternalLinks);
            }
            if (this._useLocalLinks != null)
            {
                PechkinStatic.SetObjectSetting(config, "useLocalLinks", this._useLocalLinks);
            }
            if (this._produceForms != null)
            {
                PechkinStatic.SetObjectSetting(config, "produceForms", this._produceForms);
            }
            if (this._loadUsername != null)
            {
                PechkinStatic.SetObjectSetting(config, "load.username", this._loadUsername);
            }
            if (this._loadPassword != null)
            {
                PechkinStatic.SetObjectSetting(config, "load.password", this._loadPassword);
            }
            if (this._loadJsDelay != null)
            {
                PechkinStatic.SetObjectSetting(config, "load.jsdelay", this._loadJsDelay);
            }
            if (this._loadZoomFactor != null)
            {
                PechkinStatic.SetObjectSetting(config, "load.zoomFactor", this._loadZoomFactor);
            }
            if (this._loadRepeatCustomHeaders != null)
            {
                PechkinStatic.SetObjectSetting(config, "load.repertCustomHeaders", this._loadRepeatCustomHeaders);
            }
            if (this._loadBlockLocalFileAccess != null)
            {
                PechkinStatic.SetObjectSetting(config, "load.blockLocalFileAccess", this._loadBlockLocalFileAccess);
            }
            if (this._loadStopSlowScript != null)
            {
                PechkinStatic.SetObjectSetting(config, "load.stopSlowScript", this._loadStopSlowScript);
            }
            if (this._loadDebugJavascript != null)
            {
                PechkinStatic.SetObjectSetting(config, "load.debugJavascript", this._loadDebugJavascript);
            }
            if (this._loadErrorHandling != null)
            {
                PechkinStatic.SetObjectSetting(config, "load.loadErrorHandling", this._loadErrorHandling);
            }
            if (this._loadProxy != null)
            {
                PechkinStatic.SetObjectSetting(config, "load.proxy", this._loadProxy);
            }
            if (this._webPrintBackground != null)
            {
                PechkinStatic.SetObjectSetting(config, "web.background", this._webPrintBackground);
            }
            if (this._webLoadImages != null)
            {
                PechkinStatic.SetObjectSetting(config, "web.loadImages", this._webLoadImages);
            }
            if (this._webRunJavascript != null)
            {
                PechkinStatic.SetObjectSetting(config, "web.enableJavascript", this._webRunJavascript);
            }
            if (this._webIntelligentShrinking != null)
            {
                PechkinStatic.SetObjectSetting(config, "web.enableIntelligentShrinking", this._webIntelligentShrinking);
            }
            if (this._webMinFontSize != null)
            {
                PechkinStatic.SetObjectSetting(config, "web.minimumFontSize", this._webMinFontSize);
            }
            if (this._webPrintMediaType != null)
            {
                PechkinStatic.SetObjectSetting(config, "web.printMediaType", this._webPrintMediaType);
            }
            if (this._webDefaultEncoding != null)
            {
                PechkinStatic.SetObjectSetting(config, "web.defaultEncoding", this._webDefaultEncoding);
            }
            if (this._webUserStylesheetUri != null)
            {
                PechkinStatic.SetObjectSetting(config, "web.userStyleSheet", this._webUserStylesheetUri);
            }
            if (this._webEnablePlugins != null)
            {
                PechkinStatic.SetObjectSetting(config, "web.enablePlugins", this._webEnablePlugins);
            }

            this.Header.SetUpObjectConfig(config);
            this.Footer.SetUpObjectConfig(config);
        }

        internal IntPtr CreateObjectConfig()
        {
            IntPtr config = PechkinStatic.CreateObjectSettings();

            this.SetUpObjectConfig(config);

            return config;
        }
    }
}