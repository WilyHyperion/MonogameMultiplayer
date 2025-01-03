

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
    public static List<ServerOriginatingPacket> packetsToRun = new();
    UdpClient client = new();
    public ClientServer()
    {
        client.Connect(Server.Server.ServerURL, Server.Server.port);
    }
    public async void ListenForPackets()
    {
        Console.WriteLine($"listening on {client.Client.LocalEndPoint}, connecting to {Server.Server.ServerURL}  : {Server.Server.port}");
        while (true)
        {
            try
            {
                UdpReceiveResult result = await client.ReceiveAsync();
                byte[] data = result.Buffer;
                if (PacketTypes.PacketTypeReverse.TryGetValue(data[0], out Type p))
                {
                    Console.WriteLine("Packet Recived  " + p);
                    ServerOriginatingPacket packet = (ServerOriginatingPacket)Activator.CreateInstance(p, [data.Skip(1).ToArray()]);
                    Console.WriteLine("Packet Recived");
                    packetsToRun.Add(packet);
                }
                else {
                    Console.WriteLine("unkown packet with type " + data[0]);
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

        PacketTypes.RegisterPacketTypes();
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
        else
        {
            Console.WriteLine("failed to send packet - invaild or unregistered type");
        }
    }

}