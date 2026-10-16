using Arcane.Server.Core;
using Arcane.Server.Network.Protocol.Packets;

namespace Arcane.Server.Network.Protocol;

internal ref struct ClientPacketReader
{
    public static ClientPacket Read(ref BitReader reader)
    {
        return new ClientPacket
        {
            Input = ClientInputPacketReader.Read(ref reader),
            Motion = ClientMotionPacketReader.Read(ref reader)
        };
    }
}