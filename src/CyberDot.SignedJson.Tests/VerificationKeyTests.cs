using System;
using CyberDot.SignedJson.Crypto;
using CyberDot.SignedJson.Verifying;
using Xunit;

namespace CyberDot.SignedJson.Tests
{
    public class VerificationKeyTests
    {
        private const string SigningKeyAsBase64 = "YJDBA9Xnr2sVqXD9Vj7XVUnmFZcZrlw8Md7kMW+3XA1";

        private readonly Verifier verifier;
        private readonly Ed25519Alg cryptoAlg;

        public VerificationKeyTests()
        {
            cryptoAlg = new Ed25519Alg();
            verifier = new Verifier(cryptoAlg);
        }

        [Fact]
        public void Verify_minimal()
        {
            var signedJson = JsonReader.ReadJson("data/json_type1.json");
            var publicKey = cryptoAlg.DerivePublicKey(Convert.FromBase64String(SigningKeyAsBase64.AddPadding()));
           
            var verifyKey = new VerifyKey(cryptoAlg.Alg, 1, publicKey);
            
            verifier.VerifySignedJson(signedJson, "domain", verifyKey);
        }

        [Fact]
        public void Verify_with_data()
        {
            var signedJson = JsonReader.ReadJson("data/json_type2.json");
            var publicKey = cryptoAlg.DerivePublicKey(Convert.FromBase64String(SigningKeyAsBase64.AddPadding()));
           
            var verifyKey = new VerifyKey(cryptoAlg.Alg, 1, publicKey);
            
            verifier.VerifySignedJson(signedJson, "domain", verifyKey);
        }

        [Fact]
        public void Verify_with_unsigned_properties()
        {
            var signedJson = JsonReader.ReadJson("data/json_type4.json");
            var publicKey = cryptoAlg.DerivePublicKey(Convert.FromBase64String(SigningKeyAsBase64.AddPadding()));
            var verifyKey = new VerifyKey(cryptoAlg.Alg, 1, publicKey);
            
            verifier.VerifySignedJson(signedJson, "domain", verifyKey);
        }
    }
}