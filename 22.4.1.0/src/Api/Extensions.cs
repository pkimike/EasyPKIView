namespace EasyPKIView {

    static class Extensions {
        internal static String AsString(this AdcsObjectType objectType) {
            return objectType switch {
                AdcsObjectType.EnrollmentService => "pKIEnrollmentService",
                AdcsObjectType.CertificateTemplate => "pKICertificateTemplate",
                _ => "None"
            };
        }

        internal static TimeSpan ToTimeSpan(this Byte[] bytes) {
            long period = BitConverter.ToInt64(bytes, 0);
            period /= -10000000;
            return TimeSpan.FromSeconds(period);
        }
    }
}