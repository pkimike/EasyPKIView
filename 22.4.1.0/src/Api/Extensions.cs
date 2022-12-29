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

        internal static Int32 ToInt32(this Byte[] bytes) {
            Console.WriteLine($"Bytes: {BitConverter.ToString(bytes)}");
            if (BitConverter.IsLittleEndian) {
                Array.Reverse(bytes);
            }

            return BitConverter.ToInt32(bytes, 0);
        }
    }
}