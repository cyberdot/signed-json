using System;

namespace CyberDot.SignedJson.Verifying
{
    public class VerifySignatureException : Exception
    {
        public VerifySignatureException(string message): base(message){ }
    }
}