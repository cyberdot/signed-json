Signed JSON
===========

Signs JSON objects with ED25519 signatures

Features
--------

* More than one entity can sign the same object.
* Each entity can sign the object with more than one key making it easier to
  rotate keys
* ED25519 can be replaced with a different algorithm.
* Unprotected data can be added to the object under the ``"unsigned"`` key.

Installing
----------

``` cli
   dotnet add package CyberDot.SignedJson
```

Using
-----

### Sign JSON

``` c#
using CyberDot.SignedJson.Crypto;
using CyberDot.SignedJson.Signing;
using Newtonsoft.Json.Linq;

namespace CyberDot.SignedJson.Example
{
    public class SignJsonExample
    {
        public void Sign()
        {
            var cryptoAlg = new Ed25519Alg();
            var signer = new Signer(cryptoAlg);

            var json = JObject.Parse("{}");
            var privateKeyAsBase64 = "YJDBA9Xnr2sVqXD9Vj7XVUnmFZcZrlw8Md7kMW+3XA1";
            
            var signingKey = new SigningKey(cryptoAlg.Alg, version: 1, privateKeyAsBase64);
            var entityName = "domain";
            var signedJson = signer.SignJson(json, entityName, signingKey);
        }
    }
}
```

### Verify signed JSON

``` c#
using System;
using CyberDot.SignedJson.Crypto;
using CyberDot.SignedJson.Verifying;
using Newtonsoft.Json.Linq;

namespace CyberDot.SignedJson.Example
{
    public class VerifyJsonExample
    {
        public void Verify()
        {
            var cryptoAlg = new Ed25519Alg();
            var verifier = new Verifier(cryptoAlg);

            var json = JObject.Parse(@"{
                ""signatures"": {
                    ""domain"": {
                        ""ed25519:1"": ""K8280/U9SSy9IVtjBuVeLr+HpOB4BQFWbg+UZaADMtTdGYI7Geitb76LTrr5QV/7Xg4ahLwYGYZzuHGZKM5ZAQ""
                    }
                }
            }");
            var privateKeyAsBase64 = "YJDBA9Xnr2sVqXD9Vj7XVUnmFZcZrlw8Md7kMW+3XA1=";
            var publicKeyBase64 = cryptoAlg.DerivePublicKey(Convert.FromBase64String(privateKeyAsBase64));
            
            var verifyKey = new VerifyKey(cryptoAlg.Alg, version: 1, publicKeyBase64);
            const string entityName = "domain";
            verifier.VerifySignedJson(json, entityName, verifyKey);
        }
    }
}
```

Format
------

``` json

    {
        "<protected_name>": "<protected_value>",
        "signatures": {
            "<entity_name>": {
                "ed25519:<key_id>": "<unpadded_base64_signature>"
            }
        },
        "unsigned": {
            "<unprotected_name>": "<unprotected_value>",
        }
    }
```




