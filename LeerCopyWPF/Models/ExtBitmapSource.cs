using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LeerCopyWPF.Models
{
    /// <summary>
    /// Extends functionality of BitmapSource data structure to include
    /// extra metadata from structures such as the Screen class
    /// </summary>
    public class ExtBitmapSource : IEquatable<ExtBitmapSource>, IComparable<ExtBitmapSource>
    {
        /// <summary>
        /// Bitmap source object
        /// </summary>
        public BitmapSource Bitmap { get; set; }
        /// <summary>
        /// Bounds of the Bitmap relative to screen captured on
        /// </summary>
        public Rect Bounds { get; set; }


        public ExtBitmapSource()
        {
            Bitmap = null;
            Bounds = new Rect();
        }


        public ExtBitmapSource(BitmapSource bms, Rect rect)
        {
            Bitmap = bms;
            Bounds = rect;
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

            if (!(obj is ExtBitmapSource ebs))
            {
                return false;
            }

            return this.Equals(obj as ExtBitmapSource);
        }


        /// <summary>
        /// Checks equality of this ExtBitmapSource object with other
        /// </summary>
        /// <param name="other"></param>
        /// <returns>True if equal, false otherwise</returns>
        public bool Equals(ExtBitmapSource other)
        {
            if (other == null)
            {
                return false;
            }

            return (Bounds.Equals(other.Bounds)) && (Bitmap == other.Bitmap);
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        /// <summary>
        /// Compares this ExtBitmapSource object with other. Top, left is least while bottom, right is greatest value.
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
        public int CompareTo(ExtBitmapSource other)
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
    }
}
