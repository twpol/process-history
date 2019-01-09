using System;
using System.Runtime.InteropServices;

namespace ProcessHistory
{
    static class WindowsNativeFunctions
    {
        [DllImport("kernel32", ExactSpelling = true, SetLastError = true)]
        public static extern bool QueryUnbiasedInterruptTime(out ulong unbiasedTime);

        [DllImport("kernel32", ExactSpelling = true, SetLastError = true)]
        public static extern bool QueryPerformanceFrequency(out ulong frequency);

        [DllImport("kernel32", ExactSpelling = true, SetLastError = true, EntryPoint = "QueryIdleProcessorCycleTime")]
        public static extern bool QueryIdleProcessorCycleTimeGetBufferSize(out int bufferLength, IntPtr unused);

        [DllImport("kernel32", ExactSpelling = true, SetLastError = true)]
        public static extern bool QueryIdleProcessorCycleTime(ref int bufferLength, ulong[] processorIdleCycleTimes);

        [DllImport("kernel32", ExactSpelling = true, SetLastError = true)]
        public static extern bool QueryProcessCycleTime(IntPtr processHandle, out ulong cycleTime);
    }
}
