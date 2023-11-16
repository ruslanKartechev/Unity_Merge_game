// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("G3R5rAm78L6Zc/ds+Q0cCM7AiYXfCI80Oz/l3d9/uKV/FOrfHoBLBNCiSU8P/U7tktKSUAELHbXf6qbvtcqbgLxNJbdcjRlDaJq3ZUBB6KAVXiWh570PQc8mYELHVjoJm5mp3bhC1Np48keenjGYE+Ks24NkV6j7rywiLR2vLCcvrywsLeM5RGxe/5B5AZdG1nqsb9iybobdrpbXBS/LLB2vLA8dICskB6tlq9ogLCwsKC0uc56R0vKc9dycq4XiJhCG2zn4LWD1RzGTjSPC7Gkg5peSOqZOyX6L9pMBghg5rTFhUSqX1hL0PBsiEOJ7TCGIHjMI0uCSGa+4vYeeNuw3rbaYPUDFbi4oJix4QIJHDs2/u8ManHb00rffuMNyqC8uUI0s");
        private static int[] order = new int[] { 8,12,7,11,5,6,12,13,8,13,13,11,12,13,14 };
        private static int key = 45;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
