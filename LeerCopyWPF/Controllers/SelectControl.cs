﻿using LeerCopyWPF.Models;
using LeerCopyWPF.Utilities;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace LeerCopyWPF.Controllers
{
    /// <summary>
    /// Controller logic between the UI and underlying selection data structure
    /// </summary>
    public class SelectControl
    {
        /// <summary>
        /// Bitmap of the screen
        /// </summary>
        public BitmapSource Bitmap { get; private set; }
        /// <summary>
        /// Tracks user's selection area
        /// </summary>
        public Selection Selection { get; private set; }
        /// <summary>
        /// Determines if user has made a selection
        /// </summary>
        public bool IsSelected { get; private set; }
        /// <summary>
        /// Determines if user is currently making a selection
        /// </summary>
        public bool IsSelecting { get; private set; }


        public SelectControl(BitmapSource bm)
        {
            Bitmap = bm;
            Selection = new Selection();
            IsSelecting = false;
            IsSelected = false;
        }


        /// <summary>
        /// Start new selection or continue previous
        /// </summary>
        /// <param name="start"></param>
        public void StartSelection(Point start)
        {
            if (IsSelected)
            {
                Selection.UpdateEnd(start);
            } else
            {
                Selection.SetStart(start);
            }
            IsSelecting = true;
        } // StartSelection


        /// <summary>
        /// Update current selection
        /// </summary>
        /// <param name="point"></param>
        public void UpdateSelection(Point point)
        {
            Selection.UpdateEnd(point);
        } // UpdateSelection


        /// <summary>
        /// Update current selection
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void UpdateSelection(double x, double y)
        {
            if (IsSelected && !IsSelecting)
            {
                Selection.UpdateEnd(x, y, true);
            }
        } // UpdateSelection


        /// <summary>
        /// Stop current selection
        /// </summary>
        /// <param name="point"></param>
        public void StopSelection(Point point)
        {
            if (IsSelecting && !Selection.StartPt.Equals(point))
            {
                UpdateSelection(point);
                IsSelected = true;
            } else
            {
                IsSelected = false;
            }
            IsSelecting = false;
        } // StopSelection


        /// <summary>
        /// Retrieve rectangle represeneting current selection area
        /// </summary>
        /// <returns></returns>
        public Rect GetSelectionRect()
        {
            return new Rect(Selection.StartPt, Selection.EndPt);
        } // GetSelectionRect


        /// <summary>
        /// Retrieve rectangular geometry object representing current selection area
        /// </summary>
        /// <returns></returns>
        public RectangleGeometry GetSelectionGeometry()
        {
            return new RectangleGeometry(new Rect(Selection.StartPt, Selection.EndPt));
        } // GetSelectionGeometry


        /// <summary>
        /// Copy current selection to the clipboard
        /// </summary>
        public void CopySelection()
        {
            if (!IsSelecting && IsSelected && !Selection.StartPt.Equals(Selection.EndPt))
            {
                BitmapUtilities.CopyToClipboard(Bitmap, new Rect(Selection.StartPt, Selection.EndPt));
            }
        } // CopySelection


        /// <summary>
        /// Edit current selection in default text editor
        /// </summary>
        public void EditSelection()
        {

        } // EditSelection


        /// <summary>
        /// Save the current selection to disk
        /// </summary>
        /// <param name="owner"></param>
        public void SaveSelection(Window owner)
        {
            if (!IsSelecting && IsSelected && !Selection.StartPt.Equals(Selection.EndPt))
            {
                // Configure save dialog
                Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog
                {
                    AddExtension = true,
                    DefaultExt = ".bmp",
                    FileName = "L33R",
                    Filter = "BMP (.bmp)|*.bmp|GIF (.gif)|*.gif|JPEG (.jpg)|*.jpg;*.jpeg|PNG (.png)|*.png|TIFF (.tif)|*.tif;*.tiff|WMP (.wmp)|*.wmp",
                    InitialDirectory = "./", // TODO: Retrieve last save directory from app settings
                    OverwritePrompt = true,
                    Title = "Save Leer"
                };
                // Show dialog
                Nullable<bool> res = saveDialog.ShowDialog(owner);
                if (res == true)
                {
                    // Crop bitmap to selection area
                    Rect area = new Rect(Selection.StartPt, Selection.EndPt);
                    CroppedBitmap finalBitmap = 
                        new CroppedBitmap(Bitmap, new Int32Rect((int)area.X, (int)area.Y, (int)area.Width, (int)area.Height));
                    // Save as requested format
                    string fileName = saveDialog.FileName;
                    string[] splitFN = fileName.Split('.');
                    string extension = splitFN[splitFN.Length - 1]; // Not possible for split array to have 'Length' < 2 w/valid file name
                    EncodedImage eImage = new EncodedImage(finalBitmap, extension);
                    eImage.SaveToFile(fileName);
                }
            }
        } // SaveSelection


        /// <summary>
        /// Select the entire screen
        /// </summary>
        public void MaximizeSelection()
        {
            IsSelecting = false;
            IsSelected = true;
            Selection.UpdateStart(new Point());
            Selection.UpdateEnd(Bitmap.Width, Bitmap.Height, false);
        } // MaximizeSelection


        /// <summary>
        /// Clear the current selection
        /// </summary>
        public void ClearSelection()
        {
            IsSelecting = false;
            IsSelected = false;
            Selection.Reset();
        } // ClearSelection
    }
}
