using One_Sgp4;
using System;

namespace CelesTrakLib
{
    public static class Utils
    {
        public static double GetSpeed(Point3d velocity)
        {
            return Math.Sqrt((velocity.x * velocity.x) + (velocity.y * velocity.y) + (velocity.z * velocity.z));
        }

        public static string GetRightAscension(Point3d position)
        {
            double rightAscensionDegrees = Math.Atan2(position.y, position.x) * (180.0 / Math.PI);
            if (rightAscensionDegrees < 0)
            {
                rightAscensionDegrees += 360.0;
            }

            int hours = (int)(rightAscensionDegrees / 15);
            double remainingDegrees = rightAscensionDegrees % 15;
            int minutes = (int)(remainingDegrees * 4);
            double seconds = ((remainingDegrees * 4) - minutes) * 60;

            return $"{hours:D2}h {minutes:D2}m {seconds:F0}s";
        }

        public static string GetDeclination(Point3d position)
        {
            double magnitude = Math.Sqrt((position.x * position.x) + (position.y * position.y) + (position.z * position.z));
            double declinationDegrees = Math.Asin(position.z / magnitude) * (180.0 / Math.PI);

            int degrees = (int)Math.Floor(declinationDegrees);
            double remainingDegrees = Math.Abs(declinationDegrees - degrees);
            int minutes = (int)(remainingDegrees * 60);
            double seconds = (remainingDegrees * 60 - minutes) * 60;

            return $"{degrees:D2}° {minutes:F0}' {seconds:F0}''";
        }
    }
}