#TuesPechkin
.NET Wrapper for [WkHtmlToPdf](http://github.com/antialize/wkhtmltopdf) DLL, a library that uses the Webkit engine to convert HTML pages to PDF. This fork supports .NET 2.0 and up and *now runs in both 64 and 32-bit environments*!

TuesPechkin is available as a *NuGet package* (see: https://www.nuget.org/packages/TuesPechkin/) for your convenience.



## Things to know



### Reporting issues
If something looks visually off when you print your document, try converting with [wkhtmltopdf](http://www.wkhtmltopdf.org) directly. If you still have the problem, then you will need to take your issue to [wkhtmltopdf's issues](https://github.com/wkhtmltopdf/wkhtmltopdf). Any issues related to visual problems like this will be closed unless the reporter can show that the problem is unique to this library.

Since this library is maintained on limited resources, please bring the witch here to burn (so to speak) -- it would be most helpful if you could provide steps to reproduce the issue, sample material (raw html, code, etc.), and environment information.



### Windows Azure usage
At the time of writing this, Azure Web Sites do not play nice with wkhtmltox.dll because it uses the GDI libraries (Worker Roles and Cloud Services seem to work, however.) Any opened issues in regards to this will be closed.

See: http://social.msdn.microsoft.com/Forums/windowsazure/en-US/eb48e701-8c0b-4be3-b694-2e11cc6ff2e1/wkhtmltopdf-in-windows-azure?forum=windowsazurewebsitespreview



### wkhtmltox.dll 
The unmanaged DLLs that TuesPechkin depends upon have been packaged as *embedded resources* so you don't have to worry about messing around with pre- or post-build events to copy the files wherever they go in your solution. When the library is first accessed in the application lifetime, it will copy the embedded resources to a temporary directory named after the version of TuesPechkin and the base directory from which your application is running (if they do not exist there already.)



### Release notes

#### 1.0.3
- Global setting margins now uses culture-invariant formatting of double values
- Global setting color mode now works properly ('color' or 'grayscale')
- Several boolean values now work properly ('LoadImages', 'EnableJavascript', etc.)

##### See the tagged releases for earlier release information.

##Usage

### Quickly create a document from some html:

```csharp
IPechkin converter = Factory.Create();
byte[] result = converter.Convert("<p>Lorem ipsum wampum</p>");
```

### Specify a number of options:

```csharp
// create a new document with your desired configuration
var document = new HtmlToPdfDocument
{
	GlobalSettings = {
        ProduceOutline = true,
        DocumentTitle = "Pretty Websites",
		PaperSize = PaperKind.A4, // Implicit conversion to PechkinPaperSize
        Margins =
        {
            All = 1.375,
            Unit = Unit.Centimeters
		}
	},
    Objects = {
        new ObjectSettings { HtmlText = "<h1>Pretty Websites</h1><p>This might take a bit to convert!</p>" },
        new ObjectSettings { PageUrl = "www.google.com" },
        new ObjectSettings { PageUrl = "www.microsoft.com" },
		new ObjectSettings { PageUrl = "www.github.com" }
    }
};

// create converter
IPechkin converter = Factory.Create();

// subscribe to events
converter.Begin += OnBegin;
converter.Error += OnError;
converter.Warning += OnWarning;
converter.PhaseChanged += OnPhase;
converter.ProgressChanged += OnProgress;
converter.Finished += OnFinished;

// convert document
byte[] result = converter.Convert(document);
```

## Options

### How to read this section

As of 1.1.0, TuesPechkin will not prescribe any default settings, so as to more closely imitate the command-line interface of wkhtmltopdf; however, this section will provide a convenient mapping of command-line arguments to TuesPechkin object-oriented settings for quick reference. For documentation of the settings themselves, it would be wise to refer to wkhtmltopdf's own documentation.

#### Global Options
Command line arg | Description | TuesPechkin equivalent
----|----|----
    --collate                         |Collate when printing multiple copies (default)|GlobalSettings.Collate
    --no-collate                      |Do not collate when printing multiple copies|GlobalSettings.Collate
    --cookie-jar <path>               |Read and write cookies from and to the supplied cookie jar file|GlobalSettings.CookieJar
    --copies <number>                 |Number of copies to print into the pdf file (default 1)|GlobalSettings.Copies
-d, --dpi <dpi>                       |Change the dpi explicitly (this has no effect on X11 based systems)|GlobalSettings.DPI
-H, --extended-help                   |Display more extensive help, detailing less common command switches|(no equivalent)
-g, --grayscale                       |PDF will be generated in grayscale|GlobalSettings.ColorMode
-h, --help                            |Display help|(no equivalent)
    --htmldoc                         |Output program html help|(no equivalent)
    --image-dpi * <integer>           |When embedding images scale them down to this dpi (default 600)|GlobalSettings.ImageDPI
    --image-quality * <integer>       |When jpeg compressing images use this quality (default 94)|GlobalSettings.ImageQuality
-l, --lowquality                      |Generates lower quality pdf/ps. Useful to shrink the result document space|(no equivalent)
    --manpage                         |Output program man page|(no equivalent)
-B, --margin-bottom <unitreal>        |Set the page bottom margin|GlobalSettings.MarginSettings.Bottom
-L, --margin-left <unitreal>          |Set the page left margin (default 10mm)|GlobalSettings.MarginSettings.Left
-R, --margin-right <unitreal>         |Set the page right margin (default 10mm)|GlobalSettings.MarginSettings.Right
-T, --margin-top <unitreal>           |Set the page top margin|GlobalSettings.MarginSettings.Top
-O, --orientation <orientation>       |Set orientation to Landscape or Portrait (default Portrait)|GlobalSettings.Orientation
    --output-format <format>          |Specify an output format to use pdf or ps, instead of looking at the extention of the output filename|GlobalSettings.OutputFormat
    --page-height <unitreal>          |Page height|GlobalSettings.PaperSize.Height
-s, --page-size <Size>                |Set paper size to: A4, Letter, etc. (default A4)|GlobalSettings.PaperSize (implicit conversion from PaperKind)
    --page-width <unitreal>           |Page width|GlobalSettings.PaperSize.Width
    --no-pdf-compression *            |Do not use lossless compression on pdf objects|GlobalSettings.UseCompression
-q, --quiet                           |Be less verbose|(no equivalent)
    --read-args-from-stdin            |Read command line arguments from stdin|(no equivalent)
    --readme                          |Output program readme|(no equivalent)
    --title <text>                    |The title of the generated pdf file (The title of the first document is used if not specified)|GlobalSettings.DocumentTitle
-V, --version                         |Output version information an exit|(no equivalent)

#### Header And Footer Options

Header and footer options are accessed via `ObjectSettings.HeaderSettings` and `ObjectSettings.FooterSettings`.

Command line arg | Description | TuesPechkin equivalent
----|----|----
    --footer-center * <text>          |Centered footer text|FooterSettings.CenterText
    --footer-font-name * <name>       |Set footer font name (default Arial)|FooterSettings.FontName
    --footer-font-size * <size>       |Set footer font size (default 12)|FooterSettings.FontSize
    --footer-html * <url>             |Adds a html footer|FooterSettings.HtmlUrl
    --footer-left * <text>            |Left aligned footer text|FooterSettings.LeftText
    --footer-line *                   |Display line above the footer|FooterSettings.UseLineSeparator
    --no-footer-line *                |Do not display line above the footer (default)|FooterSettings.UseLineSeparator
    --footer-right * <text>           |Right aligned footer text|FooterSettings.RightText
    --footer-spacing * <real>         |Spacing between footer and content in mm (default 0)|FooterSettings.ContentSpacing
    --header-center * <text>          |Centered header text|HeaderSettings.CenterText
    --header-font-name * <name>       |Set header font name (default Arial)|HeaderSettings.FontName
    --header-font-size * <size>       |Set header font size (default 12)|HeaderSettings.FontSize
    --header-html * <url>             |Adds a html header|HeaderSettings.HtmlUrl
    --header-left * <text>            |Left aligned header text|HeaderSettings.LeftText
    --header-line *                   |Display line below the header|HeaderSettings.UseLineSeparator
    --no-header-line *                |Do not display line below the header (default)|HeaderSettings.UseLineSeparator
    --header-right * <text>           |Right aligned header text|HeaderSettings.RightText
    --header-spacing * <real>         |Spacing between header and content in mm (default 0)|HeaderSettings.ContentSpacing
    --replace * <name> <value>        |Replace [name] with value in header and footer (repeatable)|(no equivalent)

License
-------

This work, "TuesPechkin", is a derivative of "Pechkin" by gmanny (Slava Kolobaev) used under the Creative Commons Attribution 3.0 license. This work is made available under the terms of the Creative Commons Attribution 3.0 license (viewable at http://creativecommons.org/licenses/by/3.0/) by tuespetre (Derek Gray.)
