using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;  

class Program
{
    static void PartialSum(int[] array, int start, int end, int threadNum, ref long partialResult, object lockObj)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        long sum = 0;
        for (int i = start; i < end; i++)
        {
            sum += array[i];
        }


        stopwatch.Stop();

        Console.WriteLine($"Thread {threadNum} calculated sum: {sum} in {stopwatch.Elapsed.TotalMilliseconds} ms");

        lock (lockObj)
        {
            partialResult += sum;
        }
    }

    static void Main()
    {
        int arraySize = 500000;
        int numThreads = 11;  

        int[] array = Enumerable.Repeat(1, arraySize).ToArray(); 


        long totalSum = 0;

        object lockObj = new object();

        int partSize = arraySize / numThreads;

        List<Thread> threads = new List<Thread>();

        Stopwatch totalStopwatch = new Stopwatch();
        totalStopwatch.Start();

        for (int i = 0; i < numThreads; i++)
        {
            int start = i * partSize;
            int end = (i == numThreads - 1) ? arraySize : start + partSize;
            int threadNum = i + 1;

            Thread thread = new Thread(() => PartialSum(array, start, end, threadNum, ref totalSum, lockObj));
            threads.Add(thread);
            thread.Start();
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        totalStopwatch.Stop();

        Console.WriteLine($"Total sum: {totalSum}");
       
    }
}
