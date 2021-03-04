using System;
using System.Threading;
using Xunit.Runners;

namespace MyCRM.Lodgement.Automation.Services
{
    internal static class Runner
    {
        private static readonly object ConsoleLock = new object();
        private static readonly ManualResetEvent Finished = new ManualResetEvent(false);
        private static int _result;

        public static int Run()
        {
            using var runner = AssemblyRunner.WithoutAppDomain(typeof(Runner).Assembly.Location);
            runner.OnDiscoveryComplete = OnDiscoveryComplete;
            runner.OnTestPassed = OnTestPassed;
            runner.OnExecutionComplete = OnExecutionComplete;
            runner.OnTestFailed = OnTestFailed;
            runner.OnTestSkipped = OnTestSkipped;

            Console.WriteLine("Discovering...");

            runner.Start();

            Finished.WaitOne();
            Finished.Dispose();

            return _result;
        }

        private static void OnDiscoveryComplete(DiscoveryCompleteInfo info)
        {
            lock (ConsoleLock)
                Console.WriteLine($"Running {info.TestCasesToRun} of {info.TestCasesDiscovered} tests...");
        }

        private static void OnExecutionComplete(ExecutionCompleteInfo info)
        {
            lock (ConsoleLock)
                Console.WriteLine($"Finished: {info.TotalTests} tests in {Math.Round(info.ExecutionTime, 3)}s ({info.TestsFailed} failed, {info.TestsSkipped} skipped)");

            Finished.Set();
        }

        private static void OnTestPassed(TestPassedInfo info)
        {
            lock (ConsoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[SUCCESS] {0}", info.TestDisplayName);
                Console.ResetColor();
            }

            _result = 1;
        }

        private static void OnTestFailed(TestFailedInfo info)
        {
            lock (ConsoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine("[FAIL] {0}: {1}", info.TestDisplayName, info.ExceptionMessage);
                if (info.ExceptionStackTrace != null)
                    Console.WriteLine(info.ExceptionStackTrace);

                Console.ResetColor();
            }

            _result = 1;
        }

        private static void OnTestSkipped(TestSkippedInfo info)
        {
            lock (ConsoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[SKIP] {0}: {1}", info.TestDisplayName, info.SkipReason);
                Console.ResetColor();
            }
        }
    }
}
