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

using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace LeerCopyWPF.Utilities
{
    public class EncodedImage
    {
        #region Fields
        #endregion // Fields


        #region Properties

        /// <summary>
        /// Represents all valid encodings for images
        /// </summary>
        public enum Encoding
        {
            BMP,
            GIF,
            JPEG,
            PNG,
            TIFF,
            WMP
        }

        /// <summary>
        /// Image to encode
        /// </summary>
        public BitmapSource Image { get; private set; }

        /// <summary>
        /// Encoder for stream
        /// </summary>
        public BitmapEncoder Encoder { get; private set; }

        /// <summary>
        /// Type of encoding for image
        /// </summary>
        public Encoding EncodingType { get; private set; }

        #endregion // Properties


        #region Methods

        /// <summary>
        /// Constructs an encoded image
        /// </summary>
        /// <param name="image">Image to encode</param>
        /// <param name="encoding">Encoding to use on image</param>
        public EncodedImage(BitmapSource image, Encoding encoding)
        {
            Image = image;
            EncodingType = encoding;

            switch (encoding)
            {
                case Encoding.BMP:
                    Encoder = new BmpBitmapEncoder();
                    break;
                case Encoding.GIF:
                    Encoder = new GifBitmapEncoder();
                    break;
                case Encoding.JPEG:
                    Encoder = new JpegBitmapEncoder();
                    break;
                case Encoding.PNG:
                    Encoder = new PngBitmapEncoder();
                    break;
                case Encoding.TIFF:
                    Encoder = new TiffBitmapEncoder();
                    break;
                case Encoding.WMP:
                    Encoder = new WmpBitmapEncoder();
                    break;
                default:
                    Encoder = new BmpBitmapEncoder();
                    break;
            }
        } // EncodedImage


        /// <summary>
        /// Constructs an encoded image
        /// </summary>
        /// <param name="image">Image to encode</param>
        /// <param name="extension">Extension of file format to encode image to</param>
        public EncodedImage(BitmapSource image, string extension)
        {
            Image = image;
            Encoding? enc = ConvertExtensionToEncoding(extension);

            if (enc == null)
            {
                // TODO Error logging
                throw new ArgumentException($"{extension} is not a supported file extension", "extension");
            }

            EncodingType = enc.Value;

            switch (EncodingType)
            {
                case Encoding.BMP:
                    Encoder = new BmpBitmapEncoder();
                    break;
                case Encoding.GIF:
                    Encoder = new GifBitmapEncoder();
                    break;
                case Encoding.JPEG:
                    Encoder = new JpegBitmapEncoder();
                    break;
                case Encoding.PNG:
                    Encoder = new PngBitmapEncoder();
                    break;
                case Encoding.TIFF:
                    Encoder = new TiffBitmapEncoder();
                    break;
                case Encoding.WMP:
                    Encoder = new WmpBitmapEncoder();
                    break;
                default:
                    Encoder = new BmpBitmapEncoder();
                    break;
            }
        } // EncodedImage


        /// <summary>
        /// Creates and returns memory stream of encoded image. Caller responsible for disposing stream.
        /// </summary>
        /// <returns>Memory stream representing encoded image</returns>
        public MemoryStream GetMemoryStream()
        {
            MemoryStream mStream = new MemoryStream();

            // Create local deep copy to write to stream
            BitmapSource clone = Image.Clone();
            clone.Freeze();

            Encoder.Frames.Add(BitmapFrame.Create(clone));
            Encoder.Save(mStream);
            Encoder.Frames.Clear();

            return mStream;
        } // GetMemoryStream


        /// <summary>
        /// Saves image to disk at given path in specified encoding
        /// </summary>
        /// <param name="fullPath">Path to save file to</param>
        public void SaveToFile(string fullPath)
        {
            // Remove file name from path
            int fileNamePos = fullPath.LastIndexOf(Path.DirectorySeparatorChar);
            string extractedPath = fullPath.Substring(0, fileNamePos + 1);

            // Create the path if it does not exist
            try
            {
                if (!Directory.Exists(extractedPath))
                {
                    Directory.CreateDirectory(extractedPath);
                }
            }
            catch (Exception)
            {
                // TODO Exception logging
                throw;
            }

            // Save file
            using (Stream fStream = new FileStream(fullPath, FileMode.Create))
            {
                // Create local deep copy to write to stream
                BitmapSource clone = Image.Clone();
                clone.Freeze();

                Encoder.Frames.Add(BitmapFrame.Create(clone));
                Encoder.Save(fStream);
                Encoder.Frames.Clear();
            }
        } // SaveStreamToFile


        /// <summary>
        /// Conversion from file extension to encoding
        /// </summary>
        /// <param name="extension">File extension to convert to encoding</param>
        /// <returns>Encoding represented by file extension</returns>
        private Encoding? ConvertExtensionToEncoding(string extension)
        {
            Encoding? retEnc;

            // Clean inputted string
            extension = extension.Trim('.');
            extension = extension.ToLower();

            // Determine extension for encoder
            if (extension.Equals("bmp"))
            {
                retEnc = Encoding.BMP;
            }
            else if (extension.Equals("gif"))
            {
                retEnc = Encoding.GIF;
            }
            else if (extension.Equals("jpg") || extension.Equals("jpeg"))
            {
                retEnc = Encoding.JPEG;
            }
            else if (extension.Equals("png"))
            {
                retEnc = Encoding.PNG;
            }
            else if (extension.Equals("tif") || extension.Equals("tiff"))
            {
                retEnc = Encoding.TIFF;
            }
            else if (extension.Equals("wmp"))
            {
                retEnc = Encoding.WMP;
            }
            else
            {
                retEnc = null;
            }

            return retEnc;
        } // StrToEncoding

        #endregion // Methods
    }
}
