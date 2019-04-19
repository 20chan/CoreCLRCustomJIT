using System;
using System.Runtime.InteropServices;

namespace RuntimeILPatch {
    public static class API {
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
    }
}
