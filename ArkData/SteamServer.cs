using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ArkData
{
    /// <summary>
    /// Class to retrieve online players from a steam server.
    /// </summary>
    /// <seealso cref="ArkData.ISteamServer" />
    public class SteamServer : ISteamServer
    {
        private const int MAX_PACKET_SIZE = 12288;

        private static byte[] packetBuffer = new byte[MAX_PACKET_SIZE];
        private static byte[] challenge = new byte[] { 0xff, 0xff, 0xff, 0xff, 0x55, 0x00, 0x00, 0x00, 0x00 };

        /// <summary>
        /// Gets the online player names.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <returns></returns>
        public IEnumerable<string> GetOnlinePlayerNames(IPEndPoint server)
        {
            byte[] buffer = SendRequest(server, challenge);

            int i = 4;
            if (buffer[i++] != 'A')
                return null;

            byte[] requestPlayer = new byte[] { 0xff, 0xff, 0xff, 0xff, 0x55, buffer[i++], buffer[i++], buffer[i++], buffer[i++] };

            try
            {
                buffer = SendRequest(server, requestPlayer);
            }
            catch (Exception)
            {
                return null;
            }

            i = 4;
            if (buffer[i++] != 'D')
                return null;

            byte numPlayers = buffer[i++];
            var players = new List<string>();
            for (int j = 0; j < numPlayers; j++)
            {
                i++;

                var playerName = new List<byte>();
                while (buffer[i] != 0x00)
                    playerName.Add(buffer[i++]);
                string name = Encoding.UTF8.GetString(playerName.ToArray());
                if (!string.IsNullOrEmpty(name))
                    players.Add(name);

                i += 9;
            }

            return players;
        }

        private byte[] SendRequest(IPEndPoint server, byte[] request)
        {
            int count;
            byte[] responsePacket;
            EndPoint refEndpoint = server;
            using (var srvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                srvSocket.SendTimeout = 3000;
                srvSocket.ReceiveTimeout = 3000;

                srvSocket.SendTo(request, server);
                lock (packetBuffer)
                {
                    count = srvSocket.ReceiveFrom(packetBuffer, ref refEndpoint);
                    responsePacket = new byte[count];
                    Array.Copy(packetBuffer, responsePacket, count);
                }
            }

            return responsePacket;
        }
    }
}
