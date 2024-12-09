using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft_Server.clientbound.play
{
    static class ChunkDataAndUpdateLight
    {
        public static void Send()
        {
            byte[] chunkX = BitConverter.GetBytes(0);
            byte[] chunkY = BitConverter.GetBytes(0);

            var heightmap = new
            {
                MOTION_BLOCKING = (long)0,
                WORLD_SURFACE = (long)0
            };

            byte[] heightmapData;
            using (var memoryStream = new MemoryStream())
            {
                NBTEncoder.EncodeCompound(memoryStream, heightmap);
                heightmapData = memoryStream.ToArray();
            }

            byte[] dataSize = Datatypes.VarInt.Encode(0);

            byte[] numberOfBlockEntities = Datatypes.VarInt.Encode(0);

            byte[] SkyLightMask = [0x00];
            byte[] BlockLightMask = [0x00];
            byte[] EmptySkyLightMask = [0x00];
            byte[] EmptyBlockLightMask = [0x00];


        }
    }
}
