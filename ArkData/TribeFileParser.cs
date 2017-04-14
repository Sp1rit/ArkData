using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkData
{
    /// <summary>
    /// Parser for ARK tribe files.
    /// </summary>
    /// <seealso cref="ArkData.IParser{ArkData.ITribe}" />
    public class TribeFileParser : IParser<ITribe>
    {
        /// <summary>
        /// Parses the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">The provided file doesn't exist.</exception>
        public ITribe Parse(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
                throw new FileNotFoundException("The provided file doesn't exist.", filePath);

            byte[] data = File.ReadAllBytes(filePath);

            return new Tribe()
            {
                Id = BinaryHelper.GetInt(data, "TribeID"),
                Name = BinaryHelper.GetString(data, "TribeName"),
                OwnerId = GetOwnerId(data),

                FileCreated = fileInfo.CreationTime,
                FileUpdated = fileInfo.LastWriteTime
            };
        }

        /// <summary>
        /// Parses the specified file asynchronous.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public Task<ITribe> ParseAsync(string filePath)
        {
            return Task.Run(() => Parse(filePath));
        }

        private static uint GetOwnerId(byte[] data)
        {
            byte[] id = Encoding.Default.GetBytes("OwnerPlayerDataID");
            byte[] intProperty = Encoding.Default.GetBytes("UInt32Property");

            int idPos = data.LocateFirst(id, 0);
            int intPropertyPos = data.LocateFirst(intProperty, idPos);

            return BitConverter.ToUInt32(data, intPropertyPos + intProperty.Length + 9);
        }
    }
}
