#TuesPechkin
TuesPechkin is a .NET Wrapper for the [wkhtmltopdf](https://github.com/wkhtmltopdf/wkhtmltopdf) library. 

## Things to know

### Supported usage
- It supports .NET 2.0+, 32 and 64-bit processes, and IIS-hosted applications. 
- [Azure Websites does not currently support the use of wkhtmltopdf.](http://social.msdn.microsoft.com/Forums/windowsazure/en-US/eb48e701-8c0b-4be3-b694-2e11cc6ff2e1/wkhtmltopdf-in-windows-azure?forum=windowsazurewebsitespreview)
- It is not tested with any operating systems besides Windows.
- [It is available as a *NuGet package* for your convenience.](https://www.nuget.org/packages/TuesPechkin/)
- It is built and tested around wkhtmltopdf 0.12.2.
- Even if you use the IIS-compatible method documented below, you may only use one converter/toolset instance per application pool/process. A workaround is being researched for a future version.

### wkhtmltox.dll 
The wkhtmltox.dll file and any dependencies it might have (for older versions, 0.11.0-) are not included in the TuesPechkin NuGet package; however, you can bring your own copy of the library or download one of the following NuGet packages that contain the library:

- TuesPechkin.Wkhtmltox.Win32
- TuesPechkin.Wkhtmltox.Win64

### Reporting issues
If something doesn't seem right with your converted document, try converting with [wkhtmltopdf](http://www.wkhtmltopdf.org) directly. If you still have the problem, then you will need to take your issue to [wkhtmltopdf's issues](https://github.com/wkhtmltopdf/wkhtmltopdf). Any issues related to visual problems like this will be closed unless the reporter can show that the problem is unique to this library.

Please follow similar guidelines as StackOverflow -- that is, provide any contextual information that you can about your application to help solve the issue.

### Submitting pull requests
For 2.0.0 I am wanting to use the 'git flow' style of branching/merging/releasing. If you have a hotfix, please branch off of hotfix. If you are adding something, please branch off of develop. I won't fully re-explain the methodology here.

## Usage
*The API drastically changed once more for 2.0.0 for the sake of modularity and extensibility. Please read the following sections thoroughly.*

### 1. Choose a deployment

TuesPechkin exposes an 'IDeployment' interface to represent the folder where wkhtmltox.dll resides. There exists a `StaticDeployment` implementation that accepts a string path, a `TempFolderDeployment` implementation that generates an application-scoped folder path under the `%Temp%` folder, and an abstract `EmbeddedDeployment` class that can be implemented to automatically deploy the wkhtmltopdf dll(s) wherever you need them. 

### 2. Choose a toolset

TuesPechkin exposes an `IToolset` interface to represent the wkhtmltopdf library. There are two officially supported implementations:

- `PdfToolset`: Exposes operations from the library to convert HTML into PDF
- `RemotingToolset<TToolset>`: Manages a toolset of type `TToolset` across an AppDomain boundary. This is necessary for use in IIS-hosted applications.

There is also an abstract class from which you may inherit: `NestingToolset`. It provides wrapping functionality that is used by `RemotingToolset<TToolset>`.

Feel free to submit a pull request to the develop branch for a `ImageToolset` implementation! ;)

### 3. Choose a converter

TuesPechkin exposes an `IConverter` interface. An implementation of `IConverter` properly makes all of the calls to wkhtmltopdf to convert an `IDocument` instance (which we will cover shortly.) TuesPechkin supplies two implementations:

- `StandardConverter`: A converter that may be used in single-threaded applications
- `ThreadSafeConverter`: A converter that manages a single background thread and queues document conversions against it. This is necessary for use in multi-threaded applications, including IIS-hosted applications.

### 4. Define your document

TuesPechkin exposes three interfaces and one attribute that define an HTML document. These are:

- `WkhtmltoxSettingAttribute`: an attribute for properties that instructs an `IConverter` to apply the property's value to the wkhtmltopdf global or object setting with the name passed to the attribute's constructor.
- `ISettings`: a token interface whose implementors are implied to have properties decorated with `WkhtmltoxSettingAttribute` and/or other `ISettings` properties.
- `IObject`: an interface that represents a wkhtmltopdf 'ObjectSettings'. It requires one method to be implemented: `byte[] GetData()`. All `IObject` instances are also `ISettings` instances by inheritance.
- `IDocument`: an interface that represents an HTML document. It requires one method to be implemented: `IEnumerable<IObject> GetObjects()`. All `IDocument` instances are also `ISettings` instances by inheritance.

Because TuesPechkin exposes these interfaces/attributes, you are free to write your own implementations that support whichever wkhtmltopdf settings you so desire. If TuesPechkin's included `HtmlDocument` class and its related classes do not provide support for a setting you want to use, you may then extend them or create your own classes altogether -- this also goes for use cases where you are only setting a handful of properties and you find the included implementations to be too verbose.

*The included `HtmlToPdfDocument` class and its related classes do not supply any default values to wkhtmltopdf.*

Here is how an `IDocument` is to be processed by an `IConverter`:
 
1. A wkhtmltopdf 'GlobalSettings' is created for the document. 
2. The `IDocument` is recursively crawled for all `ISettings` and `WkhtmltoxSettingAttribute`-decorated properties; these properties are applied to the 'GlobalSettings'.
3. 'GetObjects()' is called on 'IDocument', and for each 'IObject' that is not null, a wkhtmltopdf 'ObjectSettings' is created and that 'IObject' is recursively crawled for all `ISettings` and `WkhtmltoxSettingAttribute`-decorated properties; these properties are applied to the 'ObjectSettings'. The 'ObjectSettings' is then added to the converter.

### 5. Putting it all together

#### Create a document with options of your choosing.
```csharp
var document = new HtmlToPdfDocument
{
    GlobalSettings =
    {
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
```

#### Convert it in a quick and dirty console application...
```csharp    
IConverter converter =
    new StandardConverter(
        new PdfToolset(
            new Win32EmbeddedDeployment(
                new TempFolderDeployment())));

byte[] result = converter.convert(document);
```

### ...or in a multi-threaded application...
```csharp
IConverter converter =
    new ThreadSafeConverter(
        new PdfToolset(
            new Win32EmbeddedDeployment(
                new TempFolderDeployment())));

// Keep the converter somewhere static, or as a singleton instance!

byte[] result = converter.convert(document);
```

### ...or in an IIS-hosted application.
```csharp
IConverter converter =
    new ThreadSafeConverter(
        new RemotingToolset<PdfToolset>(
            new Win32EmbeddedDeployment(
                new TempFolderDeployment())));

// Keep the converter somewhere static, or as a singleton instance!

byte[] result = converter.convert(document);
```

### Use the embedded library from the TuesPechkin.Wkhtmltox.Win64 NuGet package instead.
```csharp
IConverter converter =
    new StandardConverter(
        new PdfToolset(
            new Win64EmbeddedDeployment(
                new TempFolderDeployment())));

byte[] result = converter.convert(document);
```

License
-------

This work, "TuesPechkin", is a derivative of "Pechkin" by gmanny (Slava Kolobaev) used under the Creative Commons Attribution 3.0 license. This work is made available under the terms of the Creative Commons Attribution 3.0 license (viewable at http://creativecommons.org/licenses/by/3.0/) by tuespetre (Derek Gray.)
