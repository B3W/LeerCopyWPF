﻿/*
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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace LeerCopyWPF.Utilities
{
    public class EncodedImage
    {
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
        }


        public EncodedImage(BitmapSource image, string encoding)
        {
            Image = image;
            Nullable<Encoding> enc = StrToEncoding(encoding);
            if (enc == null)
            {
                // TODO Error logging
                throw new ArgumentException(String.Format("{0} is not a supported encoding", encoding), "encoding");
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
        }


        /// <summary>
        /// Creates and returns memory stream of encoded image.
        /// Disposing of memory stream not necessary but good practice.
        /// </summary>
        /// <returns></returns>
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
        /// <param name="path"></param>
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
            } catch (Exception)
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
        /// <param name="str"></param>
        /// <returns></returns>
        private Nullable<Encoding> StrToEncoding(string str)
        {
            Nullable<Encoding> retEnc;

            // Clean inputted string
            str = str.Trim('.');
            str = str.ToLower();

            // Determine extension for encoder
            if (str.Equals("bmp"))
            {
                retEnc = Encoding.BMP;
            }
            else if (str.Equals("gif"))
            {
                retEnc = Encoding.GIF;
            }
            else if (str.Equals("jpg") || str.Equals("jpeg"))
            {
                retEnc = Encoding.JPEG;
            }
            else if (str.Equals("png"))
            {
                retEnc = Encoding.PNG;
            }
            else if (str.Equals("tif") || str.Equals("tiff"))
            {
                retEnc = Encoding.TIFF;
            }
            else if (str.Equals("wmp"))
            {
                retEnc = Encoding.WMP;
            }
            else
            {
                retEnc = null;
            }
            return retEnc;
        } // StrToEncoding
    }
}
