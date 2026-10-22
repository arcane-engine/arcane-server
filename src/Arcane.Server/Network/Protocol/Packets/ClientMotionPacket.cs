namespace Arcane.Server.Network.Protocol.Packets;

internal readonly struct Float3(float x, float y, float z)
{
    public float X { get; } = x;
    public float Y { get; } = y;
    public float Z { get; } = z;
}

internal readonly struct Float4(float x, float y, float z, float w)
{
    public float X { get; } = x;
    public float Y { get; } = y;
    public float Z { get; } = z;
    public float W { get; } = w;
}

internal struct ClientMotionPacket
{
    public Float3 Position { get; init; }
    public Float3 Velocity { get; init; }
}