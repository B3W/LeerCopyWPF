using LeerCopyWPF.Commands;
using LeerCopyWPF.Constants;
using LeerCopyWPF.Enums;
using LeerCopyWPF.Models;
using LeerCopyWPF.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LeerCopyWPF.ViewModels
{
    public class SelectionViewModel : KeyBindingHelperViewModel
    {
        #region Fields
        
        /// <summary>
        /// Modifier which determines how much to speed up selection resizing
        /// </summary>
        private const double ConstShiftModifier = 2.0;

        /// <summary>
        /// Data structure containing information on a selection
        /// </summary>
        private readonly Selection _selection;

        /// <summary>
        /// Rectangle representing selection bounds
        /// </summary>
        private Rect _selectionRect;

        #endregion // Fields
        
        #region Properties

        public override string DisplayName { get => "Leer Copy"; }

        /// <summary>
        /// Starting point of selection
        /// </summary>
        public Point StartPt
        {
            get => _selection.StartPt;
            set
            {
                if (!_selection.StartPt.Equals(value))
                {
                    _selection.StartPt = value;
                    OnPropertyChanged("StartPt");
                }
            }
        }

        /// <summary>
        /// Ending point of selection
        /// </summary>
        public Point EndPt
        {
            get => _selection.EndPt;
            set
            {
                if (!_selection.EndPt.Equals(value))
                {
                    _selection.EndPt = value;
                    OnPropertyChanged("EndPt");
                }
            }
        }

        /// <summary>
        /// Rectangle representing selection bounds
        /// </summary>
        public Rect SelectionRect
        {
            get => _selectionRect;
            set
            {
                if (!_selectionRect.Equals(value))
                {
                    _selectionRect = value;
                    OnPropertyChanged("SelectionRect");
                    OnPropertyChanged("SelectionGeometry");
                    OnPropertyChanged("BorderLeft");
                    OnPropertyChanged("BorderTop");
                    OnPropertyChanged("BorderWidth");
                    OnPropertyChanged("BorderHeight");
                }
            }
        }

        /// <summary>
        /// Rectangle geometry derived from the selection bounds
        /// </summary>
        public RectangleGeometry SelectionGeometry { get => new RectangleGeometry(SelectionRect); }

        /// <summary>
        /// Image to perform selection on
        /// </summary>
        public BitmapSource Bitmap { get => _selection.Bitmap; }

        /// <summary>
        /// Max bounds for the selection
        /// </summary>
        public Rect ScreenBounds { get => _selection.ScreenBounds; }

        /// <summary>
        /// Flag indicating if the user is currently making a selection
        /// </summary>
        public bool IsSelecting { get; private set; }

        /// <summary>
        /// Flag indicating if the user has finished making a selection
        /// </summary>
        public bool IsSelected { get; private set; }

        /// <summary>
        /// Thickness of border around selection
        /// </summary>
        public double BorderThickness { get; }

        /// <summary>
        /// X coordinate of rectangle bounds representing border
        /// </summary>
        public double BorderLeft { get => SelectionRect.Left - BorderThickness; }

        /// <summary>
        /// Y coordinate of rectangle bounds representing border
        /// </summary>
        public double BorderTop  { get => SelectionRect.Top - BorderThickness; }

        /// <summary>
        /// Width of rectangle bounds representing border
        /// </summary>
        public double BorderWidth { get => SelectionRect.Width + (BorderThickness * 2); }

        /// <summary>
        /// Height of rectangle bounds representing border
        /// </summary>
        public double BorderHeight { get => SelectionRect.Height + (BorderThickness * 2); }

        /// <summary>
        /// Flag indicating if the copy command can be performed
        /// </summary>
        public bool CanCopy { get => !IsSelecting && IsSelected && !_selection.StartPt.Equals(_selection.EndPt); }

        /// <summary>
        /// Flag indicating if the edit command can be performed
        /// </summary>
        public bool CanEdit { get => !IsSelecting && IsSelected && !_selection.StartPt.Equals(_selection.EndPt); }

        /// <summary>
        /// Flag indicating if the save command can be performed
        /// </summary>
        public bool CanSave { get => !IsSelecting && IsSelected && !_selection.StartPt.Equals(_selection.EndPt); }

        /// <summary>
        /// Flag indicating if the settings window can be opened
        /// </summary>
        public bool CanOpenSettings { get => !IsSelecting; }

        /// <summary>
        /// Flag indicating if the selection window can be closed
        /// </summary>
        public bool CanClose { get => !IsSelecting; }

        /// <summary>
        /// Command for copying selection
        /// </summary>
        public ICommand CopyCommand { get; }

        /// <summary>
        /// Command for editing selection
        /// </summary>
        public ICommand EditCommand { get; }

        /// <summary>
        /// Command for saving selection
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Command for maximizing selection to screen bounds
        /// </summary>
        public ICommand MaximizeCommand { get; }

        /// <summary>
        /// Command for clearing selection
        /// </summary>
        public ICommand ClearCommand { get; }

        /// <summary>
        /// Command for opening settings window
        /// </summary>
        public ICommand SettingsCommand { get; }


        public event EventHandler OpenSettingsEvent;

        #endregion // Properties


        #region Methods

        public SelectionViewModel(Rect bounds) : base()
        {
            // Capture screen
            BitmapSource bitmap = BitmapUtilities.CaptureRect(bounds);
            _selection = new Selection(bitmap, bounds);

            IsSelecting = false;
            IsSelected = false;
            BorderThickness = Properties.Settings.Default.SelectionBorderThickness;

            // Setup commands
            CopyCommand = new RelayCommand(param => CopySelection(), param => CanCopy);
            EditCommand = new RelayCommand(param => EditSelection(), param => CanEdit);
            SaveCommand = new RelayCommand(param => SaveSelection(), param => CanSave);
            MaximizeCommand = new RelayCommand(param => MaximizeSelection());
            ClearCommand = new RelayCommand(param => ClearSelection());
            SettingsCommand = new RelayCommand(param => ShowSettings(), param => CanOpenSettings);
        } // SelectionViewModel


        /// <summary>
        /// Start new selection or continue previous
        /// </summary>
        /// <param name="point"></param>
        public void StartSelection(Point point)
        {
            if (IsSelected)
            {
                _selection.EndPt = point;
                SelectionRect = new Rect(StartPt, EndPt);
            }
            else
            {
                _selection.StartPt = point;
                _selection.EndPt = point;
            }
            IsSelecting = true;
        } // StartSelection


        /// <summary>
        /// Update current selection
        /// </summary>
        /// <param name="point"></param>
        public void UpdateSelection(Point point)
        {
            if (IsSelecting)
            {
                _selection.EndPt = point;
                SelectionRect = new Rect(StartPt, EndPt);
            }
        } // UpdateSelection


        /// <summary>
        /// Stop current selection
        /// </summary>
        /// <param name="point"></param>
        public void StopSelection(Point point)
        {
            if (IsSelecting && !_selection.StartPt.Equals(point))
            {
                _selection.EndPt = point;
                SelectionRect = new Rect(StartPt, EndPt);
                IsSelected = true;
            }
            else
            {
                IsSelected = false;
            }

            IsSelecting = false;
        } // StopSelection


        /// <summary>
        /// Resizes the selection based on a key down action
        /// </summary>
        /// <param name="keyAction">Key action performed</param>
        public void ResizeSelection(KeyDownAction keyAction)
        {
            if (IsSelected && !IsSelecting)
            {
                bool vert = false;
                double offsetX = 0.0;
                double offsetY = 0.0;

                // Set base offset
                switch (keyAction)
                {
                    case KeyDownAction.Up:
                        offsetY = -1.0;
                        vert = true;
                        break;
                    case KeyDownAction.Down:
                        offsetY = 1.0;
                        vert = true;
                        break;
                    case KeyDownAction.Left:
                        offsetX = -1.0;
                        break;
                    case KeyDownAction.Right:
                        offsetX = 1.0;
                        break;
                    case KeyDownAction.Invalid:
                    default:
                        break;
                }

                // Check modifier keys
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    // Speed up resizing
                    if (vert)
                    {
                        offsetY *= ConstShiftModifier;
                    }
                    else
                    {
                        offsetX *= ConstShiftModifier;
                    }
                }

                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    // Reverse direction
                    if (vert)
                    {
                        offsetY = -offsetY;
                    }
                    else
                    {
                        offsetX = -offsetX;
                    }
                }

                _selection.Resize(offsetX, offsetY, keyAction);
                SelectionRect = new Rect(StartPt, EndPt);
            }
        } // ResizeSelection


        #region Command Functions

        /// <summary>
        /// Copy current selection to the clipboard
        /// </summary>
        public void CopySelection()
        {
            BitmapUtilities.CopyToClipboard(BitmapUtilities.GetCroppedBitmap(Bitmap, SelectionRect));
        } // CopySelection

        /// <summary>
        /// Edit current selection in default text editor
        /// </summary>
        public void EditSelection()
        {
            // Crop bitmap to selection area 
            CroppedBitmap finalBitmap = BitmapUtilities.GetCroppedBitmap(Bitmap, SelectionRect);

            // Save to temp file
            EncodedImage eImage = new EncodedImage(finalBitmap, Properties.Settings.Default.DefaultSaveExt);
            string tmpFileName = Properties.Settings.Default.DefaultFileName + "_" + DateTime.Now.Ticks;  // Use DateTime to avoid name collisions
            string tmpFilePath = Path.Combine(Properties.Settings.Default.AppDataLoc, tmpFileName + Properties.Settings.Default.DefaultSaveExt);
            eImage.SaveToFile(tmpFilePath);

            // Open in editor
            System.Diagnostics.ProcessStartInfo stInfo = new System.Diagnostics.ProcessStartInfo(tmpFilePath)
            {
                Verb = "edit"
            };

            try
            {
                System.Diagnostics.Process.Start(stInfo);
            }
            catch (Exception)
            {
                // TODO Error logging
            }
        } // EditSelection


        /// <summary>
        /// Save the current selection to disk
        /// </summary>
        /// <param name="owner"></param>
        public void SaveSelection()
        {
            // Configure save dialog
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = Properties.Settings.Default.DefaultSaveExt,
                FileName = Properties.Settings.Default.DefaultFileName,
                Filter = "BMP (.bmp)|*.bmp|GIF (.gif)|*.gif|JPEG (.jpg)|*.jpg;*.jpeg|PNG (.png)|*.png|TIFF (.tif)|*.tif;*.tiff|WMP (.wmp)|*.wmp",
                InitialDirectory = Properties.Settings.Default.LastSavePath,
                OverwritePrompt = true,
                Title = "Save Leer"
            };

            // Show dialog
            bool? res = saveDialog.ShowDialog();

            if (res == true)
            {
                // Crop bitmap to selection area
                CroppedBitmap finalBitmap = BitmapUtilities.GetCroppedBitmap(Bitmap, SelectionRect);

                // Save as requested format
                string filePath = saveDialog.FileName;
                int extPos = filePath.LastIndexOf('.');  // 'IndexOf' methods more performant than 'Split'

                if (extPos == -1)
                {
                    // TODO exception logging
                    throw new ArgumentException($"SelectionViewModel.SaveSelection: '{filePath}' is invalid filename", "fileName");
                }

                string extension = filePath.Substring(extPos + 1);
                EncodedImage eImage = new EncodedImage(finalBitmap, extension);
                eImage.SaveToFile(filePath);

                // Update last save directory
                int fileNamePos = filePath.LastIndexOf(System.IO.Path.DirectorySeparatorChar);

                if (fileNamePos != -1)  // If somehow backslash not found do nothing
                {
                    string lastSavePath = filePath.Substring(0, fileNamePos + 1);
                    Properties.Settings.Default.LastSavePath = lastSavePath;
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
            _selection.StartPt = new Point();
            _selection.EndPt = new Point(Bitmap.Width, Bitmap.Height);
            SelectionRect = new Rect(StartPt, EndPt);
        } // MaximizeSelection


        /// <summary>
        /// Clear the current selection
        /// </summary>
        public void ClearSelection()
        {
            IsSelecting = false;
            IsSelected = false;
            _selection.Reset();
            SelectionRect = new Rect(StartPt, EndPt);
        } // ClearSelection


        /// <summary>
        /// Opens SettingsWindow
        /// </summary>
        public void ShowSettings()
        {
            OpenSettingsEvent?.Invoke(this, EventArgs.Empty);
            RefreshKeyBindings();
        } // ShowSettings

        #endregion // Command Functions

        #endregion // Methods
    }
}
