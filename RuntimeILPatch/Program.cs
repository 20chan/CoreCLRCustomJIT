using System;

namespace RuntimeILPatch {
    class Program {
        static void Main(string[] args) {
            var jit = new JIT();

            jit.Hook();

            Console.WriteLine(Answer());
        }

        static int Answer() {
            return 0;
        }
    }
}