using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
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
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
                } catch
                {
                    bmSrc = null;
                }
            }
            return bmSrc;
        } // ToBitmapSource


        /// <summary>
        /// Convert from BitmapSource to a Bitmap
        /// </summary>
        /// <param name="bmSrc"></param>
        /// <returns></returns>
        public static Bitmap ToBitmap(BitmapSource bmSrc)
        {
            Bitmap bitmap = null;
            try
            {
                using (System.IO.MemoryStream outStream = new System.IO.MemoryStream())
                {
                    BitmapEncoder enc = new BmpBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(bmSrc));
                    enc.Save(outStream);
                    bitmap = new Bitmap(outStream);
                }
            }
            catch
            {
                // TODO exception logging
                throw;
            }
            return bitmap;
        } // ToBitmap


        /// <summary>
        /// Captures the screen as a bitmap
        /// </summary>
        /// <returns>Screen bitmap converted to BitmapSource object</returns>
        public static BitmapSource CaptureScreen()
        {
            BitmapSource bmSrc;
            int vScreenLeft = (int) SystemParameters.VirtualScreenLeft;
            int vScreenTop = (int) SystemParameters.VirtualScreenTop;
            int vScreenWidth = (int) SystemParameters.VirtualScreenWidth;
            int vScreenHeight = (int) SystemParameters.VirtualScreenHeight;

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
                // TODO exception logging
                throw new ApplicationException("BitmapUtilities.CaptureScreen: Unable to convert \'Bitmap\' to \'BitmapSouce\'.");
            }
            return bmSrc;
        } // CaptureScreen


        /// <summary>
        /// Copies bitmap source to the clipboard
        /// </summary>
        /// <param name="bm"></param>
        /// <returns>True on success, false otherwise</returns>
        public static bool CopyToClipboard(BitmapSource bmSrc, Rect area)
        {
            try
            {
                int x = (int)area.X;
                int y = (int)area.Y;
                int width = (int)area.Width;
                int height = (int)area.Height;
                BitmapSource croppedBm = new CroppedBitmap(bmSrc, new Int32Rect(x, y, width, height));

                Clipboard.SetImage(croppedBm);
            }
            catch (ExternalException)
            {
                // TODO exception logging
                throw;
            }
            return false;
        } // CopyToClipboard
    }
}
