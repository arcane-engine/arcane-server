using Arcane.Server.Network.Protocol.Packets;

namespace Arcane.Server.Core;

internal ref struct BitReader(ReadOnlySpan<byte> buffer)
{
    private readonly ReadOnlySpan<byte> _buffer = buffer;
    private int _offset;

    public bool ReadBit()
    {
        var byteIndex = _offset / 8;
        var bitIndex = 7 - _offset % 8;
        _offset++;

        return (_buffer[byteIndex] & (1 << bitIndex)) != 0;
    }

    public float ReadFloat()
    {
        uint value = 0;
        for (var i = 31; i >= 0; i--)
        {
            if (ReadBit())
            {
                value |= 1u << i;
            }
        }

        return BitConverter.Int32BitsToSingle((int)value);
    }

    public Float3 ReadFloat3()
    {
        return new Float3(ReadFloat(), ReadFloat(), ReadFloat());
    }

    public void Skip(int bits)
    {
        _offset += bits;
    }
}