namespace StereoGame.Framework.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    internal static class InteropHelpers
    {
        /// <summary>
        /// Convert a pointer to a Utf8 null-terminated string to a .NET System.String
        /// </summary>
        public static string? Utf8ToString(IntPtr handle)
        {
            if (handle == IntPtr.Zero) return null;
            return Marshal.PtrToStringUTF8(handle);
        }
    }
}
