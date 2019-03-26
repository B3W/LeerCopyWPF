using LeerCopyWPF.Models;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
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
        public static BitmapSource BitmapToBitmapSource(Bitmap bitmap)
        {
            BitmapSource bmSrc;
            System.Drawing.Imaging.BitmapData bmpData = null;

            if (bitmap == null)
            {
                throw new ArgumentNullException("bitmap");
            }

            try
            {
                Rectangle bmpRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                bmpData = bitmap.LockBits(bmpRect, System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);
                bmSrc = BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bmpData.Scan0,
                    bmpData.Stride * bmpData.Height,
                    bmpData.Stride
                    );
            }
            catch (Exception)
            {
                // TODO: Exception logging
                throw;
            }
            finally
            {
                if (bmpData != null)
                {
                    bitmap.UnlockBits(bmpData);
                }
            }

            return bmSrc;
        } // ToBitmapSource


        /// <summary>
        /// Convert from BitmapSource to a Bitmap
        /// </summary>
        /// <param name="bmSrc"></param>
        /// <returns></returns>
        public static Bitmap BitmapSourceToBitmap(BitmapSource bmSrc)
        {
            Bitmap bitmap = null;
            try
            {
                EncodedImage encodedImage = new EncodedImage(bmSrc, EncodedImage.Encoding.BMP);
                using (MemoryStream outStream = encodedImage.GetMemoryStream())
                {
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


        public static BitmapSource CaptureRect(Rect bounds)
        {
            BitmapSource bmSrc;

            using (Bitmap bitmap = new Bitmap((int)bounds.Width, (int)bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen((int)bounds.Left, (int)bounds.Top, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);
                }
                bmSrc = BitmapToBitmapSource(bitmap);
            }
            if (bmSrc == null)
            {
                // TODO exception logging
                throw new ApplicationException("BitmapUtilities.CaptureRect: Unable to convert \'Bitmap\' to \'BitmapSouce\'");
            }

            return bmSrc;
        }


        /// <summary>
        /// Captures the screen as a bitmap
        /// </summary>
        /// <returns>Screen bitmap converted to BitmapSource object</returns>
        public static ExtBitmapSource CaptureScreen(Screen screen)
        {
            BitmapSource bmSrc;
            int screenLeft = screen.Bounds.Left;
            int screenTop = screen.Bounds.Top;
            int screenWidth = screen.Bounds.Width;
            int screenHeight = screen.Bounds.Height;

            using (Bitmap bitmap = new Bitmap(screenWidth, screenHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(screenLeft, screenTop, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);
                }
                bmSrc = BitmapToBitmapSource(bitmap);
            }
            if (bmSrc == null)
            {
                // TODO exception logging
                throw new ApplicationException("BitmapUtilities.CaptureScreen: Unable to convert \'Bitmap\' to \'BitmapSouce\' for screen " + screen.DeviceName);
            }

            Rect scrBounds = new Rect(screenLeft, screenTop, screenWidth, screenHeight);
            return new ExtBitmapSource(bmSrc, scrBounds);
        } // CaptureScreen


        /// <summary>
        /// Returns sorted list all screens as bitmaps
        /// </summary>
        /// <returns>List of BitmapSource objects representing each screen</returns>
        public static List<ExtBitmapSource> CaptureScreens()
        {
            List<ExtBitmapSource> captureList = new List<ExtBitmapSource>();

            foreach (Screen screen in Screen.AllScreens)
            {
                captureList.Add(CaptureScreen(screen));
            }

            captureList.Sort();
            return captureList;
        } // CaptureScreens


        /// <summary>
        /// Copies bitmap source to the clipboard
        /// </summary>
        /// <param name="bm"></param>
        /// <returns>True on success, false otherwise</returns>
        public static bool CopyToClipboard(BitmapSource bmSrc)
        {
            try
            {
                // Use 'Clipboard' class to set image to selected area
                System.Windows.Clipboard.SetImage(bmSrc);
            }
            catch (ExternalException)
            {
                // TODO exception logging
                throw;
            }
            return false;
        } // CopyToClipboard


        public static CroppedBitmap GetCroppedBitmap(BitmapSource src, Rect area)
        {
            // Determine normalization factors
            double factorX = src.PixelWidth / src.Width;
            double factorY = src.PixelHeight / src.Height;
            // Create normalized selection area
            Int32Rect convertedArea = new Int32Rect(
                (int)Math.Round(area.X * factorX), (int)Math.Round(area.Y * factorY),
                (int)Math.Round(area.Width * factorX), (int)Math.Round(area.Height * factorY));
            // Create normalized cropped bitmap
            return new CroppedBitmap(src, convertedArea);
        }
    }
}
