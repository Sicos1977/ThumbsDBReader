using System;
using System.Collections.Generic;
using System.IO;
using OpenMcdf;
using ThumbsDBReader.Exceptions;
using ThumbsDBReader.Helpers;

namespace ThumbsDBReader
{
    public class Reader
    {
        #region Constructor
        /// <summary>
        ///     Creates this object and sets it's needed properties
        /// </summary>
        /// <param name="logStream">When set then logging is written to this stream for all conversions. If
        /// you want a separate log for each conversion then set the logstream on the <see cref="Convert"/> method</param>
        public Reader(Stream logStream = null)
        {
            Logger.LogStream = logStream;
        }
        #endregion

        #region CheckFileNameAndOutputFolder
        /// <summary>
        ///     Checks if the <paramref name="inputFile" /> and <paramref name="outputFolder" /> is valid
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFolder"></param>
        /// <exception cref="ArgumentNullException">Raised when the <paramref name="inputFile" /> or <paramref name="outputFolder" /> is null or empty</exception>
        /// <exception cref="FileNotFoundException">Raised when the <paramref name="inputFile" /> does not exists</exception>
        /// <exception cref="DirectoryNotFoundException">Raised when the <paramref name="outputFolder" /> does not exist</exception>
        /// <exception cref="TDBRFileTypeNotSupported">Raised when the extension is not .db</exception>
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
                case ".db":
                    return;

                default:
                    throw new TDBRFileTypeNotSupported("Wrong file extension, expected .db");
            }
        }
        #endregion

        #region Read
        /// <summary>
        /// This method will read the given <paramref name="inputFile"/> convert it to HTML and write it to the <paramref name="outputFolder"/>
        /// </summary>
        /// <param name="inputFile">The thumbs.db file</param>
        /// <param name="outputFolder">The folder where to save the converted vcf file</param>
        /// <param name="logStream">When set then logging will be written to this stream</param>
        /// <returns>String array containing the full path to the converted VCF file</returns>
        /// <exception cref="ArgumentNullException">Raised when the <paramref name="inputFile" /> or <paramref name="outputFolder" /> is null or empty</exception>
        /// <exception cref="FileNotFoundException">Raised when the <paramref name="inputFile" /> does not exists</exception>
        /// <exception cref="DirectoryNotFoundException">Raised when the <paramref name="outputFolder" /> does not exist</exception>
        /// <exception cref="TDBRFileTypeNotSupported">Raised when the extension is not .ics</exception>
        public List<string> Read(string inputFile, string outputFolder, Stream logStream = null)
        {
            if (logStream != null)
                Logger.LogStream = logStream;

            outputFolder = FileManager.CheckForBackSlash(outputFolder);
            CheckFileNameAndOutputFolder(inputFile, outputFolder);

            Logger.WriteToLog("Start reading thumbs.db streams");
            var datas = GetStreamData(inputFile);
            Logger.WriteToLog("Done reading thumbs.db");
            return result;
        }
        #endregion

        #region GetStreamData
        /// <summary>
        /// Returns the streams from the thumbs.db file that contain the thumbnails
        /// </summary>
        /// <param name="inputFile">The input file</param>
        /// <returns></returns>
        private IEnumerable<byte[]> GetStreamData(string inputFile)
        {
            var result = new List<byte[]>();

            using (var compoundFile = new CompoundFile(inputFile))
            {
                compoundFile.RootStorage.VisitEntries(item =>
                {
                    if (item.IsStream)
                    {
                        if (item.Name.StartsWith("64_") ||
                            item.Name.StartsWith("96_") ||
                            item.Name.StartsWith("128_") ||
                            item.Name.StartsWith("256_") ||
                            item.Name.StartsWith("512_"))
                        {
                            Logger.WriteToLog($"Reading thumbnail stream '{item.Name}'");

                            if (compoundFile.RootStorage.TryGetStream(item.Name, out var stream))
                                result.Add(stream.GetData());
                        }

                        if (item.Name.StartsWith("Catalog"))
                        { }
                    }
                }, false);
            }

            return result;
        }
        #endregion

        private void WriteHtml(List<byte[]> datas, string fileName)
        {
            foreach (var data in datas)
            {

            }
        }
    }
}
