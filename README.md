# Table of Contents

- [Table of Contents](#table-of-contents)
  - [Native AOT in .NET 7](#native-aot-in-net-7)
  - [Prerequisites](#prerequisites)
    - [Windows](#windows)
  - [Demo](#demo)
    - [Create a Console Application](#create-a-console-application)
    - [Create a new Console Application for testing Native AOT](#create-a-new-console-application-for-testing-native-aot)
      - [Copy the code from the first application's Program.cs to the new application](#copy-the-code-from-the-first-applications-programcs-to-the-new-application)
      - [Enable Native AOT](#enable-native-aot)
        - [Add NuGet Package Source](#add-nuget-package-source)
        - [Results](#results)
          - [Execution Time](#execution-time)
          - [Memory Utilization](#memory-utilization)
          - [Disk Utilization](#disk-utilization)
        - [Other Findings](#other-findings)
  - [Complete Code](#complete-code)
  - [Resources](#resources)

## Native AOT in .NET 7

Native AOT (Ahead of Time) is a group of technologies that help you build faster and lighter applications, by generating code at build time rather than at runtime, for .NET desktop client and server scenarios.

Native AOT is similar the other .NET AOT technologies, but it only produces native components.

You can see benefits in the following areas:

- Startup time
- Memory usage
- Size on disk
- Access to no JIT platforms like iOS.

Other similar technologies that already exist are [Mono AOT](https://www.mono-project.com/docs/advanced/aot/) for Mobile and WASM scenarios, and [ReadyToRun](https://docs.microsoft.com/en-us/dotnet/core/deploying/ready-to-run) for client and server scenarios.

## Prerequisites

In order to take advantage of Native AOT the following prerequisites need to be installed.

### Windows

[.NET 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

[Visual Studio 2022 Preview](https://visualstudio.microsoft.com/vs/community/)

Enable Desktop development with C++ workload

![Desktop development with C++ workload](images/2ef3f87ed115b974660b662cc68bf46006f2698573ef72d9ae87a2cabd164bb3.png)  

## Demo

The following demo is a Console Application that captures the time to generate any given numbers using the Fibonacci sequence with and without Native AOT enabled.

### Create a Console Application

![Create a new project](images/de393b6128c205b133e28e079da6c74cf711f5c81c6be481e89981b8459c96d9.png)  

![Configure your new project](images/0a2fac4cbfbcff37a5a092fb76475d43aef33da134a19f82bbcd5a19979861a1.png)  

![Target .NET 7 (Preview)](images/57b48250251db866f4800b1cae4568280f5803e68fa53b48353f6dcee446a329.png)  

Remove the default code in Program.cs

```csharp
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
```

Add code to Generate the Fibonacci Sequence.

```csharp
var numbersInSequence = 45;
Fibonacci(0, 1, 1, numbersInSequence);

static void Fibonacci(int firstNumber, int secondNumber, int numbersProcessed, int numbersInSequence)
{
    Console.Write($"{firstNumber}{(numbersProcessed < numbersInSequence ? ", " : string.Empty)}");
    if (numbersProcessed < numbersInSequence)
    {
        Fibonacci(secondNumber, firstNumber + secondNumber, numbersProcessed + 1, numbersInSequence);
    }
}
```

Add code to display execution time. (Just the code outside of the Fibonacci method.)

```csharp
using System.Diagnostics;

var numbersInSequence = 45;
var stopWatch = new Stopwatch();

stopWatch.Start();
Fibonacci(0, 1, 1, numbersInSequence);
stopWatch.Stop();

Console.WriteLine($"\n\nTotal Time elapsed: {stopWatch.ElapsedMilliseconds} milliseconds.");
Console.ReadLine();
```

Output for 45 numbers:

![Output for 45 numbers](images/09cddf6a42f86ae39aea4da17ab3efd09641980798a2c9e35591fcf4356729e6.png)  

Add code to run the Fibonacci series multiple times. (Just the code outside of the Fibonacci method.)

```csharp
using System.Diagnostics;

var numbersInSequence = 45;
var executions = 1000;
var stopWatch = new Stopwatch();

stopWatch.Start();

for (int i = 1; i <= executions; i++)
{
    Fibonacci(0, 1, 1, numbersInSequence);
}

stopWatch.Stop();

Console.WriteLine($"\n\nTotal Time elapsed for {executions} executions: {stopWatch.ElapsedMilliseconds} milliseconds.");
Console.ReadLine();

stopWatch.Reset();
```

Output for 45 numbers 1000 times:

![Output for 45 numbers 1000 times](images/00c0803858953229dd6645c746734d62ebcae6731ab344467687bcefc1c228ae.png)  

> [!TIP]
> Outputting the series into the console adds an overhead because screen drawing. Below I added a flag to just display the time elapsed outputs.

Add code to control whether to display or not the Fibonacci series.

```csharp

```

Add code to run multiple times. (Just the code outside of the Fibonacci method.)

```csharp
using System.Diagnostics;

var numbersInSequence = 45;
var executions = 1000;
var stopWatch = new Stopwatch();
var response = string.Empty;

do
{
    stopWatch.Start();

    for (int i = 1; i <= executions; i++)
    {
        Fibonacci(0, 1, 1, numbersInSequence);
    }

    stopWatch.Stop();

    Console.WriteLine($"\n\nTotal Time elapsed for {executions} executions: {stopWatch.ElapsedMilliseconds} milliseconds.");
    Console.WriteLine("Hit enter to run away or N to exit.");

    stopWatch.Reset();

    response = Console.ReadLine();
} while (response?.ToUpper() != "N");
```

### Create a new Console Application for testing Native AOT

#### Copy the code from the first application's Program.cs to the new application

#### Enable Native AOT

In order to enable Native AOT we need to add a reference to the ILCompiler NuGet package, in order to use the Native AOT Compiler and runtime. Since is still in Preview, we first need to add a new NuGet Package source.

##### Add NuGet Package Source

.NET 7 NuGet Package Source

```http
https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet7/nuget/v3/index.json
```

![.NET 7 NuGet Package Source](images/c2ecde9f6267d5816d26c97ddce974eb4dce44137c8018725b486f1ff5f8da20.png)  

Add a reference to the ILCompiler NuGet package.

![ILCompiler NuGet package](images/e561dd6590c2bea58ce8b8b7a31579a96e662d840073f7e5357f439fc2e9567f.png)  

> [!TIP]
> In the Manage NuGet Packages screen, make sure All or .NET 7 Preview under Package Source is selected, as well as the Include Prerelease checkbox is checked.

![.NET 7 Preview ](images/d6831415186e7c0221fbf8ed694e731a771d9adde095e13b2c8c2afda83ff021.png)  

Create a Publishing Profile

![Create a Publishing Profile](images/86b6582ae8761efe40ce8b1e18cffb841434a9f1e5e92c71d283447f0ef5ddc8.png)  

![Target](images/ad1c7671bc2ed35438d236958be16ac00d0583eca805640e0607477553f6600e.png)  

![Specific Target](images/d7ca1d2960be8e331a7e2999ecbdd3ec1d316ef18c4bfe4c35aef8f85b73bb8e.png)  

![Location](images/5d19344eb807a3af6d8387023871200e43c40b1af49bf3d5ad2432337be918ab.png)  

![Creation Progress](images/afdb9162f812bb75ef4a060ecfc61b2528e04dc97dc941128d266e255d5ff00b.png)  

![Publishing Profile](images/df0cfa0e60db05b3dc35df0ffacf23d235790996d6d5a262aa2a9c0d90c73f60.png)  

Click on Show all settings and change the Target runtime to your desired runtime, in this case I changed it to win-x64

![Show all settings](images/83d54865100a3e1f60423f83e7af9ce1691ec57cfbedc60bb16f85c4c453c371.png)  

> [!TIP]
> If you expand the File publish options, ReadyToRun can be set there.

![ReadyToRun](images/ea162118b22d8c48693c48875dc0e5fc71010e9664a6151443037bcd90ef7ac9.png)  

Click Save to close Profile settings and Publish to publish the application.

![Save and Publish](images/a7ec10a56ebe3de6d851199465a1213c607ecabdcfb4db383f4816a911d9972e.png)  

> [!IMPORTANT]
> When building with Native AOT You want to see the ILCompiler is being used,

![ILCompiler](images/447944a0325d7cd486f681585bfcae1a672f11c2424d7d7e935d2d41dd41012a.png)  

as opposed of the Roslyn compiler.

![Roslyn](images/930779076c1ad8d9c8cb885da4172c00e4fd6a72c8e2dab1d7a11509bafa0494.png)  

##### Results

The results obviously vary depending on multiple factors, such as Memory utilization, Garbage Collection, etc. but overall the results were consistent.

Memory seem to benefit in about 50%
File size was about 30 times greater using Native AOT.
Execution time did benefit for about 50 milliseconds running 1000 sequences, and about 650 milliseconds running 10000 sequences.

###### Execution Time

- Without Native AOT

  - Total Time elapsed for 1000 executions: 742 milliseconds.
  - Total Time elapsed for 10000 executions: 7865 milliseconds.

- With Native AOT

  - Total Time elapsed for 1000 executions: 686 milliseconds.
  - Total Time elapsed for 10000 executions: 7207 milliseconds.

###### Memory Utilization

![Memory Utilization](images/7ebe3f7a65f0314b1d8a9822edf9d646c861f3bca77774ab808efc6ebed22e2e.png)  

###### Disk Utilization

- Without Native AOT

  ![Disk Utilization Without Native AOT](images/4a7ed5237c54283c2ad34a32a3c9073801953030b3af285e2afb4c4ecde9dc92.png)  

- With Native AOT

  ![Disk Utilization With Native AOT](images/3ef9f3acfd56732dc4c768c73dfc048f2fffc164248ab5ef13a19f4796883df3.png)  

##### Other Findings

After doing some digging around, I found some useful information.

> [!IMPORTANT]
> The reason why the Desktop development with C++ workload is a prerequisite, is because of the Link process,

![C++ workload](images/24a07953e2ee52e8533c1ecf316623a610f20c0dc1d9a14b916535328ec6f7ce.png)  

which come with the C++ Build Tools

![C++ Build Tools](images/046187d6a983ad3b7c9134968fea0ae62cb6a6f3dfabce6ac89307955f9118f9.png)  

> [!IMPORTANT]
> The location of the Native files used for building the application can be found in the obj folder.

![Native files in obj](images/5877c9e83f700fb2909f77f0496f97228ae374c1575a2a529abed1913892ddd7.png)  

> [!IMPORTANT]
> The location of the Native files once built can be found in the bin folder.

![Native files in bin](images/1e56c873c5ec79050eac7c39109984aca0b5bc10245ebe635455e357f4d4cb94.png)  

> [!NOTE]
> Building the application in Linux or macOS would generate Native files for each platform.

## Complete Code

- <https://github.com/payini/NativeAOT>

## Resources

|Resource Title                 |Url                                                     |
|-------------------------------|--------------------------------------------------------|
|Download .NET 7.0              |<https://dotnet.microsoft.com/en-us/download/dotnet/7.0>|
|Visual Studio 2022 Preview     |<https://visualstudio.microsoft.com/vs/community/>|
|Compiling with Native AOT      |<https://github.com/dotnet/runtime/blob/main/src/coreclr/nativeaot/docs/compiling.md>|
|Native AOT Pre-requisites      |<https://github.com/dotnet/runtime/blob/main/src/coreclr/nativeaot/docs/prerequisites.md>|
|ReadyToRun                     |<https://docs.microsoft.com/en-us/dotnet/core/deploying/ready-to-run>|
|Mono AOT                       |<https://www.mono-project.com/docs/advanced/aot/>|
