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

License
-------

This work, "TuesPechkin", is a derivative of "Pechkin" by gmanny (Slava Kolobaev) used under the Creative Commons Attribution 3.0 license. This work is made available under the terms of the Creative Commons Attribution 3.0 license (viewable at http://creativecommons.org/licenses/by/3.0/) by tuespetre (Derek Gray.)
