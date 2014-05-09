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

#### 0.9.3.2
- Made the library unpack wkhtmltopdf in a folder named specifically for the running application since only one process can use the dll.
- Compressed the wkhtmltopdf dependencies with gzip to reduce the size of the solution and the NuGet packages



## FAQ

#### Q: Why does the produced PDF lack background images and colors? ###

**A:** By default, all backgrounds will be omitted from the document (note: this is similar to how Google Chrome operates when printing to PDF.)

You can override this setting by calling `SetPrintBackground(true)` on the `ObjectConfig` supplied with the HTML document to the `Convert()` method of the converter.

#### Q: Do I need to install wkhtmltopdf on the machine for the library to work? ###

**A:** No. Version 0.12 of wkhtmltox.dll is embedded into the package and unpacked on library initialization.

#### Q: Why is my website or application locking up when using TuesPechkin?

**A:** If you do not dispose of the IPechkin object properly, the converter will not be properly 'cleaned up' and so the next time you create and run a converter, the wkhtmltox.dll library will hang up. Be sure to dispose of your IPechkin objects every time!



##Usage
Pechkin is both easy to use....

```csharp
String html = "<html><body><h1>Hello world!</h1></body></html>";

using (var converter = Factory.Create(new GlobalConfig()))
{
    byte[] pdfBuf = converter.Convert(html);
}
```

...and functional:

```csharp
// create global configuration object
GlobalConfig gc = new GlobalConfig();

// set it up using fluent notation
gc.SetMargins(new Margins(300, 100, 150, 100))
  .SetDocumentTitle("Test document")
  .SetPaperSize(PaperKind.Letter);
//... etc

// create converter
IPechkin pechkin = Factory.Create(gc);

// subscribe to events
pechkin.Begin += OnBegin;
pechkin.Error += OnError;
pechkin.Warning += OnWarning;
pechkin.PhaseChanged += OnPhase;
pechkin.ProgressChanged += OnProgress;
pechkin.Finished += OnFinished;

// create document configuration object
ObjectConfig oc = new ObjectConfig();

// and set it up using fluent notation too
oc.SetCreateExternalLinks(false)
  .SetFallbackEncoding(Encoding.ASCII)
  .SetLoadImages(false)
  .SetPageUri("http://google.com");
//... etc

// convert document
byte[] pdfBuf = pechkin.Convert(oc);

// Be sure to dispose! Preferably with a using statement.
pechkin.Dispose();
```

License
-------

This work, "TuesPechkin", is a derivative of "Pechkin" by gmanny (Slava Kolobaev) used under the Creative Commons Attribution 3.0 license. This work is made available under the terms of the Creative Commons Attribution 3.0 license (viewable at http://creativecommons.org/licenses/by/3.0/) by tuespetre (Derek Gray.)
