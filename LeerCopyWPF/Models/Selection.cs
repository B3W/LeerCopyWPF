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

using LeerCopyWPF.Enums;
using LeerCopyWPF.Utilities;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LeerCopyWPF.Models
{
    public class Selection
    {
        #region Fields
        #endregion // Fields


        #region Properties

        /// <summary>
        /// Starting point of the selection
        /// </summary>
        public Point StartPt { get; set; }

        /// <summary>
        /// Ending point of the selection
        /// </summary>
        public Point EndPt { get; set; }

        /// <summary>
        /// Image being selected
        /// </summary>
        public BitmapSource Bitmap { get; }

        /// <summary>
        /// Max bounds for the selection. Relative application's selection window.
        /// </summary>
        public Rect SelectionBounds { get; }

        /// <summary>
        /// Bounds for screen selection is occuring on. Relative to other screens.
        /// </summary>
        public Rect ScreenBounds { get; }

        #endregion // Properties


        #region Methods

        /// <summary>
        /// Constructs a selection
        /// </summary>
        /// <param name="bitmap">Image for selection</param>
        /// <param name="screenBounds">Bounds of the screen</param>
        public Selection(BitmapSource bitmap, Rect screenBounds)
        {
            Bitmap = bitmap;
            ScreenBounds = screenBounds;
            StartPt = new Point();
            EndPt = new Point();

            // Selection bounds are derived from screen bounds but always start at (0, 0) because screen
            // coordinates are global where as selection coordinates are relative to the selection windows.
            SelectionBounds = new Rect(0, 0, ScreenBounds.Width, ScreenBounds.Height);
        } // Selection


        /// <summary>
        /// Resets the selection
        /// </summary>
        public void Reset()
        {
            StartPt = new Point();
            EndPt = new Point();
        } // Reset

        #endregion // Methods
    }
}