using System;
using System.Collections.Generic;
using System.Text;
using CyberDot.SignedJson.Crypto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CyberDot.SignedJson.Verifying
{
    public class Verifier : IVerifier
    {
        private readonly Dictionary<string, ICryptoAlg> cryptoAlgs = new Dictionary<string, ICryptoAlg>();

        public Verifier(ICryptoAlg alg)
        {
            cryptoAlgs.Add(alg.Alg, alg);   
        }
        
        public Verifier(IEnumerable<ICryptoAlg> algs)
        {
            foreach (var alg in algs)
            {
                cryptoAlgs.Add(alg.Alg, alg);
            }
        }
        
        public void VerifySignedJson(JObject json, string signatureName, VerifyKey verifyKey)
        {
            var alg = cryptoAlgs.GetCryptoAlg(verifyKey.Alg);
            
            var keyId = $"{verifyKey.Alg}:{verifyKey.Version}";
            var signatures = json[PropertyKeys.SignaturesProperty];
            if (signatures == null)
            {
                throw new VerifySignatureException("No signatures on this object");
            }

            var signature = signatures[signatureName]?[keyId];
            if (signature == null)
            {
                throw new VerifySignatureException($"Missing signature for {signatureName}, {keyId}, {signature.Value<string>()}");
            }
            var signatureData = Convert.FromBase64String(signature.Value<string>().AddPadding());

            json.Remove(PropertyKeys.SignaturesProperty);
            json.Remove(PropertyKeys.UnsignedProperty);

            var message = Encoding.UTF8.GetBytes(json.ToString(Formatting.None));
            if (!alg.VerifySignature(verifyKey, signatureData, message))
            {
                throw new VerifySignatureException($"Unable to verify signature for {signatureName}, {signature.Value<string>()}, {json.ToString(Formatting.None)}");
            }
        }

        public void VerifySignedJson(string json, string signatureName, VerifyKey verifyKey)
        {
            var jsonObject = JObject.Parse(json);
            VerifySignedJson(jsonObject, signatureName, verifyKey);
        }
    }
}