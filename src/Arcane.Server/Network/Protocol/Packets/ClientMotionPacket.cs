namespace Arcane.Server.Network.Protocol.Packets;

internal struct Float3
{
    public float X { get; init; }
    public float Y { get; init; }
    public float Z { get; init; }
}

internal struct ClientMotionPacket
{
    public Float3 Position { get; init; }
    public Float3 Velocity { get; init; }
}