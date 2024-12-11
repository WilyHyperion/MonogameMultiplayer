using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Game.Player;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace Server
{
    public class Server
    {
        public const String ServerURL = "localhost";
        public static void SendPacket<T>() where T : Packet
        {
            var obj = Activator.CreateInstance(typeof(T));
            byte[] b = ((Packet)obj).Send();
        }
        public List<PlayerEntity> players = new List<PlayerEntity>();
        public static Server Instance;
        public Server()
        {
            Instance = this;
         PacketTypes.RegisterPacketTypes();

        }
        const int port = 8080;
        public async void Start()
        {
            Console.WriteLine("running startup");
            UdpClient listener = new UdpClient(port);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, port);
            Console.WriteLine("Server started");
            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for broadcast");
                    byte[] bytes = listener.Receive(ref groupEP);
                    Console.WriteLine("Received broadcast from {0} :\n {1}\n", groupEP.ToString(), bytes);
                    byte[] response = await HandlePacket(bytes);
                    listener.Send(response, response.Length, groupEP);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                listener.Close();
            }

        }
        public async Task<Byte[]> HandlePacket(byte[] packet)
        {
            byte type = packet[0];
            if (PacketTypes.PacketTypeReverse.TryGetValue(type, out Type p))
            {
                return ((Packet)Activator.CreateInstance(p, new Object[] { packet })).ServerHandle();
            }
            return new byte[0];
        }
    }
}