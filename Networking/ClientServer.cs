

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Server;
using Server.Packets;

namespace Game.GameSystem.Networking;

public class ClientServer
{
    public static void RunPackets()
    {
        for (int i = 0; i < packetsToRun.Count; i++)
        {
            packetsToRun[i].ClientReceive();
        }
        packetsToRun.Clear();
    }
    public static List<ServerOriginatingPacket> packetsToRun = new List<ServerOriginatingPacket>();
    UdpClient client = new UdpClient();
    public ClientServer()
    {
        Console.WriteLine("Client started");
        client.Connect(Server.Server.ServerURL, Server.Server.port);
    }
    public async void ListenForPackets()
    {
        while (true)
        {
            try
            {
                Console.WriteLine($"listening on {client.Client.LocalEndPoint}");
                UdpReceiveResult result = await client.ReceiveAsync();
                byte[] data = result.Buffer;
                Console.WriteLine("received");
                Console.WriteLine(data[0]);
                Console.WriteLine(PacketTypes.PacketTypeReverse);
                if (PacketTypes.PacketTypeReverse.TryGetValue(data[0], out Type p))
                {
                    ServerOriginatingPacket packet = (ServerOriginatingPacket)Activator.CreateInstance(p, [data.Skip(1).ToArray()]);
                    packetsToRun.Add(packet);
                }
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.TimedOut)
                {
                    Console.WriteLine("Receive timed out");
                    continue; // Continue listening for packets
                }
                else
                {
                    Console.WriteLine($"SocketException: {ex.Message}");
                    break; // Exit the loop on other socket errors
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                break; // Exit the loop on other exceptions
            }
        }
    }

    public async void init()
    {
        await Task.Run(ListenForPackets);
    }

    public void Send(Packet packet)
    {
        Type t = packet.GetType();
        if (PacketTypes.PacketType.TryGetValue(t, out byte v))
        {
            byte[] data = [v];
            data = data.Concat(packet.Send()).ToArray();
            client.Send(data);
        }
    }

}