using System.Collections.Generic;
using System.Text;
using CyberDot.SignedJson.Crypto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CyberDot.SignedJson.Signing
{
    public class Signer : ISigner
    {
        private readonly Dictionary<string, ICryptoAlg> cryptoAlgs = new Dictionary<string, ICryptoAlg>();

        public Signer(ICryptoAlg alg)
        {
            cryptoAlgs.Add(alg.Alg, alg);            
        }

        public Signer(IEnumerable<ICryptoAlg> algs)
        {
            foreach (var alg in algs)
            {
                cryptoAlgs.Add(alg.Alg, alg);
            }
        }

        public JObject SignJson(JObject json, string signatureName, SigningKey signingKey)
        {
            var alg = cryptoAlgs.GetCryptoAlg(signingKey.Alg);

            var signatures = json[PropertyKeys.SignaturesProperty] != null
                ? json[PropertyKeys.SignaturesProperty].DeepClone()
                : new JObject();
            var unsigned = json[PropertyKeys.UnsignedProperty]?.DeepClone();

            json[PropertyKeys.SignaturesProperty]?.Parent.Remove();
            json[PropertyKeys.UnsignedProperty]?.Parent.Remove();

            var messageBytes = Encoding.UTF8.GetBytes(json.ToString(Formatting.None));
            var signature = alg.CreateSignature(signingKey, messageBytes).RemovePadding();

            var keyId = $"{signingKey.Alg}:{signingKey.Version}";

            var obj = signatures[signatureName] as JObject;
            if (signatures[signatureName] == null)
            {
                signatures[signatureName] = new JObject(new JProperty(keyId, signature));
            }
            else
            {
                obj.Add(new JProperty(keyId, signature));
            }

            json.Add(new JProperty(PropertyKeys.SignaturesProperty, signatures));

            if (unsigned != null)
            {
                json.Add(new JProperty(PropertyKeys.UnsignedProperty, unsigned));
            }

            return json;
        }

        public string SignJson(string json, string signatureName, SigningKey signingKey)
        {
            var jsonObject = JObject.Parse(json);

            var signedJson = SignJson(jsonObject, signatureName, signingKey);

            return signedJson.ToString(Formatting.None);
        }
    }
}