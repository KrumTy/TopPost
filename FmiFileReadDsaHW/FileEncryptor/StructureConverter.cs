namespace FileEncryptor
{
    using System.Collections.Generic;
    using System.Linq;

    public static class StructureConverter
    {
        public static byte[] GetBytesFromFile(string fileLocation)
        {
            return System.IO.File.ReadAllBytes(fileLocation);
        }

        public static byte[] GetReversedArrayFromLinkedList(LinkedList<int> linkedList)
        {
            byte index = 0;
            var dictionary = new byte[256];

            foreach (var item in linkedList)
            {
                dictionary[item] = index;
                index++;
            }

            return dictionary;
        }

        public static byte[] GetArrayFromLinkedList(LinkedList<int> linkedList)
        {
            int index = 0;
            var bytesToArray = new byte[linkedList.Count];

            foreach (var item in linkedList)
            {
                bytesToArray[index] = (byte)item;
                index++;
            }

            return bytesToArray;
        }
    }
}
