using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace WPF.RealTime.Infrastructure.Utils
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
}
