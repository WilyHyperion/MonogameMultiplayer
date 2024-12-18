

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Server;
using Server.Packets;

namespace Game.GameSystem.Networking;

public class ClientServer
{
    public static void RunPackets(){
        for(int i = 0; i < packetsToRun.Count; i++){
            packetsToRun[i].ClientReceive();
        }
        packetsToRun.Clear();
    }
    public static List<ServerOriginatingPacket> packetsToRun = new List<ServerOriginatingPacket>();
    static IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
    UdpClient client = new UdpClient();
    public ClientServer()
    {
        client.Connect(Server.Server.ServerURL, 8080);
    }
    public void ListenForPackets()
    {
        while (true)
        {
            byte[] data = client.Receive(ref ep);
            if (PacketTypes.PacketTypeReverse.TryGetValue(data[0], out Type p))
            {
                Console.WriteLine(p);
                ServerOriginatingPacket packet = (ServerOriginatingPacket)Activator.CreateInstance(p, [data.Skip(1)]);
                packetsToRun.Add(packet);
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