﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPKIView;
/// <summary>
/// Defines private key configuration settings in certificate templates.
/// <para>This enumeration has a <see cref="FlagsAttribute"/> attribute that allows a bitwise combination of its member values.</para>
/// </summary>
[Flags]
public enum PrivateKeyFlags {
    /// <summary>
    /// This flag indicates that attestation data is not required when creating the certificate request.
    /// It also instructs the server to not add any attestation OIDs to the issued certificate.
    /// </summary>
    None = 0,
    /// <summary>
    /// This flag instructs the client to create a key archival certificate request.
    /// </summary>
    RequireKeyArchival = 0x00000001, // 1
    /// <summary>
    /// This flag instructs the client to allow other applications to copy the private key to a .pfx file at a later time.
    /// </summary>
    AllowKeyExport = 0x00000010, // 16
    /// <summary>
    /// This flag instructs the client to use additional protection for the private key.
    /// </summary>
    RequireStrongProtection            = 0x00000020, // 32
    /// <summary>
    /// This flag instructs the client to use an alternate signature format.
    /// </summary>
    RequireAlternateSignatureAlgorithm = 0x00000040, // 64
    /// <summary>
    /// This flag instructs the client to use the same key when renewing the certificate.
    /// <para><strong>Windows Server 2003, Windows Server 2008, Windows Server 2008 R2</strong> - this flag is not supported.</para>
    /// </summary>
    ReuseKeysRenewal                   = 0x00000080, // 128
    /// <summary>
    /// This flag instructs the client to process the msPKI-RA-Application-Policies attribute.
    /// <para><strong>Windows Server 2003, Windows Server 2008, Windows Server 2008 R2</strong> - this flag is not supported.</para>
    /// </summary>
    UseLegacyProvider                  = 0x00000100,  // 256
    /// <summary>
    /// This flag indicates that attestation based on the user's credentials is to be performed.
    /// </summary>
    TrustOnUse                         = 0x00000200,
    /// <summary>
    /// This flag indicates that attestation based on the hardware certificate of the Trusted Platform Module (TPM)
    /// is to be performed.
    /// </summary>
    ValidateCert                       = 0x00000400,
    /// <summary>
    /// This flag indicates that attestation based on the hardware key of the TPM is to be performed.
    /// </summary>
    ValidateKey                        = 0x00000800,
    /// <summary>
    /// This flag informs the client that it SHOULD include attestation data if it is capable of doing
    /// so when creating the certificate request. It also instructs the server that attestation might or
    /// might not be completed before any certificates can be issued.
    /// </summary>
    AttestationPreferred               = 0x00001000,
    /// <summary>
    /// This flag informs the client that attestation data is required when creating the certificate request.
    /// It also instructs the server that attestation must be completed before any certificates can be issued.
    /// </summary>
    AttestationRequired               = 0x00002000,
    /// <summary>
    /// This flag instructs the server to not add any certificate policy OIDs to the issued certificate even though
    /// attestation SHOULD be performed.
    /// </summary>
    AttestationWithoutPolicy          = 0x00004000,
    /// <summary>
    /// This template is supported by Windows Server 2003 CA server or newer.
    /// </summary>
    Server2003                        = 0x00010000,
    /// <summary>
    /// This template is supported by Windows Server 2008 CA server or newer.
    /// </summary>
    Server2008                        = 0x00020000,
    /// <summary>
    /// This template is supported by Windows Server 2008 R2 CA server or newer.
    /// </summary>
    Server2008R2                      = 0x00030000,
    /// <summary>
    /// This template is supported by Windows Server 2012 CA server or newer.
    /// </summary>
    Server2012                        = 0x00040000,
    /// <summary>
    /// This template is supported by Windows Server 2012 R2 CA server or newer.
    /// </summary>
    Server2012R2                      = 0x00050000,
    /// <summary>
    /// This template is supported by Windows Server 2016 CA server or newer.
    /// <para><strong>Note:</strong> this template is not supported by Enrollment Web Services.</para>
    /// </summary>
    Server2016R2                      = 0x00060000,
    /// <summary>
    /// This flag indicates that the key is used for Windows Hello logon.
    /// </summary>
    HelloLogonKey                     = 0x00200000,
    /// <summary>
    /// This template is supported by Windows XP/Windows Server 2003 client or newer.
    /// </summary>
    ClientXP                          = 0x01000000,
    /// <summary>
    /// This template is supported by Windows Vista/Windows Server 2008 client or newer.
    /// </summary>
    ClientVista                       = 0x02000000,
    /// <summary>
    /// This template is supported by Windows 7/Windows Server 2008 R2 client or newer.
    /// </summary>
    ClientWin7                        = 0x03000000,
    /// <summary>
    /// This template is supported by Windows 8/Windows Server 2012 client or newer.
    /// </summary>
    ClientWin8                        = 0x04000000,
    /// <summary>
    /// This template is supported by Windows 8.1/Windows Server 2012 R2 client or newer.
    /// </summary>
    ClientWin81                       = 0x05000000,
    /// <summary>
    /// This template is supported by Windows 10/Windows Server 2016 client or newer.
    /// <para><strong>Note:</strong> this template is not supported by Enrollment Web Services.</para>
    /// </summary>
    ClientWin10                       = 0x06000000
}
