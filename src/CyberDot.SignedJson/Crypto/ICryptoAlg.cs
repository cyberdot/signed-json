using CyberDot.SignedJson.Signing;
using CyberDot.SignedJson.Verifying;

namespace CyberDot.SignedJson.Crypto
{
    public interface ICryptoAlg
    {
        string Alg { get; }
        string CreateSignature(SigningKey key, byte[] message);
        bool VerifySignature(VerifyKey key, byte[] signature, byte[] message);
    }
}