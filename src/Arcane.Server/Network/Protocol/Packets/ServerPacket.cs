namespace Arcane.Server.Network.Protocol.Packets;

internal struct ServerPacket
{
    public List<ServerActorPacket> Actor { get; set; }
    public List<ServerActorMovedPacket> ActorMoved { get; set; }
    public List<ServerSceneNodePacket> SceneNodes { get; set; }
}

internal struct ServerActorPacket
{
    public ushort Id { get; set; }
    public bool Register { get; set; }
}

internal struct ServerActorMovedPacket
{
    public ushort Id { get; set; }
    public Float3 Position { get; set; }
    public Float3 Velocity { get; set; }
}

internal struct ServerSceneNodePacket
{
    public ushort Id { get; set; }
    public Float3 Position { get; set; }
    public Float4 Rotation { get; set; }
    public Float3 Scale { get; set; }
}