using System;
using System.Collections.Generic;
using System.Text;

namespace RuntimeILPatch {
    public static class CoreCLRVersion {
        public static Guid Core1_0 = new Guid( /* 718c4238-2a85-45de-88ad-9b1fed806547 */
            0x718c4238,
            0x2a85,
            0x45de,
            0x88, 0xad, 0x9b, 0x1f, 0xed, 0x80, 0x65, 0x47);

        public static Guid Core1_1 = new Guid( /* 0b17dfeb-1ead-4e06-b025-d60d3a493b53 */
            0x0b17dfeb,
            0x1ead,
            0x4e06,
            0xb0, 0x25, 0xd6, 0x0d, 0x3a, 0x49, 0x3b, 0x53);

        public static Guid Core2_0 = new Guid( /* f00b3f49-ddd2-49be-ba43-6e49ffa66959 */
            0xf00b3f49,
            0xddd2,
            0x49be,
            0xba, 0x43, 0x6e, 0x49, 0xff, 0xa6, 0x69, 0x59 );

        public static Guid Core2_1 = new Guid( /* 0ba106c8-81a0-407f-99a1-928448c1eb62 */
            0x0ba106c8,
            0x81a0,
            0x407f,
            0x99, 0xa1, 0x92, 0x84, 0x48, 0xc1, 0xeb, 0x62);

        public static Guid Core2_2 = Core2_1;

        public static Guid Core3_0 = new Guid( /* d609bed1-7831-49fc-bd49-b6f054dd4d46 */
            0xd609bed1,
            0x7831,
            0x49fc,
            0xbd, 0x49, 0xb6, 0xf0, 0x54, 0xdd, 0x4d, 0x46);
    }
}
