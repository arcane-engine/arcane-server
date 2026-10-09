using Arcane.Server.Core;
using Arcane.Server.Network.Protocol;
using Arcane.Server.Network.Protocol.Packets;

using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Channels;

namespace Arcane.Server;

internal class Player
{
    public int Id { get; set; }
}

internal class Program
{
    private static readonly Lock Lock = new();
    private static int _players;
    private const int Port = 5000;

    private static readonly ConcurrentDictionary<int, ChannelWriter<byte[]>> ActivePlayers = new();

    static async Task<int> Main(string[] args)
    {
        try
        {
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, Port));
            listener.Listen(10);

            Console.WriteLine($"TCP server listening on port {Port}.");

            while (true)
            {
                var client = await listener.AcceptAsync();
                lock (Lock)
                {
                    _players++;
                    var player = new Player
                    {
                        Id = _players
                    };
                    _ = HandleClientAsync(client, player);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal server error: {ex.Message}");

            return 1;
        }
    }

    private static async Task HandleClientAsync(Socket client, Player player)
    {
        Console.WriteLine($"Client connected: {client.RemoteEndPoint}");

        var outboundChannel = Channel.CreateUnbounded<byte[]>(new UnboundedChannelOptions { SingleReader = true });

        ActivePlayers[player.Id] = outboundChannel.Writer;

        await BroadcastPlayerJoinedAsync(player.Id);

        var writeTask = ProcessOutboundQueueAsync(client, outboundChannel.Reader, player);

        try
        {
            var lengthBuffer = new byte[4];
            while (true)
            {
                var read = await ReceiveAllAsync(client, lengthBuffer);
                if (read == 0) break;

                var messageLength = BitConverter.ToInt32(lengthBuffer, 0);
                var payloadBuffer = new byte[messageLength];
                await ReceiveAllAsync(client, payloadBuffer);

                var reader = new BitReader(payloadBuffer);
                var data = new ClientPacketReader(ref reader).Read();

                Console.WriteLine($"Received: {JsonSerializer.Serialize(data)}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling client: {ex.Message}");
        }
        finally
        {
            ActivePlayers.TryRemove(player.Id, out _);

            await BroadcastPlayerLeftAsync(player.Id);

            outboundChannel.Writer.Complete();
            try
            {
                await writeTask;
            }
            catch
            {
                // TODO: Implement
            }

            client.Close();
            Console.WriteLine("Client connection closed.");
        }
    }

    private static async Task BroadcastPlayerJoinedAsync(int playerId)
    {
        var buffer = new byte[256];
        var writer = new BitWriter(buffer);
        var builder = new ServerPacketBuilder(ref writer);

        var packet = new ServerPacket
        {
            Register =
            [
                new ServerRegisterPacket { Register = true, Id = (ushort)playerId }
            ]
        };

        var size = builder.Write(packet);
        var finalPacket = WrapWithLength(buffer, size);

        foreach (var kvp in ActivePlayers)
        {
            if (kvp.Key == playerId)
            {
                continue;
            }

            await kvp.Value.WriteAsync(finalPacket);
        }
    }

    private static async Task BroadcastPlayerLeftAsync(int playerId)
    {
        var buffer = new byte[256];
        var writer = new BitWriter(buffer);
        var builder = new ServerPacketBuilder(ref writer);

        var packet = new ServerPacket
        {
            Register =
            [
                new ServerRegisterPacket { Register = false, Id = (ushort)playerId }
            ]
        };

        var size = builder.Write(packet);
        var finalPacket = WrapWithLength(buffer, size);

        foreach (var kvp in ActivePlayers)
        {
            await kvp.Value.WriteAsync(finalPacket);
        }
    }

    private static async Task ProcessOutboundQueueAsync(Socket client, ChannelReader<byte[]> reader, Player player)
    {
        try
        {
            await foreach (var packet in reader.ReadAllAsync())
            {
                await client.SendAsync(packet, SocketFlags.None);
            }
        }
        catch
        {
            // TODO: Implement
        }
    }

    private static async Task<int> ReceiveAllAsync(Socket client, byte[] buffer)
    {
        var totalBytesRead = 0;

        while (totalBytesRead < buffer.Length)
        {
            var bytesRead = await client.ReceiveAsync(new Memory<byte>(buffer, totalBytesRead, buffer.Length - totalBytesRead), SocketFlags.None);

            if (bytesRead == 0)
            {
                return 0;
            }

            totalBytesRead += bytesRead;
        }

        return totalBytesRead;
    }

    private static byte[] WrapWithLength(byte[] buffer, int payloadSize)
    {
        var finalPacket = new byte[4 + payloadSize];
        BitConverter.GetBytes(payloadSize).CopyTo(finalPacket, 0);
        Buffer.BlockCopy(buffer, 0, finalPacket, 4, payloadSize);
        return finalPacket;
    }
}