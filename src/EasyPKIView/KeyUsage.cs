using System.Collections.Generic;
using System.Linq;

namespace EasyPKIView
{
    public class KeyUsage
    {
        public string Name { get; private set; }
        public byte BitPosition { get; private set; }

        private KeyUsage(byte bitPosition, string name)
        {
            BitPosition = bitPosition;
            Name = name;
        }

        private bool IsSet(byte value)
        {
            return (value & BitPosition) > 0;
        }

        public static readonly KeyUsage DigitalSignature = new KeyUsage(128, @"Digital Signature");
        public static readonly KeyUsage NonRepudiation = new KeyUsage(64, @"Non-repudiation (proof of origin)");
        public static readonly KeyUsage KeyEncipherment = new KeyUsage(32, @"Key Encipherment");
        public static readonly KeyUsage EncryptData = new KeyUsage(16, @"Data Encipherment");
        public static readonly KeyUsage KeyAgreement = new KeyUsage(8, @"Key Agreement");
        public static readonly KeyUsage CertificateSigning = new KeyUsage(4, @"Certificate Signing");
        public static readonly KeyUsage CRLSigning = new KeyUsage(2, @"CRL Signing");
        public static readonly KeyUsage EncryptOnly = new KeyUsage(1, @"Encrypt Only");

        public static readonly List<KeyUsage> Supported = new List<KeyUsage> { DigitalSignature, NonRepudiation, KeyEncipherment, EncryptData, KeyAgreement, CertificateSigning, CRLSigning, EncryptOnly };

        public static List<KeyUsage> GetKeyUsages(byte[] value)
        {
            return Supported.Where(p => p.IsSet(value[0])).ToList();
        }
    }
}
