using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RumbaDotNet.Tests;

[TestClass]
public class RumbaTests
{
    // There are apparently no test vectors or other implementations on GitHub
    [TestMethod]
    public void Constants_Valid()
    {
        Assert.AreEqual(64, Rumba20.OutputSize);
        Assert.AreEqual(192, Rumba20.MessageSize);
        Assert.AreEqual(64, Rumba12.OutputSize);
        Assert.AreEqual(192, Rumba12.MessageSize);
        Assert.AreEqual(64, Rumba8.OutputSize);
        Assert.AreEqual(192, Rumba8.MessageSize);
    }

    [TestMethod]
    [DataRow("8c7522e697d9f2c82be781fb73adbb169ded91b170d5a79b7b1bd07b2e0c099ecf3e8d21456e83112fba68dc39e4cbb2d40f8cdcb92fb3006d26fb918b7f23b7", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f")]
    public void Rumba20_Compress_Valid(string output, string message)
    {
        Span<byte> o = stackalloc byte[output.Length / 2];
        Span<byte> m = Convert.FromHexString(message);

        Rumba20.Compress(o, m);

        Assert.AreEqual(output, Convert.ToHexString(o).ToLower());
    }

    [TestMethod]
    [DataRow("4603edb0bfeeb0d401ebe73826cfe6b01c0fbb4ac373a081c997e369ea0151310c5f5a9a80248c344c344ac1e9c6b41dbf79ebb2144d4b2977edf7756e66435e", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f")]
    public void Rumba12_Compress_Valid(string output, string message)
    {
        Span<byte> o = stackalloc byte[output.Length / 2];
        Span<byte> m = Convert.FromHexString(message);

        Rumba12.Compress(o, m);

        Assert.AreEqual(output, Convert.ToHexString(o).ToLower());
    }

    [TestMethod]
    [DataRow("b8c8cd1a364455743893d6a6f79206f91dac71b097deb3c373a40ceb0b240ceabcb20e3c4d197ec1317eb98b230678d4dcc6ff5045c14336d90afd9256b119b2", "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f")]
    public void Rumba8_Compress_Valid(string output, string message)
    {
        Span<byte> o = stackalloc byte[output.Length / 2];
        Span<byte> m = Convert.FromHexString(message);

        Rumba8.Compress(o, m);

        Assert.AreEqual(output, Convert.ToHexString(o).ToLower());
    }

    [TestMethod]
    [DataRow(Rumba20.OutputSize + 1, Rumba20.MessageSize)]
    [DataRow(Rumba20.OutputSize - 1, Rumba20.MessageSize)]
    [DataRow(Rumba20.OutputSize, Rumba20.MessageSize + 1)]
    [DataRow(Rumba20.OutputSize, Rumba20.MessageSize - 1)]
    public void Compress_Invalid(int outputSize, int messageSize)
    {
        var o = new byte[outputSize];
        var m = new byte[messageSize];

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Rumba20.Compress(o, m));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Rumba12.Compress(o, m));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Rumba8.Compress(o, m));
    }
}
