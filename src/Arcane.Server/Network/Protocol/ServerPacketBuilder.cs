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
                _writer.WriteFloat3(item.Position);
                _writer.WriteFloat3(item.Velocity);
            }
        }

        _writer.WriteBit(packet.SceneNodes.Count != 0);
        if (packet.SceneNodes.Count != 0)
        {
            _writer.WriteUnsignedShort((ushort)packet.SceneNodes.Count);
            foreach (var sceneNode in packet.SceneNodes)
            {
                _writer.WriteUnsignedShort(sceneNode.Id);
                _writer.WriteFloat3(sceneNode.Position);
                _writer.WriteFloat4(sceneNode.Rotation);
                _writer.WriteFloat3(sceneNode.Scale);
            }
        }

        return _writer.Size;
    }
}