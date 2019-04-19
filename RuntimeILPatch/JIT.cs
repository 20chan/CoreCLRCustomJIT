using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using static RuntimeILPatch.API;

namespace RuntimeILPatch {
    public static class JIT {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr GetJitDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GetVersionIdentifierDelegate(IntPtr thisPtr, out Guid versionIdentifier);

        public static bool Hook() {
            foreach (ProcessModule module in Process.GetCurrentProcess().Modules) {
                if (Path.GetFileName(module.FileName) == "clrjit.dll") {
                    var jitAddr = GetProcAddress(module.BaseAddress, "getJit");
                    if (jitAddr != IntPtr.Zero) {
                        ReplaceJit(jitAddr);
                        return true;
                    }
                }
            }
            return false;
        }


        static void ReplaceJit(IntPtr jitAddr) {
            var getJit = Marshal.GetDelegateForFunctionPointer<GetJitDelegate>(jitAddr);
            var jit = getJit();
            var jitTable = Marshal.ReadIntPtr(jit);
            var getVerIdPtr = Marshal.ReadIntPtr(jitTable, IntPtr.Size * 4);
            var getVerId = Marshal.GetDelegateForFunctionPointer<GetVersionIdentifierDelegate>(getVerIdPtr);
            getVerId(jitAddr, out var guid);

            Console.WriteLine(guid);
        }
    }
}
