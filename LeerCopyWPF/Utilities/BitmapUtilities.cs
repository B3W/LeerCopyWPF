using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace LeerCopyWPF.Utilities
{
    public static class BitmapUtilities
    {
        /// <summary>
        /// Internal class for all marshalled functions
        /// </summary>
        internal static class NativeMethods
        {
            [DllImport("gdi32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DeleteObject(IntPtr hObject);
        }

        /// <summary>
        /// Wrapper for creating safe handles from unmanaged handles
        /// Reference: https://stackoverflow.com/a/7035036
        /// </summary>
        private class SafeHBitmapHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            [System.Security.SecurityCritical]
            public SafeHBitmapHandle(IntPtr preexistingHandle, bool ownsHandle)
                : base(ownsHandle)
            {
                SetHandle(preexistingHandle);
            }

            protected override bool ReleaseHandle()
            {
                return NativeMethods.DeleteObject(handle);
            } // SafeHBitmapHandle
        }

        /// <summary>
        /// Convert a System.Drawing.Bitmap object to a System.Windows.Media.Imaging.BitmapSource object
        /// </summary>
        /// <param name="bitmap">Bitmap to convert</param>
        /// <returns>BitmapSource representing original bitmap</returns>
        public static BitmapSource ToBitmapSource(Bitmap bitmap)
        {
            BitmapSource bmSrc;
            IntPtr hBitmap = bitmap.GetHbitmap();
            SafeHBitmapHandle safeHBitmapHandle = new SafeHBitmapHandle(hBitmap, true);

            using (safeHBitmapHandle)
            {
                try
                {
                    bmSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                            hBitmap,
                            IntPtr.Zero,
                            System.Windows.Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
                } catch
                {
                    bmSrc = null;
                }
            }
            return bmSrc;
        } // ToBitmapSource

        /// <summary>
        /// Captures the screen as a bitmap
        /// </summary>
        /// <returns>Screen bitmap converted to BitmapSource object</returns>
        public static BitmapSource CaptureScreen()
        {
            BitmapSource bmSrc;
            int vScreenLeft = (int) System.Windows.SystemParameters.VirtualScreenLeft;
            int vScreenTop = (int)System.Windows.SystemParameters.VirtualScreenTop;
            int vScreenWidth = (int)System.Windows.SystemParameters.VirtualScreenWidth;
            int vScreenHeight = (int)System.Windows.SystemParameters.VirtualScreenHeight;

            using (Bitmap bitmap = new Bitmap(vScreenWidth, vScreenHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(vScreenLeft, vScreenTop, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);
                }
                bmSrc = ToBitmapSource(bitmap);
            }
            if (bmSrc == null)
            {
                throw new ApplicationException("BitmapUtilities.CaptureScreen: Unable to convert \'Bitmap\' to \'BitmapSouce\'.");
            }
            return bmSrc;
        } // CaptureScreen
    }
}
