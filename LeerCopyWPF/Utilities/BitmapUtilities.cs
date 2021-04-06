/*
 *  Leer Copy - Quick and Accurate Screen Capturing Application
 *  Copyright (C) 2019  Weston Berg
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */

using LeerCopyWPF.Models;
using Microsoft.Win32.SafeHandles;
using Serilog;
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
        #region Fields

        #region Public Fields
        #endregion

        #region Protected Fields
        #endregion

        #region Private Fields

        /// <summary>
        /// Handle to logger for this source context
        /// </summary>
        private static readonly ILogger _logger = Log.ForContext(typeof(BitmapUtilities));

        #endregion

        #endregion // Fields


        #region Properties

        #region Public Properties
        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties
        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods

        /// <summary>
        /// Convert a System.Drawing.Bitmap object to a System.Windows.Media.Imaging.BitmapSource object
        /// </summary>
        /// <param name="bitmap">Bitmap to convert</param>
        /// <returns>BitmapSource representing original bitmap</returns>
        public static BitmapSource BitmapToBitmapSource(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                _logger.Error("Null bitmap passed in");
                throw new ArgumentNullException("bitmap");
            }

            BitmapSource bmSrc;
            System.Drawing.Imaging.BitmapData bmpData = null;

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
                    bmpData.Stride);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception converting Bitmap {Bitmap} to BitmapSource", bitmap);
                throw;
            }
            finally
            {
                bitmap?.UnlockBits(bmpData);
            }

            return bmSrc;
        }


        /// <summary>
        /// Convert from BitmapSource to a Bitmap
        /// </summary>
        /// <param name="bmSrc">BitmapSource to convert</param>
        /// <returns>Bitmap representing original BitmapSource</returns>
        public static Bitmap BitmapSourceToBitmap(BitmapSource bmSrc)
        {
            if (bmSrc == null)
            {
                _logger.Error("Null bitmap source passed in");
                throw new ArgumentNullException("bmSrc");
            }

            Bitmap bitmap = null;

            try
            {
                EncodedImage encodedImage = new EncodedImage(bmSrc, EncodedImage.Encoding.BMP);
                using (MemoryStream outStream = encodedImage.GetMemoryStream())
                {
                    bitmap = new Bitmap(outStream);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception converting BitmapSource {BitmapSource} to Bitmap", bmSrc);
                throw;
            }

            return bitmap;
        }


        /// <summary>
        /// Captures portion of the screen represented by passed in Rect
        /// </summary>
        /// <param name="bounds">Bounds representing screen portion to capture</param>
        /// <returns>BitmapSource containing portion of screen captured</returns>
        public static BitmapSource CaptureRect(Rect bounds)
        {
            BitmapSource bmSrc;

            using (Bitmap bitmap = new Bitmap((int)bounds.Width, (int)bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                Graphics g = null;

                try
                {
                    g = Graphics.FromImage(bitmap);
                    g.CopyFromScreen((int)bounds.Left, (int)bounds.Top, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    _logger.Error(ex, "Exception capturing screen bounds {Bounds}", bounds);
                    throw;
                }
                finally
                {
                    g?.Dispose();
                }

                bmSrc = BitmapToBitmapSource(bitmap);
            }

            return bmSrc;
        }


        /// <summary>
        /// Captures the screen as a bitmap
        /// </summary>
        /// <returns>BitmapSource of captured screen</returns>
        public static BitmapSource CaptureScreen(SimpleScreen screen)
        {
            if (screen == null)
            {
                _logger.Error("Null screen passed in");
                throw new ArgumentNullException("screen");
            }

            BitmapSource bmSrc;

            // Define the screen bounds
            int screenLeft = (int)screen.Bounds.Left;
            int screenTop = (int)screen.Bounds.Top;
            int screenWidth = (int)screen.Bounds.Width;
            int screenHeight = (int)screen.Bounds.Height;

            // Capture screen
            using (Bitmap bitmap = new Bitmap(screenWidth, screenHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                Graphics g = null;

                try
                {
                    g = Graphics.FromImage(bitmap);
                    g.CopyFromScreen(screenLeft, screenTop, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    _logger.Error(ex, "Exception capturing screen {Screen}", screen);
                    throw;
                }
                finally
                {
                    g?.Dispose();
                }

                bmSrc = BitmapToBitmapSource(bitmap);
            }

            return bmSrc;
        }


        /// <summary>
        /// Returns list all screens sorted left to right, top to bottom
        /// </summary>
        /// <returns>List of SimpleScreen objects representing each screen</returns>
        public static List<SimpleScreen> DetectScreens()
        {
            List<SimpleScreen> screenList = new List<SimpleScreen>();

            foreach (Screen screen in Screen.AllScreens)
            {
                Rect screenBounds = new Rect(screen.Bounds.Left, screen.Bounds.Top, screen.Bounds.Width, screen.Bounds.Height);
                SimpleScreen simpleScreen = new SimpleScreen(screen.BitsPerPixel, screenBounds, screen.DeviceName);

                screenList.Add(simpleScreen);

                _logger.Debug("Detected screen {Screen}", simpleScreen);
            }

            screenList.Sort();

            return screenList;
        }


        /// <summary>
        /// Copies BitmapSource to the clipboard
        /// </summary>
        /// <param name="bmSrc">BitmapSource to copy to clipboard</param>
        /// <returns>True on success, false otherwise</returns>
        public static bool CopyToClipboard(BitmapSource bmSrc)
        {
            bool success = true;

            try
            {
                // Use 'Clipboard' class to set image to selected area
                System.Windows.Clipboard.SetImage(bmSrc);
            }
            catch (ExternalException ex)
            {
                _logger.Error(ex, "Exception copying BitmapSource {BitmapSource} to clipboard", bmSrc);
                success = false;
            }

            return success;
        }


        /// <summary>
        /// Crops inputted BitmapSource to specified Rect
        /// </summary>
        /// <param name="src">BitmapSource to crop</param>
        /// <param name="area">Area to crop</param>
        /// <returns>BitmapSource cropped to specified area</returns>
        public static CroppedBitmap GetCroppedBitmap(BitmapSource src, Rect area)
        {
            // Determine normalization factors
            double factorX = src.PixelWidth / src.Width;
            double factorY = src.PixelHeight / src.Height;

            // Create normalized selection area
            Int32Rect convertedArea = new Int32Rect((int)Math.Round(area.X * factorX), (int)Math.Round(area.Y * factorY),
                                                    (int)Math.Round(area.Width * factorX), (int)Math.Round(area.Height * factorY));

            // Create normalized cropped bitmap
            return new CroppedBitmap(src, convertedArea);
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods

        /// <summary>
        /// DELETE ONCE SAFEHBITMAPHANDLE IS NO LONGER NEEDED
        /// Internal class for all marshalled functions
        /// </summary>
        internal static class NativeMethods
        {
            [DllImport("gdi32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DeleteObject(IntPtr hObject);
        }


        /// <summary>
        /// DELETE LATER ONCE CONFIRMED NO HANDLES NEEDED THROUGHOUT APPLICATION
        /// Wrapper for creating safe handles from unmanaged handles (Reference: https://stackoverflow.com/a/7035036)
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

        #endregion

        #endregion // Methods
    }
}
