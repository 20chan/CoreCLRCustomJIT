using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using static RuntimeILPatch.API;

namespace RuntimeILPatch {
    public class JIT {
        public bool IsHooked { get; private set; }
        public Guid CLRVersionID { get; private set; }

        private IntPtr _jitAddr;
        private CompileMethodDelegate _originalCompileMethod;
        private CompileMethodDelegate _replacedCompileMethod;

        public JIT() {
            IsHooked = false;
        }

        public bool Hook() {
            foreach (ProcessModule module in Process.GetCurrentProcess().Modules) {
                if (Path.GetFileName(module.FileName) == "clrjit.dll") {
                    var jitAddr = GetProcAddress(module.BaseAddress, "getJit");
                    if (jitAddr != IntPtr.Zero) {
                        ReplaceJit(jitAddr);
                        return IsHooked;
                    }
                }
            }
            return false;
        }

        public bool UnHook() {
            throw new NotImplementedException();
        }


        private void ReplaceJit(IntPtr jitAddr) {
            _jitAddr = jitAddr;
            var getJit = Marshal.GetDelegateForFunctionPointer<GetJitDelegate>(jitAddr);
            var jit = getJit();
            var jitTable = Marshal.ReadIntPtr(jit);
            var getVerIdPtr = Marshal.ReadIntPtr(jitTable, IntPtr.Size * 4);
            var getVerId = Marshal.GetDelegateForFunctionPointer<GetVersionIdentifierDelegate>(getVerIdPtr);
            getVerId(jitAddr, out var guid);
            CLRVersionID = guid;

            var compileMethodPtr = Marshal.ReadIntPtr(jitTable, 0);
            _originalCompileMethod = Marshal.GetDelegateForFunctionPointer<CompileMethodDelegate>(compileMethodPtr);
            _replacedCompileMethod = CompileMethod;
            var replacedCompileMethodPtr = Marshal.GetFunctionPointerForDelegate(_replacedCompileMethod);

            var trampolinePtr = AllocateTrampoline(replacedCompileMethodPtr);
            var trampoline = Marshal.GetDelegateForFunctionPointer<CompileMethodDelegate>(trampolinePtr);
            var emptyInfo = default(CORINFO_METHOD_INFO);

            trampoline(IntPtr.Zero, IntPtr.Zero, ref emptyInfo, 0, out var _, out var _);
            FreeTrampoline(trampolinePtr);

            VirtualProtect(jitTable, new IntPtr(IntPtr.Size), MemoryProtection.ReadWrite, out var oldFlags);
            Marshal.WriteIntPtr(jitTable, 0, replacedCompileMethodPtr);
            VirtualProtect(jitTable, new IntPtr(IntPtr.Size), oldFlags, out _);

            IsHooked = true;
        }

        readonly byte[] DelegateTrampolineCode = {
            // mov rax, 0000000000000000h ;
            0x48, 0xB8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            // jmp rax
            0xFF, 0xE0
        };

        private IntPtr AllocateTrampoline(IntPtr dest) {
            var jmp = VirtualAlloc(IntPtr.Zero, DelegateTrampolineCode.Length, AllocationType.Commit, MemoryProtection.ExecuteReadWrite);
            Marshal.Copy(DelegateTrampolineCode, 0, jmp, DelegateTrampolineCode.Length);
            Marshal.WriteIntPtr(jmp, 2, dest);
            return jmp;
        }

        private void FreeTrampoline(IntPtr trampoline) {
            VirtualFree(trampoline, new IntPtr(DelegateTrampolineCode.Length), FreeType.Release);
        }

        private int CompileMethod(
            IntPtr thisPtr,
            IntPtr comp,
            ref CORINFO_METHOD_INFO info,
            uint flags,
            out IntPtr nativeEntry,
            out int nativeSizeOfCode) {
            if (!IsHooked) {
                nativeEntry = IntPtr.Zero;
                nativeSizeOfCode = 0;
                return 0;
            }
            return _originalCompileMethod(thisPtr, comp, ref info, flags, out nativeEntry, out nativeSizeOfCode);
        }
    }
}
