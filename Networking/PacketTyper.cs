

using System;
using System.Collections.Generic;
using Game;
using Server;

public static class PacketTypes {
    static byte id = 0;
    public static Dictionary<Type, byte> PacketType = new Dictionary<Type, byte>();
    public static Dictionary<byte, Type> PacketTypeReverse = new Dictionary<byte, Type>();
    public static void RegisterPacketType(Type type){
        Console.WriteLine($"Packet Typer:  typing at {id} for {type} ");
        PacketType[type] = id;
        PacketTypeReverse[id] = type;
        id++;
    }
    public static void RegisterPacketTypes() {
        var types = UnamedGame.Instance.GetType().Assembly.GetTypes();
        foreach(var type in types) {
            if(type.IsSubclassOf(typeof(Packet) ) && !type.IsAbstract){
                RegisterPacketType(type);
            }
        }
    }
    public static Type GetPacketType(byte id){
        return PacketTypeReverse[id];
    }

    public static byte GetPacketType<T>() where T : Packet{
        return PacketType[typeof(T)];
    }
    public static byte GetPacketType(Packet p) {
        return PacketType[p.GetType()];
    }
}