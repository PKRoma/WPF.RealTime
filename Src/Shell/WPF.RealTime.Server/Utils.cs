using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace WPF.RealTime.Server
{
    public static class Win32Native
    {
        //GetCurrentThread() returns only a pseudo handle. No need for a SafeHandle here.
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentThread();

        [HostProtection(SelfAffectingThreading = true)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern UIntPtr SetThreadAffinityMask(IntPtr handle, UIntPtr mask);
    }

    public static class ProcessorAffinity
    {
        private static UIntPtr _lastaffinity;

        private static ulong Maskfromids(params int[] ids)
        {
            ulong mask = 0;
            foreach (int id in ids)
            {
                if (id < 0 || id >= Environment.ProcessorCount)
                    throw new ArgumentOutOfRangeException("CPUId", id.ToString());
                mask |= 1UL << id;
            }
            return mask;
        }

        /// <summary>
        /// Sets a processor affinity mask for the current thread.
        /// </summary>
        /// <param name="mask">A thread affinity mask where each bit set to 1 specifies a logical processor on which this thread is allowed to run. 
        /// <remarks>Note: a thread cannot specify a broader set of CPUs than those specified in the process affinity mask.</remarks> 
        /// </param>
        /// <returns>The previous affinity mask for the current thread.</returns>
        private static UIntPtr SetThreadAffinityMask(UIntPtr mask)
        {
            UIntPtr lastaffinity = Win32Native.SetThreadAffinityMask(Win32Native.GetCurrentThread(), mask);
            _lastaffinity = lastaffinity;
            if (lastaffinity == UIntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return lastaffinity;
        }

        /// <summary>
        /// Sets the logical CPUs that the current thread is allowed to execute on.
        /// </summary>
        /// <param name="CPUIds">One or more logical processor identifier(s) the current thread is allowed to run on.<remarks>Note: numbering starts from 0.</remarks></param>
        /// <returns>The previous affinity mask for the current thread.</returns>
        public static UIntPtr BeginAffinity(params int[] CPUIds)
        {
            return SetThreadAffinityMask(((UIntPtr)Maskfromids(CPUIds)));
        }

        public static void EndAffinity()
        {
            if (_lastaffinity != UIntPtr.Zero)
            {
                Win32Native.SetThreadAffinityMask(Win32Native.GetCurrentThread(), _lastaffinity);
                _lastaffinity = UIntPtr.Zero;
            }
        }
    }
}
