using Newtonsoft.Json.Linq;

namespace CyberDot.SignedJson.Verifying
{
    public interface IVerifier
    {
        void VerifySignedJson(JObject json, string signatureName, VerifyKey verifyKey);
        void VerifySignedJson(string json, string signatureName, VerifyKey verifyKey);
    }
}