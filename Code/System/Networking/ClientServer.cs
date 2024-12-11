

using System;
using System.Net;
using System.Net.Sockets;
using Server;

namespace Game.GameSystem.Networking;

public class ClientServer {
    static IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
    UdpClient client = new UdpClient();
    public ClientServer(){
        client.Connect(Server.Server.ServerURL, 8080);
    }
    public void Send(Packet p ){
        byte[] send = p.Send();
        client.Send(send, send.Length);
        p.data = client.Receive(ref ep);
        Console.WriteLine(p.data.Length);
        foreach(byte b in p.data){
            Console.WriteLine(b);
        }
        p.ClientReceive();

    }
}