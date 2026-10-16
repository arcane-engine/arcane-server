namespace Arcane.Server.Network.Protocol.Packets;

internal struct ServerPacket
{
    public List<ServerActorPacket> Actor { get; set; }
}

internal struct ServerActorPacket
{
    public bool Register { get; set; }
    public ushort Id { get; set; }
}
