using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft_Server.clientbound.play
{
    static class GameEvent
    {
        public enum gameEvents
        {
            NoRespawnBlockAvailable = 0,
            BeginRaining = 1,
            EndRaining = 2,
            ChangeGameMode = 3,
            WinGame = 4,
            DemoEvent = 5,
            ArrowHitPlayer = 6,
            RainLevelChange = 7,
            ThunderLevelChange = 8,
            PlayPufferfishStingSound = 9,
            PlayElderGuardianMobAppearance = 10,
            EnableRespawnScreen = 11,
            LimitedCrafting = 12,
            StartWaitingLevelChunks = 13
        }


        public static void Send(TcpClient client, gameEvents gameEvent, float value)
        {
            byte[] valueData = BitConverter.GetBytes(value);
            byte[] gameEventData = [(byte)gameEvent];
            byte[] responseData = gameEventData.Concat(valueData).ToArray();

            byte[] response = Datatypes.VarInt.Encode(responseData.Length + 1)
                .Concat(new byte[] {0x23})
                .Concat(responseData)
                .ToArray();

            var stream = client.GetStream();
            stream.Write(response);
        }
    }
}
