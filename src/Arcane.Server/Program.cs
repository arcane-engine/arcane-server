using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Arcane.Server;

internal class Program
{
    private static int Port => 5000;

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
                _ = HandleClientAsync(client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal server error: {ex.Message}");
            return 1;
        }
    }

    private static async Task HandleClientAsync(Socket client)
    {
        Console.WriteLine($"Client connected: {client.RemoteEndPoint}");

        try
        {
            var lengthBuffer = new byte[4];
            while (true)
            {
                var read = await ReceiveAllAsync(client, lengthBuffer);
                if (read == 0)
                {
                    break;
                }

                var messageLength = BitConverter.ToInt32(lengthBuffer, 0);

                var payloadBuffer = new byte[messageLength];
                await ReceiveAllAsync(client, payloadBuffer);

                var request = Encoding.UTF8.GetString(payloadBuffer);
                Console.WriteLine($"Received {messageLength} bytes: {request}");

                var responseData = "Hello, I'm Server."u8.ToArray();
                var responseLength = BitConverter.GetBytes(responseData.Length);

                await client.SendAsync(responseLength, SocketFlags.None);
                await client.SendAsync(responseData, SocketFlags.None);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling client: {ex.Message}");
        }
        finally
        {
            client.Close();
            Console.WriteLine("Client connection closed.");
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
}