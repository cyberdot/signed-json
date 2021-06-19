using System;
using System.Collections.Generic;
using CyberDot.SignedJson.Crypto;

namespace CyberDot.SignedJson
{
    public static class CryptoAlgDictionaryExtensions
    {
        public static ICryptoAlg GetCryptoAlg(this Dictionary<string, ICryptoAlg> cryptoAlgs,  string key)
        {
            if (cryptoAlgs.ContainsKey(key))
            {
                return cryptoAlgs[key];
            }
            else
            {
                throw new ArgumentException($"Cannot find cryptographic algorithm implementation for: {key}");
            }
        }
    }
}