/*
 * Leer Copy - Quick and Accurate Screen Capturing Application
 * Copyright (C) 2021  Weston Berg
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Serilog;
using System;
using System.IO;
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

        #region Fields

        #region Public Fields
        #endregion

        #region Protected Fields
        #endregion

        #region Private Fields

        /// <summary>
        /// Handle to logger for this source context
        /// </summary>
        private readonly ILogger _logger;

        #endregion

        #endregion // Fields


        #region Properties

        #region Public Properties

        /// <summary>
        /// Image to encode
        /// </summary>
        public BitmapSource Image { get; }

        /// <summary>
        /// Encoder for stream
        /// </summary>
        public BitmapEncoder Encoder { get; }

        /// <summary>
        /// Type of encoding for image
        /// </summary>
        public Encoding EncodingType { get; }

        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties
        #endregion

        #endregion // Properties


        #region Methods

        /// <summary>
        /// Constructs an encoded image
        /// </summary>
        /// <param name="image">Image to encode</param>
        /// <param name="encoding">Encoding to use on image</param>
        public EncodedImage(BitmapSource image, Encoding encoding)
        {
            _logger = Log.ForContext<EncodedImage>();

            if (image == null)
            {
                _logger.Error("Null image passed to constructor");
                throw new ArgumentNullException("image", "Image cannot be null");
            }

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


        /// <summary>
        /// Constructs an encoded image
        /// </summary>
        /// <param name="image">Image to encode</param>
        /// <param name="extension">Extension of file format to encode image to</param>
        public EncodedImage(BitmapSource image, string extension)
        {
            _logger = Log.ForContext<EncodedImage>();

            if (image == null)
            {
                _logger.Error("Null image passed to constructor");
                throw new ArgumentNullException("image", "Image cannot be null");
            }

            Image = image;
            Encoding? encoding = ConvertExtensionToEncoding(extension);

            if (!encoding.HasValue)
            {
                _logger.Error("Unsupported file extension {Extension}", extension);
                throw new ArgumentException($"'{extension}' is not a supported file extension", "extension");
            }

            EncodingType = encoding.Value;

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
        /// Creates and returns memory stream of encoded image. Caller responsible for disposing stream.
        /// </summary>
        /// <returns>Memory stream representing encoded image</returns>
        public MemoryStream GetMemoryStream()
        {
            MemoryStream mStream = new MemoryStream();

            // Create local deep copy to write to stream
            BitmapSource clone = Image.Clone();
            clone.Freeze();

            // Write copy to stream
            Encoder.Frames.Add(BitmapFrame.Create(clone));
            Encoder.Save(mStream);
            Encoder.Frames.Clear();

            return mStream;
        }


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
                Directory.CreateDirectory(extractedPath);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception creating save path {SavePath}", extractedPath);
                throw;
            }

            // Save file (allow any IOExceptions to bubble up)
            Stream fStream = null;

            try
            {
                fStream = new FileStream(fullPath, FileMode.Create);

                // Create local deep copy to write to stream
                BitmapSource clone = Image.Clone();
                clone.Freeze();

                // Write copy to stream
                Encoder.Frames.Add(BitmapFrame.Create(clone));
                Encoder.Save(fStream);
                Encoder.Frames.Clear();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception saving BitmapSource {BitmapSource} to file {FilePath}", Image, fullPath);
                throw;
            }
            finally
            {
                fStream?.Dispose();
            }
        }


        /// <summary>
        /// Conversion from file extension to encoding
        /// </summary>
        /// <param name="extension">File extension to convert to encoding</param>
        /// <returns>Encoding represented by file extension</returns>
        private Encoding? ConvertExtensionToEncoding(string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                _logger.Error("Null or empty file extension");
                throw new ArgumentNullException("extension", "File extension cannot be null or empty");
            }

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
        }

        #endregion // Methods
    }
}
