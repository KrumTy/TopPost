namespace FileEncryptor
{
    using System.Collections.Generic;

    public interface IFileEncoder
    {
        /// <summary>
        /// Encodes a file with the specified key and saves the result to a given file.
        /// </summary>
        /// <param name="sourceFile">path to the initial file</param>
        /// <param name="destinationFile">path to the result file</param>
        /// <param name="key">List of replacement bytes</param>
        void Encode(string sourceFile, string destinationFile, LinkedList<int> key);

        /// <summary>
        /// Decodes a file that was encoded with the above algorithm.
        /// </summary>
        /// <param name="sourceFile">path to encoded file</param>
        /// <param name="destinationFile">path to the result file</param>
        /// <param name="key">list of replacement bytes that were used to encode the file</param>
        void Decode(string encodedFile, string destinationFile, LinkedList<int> key);
    }
}
