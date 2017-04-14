using System;
using System.Text;

namespace ArkData
{
    internal class BinaryHelper
    {
        public static int GetInt(byte[] data, string name)
        {
            byte[] tribeId = Encoding.Default.GetBytes(name);
            byte[] intProperty = Encoding.Default.GetBytes("IntProperty");

            int tribeIdPos = data.LocateFirst(tribeId);
            int intPropertyPos = data.LocateFirst(intProperty, tribeIdPos);

            if (intPropertyPos > -1)
                return BitConverter.ToInt32(data, intPropertyPos + intProperty.Length + 9);

            return -1;
        }

        public static ushort GetUInt16(byte[] data, string name)
        {
            byte[] tribeId = Encoding.Default.GetBytes(name);
            byte[] intProperty = Encoding.Default.GetBytes("UInt16Property");

            int tribeIdPos = data.LocateFirst(tribeId);
            int intPropertyPos = data.LocateFirst(intProperty, tribeIdPos);

            if (intPropertyPos > -1)
                return BitConverter.ToUInt16(data, intPropertyPos + intProperty.Length + 9);

            return 0;
        }

        public static string GetString(byte[] data, string name)
        {
            byte[] nameBytes = Encoding.Default.GetBytes(name);
            byte[] strProperty = Encoding.Default.GetBytes("StrProperty");

            int namePos = data.LocateFirst(nameBytes);
            int strPropertyPos = data.LocateFirst(strProperty, namePos);
            if (strPropertyPos >= 0)
            {
                byte[] data2 = new byte[1];
                Array.Copy(data, strPropertyPos + strProperty.Length + 1, data2, 0, 1);

                int length = ((int)data2[0]) - ((data[strPropertyPos + strProperty.Length + 12] == 0xff) ? 6 : 5);

                byte[] stringBytes = new byte[length];
                Array.Copy(data, strPropertyPos + strProperty.Length + 13, stringBytes, 0, length);

                if (data[strPropertyPos + strProperty.Length + 12] == 0xff)
                    return Encoding.Unicode.GetString(stringBytes);

                return Encoding.Default.GetString(stringBytes);
            }

            return string.Empty;
        }
    }
}
