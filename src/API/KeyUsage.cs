using System.Collections.Generic;
using System.Linq;

namespace EasyPKIView
{
    /// <summary>
    /// Describes an X509 Key Usage.  
    /// See https://tinyurl.com/nr47gkg for more information
    /// </summary>
    public class KeyUsage
    {
        /// <summary>
        /// The name of the key usage
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The bit that must be set in the Active Directory certificate template object in order for it to assert this key usage
        /// </summary>
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

        /// <summary>
        /// Digital Signature
        /// </summary>
        public static readonly KeyUsage DigitalSignature = new KeyUsage(128, @"Digital Signature");

        /// <summary>
        /// Non-repudiation - Digital signature asserts Proof of origin
        /// </summary>
        public static readonly KeyUsage NonRepudiation = new KeyUsage(64, @"Non-repudiation (proof of origin)");

        /// <summary>
        /// Key Encipherment - public key may be used to encrypt symmetric keys but not data
        /// </summary>
        public static readonly KeyUsage KeyEncipherment = new KeyUsage(32, @"Key Encipherment");

        /// <summary>
        /// Data Encipherment - public key may be used to directly encrypt data
        /// </summary>
        public static readonly KeyUsage EncryptData = new KeyUsage(16, @"Data Encipherment");

        /// <summary>
        /// Key Agreement - private key may be used verify a signature on certificates. This extension can be used only in CA certificates.
        /// </summary>
        public static readonly KeyUsage KeyAgreement = new KeyUsage(8, @"Key Agreement");

        /// <summary>
        /// Certificate Signing - private key may be used to sign other certificates. This extension can be used only in CA certificates.
        /// </summary>
        public static readonly KeyUsage CertificateSigning = new KeyUsage(4, @"Certificate Signing");

        /// <summary>
        /// CRL Signing - private key may be used to sign Certificate Revocation Lists (CRLs). This extension can be used only in CA certificates.
        /// </summary>
        public static readonly KeyUsage CRLSigning = new KeyUsage(2, @"CRL Signing");

        /// <summary>
        /// Encrypt Only - Unclear on what this key usage asserts. Not typically used.
        /// </summary>
        public static readonly KeyUsage EncryptOnly = new KeyUsage(1, @"Encrypt Only");

        /// <summary>
        /// The list of supported Key usages
        /// </summary>
        public static readonly List<KeyUsage> Supported = new List<KeyUsage> { DigitalSignature, NonRepudiation, KeyEncipherment, EncryptData, KeyAgreement, CertificateSigning, CRLSigning, EncryptOnly };

        internal static List<KeyUsage> GetKeyUsages(byte[] value)
        {
            return Supported.Where(p => p.IsSet(value[0])).ToList();
        }
    }
}
