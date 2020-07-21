﻿using System;
using System.Threading;
using System.Threading.Tasks;

public class Example
{
    public static async Task Main()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;
        cts.Cancel();

        var task = Task.FromCanceled(token);
        var continuation =
            task.ContinueWith(
                antecedent => Console.WriteLine("The continuation is running."),
                TaskContinuationOptions.NotOnCanceled);

        try
        {
            await task;
        }
        catch (Exception e)
        {
            static void WriteException(Exception ex) =>
                Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");

            if (e is AggregateException ae)
            {
                foreach (Exception ie in ae.InnerExceptions)
                {
                    WriteException(ie);
                }
            }
            else
            {
                WriteException(e);
            }

            Console.WriteLine();
        }

        Console.WriteLine($"Task {task.Id}: {task.Status:G}");
        Console.WriteLine($"Task {continuation.Id}: {continuation.Status:G}");
    }
}
// The example displays the following output:
//       TaskCanceledException: A task was canceled.
//
//       Task 1: Canceled
//       Task 2: Canceled