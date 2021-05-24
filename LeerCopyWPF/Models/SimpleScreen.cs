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

using System;
using System.Windows;

namespace LeerCopyWPF.Models
{
    /// <summary>
    /// 'Wrapper' class for System.Windows.Forms.Screen to hold relevant
    /// screen data for screen capturing
    /// </summary>
    public class SimpleScreen : IEquatable<SimpleScreen>, IComparable<SimpleScreen>
    {
        #region Fields
        #endregion // Fields


        #region Properties

        /// <summary>
        /// The number of bits of memory, associated with one pixel of data
        /// </summary>
        public int BitsPerPixel { get; set; }

        /// <summary>
        /// Bounds of the Bitmap relative to screen captured on
        /// </summary>
        public Rect Bounds { get; set; }

        /// <summary>
        /// Name of the device associated with screen
        /// </summary>
        public string DeviceName { get; set; }

        #endregion // Properties


        #region Methods

        public SimpleScreen()
        {
            BitsPerPixel = -1;
            Bounds = new Rect();
            DeviceName = "<none>";
        }


        public SimpleScreen(int bpp, Rect bounds, string name)
        {
            BitsPerPixel = bpp;
            Bounds = bounds;
            DeviceName = name;
        }


        /// <summary>
        /// Checks equality of this ExtBitmapSource object to 'obj' as an ExtBitmapSource
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is SimpleScreen ss))
            {
                return false;
            }

            return this.Equals(obj as SimpleScreen);
        }


        /// <summary>
        /// Checks equality of this ExtBitmapSource object with other
        /// </summary>
        /// <param name="other"></param>
        /// <returns>True if equal, false otherwise</returns>
        public bool Equals(SimpleScreen other)
        {
            if (other == null)
            {
                return false;
            }

            return (Bounds.Equals(other.Bounds)) && (DeviceName.Equals(other.DeviceName));
        }


        /// <summary>
        /// Calculates object's hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        /// <summary>
        /// Compares this SimpleScreen object with other. Top, left is least while bottom, right is greatest value.
        /// Vertical positioning has greater priority than horizontal positioning.
        /// -1 -1 -1
        /// -1  x  1
        ///  1  1  1
        /// </summary>
        /// <param name="other"></param>
        /// <returns>
        /// Less than 0 if this object precedes other in sort order,
        /// 0 if this object occurs in the same position in the sort order as other,
        /// or greater than 0 if this object follows other in sort order
        /// </returns>
        public int CompareTo(SimpleScreen other)
        {
            Rect oBounds = other.Bounds;

            if (Bounds.Equals(oBounds))
            {
                return 0;
            }
            else if (Bounds.Right <= oBounds.Left) // 'This' left of 'other'
            {
                if (Bounds.Bottom <= oBounds.Top) // 'This' above 'other'
                {
                    return -1;
                }
                else if (Bounds.Top >= oBounds.Bottom) // 'This' below 'other'
                {
                    return 1;
                }
                else // 'This' in line with 'other'
                {
                    return -1;
                }
            }
            else if (Bounds.Left >= oBounds.Right) // 'This' right of 'other'
            {
                if (Bounds.Bottom <= oBounds.Top) // 'This' above 'other'
                {
                    return -1;
                }
                else if (Bounds.Top >= oBounds.Bottom) // 'This' below 'other'
                {
                    return 1;
                }
                else // 'This' in line with 'other'
                {
                    return 1;
                }
            }
            else // 'This' overlapping with 'other' horizontally
            {
                if (Bounds.Bottom <= oBounds.Top) // 'This' above 'other'
                {
                    return -1;
                }
                else if (Bounds.Top >= oBounds.Bottom) // 'This' below 'other'
                {
                    return 1;
                }
                else // 'This' overlapping with 'other' vertically
                {
                    if (Bounds.Top < oBounds.Top || Bounds.Left < oBounds.Left) // 'This' left/up, left/up/up, right of 'other'
                    {
                        return -1;
                    }
                    else // 'This' down/down, right/right of 'other'
                    {
                        return 1;
                    }
                }
            }
        }


        public override string ToString()
        {
            string boundsJSONStr = $"\"Bounds\":{{\"X\":{Bounds.X},\"Y\":{Bounds.Y},\"Width\":{Bounds.Width},\"Height\":{Bounds.Height}}}";

            return $"{{\"BitsPerPixel\":{BitsPerPixel},{boundsJSONStr},\"DeviceName\":\"{DeviceName}\"}}";
        }

        #endregion // Methods
    }
}
