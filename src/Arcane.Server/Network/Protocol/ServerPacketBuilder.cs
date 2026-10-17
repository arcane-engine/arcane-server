using Arcane.Server.Core;
using Arcane.Server.Network.Protocol.Packets;

namespace Arcane.Server.Network.Protocol;

internal ref struct ServerPacketBuilder(ref BitWriter writer)
{
    private BitWriter _writer = writer;

    public int Write(ServerPacket packet)
    {
        _writer.WriteBit(packet.Actor.Count != 0);
        if (packet.Actor.Count != 0)
        {
            _writer.WriteUnsignedShort((ushort)packet.Actor.Count);
            foreach (var item in packet.Actor)
            {
                _writer.WriteUnsignedShort(item.Id);
                _writer.WriteBit(item.Register);
            }
        }
        _writer.WriteBit(packet.ActorMoved.Count != 0);
        if (packet.ActorMoved.Count != 0)
        {
            _writer.WriteUnsignedShort((ushort)packet.ActorMoved.Count);
            foreach (var item in packet.ActorMoved)
            {
                _writer.WriteUnsignedShort(item.Id);
                _writer.WriteFloat(item.Position.X);
                _writer.WriteFloat(item.Position.Y);
                _writer.WriteFloat(item.Position.Z);
                _writer.WriteFloat(item.Velocity.X);
                _writer.WriteFloat(item.Velocity.Y);
                _writer.WriteFloat(item.Velocity.Z);
            }
        }

        return _writer.Size;
    }
}