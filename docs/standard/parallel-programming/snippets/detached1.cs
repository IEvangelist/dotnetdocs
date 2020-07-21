﻿using System;
using System.Threading;
using System.Threading.Tasks;

public class Example
{
    public static async Task Main()
    {
        Task task =
            Task.Factory.StartNew(
                () =>
                {
                    Console.WriteLine($"Running antecedent task {Task.CurrentId}...");
                    Console.WriteLine("Launching attached child tasks...");
                    for (int ctr = 1; ctr <= 5; ctr++)
                    {
                        int index = ctr;
                        Task.Factory.StartNew(
                            value =>
                            {
                                Console.WriteLine($"   Attached child task #{value} running");
                                Thread.Sleep(1000);
                            }, index);
                    }
                    Console.WriteLine("Finished launching detached child tasks...");
                }, TaskCreationOptions.DenyChildAttach);

        Task continuation =
            task.ContinueWith(
                antecedent =>
                    Console.WriteLine($"Executing continuation of Task {antecedent.Id}"));

        await continuation;

        Console.ReadLine();
    }
}
// The example displays output like the following:
//     Running antecedent task 1...
//     Launching attached child tasks...
//     Finished launching detached child tasks...
//        Attached child task #2 running
//        Attached child task #3 running
//        Attached child task #1 running
//        Attached child task #4 running
//        Attached child task #5 running
//     Executing continuation of Task 1