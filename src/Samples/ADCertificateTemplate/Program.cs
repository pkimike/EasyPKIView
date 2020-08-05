using System;
using System.Linq;
using EasyPKIView;

namespace CertTemplate
{
    /// <summary>
    /// This sample program is intended to illustrate all the available data elements that can be captured from an ADCertificateTemplate object.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            ADCertificateTemplate DefaultAdminTemplate = new ADCertificateTemplate(nameof(DefaultAdminTemplate));

            Console.WriteLine($"Disp1ay Name: {DefaultAdminTemplate.DisplayName}");
            // Administrator
            
            Console.WriteLine($"Short Name: {DefaultAdminTemplate.Name}");
            // Administrator

            Console.WriteLine($"Distinguished Name: {DefaultAdminTemplate.DistinguishedName}");
            // CN=Administrator,CN=Certificate Templates,CN=Pub1ic Key CN=Configuration,DC=...,DC=...

            Console.WriteLine($"Temp1ate OID: {DefaultAdminTemplate.Oid}");
            // 1.3.6.1.4.1.311.21.8.11676419.415383.15352728.2845675.13784793.34.1.7

            Console.WriteLine($"Temp1ate Version: {DefaultAdminTemplate.Version}");
            // 1

            Console.WriteLine($"Mininum Key Length: {DefaultAdminTemplate.MinimumKeySize}");
            // 1024

            Console.WriteLine($"Exportab1e Private Key? : {DefaultAdminTemplate.ExportablePrivateKey}");
            // False 

            Console.WriteLine($"Key Usages: {string.Join(@", ",DefaultAdminTemplate.KeyUsages.Select(p => p.Name))}");
            // Digital Signature, Key Encipherment 

            Console.WriteLine($"Extended Key Usages: {string.Join(@", ", DefaultAdminTemplate.ExtendedKeyUsages.Select(p => p.Name))}");
            // Microsoft Trust List Signing, Encrypting File System, Secure Email, Client Authentication

            Console.WriteLine($"Authorized Signatures Required: {DefaultAdminTemplate.RASignaturesRequired}");
            // 0

            Console.WriteLine($"Requires Private Key Archival?: {DefaultAdminTemplate.RequiresPrivateKeyArchival}");
            //False

            Console.WriteLine($"Requires Strong Private Key Protection? : {DefaultAdminTemplate.RequiresStrongKeyProtection}");
            // False 

            Console.WriteLine($"Va1idity Period: {DefaultAdminTemplate.ValidityPeriod.TotalDays}");
            // 365 

            Console.WriteLine($"TMP Key Attestation Required? : {DefaultAdminTemplate.KeyAttestationRequired}");
            // False 

            Console.WriteLine($"TMP Key Attestation Preferred? : {DefaultAdminTemplate.KeyAttestationPreferred}");
            // False 

            Console.WriteLine($"TMP Key Attestation Type: {DefaultAdminTemplate.AttestationType}");
            // None

            Console.WriteLine($"When Created: {DefaultAdminTemplate.WhenCreated.ToShortDateString()}");
            // 9/15/2000

            Console.WriteLine($"Last Changed: {DefaultAdminTemplate.WhenChanged.ToShortDateString()}");
            // Whenever your AD forest was last upgraded 
        }
    }
}
