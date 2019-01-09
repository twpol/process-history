using System;

namespace ProcessHistory
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!WindowsNativeFunctions.QueryUnbiasedInterruptTime(out ulong time)) throw new System.ComponentModel.Win32Exception();
            Console.WriteLine($"System run time: {time:N0} 100ns ~ {time / 10_000_000:N0} seconds");

            if (!WindowsNativeFunctions.QueryPerformanceFrequency(out ulong ticksPerSecond)) throw new System.ComponentModel.Win32Exception();
            Console.WriteLine($"Performance frequency: {ticksPerSecond:N0} ticks/second");

            var cyclesPerTick = 1024u;
            Console.WriteLine($"Assumed performance scaling: {cyclesPerTick:N0} cycles/tick");

            if (!WindowsNativeFunctions.QueryIdleProcessorCycleTimeGetBufferSize(out int bufferLength, IntPtr.Zero)) throw new System.ComponentModel.Win32Exception();
            var processorIdleTimes = new ulong[bufferLength / 8];
            if (!WindowsNativeFunctions.QueryIdleProcessorCycleTime(ref bufferLength, processorIdleTimes)) throw new System.ComponentModel.Win32Exception();
            foreach (var processorIdleTime in processorIdleTimes)
            {
                Console.WriteLine($"Processor core idle time: {processorIdleTime:N0} cycles ~ {processorIdleTime / cyclesPerTick / ticksPerSecond:N0} seconds");
            }

            var processes = System.Diagnostics.Process.GetProcesses();
            foreach (var process in processes)
            {
                try
                {
                    WindowsNativeFunctions.QueryProcessCycleTime(process.Handle, out ulong cycleTime);
                    Console.WriteLine($"Process {process.Id,7:N0} ({process.ProcessName.PadRight(32).Substring(0, 32)}) CPU time: {cycleTime,19:N0} cycles ~ {cycleTime / cyclesPerTick / ticksPerSecond,6:N0} seconds");
                }
                catch (Exception)
                {
                }

                // var startTime = DateTime.MinValue;
                // try
                // {
                //     startTime = process.StartTime;
                // }
                // catch (Exception)
                // {
                // }

                // var handle = IntPtr.Zero;
                // try
                // {
                //     handle = process.Handle;
                // }
                // catch (Exception)
                // {
                // }

                // var mainModuleFileName = "";
                // try
                // {
                //     mainModuleFileName = process.MainModule.FileName;
                // }
                // catch (Exception)
                // {
                // }

                // Console.WriteLine($"{startTime} {process.SessionId} {process.Id} {process.ProcessName} {handle} {mainModuleFileName} {process.MainWindowTitle} {process.NonpagedSystemMemorySize64} {process.PagedSystemMemorySize64} {process.PagedMemorySize64} {process.PrivateMemorySize64} {process.VirtualMemorySize64} {process.WorkingSet64}");
            }
        }
    }
}
