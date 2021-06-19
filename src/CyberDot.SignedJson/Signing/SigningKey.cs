using System;

namespace CyberDot.SignedJson.Signing
{
    public class SigningKey
    {
        public SigningKey(string alg, int version, string keyAsBase64)
        {
            Alg = alg;
            Version = version;
            Key = Convert.FromBase64String(keyAsBase64.AddPadding());
        }
        public int Version { get; }
        public string Alg { get; }
        public byte[] Key { get; }
    }
}