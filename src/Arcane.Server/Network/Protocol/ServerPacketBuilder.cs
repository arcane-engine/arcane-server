using Arcane.Server.Core;
using Arcane.Server.Network.Protocol.Packets;

namespace Arcane.Server.Network.Protocol;

internal ref struct ServerPacketBuilder(ref BitWriter writer)
{
    private BitWriter _writer = writer;

    public int Write(ServerPacket packet)
    {
        _writer.WriteBit(packet.Register.Count != 0);
        if (packet.Register.Count != 0)
        {
            _writer.WriteUnsignedShort((ushort)packet.Register.Count);
            foreach (var item in packet.Register)
            {
                _writer.WriteBit(item.Register);
                _writer.WriteUnsignedShort(item.Id);
            }
        }

        return _writer.Size;
    }
}