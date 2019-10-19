using System;

namespace Orion.Util
{
    public static class Verification
    {
        public static string NewPIN()
        {
            // Simple PIN for verifying phone number
            return Guid.NewGuid().ToString("n").Substring(0, 5);
        }
    }
}
