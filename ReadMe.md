# Introducing The EasyPKIView Class Library
Managing a Public Key Infrastructure (PKI), especially in a large, complex environment environment can be incredibly hard. Microsoft provides very few native .NET interfaces for working with its PKI implementation (Active Directory Certificate Services).

EasyPKIView provides a simple interface for inspecting components of your Microsoft PKI environment including:

- Enterprise Certification Authorities registered in Active Directory
- Certificate Templates registered in Active Directory
- Certificate Revocation Lists (CRLs)

## Working with Issuing CAs

Let's say you want to generate a list of all the Enterprise CAs in your environment:

```
using EasyPKIView;

List<ADCertificationAuthority> MyCAs = ADCertificationAuthority.GetAll();
```
The *MyCAs* object will contain just about all the metadata you may need about every Issuing CA including:

- The DNS host name where the CA is installed
- The full LDAP Distinguished name of the CA
- The list of certificate templates advertised on the CA
- The CA Config string (useful for executing certutil or certreq commands and using the ICertAdmin Win32 interface)

You can also pull up a particular CA if you know it's display name:

```
var MyCA = new ADCertificationAuthority(@"Vandelay Industries Issuing CA 1");
```

Or by passing in the CA certificate itself:

```
X509Certificate2 CACert = new X509Certificate2(@"C:\CA.cer");
var MyCA = new ADCertificationAuthority(CACert);
```

## Working with Certificate Templates

Certificate templates act as the framework for how Microsoft Enterprise CAs generate end-entity certificates. They govern the key strength and the key usages asserted on each certificate they're used to model. Thus, it should come as no surprise that it's important to keep track and control over what certificate templates are being used to model certificates in your environment. 

Obtaining information about the certificate templates in your AD forest programmatically can be quite challenging since there's no native .NET interface with which to retreive & inspect them. EasyPKIView makes this a borderline trivial task.

To simply generate a list of all certificate templates in your AD forest:

```
var MyTemplates = ADCertificateTemplate.GetAll();
```

Each ADCertificateTemplate object will contain metadata about a certificate template, including:

- Name and Display Name
- Version
- LDAP Distinguished Name
- Minimal Allowed key size
- The template Oid (if version 2 or higher)
- The list of key usages and extended key usages included on issued certificates

Let's say you're asked to identify any certificate templates that allow for weak key strength.  Linq is your friend:

```
using System.Linq;
using EasyPKIView;

var WeakTemplates = ADCertificateTemplate.GetAll()
                                         .Where(p => p.MinimumKeySize < 2048)
					 .ToList();
```

You can narrow your work down further by only focusing on certificate templates that are actually assigned to one or more issuing CA in your AD forest:

```
var WeakTemplates = ADCertificationAuthority.GetAll()
					    .Select(p => p.Templates)
					    .SelectMany(p => p)
					    .Where(p => p.MinimumKeySize < 2048)
					    .Distinct().ToList();
```

## Working with Certificate Revocation Lists (CRLs)

EasyPKIView leverages the excellent BouncyCastle library to enable you to pull up the most pertinent information about an indicated CRL. In the current version of the library, this includes:

- The date the CRL expires
- The list of certificate serial numbers included in the CRL

The returned collection of revoked certificate serial numbers is expressed as a list of strings. This makes it trivial to compare against, for instance, an X509Certificate2 object.

You can use the **CrlReader** class to inspect a CRL by passing it in as an already-downloaded file:

```
using System.IO;
using EasyPKIView;

var MyCRL = new CrlReader(new FileInfo(@"c:\Vandelay.crl"));
```

or by passing in the URL of the CRL distribution point:

```
var MyCRL = new CrlReader(@"http://certificates.vandelay.com/crls/Vandelay Industries CA.crl");
```