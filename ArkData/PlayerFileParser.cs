using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkData
{
    /// <summary>
    /// Parser for ARK player files.
    /// </summary>
    /// <seealso cref="ArkData.IParser{ArkData.IPlayer}" />
    public class PlayerFileParser : IParser<IPlayer>
    {
        /// <summary>
        /// Parses the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">The provided file doesn't exist.</exception>
        public IPlayer Parse(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
                throw new FileNotFoundException("The provided file doesn't exist.", filePath);

            byte[] data = File.ReadAllBytes(filePath);

            return new Player()
            {
                Id = GetId(data),
                SteamId = GetSteamId(data),
                SteamName = BinaryHelper.GetString(data, "PlayerName"),
                CharacterName = BinaryHelper.GetString(data, "PlayerCharacterName"),
                TribeId = BinaryHelper.GetInt(data, "TribeID"),
                Level = BinaryHelper.GetUInt16(data, "CharacterStatusComponent_ExtraCharacterLevel"),
                ExperiencePoints = BinaryHelper.GetFloat(data, "CharacterStatusComponent_ExperiencePoints"),
                TotalEngramPoints = BinaryHelper.GetInt(data, "PlayerState_TotalEngramPoints"),
                FirstSpawned = BinaryHelper.GetBool(data, "FirstSpawned"),

                FileCreated = fileInfo.CreationTime,
                FileUpdated = fileInfo.LastWriteTime
            };
        }

        /// <summary>
        /// Parses the specified file asynchronous.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public Task<IPlayer> ParseAsync(string filePath)
        {
            return Task.Run((() => Parse(filePath)));
        }

        private ulong GetId(byte[] data)
        {
            byte[] id = Encoding.Default.GetBytes("PlayerDataID");
            byte[] intProperty = Encoding.Default.GetBytes("UInt64Property");

            int idPos = data.LocateFirst(id, 0);
            int intPropertyPos = data.LocateFirst(intProperty, idPos);

            return BitConverter.ToUInt64(data, intPropertyPos + intProperty.Length + 9);
        }

        private string GetSteamId(byte[] data)
        {
            byte[] steamName = Encoding.Default.GetBytes("UniqueNetIdRepl");
            int steamNamePos = data.LocateFirst(steamName, 0);

            byte[] stringBytes = new byte[17];
            Array.Copy(data, steamNamePos + steamName.Length + 9, stringBytes, 0, 17);

            return Encoding.Default.GetString(stringBytes);
        }
    }
}
