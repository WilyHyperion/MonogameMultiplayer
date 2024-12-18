using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Game.Abstract;
using Game.Player;
using Game.System.Collision;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace Server
{
    public class Server
    {
        public List<Entity> entities = new List<Entity>();
        public CollisionManager collisionManager = new CollisionManager(5000, 5000);
        public const String ServerURL = "localhost";
        public static void SendPacket<T>() where T : Packet
        {
            var obj = Activator.CreateInstance(typeof(T));
            byte[] b = ((Packet)obj).Send();
        }
        public Dictionary<IPEndPoint, ServerPlayer> connected = new Dictionary<IPEndPoint, ServerPlayer>();
        public static Server Instance;
        public Server()
        {
            Instance = this;
            PacketTypes.RegisterPacketTypes();

        }
        const int port = 8080;
        public async void SendReSyncPackets(){
            await Task.Delay(1000/60);
        }
        public async void Start()
        {
            Console.WriteLine("running startup");
            UdpClient listener = new UdpClient(port);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, port);
            Console.WriteLine("Server started");
            try
            {
                await Task.Run(() => {SendReSyncPackets();});
                while (true)
                {
                    Console.WriteLine("Waiting for broadcast");
                    byte[] bytes = listener.Receive(ref groupEP);
                    Console.WriteLine("Received broadcast from {0} :\n {1}\n", groupEP.ToString(), bytes.Length);
                    await Task.Run(() => { HandlePacket(bytes, groupEP); });
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
        public void HandlePacket(byte[] packet, IPEndPoint from)
        {
            byte type = packet[0];
            if (PacketTypes.PacketTypeReverse.TryGetValue(type, out Type p))
            {
                object oo = Activator.CreateInstance(p, [packet.Skip(1).ToArray(), from]);
                if (oo is ClientOrigniatingPacket clientpacket)
                {
                    if (connected.TryGetValue(from, out ServerPlayer player))
                    {
                        clientpacket.ServerReceive(player);
                    }
                    else {
                        ServerPlayer serverPlayer = new ServerPlayer(new PlayerEntity(), from);
                        connected.Add(from, serverPlayer);
                        clientpacket.ServerReceive(serverPlayer);
                    }
                }
            }
            else
            {
                Console.WriteLine("No Type Found for " + type);
            }
        }
    }

    public class ServerPlayer
    {
        public PlayerEntity player;
        public IPEndPoint IP;
        public ServerPlayer(PlayerEntity p, IPEndPoint from)
        {
            this.player = p;
            this.IP = from;
        }
    }
}