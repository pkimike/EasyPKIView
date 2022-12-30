using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyPKIView.CertificateTemplates;
using EasyPKIView.EnrollmentServices;

namespace EasyPKIView;
/// <summary>
/// The public key services container contents from the current Active Directory forest
/// </summary>
public class PublicKeyServices {
    /// <summary>
    /// The list of certificate templates published in the Active Directory forest
    /// </summary>
    public List<AdcsCertificateTemplate> CertificateTemplates { get; set; } = new();

    /// <summary>
    /// The list of enrollment services (online CAs) published in the Active Directory forest.
    /// </summary>
    public List<AdcsEnrollmentService> EnrollmentServices { get; set; } = new();

    /// <summary>
    /// Gets the collection of certificate templates and enrollment services objects published in the Active Directory forest.
    /// </summary>
    /// <returns>the collection of certificate templates and enrollment services objects published in the Active Directory forest.</returns>
    public static PublicKeyServices GetFromActiveDirectory() {
        var retValue = new PublicKeyServices {
            CertificateTemplates = AdcsCertificateTemplate.GetAllFromDirectory(),
            EnrollmentServices = AdcsEnrollmentService.GetAllFromDirectory()
        };

        retValue.CertificateTemplates.ForEach(t => t.SetAssignedEnrollmentServices(retValue.EnrollmentServices));

        return retValue;
    }
}
