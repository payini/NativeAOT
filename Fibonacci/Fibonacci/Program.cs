using System.Diagnostics;

var numbersInSequence = 45;
var executions = 10000;
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
    Console.WriteLine("\nHit enter to run away or N to exit.");
    
    stopWatch.Reset();

    response = Console.ReadLine();
} while (response?.ToUpper() != "N");

static void Fibonacci(int firstNumber, int secondNumber, int numbersProcessed, int numbersInSequence)
{
    Console.Write($"{firstNumber}{(numbersProcessed < numbersInSequence ? ", " : string.Empty)}");
    if (numbersProcessed < numbersInSequence)
    {
        Fibonacci(secondNumber, firstNumber + secondNumber, numbersProcessed + 1, numbersInSequence);
    }
}