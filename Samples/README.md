![image](http://www.ark-energy.eu/wp-content/uploads/ark-dark.png)
# Artesian.COMSample

This Project show how to write and use a wrapper for embed Artesian.SDK on Excel (COM Interop dll)

## Wrapper
For using Artesian.SDK as COM DLL to VBA, a creation of a WRAPPER class is needed.
The new class created should use public methods for calling the Artesian SDK.

To correctly configure the wrapper:

1) Use this attribute on each class that has to be visible from external
	```csharp
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDual)]
	```
	
2) Use the following on each method that has to be visible from external
	```csharp
	[ComVisible(true)]
	```
	
3) Compile the library (Debug or Release) to x86 or x64, depending of the version of installed Excel

### Errors
The Artesian.SDK needs assembly redirect to work correctly on internal dependencies. COM doesn't use this in a correct way: a solution could be strong-name binding.
Since Artesian.SDK is not strong-name actually, a solution has been found on Wrapper side: the implementation of ```AssemblyResolve``` to emulate the assembly redirect, 
in this way the DLL will point to the correct versions of internal dependencies. 


Look at the following code (*ArtesianCOMWrapper.cs*) for further details.

```csharp
static ArtesianCOMWrapperFactory()
{
	AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(_assemblyResolve);
	ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
}

static Assembly _assemblyResolve(object sender, ResolveEventArgs args)
{
	try
	{
		// *** Try to load by filename - split out the filename of the full assembly name
		// *** and append the base path of the original assembly (ie. look in the same dir)
		// *** NOTE: this doesn't account for special search paths but then that never
		//           worked before either.
		string[] Parts = args.Name.Split(',');
		string File = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Parts[0].Trim() + ".dll";

		return System.Reflection.Assembly.LoadFrom(File);
	}
	catch
	{
		// ignore load error 
	}

	Assembly assembly = System.Reflection.Assembly.Load(args.Name);
	return assembly;
}
}
```


## COM Registration
Now the created DLL has to be registered to be used on COM.

With **Command Prompt** (run as ADMINISTRATOR) go to one of the fllowing path, depending of the version (x86 or x64) of installed Excel

**x86:** ```C:\Windows\Microsoft.NET\Framework\v4.0.30319```

**x64:** ```C:\Windows\Microsoft.NET\Framework64\v4.0.30319```


On the path directory choosen, use the following command for:

**Register:** ```regasm.exe "DLL complete path" /codebase /tlb```

**Un-Register a type:** ```regasm.exe "DLL complete path" /tlb /unregister```


## Use DLL on Excel
At this point open Excel and VBA Editor: 

- Go to TOOLS -> REFERENCES to add the library to your project
- Start writing code that use the Wrapper class, follow this example as guideline


```vbnet
Sub TestSubRoutine()
Dim sheet As Worksheet

Dim url As String
url = "Url link to Artesian"

Dim apiKey As String
apiKey = "your APIKEY"

Dim factory As New ArtesianCOMWrapperFactory
Dim client As ArtesianCOMWrapper
Set client = factory.Create(url:=url, apiKey:=apiKey)

Dim ids As String
ids = "100147286;100147284;100147282"

Dim startDate As String
startDate = "2019-11-11"

Dim endDate As String
endDate = "2019-11-12"

Dim resultArray() As ActualTimeSerieRow
resultArray = client.GetActuals(csvMarketDataId:=ids, start:=startDate, End:=endDate)

Set sheet = ActiveSheet
    sheet.Cells(1, 1) = "ProviderName"
    sheet.Cells(1, 2) = "CurveName"
    sheet.Cells(1, 3) = "TSID"
    sheet.Cells(1, 4) = "Time"
    sheet.Cells(1, 5) = "Value"
    sheet.Cells(1, 6) = "CompetenceStart"
    sheet.Cells(1, 7) = "CompetenceEnd"

Dim i As Integer
i = 1
For Each v In resultArray
    i = i + 1
    sheet.Cells(i, 1) = v.ProviderName
    sheet.Cells(i, 2) = v.CurveName
    sheet.Cells(i, 3) = v.TSID
    sheet.Cells(i, 4) = v.Time
    sheet.Cells(i, 5) = v.value
    sheet.Cells(i, 6) = v.CompetenceStart
    sheet.Cells(i, 7) = v.CompetenceEnd
Next

End Sub
```

**Notes:** VBA array on input function (```resultArray = client.GetActuals(csvMarketDataId:=ids, start:=startDate, End:=endDate)```) doesn't work so the idea is to pass multiple values in CSV format, separated by semicolon ( **;** ).
See ```ids``` variable for better understanding.