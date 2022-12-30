namespace EasyPKIView {

    static class Extensions {
        internal static String GetObjectClassName(this AdcsObjectType objectType) {
            return objectType switch {
                AdcsObjectType.EnrollmentService   => "pKIEnrollmentService",
                AdcsObjectType.CertificateTemplate => "pKICertificateTemplate",
                AdcsObjectType.Oid                 => "msPKI-Enterprise-Oid",
                _                                  => "None"
            };
        }

        internal static TimeSpan ToTimeSpan(this Byte[] bytes) {
            if (bytes is null) {
                return default;
            }
            long period = BitConverter.ToInt64(bytes, 0);
            period /= -10000000;
            return TimeSpan.FromSeconds(period);
        }

        internal static Int32 ToInt32(this Byte[] bytes) {
            if (BitConverter.IsLittleEndian) {
                Array.Reverse(bytes);
            }

            return Convert.ToInt32(String.Join("",bytes.Select(b => $"{b:x2}").ToArray()), 16);
        }
    }
}