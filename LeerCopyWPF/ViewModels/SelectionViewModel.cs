using LeerCopyWPF.Commands;
using LeerCopyWPF.Enums;
using LeerCopyWPF.Models;
using LeerCopyWPF.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LeerCopyWPF.ViewModels
{
    public class SelectionViewModel : BaseViewModel
    {
        #region Fields
        private Selection _selection;
        private Rect _selectionRect;
        /// <summary>
        /// Key binding
        /// </summary>
        private string _copy, _edit, _save, _clear, _selectAll, _border, _tips, _swtchScrn, _settings, _quit;
        private ICommand _copyCommand;
        private ICommand _editCommand;
        private ICommand _saveCommand;
        private ICommand _maximizeCommand;
        private ICommand _clearCommand;
        private ICommand _settingsCommand;
        #endregion // Fields

        #region Constants
        private const string ConstDisplayName = "Leer Copy";
        private const string ConstCopyPropName = "CopyKey";
        private const string ConstEditPropName = "EditKey";
        private const string ConstSavePropName = "SaveKey";
        private const string ConstClearPropName = "ClearKey";
        private const string ConstSelectAllPropName = "SelectAll";
        private const string ConstBorderPropName = "BorderKey";
        private const string ConstTipsPropName = "TipsKey";
        private const string ConstSwtchScrnPropName = "SwitchScreenKey";
        private const string ConstSettingsPropName = "SettingsWin";
        private const string ConstQuitPropName = "QuitKey";
        #endregion // Constants

        #region Properties
        public override string DisplayName { get => ConstDisplayName; }

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

        public RectangleGeometry SelectionGeometry { get => new RectangleGeometry(SelectionRect); }

        public BitmapSource Bitmap { get => _selection.Bitmap; }

        public Rect ScreenBounds { get => _selection.ScreenBounds; }

        public bool IsSelecting { get; private set; } = false;

        public bool IsSelected { get; private set; } = false;

        public double BorderThickness { get; }

        public double BorderLeft { get => SelectionRect.Left - BorderThickness; }

        public double BorderTop  { get => SelectionRect.Top - BorderThickness; }

        public double BorderWidth { get => SelectionRect.Width + (BorderThickness * 2); }

        public double BorderHeight { get => SelectionRect.Height + (BorderThickness * 2); }

        public string CopyKey
        {
            get => _copy;
            set
            {
                if (_copy != value)
                {
                    _copy = value;
                    OnPropertyChanged(ConstCopyPropName);
                }
            }
        }

        public string EditKey
        {
            get => _edit;
            set
            {
                if (_edit != value)
                {
                    _edit = value;
                    OnPropertyChanged(ConstEditPropName);
                }
            }
        }

        public string SaveKey
        {
            get => _save;
            set
            {
                if (_save != value)
                {
                    _save = value;
                    OnPropertyChanged(ConstSavePropName);
                }
            }
        }

        public string ClearKey
        {
            get => _clear;
            set
            {
                if (_clear != value)
                {
                    _clear = value;
                    OnPropertyChanged(ConstClearPropName);
                }
            }
        }

        public string SelectAll
        {
            get => _selectAll;
            set
            {
                if (_selectAll != value)
                {
                    _selectAll = value;
                    OnPropertyChanged(ConstSelectAllPropName);
                }
            }
        }

        public string BorderKey
        {
            get => _border;
            set
            {
                if (_border != value)
                {
                    _border = value;
                    OnPropertyChanged(ConstBorderPropName);
                }
            }
        }

        public string TipsKey
        {
            get => _tips;
            set
            {
                if (_tips != value)
                {
                    _tips = value;
                    OnPropertyChanged(ConstTipsPropName);
                }
            }
        }

        public string SwitchScreenKey
        {
            get => _swtchScrn;
            set
            {
                if (_swtchScrn != value)
                {
                    _swtchScrn = value;
                    OnPropertyChanged(ConstSwtchScrnPropName);
                }
            }
        }

        public string SettingsWin
        {
            get => _settings;
            set
            {
                if (_settings != value)
                {
                    _settings = value;
                    OnPropertyChanged(ConstSettingsPropName);
                }
            }
        }

        public string QuitKey
        {
            get => _quit;
            set
            {
                if (_quit != value)
                {
                    _quit = value;
                    OnPropertyChanged(ConstQuitPropName);
                }
            }
        }

        public bool CanCopy { get => !IsSelecting && IsSelected && !_selection.StartPt.Equals(_selection.EndPt); }

        public ICommand CopyCommand { get => _copyCommand ?? (_copyCommand = new RelayCommand(param => CopySelection(), param => CanCopy)); }

        public bool CanEdit { get => !IsSelecting && IsSelected && !_selection.StartPt.Equals(_selection.EndPt); }

        public ICommand EditCommand { get => _editCommand ?? (_editCommand = new RelayCommand(param => EditSelection(), param => CanEdit)); }

        public bool CanSave { get => !IsSelecting && IsSelected && !_selection.StartPt.Equals(_selection.EndPt); }

        public ICommand SaveCommand { get => _saveCommand ?? (_saveCommand = new RelayCommand(param => SaveSelection(), param => CanSave)); }

        public ICommand MaximizeCommand { get => _maximizeCommand ?? (_maximizeCommand = new RelayCommand(param => MaximizeSelection())); }

        public ICommand ClearCommand { get => _clearCommand ?? (_clearCommand = new RelayCommand(param => ClearSelection())); }

        public event EventHandler OpenSettingsEvent;

        public bool CanOpenSettings { get => !IsSelecting; }

        public ICommand SettingsCommand { get => _settingsCommand ?? (_settingsCommand = new RelayCommand(param => ShowSettings(), param => CanOpenSettings)); }

        public bool CanClose { get => !IsSelecting; }

        #endregion // Properties

        #region Constructors
        public SelectionViewModel(Rect bounds)
        {
            // Capture screen
            BitmapSource bitmap = BitmapUtilities.CaptureRect(bounds);
            _selection = new Selection(bitmap, bounds);

            RefreshKeyBindings();
            BorderThickness = Properties.Settings.Default.SelectionBorderThickness;
        }
        #endregion // Constructors

        #region Methods
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


        public void ResizeSelection(KeyActions.KeyDown dir)
        {
            if (IsSelected && !IsSelecting)
            {
                double shftMod = 2.0;
                bool vert = false;
                double offsetX = 0.0, offsetY = 0.0;

                // Set base offset
                switch (dir)
                {
                    case KeyActions.KeyDown.Up:
                        offsetY = -1.0;
                        vert = true;
                        break;
                    case KeyActions.KeyDown.Down:
                        offsetY = 1.0;
                        vert = true;
                        break;
                    case KeyActions.KeyDown.Left:
                        offsetX = -1.0;
                        break;
                    case KeyActions.KeyDown.Right:
                        offsetX = 1.0;
                        break;
                    case KeyActions.KeyDown.Invalid:
                    default:
                        break;
                }

                // Check modifier keys
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    // Speed up resizing
                    if (vert)
                    {
                        offsetY *= shftMod;
                    }
                    else
                    {
                        offsetX *= shftMod;
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
                _selection.Resize(offsetX, offsetY, dir);
                SelectionRect = new Rect(StartPt, EndPt);
            }
        }


        /// <summary>
        /// Fetches the key bindings from settings
        /// </summary>
        private void RefreshKeyBindings()
        {
            Properties.Settings settingsInst = Properties.Settings.Default;

            CopyKey = (string)settingsInst[ConstCopyPropName];
            EditKey = (string)settingsInst[ConstEditPropName];
            SaveKey = (string)settingsInst[ConstSavePropName];
            ClearKey = (string)settingsInst[ConstClearPropName];
            SelectAll = (string)settingsInst[ConstSelectAllPropName];
            BorderKey = (string)settingsInst[ConstBorderPropName];
            TipsKey = (string)settingsInst[ConstTipsPropName];
            SwitchScreenKey = (string)settingsInst[ConstSwtchScrnPropName];
            SettingsWin = (string)settingsInst[ConstSettingsPropName];
            QuitKey = (string)settingsInst[ConstQuitPropName];
        } // RefreshKeyBindings
        #endregion // Methods

        #region Command Functions
        /// <summary>
        /// Copy current selection to the clipboard
        /// </summary>
        public void CopySelection()
        {
            BitmapUtilities.CopyToClipboard(BitmapUtilities.GetCroppedBitmap(Bitmap, SelectionRect));
        }

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
            string tmpFilePath = Properties.Settings.Default.AppDataLoc + tmpFileName + Properties.Settings.Default.DefaultSaveExt;
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
                    throw new ArgumentException("Invalid filename during save", "fileName");
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
        } // OpenSettings
        #endregion // Command Functions
    }
}
