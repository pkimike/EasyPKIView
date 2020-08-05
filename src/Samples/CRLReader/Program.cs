using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using EasyPKIView;

namespace CRL
{
    class Program
    {
        static void Main(string[] args)
        {
            //To load a CRL from its HTTP distribution point, just pass the URL
            CrlReader crlReader1 = new CrlReader(@"http://crl.pki.goog/GTS1O1core.crl");

            //If you have a pre-downloaded CRL file, pass it as a FileInfo object:
            CrlReader crlReader2 = new CrlReader(new FileInfo(@"..\..\..\content\GTS1O1core.crl"));

            //You can dump the list of revoked certificate serial numbers
            crlReader1.Certificates.ForEach(sn => Console.WriteLine(sn));

            //You can check if an individual certificate serial number is revoked by serial number:
            bool revoked1 = crlReader1.IsCertRevoked(@"31be077b1a9ccce00200000000726c62");

            //...Or by passing a cert as a X509Certificate2 object
            bool revoked2 = crlReader1.IsCertRevoked(new X509Certificate2(@"..\..\..\content\google.cer"));

            //Finally, you can extract the date when this CRL expires
            DateTime ExpDate = crlReader1.NextUpdate;
        }
    }
}
