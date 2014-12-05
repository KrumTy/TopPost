namespace FileEncryptor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PrimeNumbersFileEncoder : IFileEncoder
    {
        private static bool?[] primals;

        public void Encode(string sourceFile, string destinationFile, LinkedList<int> key)
        {
            var keyToArray = StructureConverter.GetArrayFromLinkedList(key);
            Code(destinationFile, sourceFile, keyToArray);
        }

        public void Decode(string encodedFile, string destinationFile, LinkedList<int> key)
        {
            var keyToArray = StructureConverter.GetReversedArrayFromLinkedList(key);
            Code(destinationFile, encodedFile, keyToArray);
        }

        private static void Code(string destinationFile, string sourceFile, byte[] keyToArray)
        {
            var sourceFileToList = StructureConverter.GetBytesFromFile(sourceFile);
            var encodedBytes = new byte[sourceFileToList.Count()];
            if (primals == null)
            {
                InitPrimals(sourceFileToList.Count());
            }

            for (int i = 0; i < sourceFileToList.Length; i++)
            {
                var b = sourceFileToList[i];

                if (IsPrime(i))
                {
                    encodedBytes[i] = b;
                }
                else
                {
                    encodedBytes[i] = keyToArray[b];
                }
            }

            System.IO.File.WriteAllBytes(destinationFile, encodedBytes);
        }

        private static void InitPrimals(int size)
        {
            primals = new bool?[size];
            primals[0] = false;
            primals[1] = true;
            primals[2] = true;
        }

        private static bool IsPrime(int number)
        {
            if (primals[number] != null)
            {
                return (bool)primals[number];
            }

            if (number % 2 == 0)
            {
                primals[number] = false;

                return false;
            }

            int size = (int)Math.Sqrt(number) + 1;

            for (int i = 3; i <= size; i += 2)
            {
                if (number % i == 0)
                {
                    primals[number] = false;

                    return false;
                }
            }

            primals[number] = true;

            return true;
        }
    }
}
