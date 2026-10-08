namespace Arcane.Server.Core;

internal ref struct BitReader
{
    private readonly ReadOnlySpan<byte> _buffer;
    private int _offset;

    public BitReader(ReadOnlySpan<byte> buffer)
    {
        _buffer = buffer;
    }

    public bool ReadBit()
    {
        var byteIndex = _offset / 8;
        var bitIndex = 7 - _offset % 8;
        _offset++;

        return (_buffer[byteIndex] & (1 << bitIndex)) != 0;
    }

    public ushort ReadUShort()
    {
        ushort value = 0;
        for (var i = 0; i < 16; i++)
        {
            value = (ushort)((value << 1) | (ReadBit() ? 1 : 0));
        }

        return value;
    }

    public void Skip(int bits)
    {
        _offset += bits;
    }

    public int CurrentOffset => _offset;
}