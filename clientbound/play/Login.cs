using System.Net.Sockets;
using System.Text;
using static Minecraft_Server.Datatypes;

namespace Minecraft_Server.clientbound.play
{
    static class Login
    {
        public static void Send(TcpClient client)
        {
            uint entity_id = 0;
            long hashed_seed = 0;

            byte[] dimension_overworld = [];
            Datatypes.String.Encode(ref dimension_overworld, "minecraft:overworld");
            
            byte[] dimensions = [];
            Datatypes.String.Encode(ref dimensions, "minecraft:overworld");
            //Datatypes.String.Encode(ref dimensions, "minecraft:the_nether");
            //Datatypes.String.Encode(ref dimensions, "minecraft:the_end");


            byte[] responseData = BitConverter.GetBytes(entity_id)  // Entity ID
                .Concat(new byte[] { 0x00 })                        // Is hardcore
                .Concat(VarInt.Encode(1))                           // Dimension Count
                .Concat(dimensions)                                 // Dimension Names
                .Concat(VarInt.Encode(20))                          // Max Players
                .Concat(VarInt.Encode(10))                          // View Distance
                .Concat(VarInt.Encode(10))                          // Simulation Distance
                .Concat(new byte[] {0x00})                          // Reduced Debug Info
                .Concat(new byte[] {0x01})                          // Enable respawn screen
                .Concat(new byte[] {0x00})                          // Do limited crafting
                .Concat(VarInt.Encode(0))                           // Dimension Type
                .Concat(dimension_overworld)                        // Dimension Name
                .Concat(BitConverter.GetBytes(hashed_seed))         // Hashed seed
                .Concat(new byte[] {1})                             // Game mode
                .Concat(new byte[] {0xff})                          // Previous Game mode
                .Concat(new byte[] {0})                             // Is Debug
                .Concat(new byte[] {0})                             // Is Flat
                .Concat(new byte[] {0})                             // Has death location
                .Concat(new byte[] {0})                             // idfk and idfc
                .Concat(VarInt.Encode(20))                          // Portal cooldown
                .Concat(new byte[] {1})                             // Enforces Secure Chat
                .ToArray();

            byte[] response = VarInt.Encode(responseData.Length + 1)
                .Concat(new byte[] { 0x2C })
                .Concat(responseData)
                .ToArray();

            var stream = client.GetStream();
            stream.Write(response);
        }
    }
}
