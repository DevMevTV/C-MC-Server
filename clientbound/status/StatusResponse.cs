using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using static Minecraft_Server.Datatypes;

namespace Minecraft_Server.clientbound.status
{
    static class StatusResponse
    {
        public static void send(TcpClient client)
        {
            var response = new
            {
                version = new
                {
                    name = "DevCraft ;3",
                    protocol = 768
                },
                players = new
                {
                    max = 100,
                    online = 2,
                    sample = new[]
                    {
                        new { name = "Steve", id = "8667ba71-b85a-4004-af54-457a9734eed7" },
                        new { name = "Alex", id = "7a3f575e-c85f-4ff2-92da-dc18a78cb7465" }
                    }
                },
                description = new
                {
                    text = "§4§l§nC§r§4§n# §lSERVER BE BRRRRRR ;3!"
                },
                enforcesSecureChat = false
            };

            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string jsonString = JsonSerializer.Serialize(response, options);

            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);
            byte[] jsonLength = VarInt.Encode(jsonBytes.Length);
            jsonBytes = jsonLength.Concat(jsonBytes).ToArray();

            int packetLength = jsonBytes.Length + 1;
            byte[] lengthVarInt = VarInt.Encode(packetLength);

            byte[] buffer = lengthVarInt
                .Concat(new byte[] { 0x00 })
                .Concat(jsonBytes)
                .ToArray();


            var stream = client.GetStream();
            stream.Write(buffer, 0, buffer.Length);
        }

    }
}
