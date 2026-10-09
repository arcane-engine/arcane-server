using Arcane.Server.Core;
using Arcane.Server.Network.Protocol.Packets;

namespace Arcane.Server.Network.Protocol;

internal ref struct ClientPacketReader(ref BitReader reader)
{
    private BitReader _reader = reader;

    public ClientPacket Read()
    {
        return new ClientPacket
        {
            Input = new ClientInputPacketReader(ref _reader).Read()
        };
    }
}