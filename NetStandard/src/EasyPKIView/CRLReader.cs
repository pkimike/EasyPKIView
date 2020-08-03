using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Org.BouncyCastle.X509;

namespace EasyPKIView
{
    /// <summary>
    /// Describes an X509 Certificate Revocation List (CRL). Includes the most relevant fields
    /// </summary>
    public class CrlReader
    {
        /// <summary>
        /// The expiration date of this CRL (when it can no longer be used to validate certificates)
        /// </summary>
        public DateTime NextUpdate { get; private set; }

        /// <summary>
        /// The list of certificate serial numbers that are indicated as revoked by the CRL.
        /// </summary>
        public List<string> Certificates { get; private set; } = new List<string>();

        /// <summary>
        /// CrlReader Constructor 1
        /// </summary>
        /// <param name="crlBytes">byte array containing the CRL contents</param>
        public CrlReader(byte[] crlBytes)
        {
            X509CrlParser Parser = new X509CrlParser();
            X509Crl CRL = Parser.ReadCrl(crlBytes);
            NextUpdate = CRL.NextUpdate.Value;
            var RevokedCerts = CRL.GetRevokedCertificates();
            if (RevokedCerts != null)
            {
                foreach (X509CrlEntry Entry in CRL.GetRevokedCertificates())
                {
                    Certificates.Add(Entry.SerialNumber.ToString(16));
                }
            }
        }

        /// <summary>
        /// CrlReader Constructor 2
        /// </summary>
        /// <param name="crlFile">FileInfo object pointing to a CRL file</param>
        public CrlReader(FileInfo crlFile)
            : this(File.ReadAllBytes(crlFile.FullName))
        { }

        /// <summary>
        /// CrlReader Constructor 3
        /// </summary>
        /// <param name="CrlUrl">The HTTP URL from which to download the CRL</param>
        public CrlReader(string CrlUrl)
            : this(DownloadFile(CrlUrl))
        { }

        /// <summary>
        /// Indicates whether the certificate with the specified serial number is revoked per the CRL
        /// </summary>
        /// <param name="serialNumber">Serial number of the certificate to be checked.</param>
        /// <returns>true or false depending on whether the certificate is revoked.</returns>
        public bool IsCertRevoked(string serialNumber)
        {
            return Certificates.Any(p => p.Matches(serialNumber));
        }

        /// <summary>
        /// Indicates whether the specified certificate is revoked per the CRL
        /// </summary>
        /// <param name="Cert">Certificate to be checked.</param>
        /// <returns>true or false depending on whether the certificate is revoked.</returns>
        public bool IsCertRevoked(X509Certificate2 Cert)
        {
            return IsCertRevoked(Cert.SerialNumber);
        }

        private static byte[] DownloadFile(string url)
        {
            byte[] responseBytes = null;
            url = url.Replace(@" ", @"%20");
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new FormatException($"\"{url}\": Not a valid HTTP URL");
            }

            WebRequest Request = WebRequest.CreateHttp(url);
            Request.Timeout = 10000;

            using (WebResponse Response = Request.GetResponse())
            {
                using (Stream ResponseStream = Response.GetResponseStream())
                {
                    using (MemoryStream MemStream = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            byte[] buf = new byte[1024];
                            count = ResponseStream.Read(buf, 0, 1024);
                            MemStream.Write(buf, 0, count);
                        }
                        while (ResponseStream.CanRead && count > 0);
                        responseBytes = MemStream.ToArray();
                    }
                }
            }
            return responseBytes;
        }
    }
}
