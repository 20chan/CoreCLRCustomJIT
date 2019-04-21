using System;
using System.Runtime.InteropServices;

namespace RuntimeILPatch {
    public static class API {
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32")]
        internal static extern IntPtr VirtualAlloc(IntPtr lpAddress, int dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32")]
        internal static extern int VirtualFree(IntPtr lpAddress, IntPtr dwSize, FreeType freeType);

        [DllImport("kernel32")]
        internal static extern int VirtualProtect(IntPtr lpAddress, IntPtr dwSize, MemoryProtection flNewProtect, out MemoryProtection lpflOldProtect);


        [Flags]
        internal enum FreeType {
            Decommit = 0x4000,
            Release = 0x8000,
        }

        [Flags]
        internal enum AllocationType {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        internal enum MemoryProtection {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }
    }
}
