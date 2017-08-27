# SwissEphNet.Samples

Samples of using the [SwissEphNet library](https://github.com/ygrenier/SwissEphNet) in
different types of .Net projects in particular the loading file
mecanism.

All this projects use the `SwissEphNet.Samples.Shared`project 
that contains the test logic based on the official [`SweWin`](https://github.com/ygrenier/SwissEphNet/tree/master/Programs/SweWin) 
project.

Each project implements the `ITestProvider` for print and
load file implementation.

## Project SwissEphNet.Samples.ConsoleNet40

Console application in .Net 4.0.

The data files are copied in a `datas` folder.

The `Net40TestProvider` read the data files from the folder.

