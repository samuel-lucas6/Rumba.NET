namespace RumbaDotNet;

public static class Rumba8
{
    public const int OutputSize = Rumba.OutputSize;
    public const int MessageSize = Rumba.MessageSize;

    public static void Compress(Span<byte> output, ReadOnlySpan<byte> message)
    {
        Rumba.Compress(output, message, rounds: 8);
    }
}
