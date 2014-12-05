namespace FileEncryptor
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    internal class Program
    {
        internal static void Main()
        {
            string encodedFileLocation = @"C:\Users\Ghos7\Documents\GitHub\TopPost\FmiFileReadDsaHW\Files\test.png";
            string destinationFileLocation = @"C:\Users\Ghos7\Documents\GitHub\TopPost\FmiFileReadDsaHW\Files\destinationFile.png";
            string destinationFileLocation2 = @"C:\Users\Ghos7\Documents\GitHub\TopPost\FmiFileReadDsaHW\Files\destinationFile2.png";
            string bytesLocation = @"C:\Users\Ghos7\Documents\GitHub\TopPost\FmiFileReadDsaHW\Files\bytes.txt";

            try
            {
                var bytesToLinkedList = new LinkedList<int>(System.IO.File.ReadAllText(bytesLocation).Trim().Split(' ').Select(x => int.Parse(x)).ToList());

                Stopwatch timer = new Stopwatch();
                timer.Start();

                var fileEncoder = new PrimeNumbersFileEncoder();
                fileEncoder.Encode(encodedFileLocation, destinationFileLocation, bytesToLinkedList);
                fileEncoder.Decode(destinationFileLocation, destinationFileLocation2, bytesToLinkedList);
                
                System.Console.WriteLine(timer.Elapsed);
            }
            catch
            {
                System.Console.WriteLine("Error!");
            }
        }
    }
}
