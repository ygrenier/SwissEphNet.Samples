# SwissEphNet.Samples

Samples of using the [SwissEphNet library](https://github.com/ygrenier/SwissEphNet) in
different types of .Net projects in particular the loading file
mecanism.

All this projects use the `SwissEphNet.Samples.Shared`project 
that contains the test logic based on the official [`SweWin`](https://github.com/ygrenier/SwissEphNet/tree/master/Programs/SweWin) 
project.

Each project implements the `ITestProvider` for print and
load file implementation.

The test use the `ITestProvider.LoadFile` for update the `LoadEventArgs` 
object:

```csharp
private void SwissEph_OnLoadFile(object sender, LoadFileEventArgs e)
{
    Encoding enc = e.Encoding;
    e.File = Provider.LoadFile(e.FileName, out enc);
    e.Encoding = enc;
    Provider.Debug.WriteLine($"Required file:{e.FileName} => {(e.File != null ? "OK" : "Not found")}");
}
```

## Async context

Unfortunally the SwissEphNet library is not async, and can't be it (too complex to implements).

Fortunally we have a solution: wait the result. But beause the async context can changed the
risk is to create an interlock freeze.

In general the solution is to change the async context by a code of this type:
```csharp
var file = Task.Run(async () => await LoadFileAsync(filename)).Result;
```

## Project SwissEphNet.Samples.ConsoleNet40

Console application in .Net 4.0.

The data files are copied in a `datas` folder.

The `Net40TestProvider` read the data files from the folder.

## Project SwissEphNet.Samples.ConsoleNet46

Console application in .Net 4.6.

Works like the SwissEphNet.Samples.ConsoleNet40 project.

## Project SwissEphNet.Samples.WinForms46

WinForms application in .Net 4.6.

This sample has 3 tests:

- **Run test** : Works like the console samples for the 
files using the `WinFormsTestProvider`.
- **Run test load async** : This test use the `WinFormsTestProviderAsync`
wich simulate an async loading file, but the call is sync
- **Run test async**: this test use the `WinFormsTestProviderAsync`
wich simulate an async loading file and the test is run in a task so in 
an async mode

The `WinFormsTestProviderAsync` make a pause on each loading file to
simulate a long time running and see the blocking or non-blocking of the main 
thread.

A progress bar is displayed to show the test running. In the 2 first cases
this progress bar don't move because the test run in the main thread. For
the last case, the progress bar is animated that indicates the main thread
is not blocked.

## Project SwissEphNet.Samples.WPF46

WPF application in .Net 4.6.

Works exactly like the WinForms sample.
