#TuesPechkin
.NET Wrapper for [WkHtmlToPdf](http://github.com/antialize/wkhtmltopdf) DLL, a library that uses the Webkit engine to convert HTML pages to PDF. This fork supports .NET 2.0 and up and *now runs in both 64 and 32-bit environments*!

TuesPechkin is available as a *NuGet package* (see: https://www.nuget.org/packages/TuesPechkin/) for your convenience.



## Things to know



### Reporting issues
If you experience any problems using TuesPechkin, please be sure to create the issues on the TuesPechkin repository and not the original Pechkin repository. I am so grateful to gmanny for his contribution but at this point I see my fork evolving further away from the original and I would like to continue to support it for us.



### Windows Azure usage
Azure does not play nice with wkhtmltox.dll because it uses the GDI libraries. Chances are it will not work for you. Any opened issues in regards to this will be closed.

See: http://social.msdn.microsoft.com/Forums/windowsazure/en-US/eb48e701-8c0b-4be3-b694-2e11cc6ff2e1/wkhtmltopdf-in-windows-azure?forum=windowsazurewebsitespreview



### wkhtmltox.dll 
The unmanaged DLLs that TuesPechkin depends upon have been packaged as *embedded resources* so you don't have to worry about messing around with pre- or post-build events to copy the files wherever they go in your solution. When the library is first accessed in the application lifetime, it will copy the embedded resources to a temporary directory named after the version of TuesPechkin and the base directory from which your application is running (if they do not exist there already.)



### Release notes

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
- You better just read the 'Usage' section for a glance at how the API has changed.

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
		PaperSize = PaperKinds.A4 // Implicit conversion to PechkinPaperSize
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
byte[] result = pechkin.Convert(document);
```

License
-------

This work, "TuesPechkin", is a derivative of "Pechkin" by gmanny (Slava Kolobaev) used under the Creative Commons Attribution 3.0 license. This work is made available under the terms of the Creative Commons Attribution 3.0 license (viewable at http://creativecommons.org/licenses/by/3.0/) by tuespetre (Derek Gray.)
