using System;
using System.IO;
using OpenMcdf;

namespace ThumbsDBReader
{
    public class Reader
    {
        #region CheckFileNameAndOutputFolder
        /// <summary>
        ///     Checks if the <paramref name="inputFile" /> and <paramref name="outputFolder" /> is valid
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFolder"></param>
        /// <exception cref="ArgumentNullException">Raised when the <paramref name="inputFile" /> or <paramref name="outputFolder" /> is null or empty</exception>
        /// <exception cref="FileNotFoundException">Raised when the <paramref name="inputFile" /> does not exists</exception>
        /// <exception cref="DirectoryNotFoundException">Raised when the <paramref name="outputFolder" /> does not exist</exception>
        /// <exception cref="VCRFileTypeNotSupported">Raised when the extension is not .vcf</exception>
        private static void CheckFileNameAndOutputFolder(string inputFile, string outputFolder)
        {
            if (string.IsNullOrEmpty(inputFile))
                throw new ArgumentNullException(inputFile);

            if (string.IsNullOrEmpty(outputFolder))
                throw new ArgumentNullException(outputFolder);

            if (!File.Exists(inputFile))
                throw new FileNotFoundException(inputFile);

            if (!Directory.Exists(outputFolder))
                throw new DirectoryNotFoundException(outputFolder);

            var extension = Path.GetExtension(inputFile);
            if (string.IsNullOrEmpty(extension))
                throw new TDBRFileTypeNotSupported("Expected .db extension on the inputFile");

            extension = extension.ToLowerInvariant();

            switch (extension)
            {
                case ".vcf":
                    return;

                case ".ics":
                    return;

                default:
                    throw new TDBRFileTypeNotSupported("Wrong file extension, expected .db");
            }
        }
        #endregion

        private void WriteHtml(string fileName)
        {
            using (var compoundFile = new CompoundFile(fileName))
            {
                if(compoundFile.RootStorage.TryGetStream("EncryptedPackage", out var stream))
                {

                }
            }
        }
    }
}
