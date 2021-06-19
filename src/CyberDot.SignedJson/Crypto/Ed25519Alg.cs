using System;
using CyberDot.SignedJson.Signing;
using CyberDot.SignedJson.Verifying;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Security;

namespace CyberDot.SignedJson.Crypto
{
    public class Ed25519Alg : ICryptoAlg
    {
        public string Alg => "ed25519";
        public string CreateSignature(SigningKey key, byte[] message)
        {
            var privateKey = ToPrivateKey(key.Key);
            
            var signer = new Ed25519Signer();
            signer.Init(true, privateKey);
            signer.BlockUpdate(message, 0, message.Length);

            var signature = signer.GenerateSignature();
            return Convert.ToBase64String(signature, Base64FormattingOptions.None);
        }

        public bool VerifySignature(VerifyKey key, byte[] signature, byte[] message)
        {
            var verifier = new Ed25519Signer();
            verifier.Init(false, ToPublicKey(key.Key));
            
            verifier.BlockUpdate(message, 0, message.Length);
            
            return verifier.VerifySignature(signature);
        }

        public byte[] DerivePublicKey(byte[] privateKeyData)
        {
            var parameter = new Ed25519PrivateKeyParameters(privateKeyData, 0);
            return parameter.GeneratePublicKey().GetEncoded();
        }

        public (SigningKey, VerifyKey) GenerateKeyPair(int keyVersion)
        {
            var parameter = new Ed25519PrivateKeyParameters(new SecureRandom());
            var signingKey = new SigningKey(Alg, keyVersion, Convert.ToBase64String(parameter.GetEncoded()));
            var verifyKey = new VerifyKey(Alg, keyVersion, Convert.ToBase64String(parameter.GeneratePublicKey().GetEncoded()));
            return (signingKey, verifyKey);
        }

        private Ed25519PrivateKeyParameters ToPrivateKey(string base64)
        {
            var bytes = Convert.FromBase64String(base64);
            return ToPrivateKey(bytes);
        }
        
        private static Ed25519PrivateKeyParameters ToPrivateKey(byte[] keyData)
        {
            if (keyData.Length < Ed25519PrivateKeyParameters.KeySize)
            {
                throw new ArgumentException("Array length is less than required: " + keyData.Length);
            }

            return new Ed25519PrivateKeyParameters(keyData, 0);
        }

        public Ed25519PublicKeyParameters ToPublicKey(byte[] keyData)
        {
            return new Ed25519PublicKeyParameters(keyData, 0);
        }
    }
}