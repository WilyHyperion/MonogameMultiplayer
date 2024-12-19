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
using Server.Packets;
using Server.Packets.ServerSided;

namespace Server
{
    public class Server
    {
        public List<Entity> entities = new List<Entity>();
        public CollisionManager collisionManager = new CollisionManager(5000, 5000);
        public const String ServerURL = "localhost";
        public static void SendPacket<T>(IPEndPoint ip) where T : ServerOriginatingPacket
        {
            var obj = Activator.CreateInstance(typeof(T));
            SendPacket((ServerOriginatingPacket)obj, ip);
        }
        public static void SendPacket(ServerOriginatingPacket p, IPEndPoint ip)
        {
            byte[] data = p.Send();
            byte typer = PacketTypes.GetPacketType(p);
            byte[] res = new byte[] { typer }.Concat(data).ToArray();
            Instance.listener.Send(res, res.Length, ip);
        }
        public Dictionary<IPEndPoint, ServerPlayer> connected = new Dictionary<IPEndPoint, ServerPlayer>();
        public static Server Instance;
        public Server()
        {
            Instance = this;

        }
        public const int port = 8080;
        public async void SendReSyncPackets()
        {
            while (true)
            {
                await Task.Delay(1000);
                ResyncAllClients();
            }
        }
        public void ResyncAllClients()
        {

            for (int i = 0; i < connected.Keys.Count; i++)
            {
                var endpoint = connected.Keys.ElementAt(i);
                Server.SendPacket<ReSyncPacket>(endpoint);
            }
        }
        UdpClient listener = new UdpClient(port);
        public async void Start()
        {
            Console.WriteLine("running startup");
            Console.WriteLine("Server started");
            try
            {
                await Task.Run(() => { SendReSyncPackets(); });
                while (true)
                {
                    IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, port);
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
                    else
                    {

                        ServerPlayer serverPlayer = new ServerPlayer(new PlayerEntity(), from);
                        connected.Add(from, serverPlayer);
                        serverPlayer.id = connected.Count - 1;
                        clientpacket.ServerReceive(serverPlayer);
                        ConnectRecive connect = new ConnectRecive();
                        connect.ID = (byte)serverPlayer.id;
                        Server.SendPacket(connect, from);
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
        public int id{
            get{
                return player.ID;
            }
            set {
                player.ID = value;
            }
        }
        public PlayerEntity player;
        public IPEndPoint IP;
        public ServerPlayer(PlayerEntity p, IPEndPoint from)
        {
            this.player = p;
            
            this.IP = from;
        }
    }
}