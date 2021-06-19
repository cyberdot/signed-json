using Newtonsoft.Json.Linq;

namespace CyberDot.SignedJson.Signing
{
    public interface ISigner
    {
        JObject SignJson(JObject json, string signatureName, SigningKey signingKey);
        string SignJson(string json, string signatureName, SigningKey signingKey);
    }
}