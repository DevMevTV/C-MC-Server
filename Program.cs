using System.Net;
using System.Net.Sockets;
using Minecraft_Server;
using Minecraft_Server.clientbound.play.ping;
using Minecraft_Server.serverbound;
using Minecraft_Server.serverbound.configuration;
using Minecraft_Server.serverbound.login;

IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 25565);
TcpListener listener = new(ipEndPoint);



try
{
    listener.Start();
    Console.WriteLine("Server Listening on 0.0.0.0:25565");

    new SetTimeout(() =>
    {
        foreach (var socket in Players.ConnectedSockets)
        {
            if ((ConnectionStates)socket.Value["NextState"] == ConnectionStates.Play)
            {
                PlayPing.Send(socket.Key);
            }
        }
    }, 13000, true);

    while (true)
    {
        TcpClient client = listener.AcceptTcpClient();
        NetworkStream stream = client.GetStream();

        Players.ConnectedSockets.Add(client, []);

        Players.ConnectedSockets[client]["NextState"] = ConnectionStates.Handskake;

        while (client.Connected)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            if (bytesRead == 0)
                break;

            ProcessBuffer(client, buffer);

        }
        Players.ConnectedSockets.Remove(client);
        Console.WriteLine("Client Disconnected");
    }
}
finally
{
    listener.Stop();
}

static void ProcessBuffer(TcpClient client, byte[] buffer)
{
    int offset = 0;

    while (offset < buffer.Length)
    {
        int length = Datatypes.VarInt.Decode(buffer, ref offset);

        if (offset + length > buffer.Length)
        {
            Console.WriteLine("Invalid length detected.");
            break;
        }

        byte[] packet = new byte[length];
        Array.Copy(buffer, offset, packet, 0, length);
        HandlePacket(client, packet);

        offset += length;

        if (offset < buffer.Length && buffer[offset] != 0x00)
        {
            continue;
        }

        break;
    }
}

static void HandlePacket(TcpClient client, byte[] buffer)
{
    
    switch (Players.ConnectedSockets[client]["NextState"])
    {
        case ConnectionStates.Handskake:
            Handshake.Handle(client, buffer);
            break;
        case ConnectionStates.Status:
            Status.Handle(client, buffer);
            break;
        case ConnectionStates.Login:
            Login.Handle(client, buffer);
            break;
        case ConnectionStates.Configuration:
            Configuration.Handle(client, buffer);
            break;
        case ConnectionStates.Play:
            break;
        case ConnectionStates.Disconnect:
            break;
    }
}