using System.Net.Sockets;
using System.Text;

namespace Minecraft_Server.clientbound.configuration
{
    static class RegistryData
    {
        public static void Send(TcpClient client)
        {

            {
                byte[] responseData = [];
                Datatypes.String.Encode(ref responseData, "minecraft:dimension_type");
                Datatypes.VarInt.Encode(ref responseData, 1);
                Datatypes.String.Encode(ref responseData, "minecraft:overworld");
                responseData = responseData.Concat(new byte[] { 0x00 }).ToArray();

                byte[] response = Datatypes.VarInt.Encode(responseData.Length + 1)
                .Concat(new byte[] { 0x07 })
                .Concat(responseData)
                .ToArray();

                var stream = client.GetStream();
                stream.Write(response);
            }

            {
                byte[] responseData = [];
                Datatypes.String.Encode(ref responseData, "minecraft:painting_variant");
                Datatypes.VarInt.Encode(ref responseData, 1);
                Datatypes.String.Encode(ref responseData, "minecraft:backyard");
                responseData = responseData.Concat(new byte[] { 0x00 }).ToArray();

                byte[] response = Datatypes.VarInt.Encode(responseData.Length + 1)
                    .Concat(new byte[] { 0x07 })
                    .Concat(responseData)
                    .ToArray();

                var stream = client.GetStream();
                stream.Write(response);
            }

            {
                byte[] responseData = [];
                Datatypes.String.Encode(ref responseData, "minecraft:wolf_variant");
                Datatypes.VarInt.Encode(ref responseData, 1);
                Datatypes.String.Encode(ref responseData, "minecraft:chestnut");
                responseData = responseData.Concat(new byte[] { 0x00 }).ToArray();

                byte[] response = Datatypes.VarInt.Encode(responseData.Length + 1)
                    .Concat(new byte[] { 0x07 })
                    .Concat(responseData)
                    .ToArray();

                var stream = client.GetStream();
                stream.Write(response);
            }

            {
                byte[] responseData = [];

                var biomes = new[]
                {
                    "minecraft:old_growth_spruce_taiga",
                    "minecraft:plains"
                };
                Datatypes.String.Encode(ref responseData, "minecraft:worldgen/biome");
                Datatypes.VarInt.Encode(ref responseData, biomes.Length);

                foreach (var biome in biomes)
                {
                    Datatypes.String.Encode(ref responseData, biome);
                    responseData = responseData.Concat(new byte[] { 0x00 }).ToArray();
                }

                byte[] response = Datatypes.VarInt.Encode(responseData.Length + 1)
                    .Concat(new byte[] { 0x07 })
                    .Concat(responseData)
                    .ToArray();

                var stream = client.GetStream();
                stream.Write(response);
            }

            {
                byte[] responseData = [];

                var damage_types = new[]
                {
                    "in_fire",
                    "campfire",
                    "lightning_bolt",
                    "on_fire",
                    "lava",
                    "hot_floor",
                    "in_wall",
                    "cramming",
                    "drown",
                    "starve",
                    "cactus",
                    "fall",
                    "ender_pearl",
                    "fly_into_wall",
                    "out_of_world",
                    "generic",
                    "magic",
                    "wither",
                    "dragon_breath",
                    "dry_out",
                    "sweet_berry_bush",
                    "freeze",
                    "stalagmite",
                    "outside_border",
                    "generic_kill"
                };

                Datatypes.String.Encode(ref responseData, "minecraft:damage_type");
                Datatypes.VarInt.Encode(ref responseData, damage_types.Length);

                foreach (var type in damage_types)
                {
                    Datatypes.String.Encode(ref responseData, type);
                    responseData = responseData.Concat(new byte[] { 0x00 }).ToArray();
                }

                byte[] response = Datatypes.VarInt.Encode(responseData.Length + 1)
                    .Concat(new byte[] { 0x07 })
                    .Concat(responseData)
                    .ToArray();

                var stream = client.GetStream();
                stream.Write(response);
            }
        }

    }
}
