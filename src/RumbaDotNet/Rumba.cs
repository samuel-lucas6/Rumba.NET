using System.Buffers.Binary;
using System.Security.Cryptography;

namespace RumbaDotNet;

internal static class Rumba
{
    internal const int OutputSize = 64;
    internal const int MessageSize = 192;

    internal static void Compress(Span<byte> output, ReadOnlySpan<byte> message, int rounds)
    {
        if (output.Length != OutputSize) { throw new ArgumentOutOfRangeException(nameof(output), output.Length, $"{nameof(output)} must be {OutputSize} bytes long."); }
        if (message.Length != MessageSize) { throw new ArgumentOutOfRangeException(nameof(message), $"{nameof(message)} must be {MessageSize} bytes long."); }
        if (rounds != 20 && rounds != 12 && rounds != 8) { throw new ArgumentOutOfRangeException(nameof(rounds), rounds, $"{nameof(rounds)} must be 8, 12, or 20."); }

        Span<byte> buffer = stackalloc byte[OutputSize];
        Salsa20Core(output, message[..48], "firstRumba20bloc"u8, rounds);
        Salsa20Core(buffer, message[48..96], "secondRumba20blo"u8, rounds);
        Xor(output, buffer);
        Salsa20Core(buffer, message[96..144], "thirdRumba20bloc"u8, rounds);
        Xor(output, buffer);
        Salsa20Core(buffer, message[144..], "fourthRumba20blo"u8, rounds);
        Xor(output, buffer);
        CryptographicOperations.ZeroMemory(buffer);
    }

    private static void Salsa20Core(Span<byte> output, ReadOnlySpan<byte> message, ReadOnlySpan<byte> constant, int rounds)
    {
        uint j0 = BinaryPrimitives.ReadUInt32LittleEndian(constant[..4]);
        uint j5 = BinaryPrimitives.ReadUInt32LittleEndian(constant[4..8]);
        uint j10 = BinaryPrimitives.ReadUInt32LittleEndian(constant[8..12]);
        uint j15 = BinaryPrimitives.ReadUInt32LittleEndian(constant[12..]);
        uint j1 = BinaryPrimitives.ReadUInt32LittleEndian(message[..4]);
        uint j2 = BinaryPrimitives.ReadUInt32LittleEndian(message[4..8]);
        uint j3 = BinaryPrimitives.ReadUInt32LittleEndian(message[8..12]);
        uint j4 = BinaryPrimitives.ReadUInt32LittleEndian(message[12..16]);
        uint j6 = BinaryPrimitives.ReadUInt32LittleEndian(message[16..20]);
        uint j7 = BinaryPrimitives.ReadUInt32LittleEndian(message[20..24]);
        uint j8 = BinaryPrimitives.ReadUInt32LittleEndian(message[24..28]);
        uint j9 = BinaryPrimitives.ReadUInt32LittleEndian(message[28..32]);
        uint j11 = BinaryPrimitives.ReadUInt32LittleEndian(message[32..36]);
        uint j12 = BinaryPrimitives.ReadUInt32LittleEndian(message[36..40]);
        uint j13 = BinaryPrimitives.ReadUInt32LittleEndian(message[40..44]);
        uint j14 = BinaryPrimitives.ReadUInt32LittleEndian(message[44..]);

        uint x0 = j0;
        uint x1 = j1;
        uint x2 = j2;
        uint x3 = j3;
        uint x4 = j4;
        uint x5 = j5;
        uint x6 = j6;
        uint x7 = j7;
        uint x8 = j8;
        uint x9 = j9;
        uint x10 = j10;
        uint x11 = j11;
        uint x12 = j12;
        uint x13 = j13;
        uint x14 = j14;
        uint x15 = j15;

        for (int i = 0; i < rounds / 2; i++) {
            QuarterRound(ref x0, ref x4, ref x8, ref x12);
            QuarterRound(ref x5, ref x9, ref x13, ref x1);
            QuarterRound(ref x10, ref x14, ref x2, ref x6);
            QuarterRound(ref x15, ref x3, ref x7, ref x11);
            QuarterRound(ref x0, ref x1, ref x2, ref x3);
            QuarterRound(ref x5, ref x6, ref x7, ref x4);
            QuarterRound(ref x10, ref x11, ref x8, ref x9);
            QuarterRound(ref x15, ref x12, ref x13, ref x14);
        }

        x0 += j0;
        x1 += j1;
        x2 += j2;
        x3 += j3;
        x4 += j4;
        x5 += j5;
        x6 += j6;
        x7 += j7;
        x8 += j8;
        x9 += j9;
        x10 += j10;
        x11 += j11;
        x12 += j12;
        x13 += j13;
        x14 += j14;
        x15 += j15;

        BinaryPrimitives.WriteUInt32LittleEndian(output[..4], x0);
        BinaryPrimitives.WriteUInt32LittleEndian(output[4..8], x1);
        BinaryPrimitives.WriteUInt32LittleEndian(output[8..12], x2);
        BinaryPrimitives.WriteUInt32LittleEndian(output[12..16], x3);
        BinaryPrimitives.WriteUInt32LittleEndian(output[16..20], x4);
        BinaryPrimitives.WriteUInt32LittleEndian(output[20..24], x5);
        BinaryPrimitives.WriteUInt32LittleEndian(output[24..28], x6);
        BinaryPrimitives.WriteUInt32LittleEndian(output[28..32], x7);
        BinaryPrimitives.WriteUInt32LittleEndian(output[32..36], x8);
        BinaryPrimitives.WriteUInt32LittleEndian(output[36..40], x9);
        BinaryPrimitives.WriteUInt32LittleEndian(output[40..44], x10);
        BinaryPrimitives.WriteUInt32LittleEndian(output[44..48], x11);
        BinaryPrimitives.WriteUInt32LittleEndian(output[48..52], x12);
        BinaryPrimitives.WriteUInt32LittleEndian(output[52..56], x13);
        BinaryPrimitives.WriteUInt32LittleEndian(output[56..60], x14);
        BinaryPrimitives.WriteUInt32LittleEndian(output[60..], x15);
    }

    private static void QuarterRound(ref uint a, ref uint b, ref uint c, ref uint d)
    {
        b ^= uint.RotateLeft(a + d, 7);
        c ^= uint.RotateLeft(b + a, 9);
        d ^= uint.RotateLeft(c + b, 13);
        a ^= uint.RotateLeft(d + c, 18);
    }

    private static void Xor(Span<byte> output, ReadOnlySpan<byte> buffer)
    {
        for (int i = 0; i < output.Length; i++) {
            output[i] ^= buffer[i];
        }
    }
}
