using System;

namespace CyberDot.SignedJson.Verifying
{
    public class VerifyKey
    {
        public VerifyKey(string alg, int version, string base64Key)
        {
            Alg = alg;
            Version = version;
            Key = Convert.FromBase64String(base64Key.AddPadding());
        }

        public VerifyKey(string alg, int version, byte[] keyData)
        {
            Alg = alg;
            Version = version;
            Key = keyData;
        }
        public string Alg { get; }
        public int Version { get; }
        public byte[] Key { get; }
        
    }
}