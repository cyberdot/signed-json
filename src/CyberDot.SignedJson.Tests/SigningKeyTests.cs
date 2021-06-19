using System;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CyberDot.SignedJson.Crypto;
using CyberDot.SignedJson.Signing;
using Xunit;

namespace CyberDot.SignedJson.Tests
{
    public class SigningKeyTests
    {
        private const string SigningKeyAsBase64 = "YJDBA9Xnr2sVqXD9Vj7XVUnmFZcZrlw8Md7kMW+3XA1";
        private const string AltKeyAsBase64 = "YXNkZmFzZGZhc2RmYXNkZmFzZGZhc2RmYXNkZmFzZGZhc2Rm";

        private readonly Signer signer;
        private readonly Ed25519Alg cryptoAlg;

        public SigningKeyTests()
        {
            cryptoAlg = new Ed25519Alg();
            signer = new Signer(cryptoAlg);
        }

        [Fact]
        public void Sign_minimal()
        {
            var expectedJson = JsonReader.ReadJson("data/json_type1.json");
            var signingKey = new SigningKey(cryptoAlg.Alg, 1, SigningKeyAsBase64);
            
            var signedJson = signer.SignJson(JObject.Parse("{}"), "domain", signingKey);
            
            signedJson.ToString(Formatting.None).Should().Be(expectedJson);
        }

        [Fact]
        public void Sign_with_data()
        {
            var signingKey = new SigningKey(cryptoAlg.Alg, 1, SigningKeyAsBase64);
            var expectedJson = JsonReader.ReadJson("data/json_type2.json");
            var json = JObject.Parse("{ 'one': 1, 'two': \"Two\"}");
            
            var signedJson = signer.SignJson(json, "domain", signingKey);
            
            signedJson.ToString(Formatting.None).Should().Be(expectedJson);
        }

        [Fact]
        public void Sign_with_unsigned_properties()
        {
            var signingKey = new SigningKey(cryptoAlg.Alg, 1, SigningKeyAsBase64);
            var json = JObject.Parse(JsonReader.ReadJson("data/json_type3.json"));

            var signedJson = signer.SignJson(json, "domain", signingKey);

            signedJson["signatures"].Should().NotBeNull();
        }

        [Fact]
        public void More_than_one_entity_can_sign_same_object()
        {
            var signingKey = new SigningKey(cryptoAlg.Alg, 1, SigningKeyAsBase64);
            var json = JObject.Parse(JsonReader.ReadJson("data/json_type4.json"));

            var signedJson = signer.SignJson(json, "entity2", signingKey);

            var signatures = signedJson["signatures"];
            signatures["entity2"].Should().NotBeNull();
            signatures["domain"].Should().NotBeNull();
        }

        [Fact]
        public void Single_entity_can_have_multiple_signatures()
        {
            var signingKey = new SigningKey(cryptoAlg.Alg, 2, AltKeyAsBase64);
            var json = JObject.Parse(JsonReader.ReadJson("data/json_type5.json"));

            var signedJson = signer.SignJson(json, "domain", signingKey);
            
            var signatures = signedJson["signatures"];
            signatures["domain"]["ed25519:1"].Should().NotBeNull();
            signatures["domain"]["ed25519:2"].Should().NotBeNull();
        }

        [Fact]
        public void Override_existing_signature_for_an_entity_should_raise_exception()
        {
            var signingKey = new SigningKey(cryptoAlg.Alg, 1, AltKeyAsBase64);
            var json = JObject.Parse(JsonReader.ReadJson("data/json_type5.json"));

            Action act = () => signer.SignJson(json, "domain", signingKey);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Signature_should_not_change_if_unsigned_property_collection_is_different()
        {
            var signingKey = new SigningKey(cryptoAlg.Alg, 1, SigningKeyAsBase64);
            var json = JObject.Parse(JsonReader.ReadJson("data/json_type6.json"));
            var existingSignature = json["signatures"]["domain"]["ed25519:1"].Value<string>();

            var signedJson = signer.SignJson(json, "entity", signingKey);

            signedJson["signatures"]["entity"]["ed25519:1"].Value<string>().Should().Be(existingSignature);
        }
    }
}