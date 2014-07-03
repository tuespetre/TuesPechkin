#TuesPechkin
.NET Wrapper for [WkHtmlToPdf](http://github.com/antialize/wkhtmltopdf) DLL, a library that uses the Webkit engine to convert HTML pages to PDF. This fork supports .NET 2.0 and up and *now runs in both 64 and 32-bit environments*!

TuesPechkin is available as a *NuGet package* (see: https://www.nuget.org/packages/TuesPechkin/) for your convenience.



## Things to know



### Reporting issues
If something looks visually off when you print your document, try converting with [wkhtmltopdf](http://www.wkhtmltopdf.org) directly. If you still have the problem, then you will need to take your issue to [wkhtmltopdf's issues](https://github.com/wkhtmltopdf/wkhtmltopdf). Any issues related to visual problems like this will be closed unless the reporter can show that the problem is unique to this library.

Since this library is maintained on limited resources, please bring the witch here to burn (so to speak), rather than declaring that a witch is out there and that someone needs to find it and burn it. It would be most helpful if you could provide steps to reproduce the issue, sample material (raw html, code, etc.), and environment information.



### Windows Azure usage
At the time of writing this, Azure does not play nice with wkhtmltox.dll because it uses the GDI libraries. Chances are it will not work for you. Any opened issues in regards to this will be closed.

See: http://social.msdn.microsoft.com/Forums/windowsazure/en-US/eb48e701-8c0b-4be3-b694-2e11cc6ff2e1/wkhtmltopdf-in-windows-azure?forum=windowsazurewebsitespreview



### wkhtmltox.dll 
The unmanaged DLLs that TuesPechkin depends upon have been packaged as *embedded resources* so you don't have to worry about messing around with pre- or post-build events to copy the files wherever they go in your solution. When the library is first accessed in the application lifetime, it will copy the embedded resources to a temporary directory named after the version of TuesPechkin and the base directory from which your application is running (if they do not exist there already.)



### Release notes

#### 1.0.3
- Global setting margins now uses culture-invariant formatting of double values
- Global setting color mode now works properly ('color' or 'grayscale')
- Several boolean values now work properly ('LoadImages', 'EnableJavascript', etc.)

#### 1.0.2
- Revert to process identity within the synchronized thread, see issue #14

#### 1.0.1
- Corrected an issue with the AppDomain hanging on unload; introduced unit test to cover this scenario

#### 1.0.0 - HUGE changes
- Began to use semantic versioning.
- Removed ```ExtendedQtAvailable``` and ```Version``` properties from ```Factory```.
- Setting ```UseX11Graphics``` on ```Factory``` will no longer throw an ```InvalidOperationException```.
- The initialization of the library is now thread-safe.
- Removed ```UseSynchronization``` property from ```Factory```. Synchronization is now mandatory.
- IPechkin no longer implements ```IDisposable``` or has property ```IsDisposed``` or event ```Disposed```. 
- Consequently, ```UseDynamicLoading``` is no longer an option for ```Factory```.
- ```SynchronizedDispatcherThread``` is no longer a public class available for reuse.
- Removed ```LibInitEventHandler```, ```LibDeInitEventHandler```, and ```DisposedEventHandler``` delegates.
- Improved exception handling within synchronized thread. Exceptions now bubble out
- Set 'PrintBackground' to 'true' by default
- GlobalConfig/ObjectConfig have been radically redesigned. See API and usage for details.
- Multiple objects in one conversion now supported! (Multiple web pages, HTML documents, etc in one PDF)
- Everything is in the 'TuesPechkin' namespace now (instead of 'Pechkin.')
- Margins for the document are in 'double + unit' form, rather than 'hundredths of an inch as int' form.

###### You better just read the 'Usage' section for a glance at how the API has changed.

#### 0.9.3.3
- Fixed a problem with concurrency related to when the Pechkin object tells wkhtmltopdf to destroy its converter object. Now happens immediately after conversion, before anything else hits the queue.

#### 0.9.3.2
- Made the library unpack wkhtmltopdf in a folder named specifically for the running application since only one process can use the dll.
- Compressed the wkhtmltopdf dependencies with gzip to reduce the size of the solution and the NuGet packages

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

This section spells out each settings class, a description for each possible setting, and the default values TuesPechkin and wkhtmltopdf uses. There are differences in the defaults which may affect rendering. The defaults may be normalized to match that of wkhtmltopdf in a future version.

#### WebSettings

Setting | Description | TuesPechkin default | wkhtmltopdf default
--------|-------------|---------------------|--------------------
DefaultEncoding|(string) Default encoding used to render the HTML|`"utf-8"`|`""`
EnableIntelligentShrinking|(bool) Whether or not to enable intelligent compression of content to fit in the page|`true`|`true`
EnableJavascript|Whether or not to enable JavaScript|`true`|`true`
EnablePlugins|Whether to enable plugins (maybe like Flash? unsure)|`false`|`false`
LoadImages|Whether or not to load images|`true`|`true`
MinimumFontSize|The minimum font size to use|`-1`|`-1`
PrintBackground|Whether or not to print the background on elements|`true`|`true`
PrintMediaType|Whether to print the content using the print media type instead of the screen media type|`true`|`false`
UserStyleSheet|Path to a user specified style sheet|`""`|`""`

License
-------

This work, "TuesPechkin", is a derivative of "Pechkin" by gmanny (Slava Kolobaev) used under the Creative Commons Attribution 3.0 license. This work is made available under the terms of the Creative Commons Attribution 3.0 license (viewable at http://creativecommons.org/licenses/by/3.0/) by tuespetre (Derek Gray.)
