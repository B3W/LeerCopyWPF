using LeerCopyWPF.Commands;
using LeerCopyWPF.Constants;
using LeerCopyWPF.Controller;
using LeerCopyWPF.Enums;
using LeerCopyWPF.Models;
using LeerCopyWPF.Utilities;
using Serilog;
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
    public class SelectionViewModel : BaseViewModel
    {
        #region Fields

        /// <summary>
        /// Handle to logger for this source context
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Modifier which determines how much to speed up selection resizing
        /// </summary>
        private const double ConstShiftModifier = 2.0;

        /// <summary>
        /// WPF element that owns this viewmodel
        /// </summary>
        private readonly IInputElement _inputOwner;

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
        public Rect SelectionBounds { get => _selection.SelectionBounds; }

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
        /// Command for toggling the border on/off
        /// </summary>
        public ICommand BorderCommand { get; }

        /// <summary>
        /// Command for toggling the tips on/off
        /// </summary>
        public ICommand TipsCommand { get; }

        /// <summary>
        /// Command for key down event
        /// </summary>
        public ICommand KeyDownCommand { get; }

        /// <summary>
        /// Command for mouse down event
        /// </summary>
        public ICommand MouseDownCommand { get; }

        /// <summary>
        /// Command for mouse move event
        /// </summary>
        public ICommand MouseUpCommand { get; }

        /// <summary>
        /// Command for mouse move event
        /// </summary>
        public ICommand MouseMoveCommand { get; }

        #endregion // Properties


        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputOwner"></param>
        /// <param name="bounds"></param>
        public SelectionViewModel(IInputElement inputOwner, Rect bounds)
        {
            _logger = Log.ForContext<SelectionViewModel>();
            _inputOwner = inputOwner;

            // Capture screen
            BitmapSource bitmap = BitmapUtilities.CaptureRect(bounds);
            _selection = new Selection(bitmap, bounds);

            IsSelecting = false;
            IsSelected = false;
            BorderThickness = Properties.Settings.Default.SelectionBorderThickness;

            // Setup commands
            CopyCommand = new RelayCommand(param => CopySelection(), param => CanCopy);
            EditCommand = new RelayCommand(param => EditSelection(), param => CanEdit);
            SaveCommand = new RelayCommand<string>(SaveSelection, param => CanSave);
            MaximizeCommand = new RelayCommand(param => MaximizeSelection());
            ClearCommand = new RelayCommand(param => ClearSelection());
            BorderCommand = new RelayCommand(param => ToggleBorder());
            TipsCommand = new RelayCommand(param => ToggleTips());
            KeyDownCommand = new RelayCommand<KeyEventArgs>(KeyDown);
            MouseDownCommand = new RelayCommand<MouseButtonEventArgs>(MouseDown);
            MouseUpCommand = new RelayCommand<MouseButtonEventArgs>(MouseUp);
            MouseMoveCommand = new RelayCommand<MouseEventArgs>(MouseMove);
        }


        /// <summary>
        /// Start new selection or continue previous
        /// </summary>
        /// <param name="point"></param>
        private void StartSelection(Point point)
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
        }


        /// <summary>
        /// Update current selection
        /// </summary>
        /// <param name="point"></param>
        private void UpdateSelection(Point point)
        {
            if (IsSelecting)
            {
                _selection.EndPt = point;
                SelectionRect = new Rect(StartPt, EndPt);
            }
        }


        /// <summary>
        /// Stop current selection
        /// </summary>
        /// <param name="point"></param>
        private void StopSelection(Point point)
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
        }


        /// <summary>
        /// Resizes the selection based on a key down action
        /// </summary>
        /// <param name="dir">Direction to resize selection</param>
        private void ResizeSelection(ResizeDirection dir, bool fastResize, bool reverseResize)
        {
            if (IsSelected && !IsSelecting)
            {
                double offsetX;
                double offsetY;

                // Set base offset
                switch (dir)
                {
                    case ResizeDirection.Up:
                        // Calculate total offset for resize
                        offsetY = fastResize ? -ConstShiftModifier : -1.0;

                        if (reverseResize)
                        {
                            offsetY = -offsetY;
                        }

                        // Resize
                        if (StartPt.Y > EndPt.Y)
                        {
                            double modifiedY = EndPt.Y + offsetY;
                            double clampedY = modifiedY.Clamp(SelectionBounds.Top, StartPt.Y);

                            EndPt = new Point(EndPt.X, clampedY);
                        }
                        else
                        {
                            double modifiedY = StartPt.Y + offsetY;
                            double clampedY = modifiedY.Clamp(SelectionBounds.Top, EndPt.Y);

                            StartPt = new Point(StartPt.X, clampedY);
                        }

                        SelectionRect = new Rect(StartPt, EndPt);
                        break;

                    case ResizeDirection.Down:
                        // Calculate total offset for resize
                        offsetY = fastResize ? ConstShiftModifier : 1.0;

                        if (reverseResize)
                        {
                            offsetY = -offsetY;
                        }

                        // Resize
                        if (StartPt.Y > EndPt.Y)
                        {
                            double modifiedY = StartPt.Y + offsetY;
                            double clampedY = modifiedY.Clamp(EndPt.Y, SelectionBounds.Bottom);

                            StartPt = new Point(StartPt.X, clampedY);
                        }
                        else
                        {
                            double modifiedY = EndPt.Y + offsetY;
                            double clampedY = modifiedY.Clamp(StartPt.Y, SelectionBounds.Bottom);

                            EndPt = new Point(EndPt.X, clampedY);
                        }

                        SelectionRect = new Rect(StartPt, EndPt);
                        break;

                    case ResizeDirection.Left:
                        // Calculate total offset for resize
                        offsetX = fastResize ? -ConstShiftModifier : -1.0;

                        if (reverseResize)
                        {
                            offsetX = -offsetX;
                        }

                        // Resize
                        if (StartPt.X > EndPt.X)
                        {
                            double modifiedX = EndPt.X + offsetX;
                            double clampedX = modifiedX.Clamp(SelectionBounds.Left, StartPt.X);

                            EndPt = new Point(clampedX, EndPt.Y);
                        }
                        else
                        {
                            double modifiedX = StartPt.X + offsetX;
                            double clampedX = modifiedX.Clamp(SelectionBounds.Left, EndPt.X);

                            StartPt = new Point(clampedX, StartPt.Y);
                        }

                        SelectionRect = new Rect(StartPt, EndPt);
                        break;

                    case ResizeDirection.Right:
                        // Calculate total offset for resize
                        offsetX = fastResize ? ConstShiftModifier : 1.0;

                        if (reverseResize)
                        {
                            offsetX = -offsetX;
                        }

                        // Resize
                        if (StartPt.X > EndPt.X)
                        {
                            double modifiedX = StartPt.X + offsetX;
                            double clampedX = modifiedX.Clamp(EndPt.X, SelectionBounds.Right);

                            StartPt = new Point(clampedX, StartPt.Y);
                        }
                        else
                        {
                            double modifiedX = EndPt.X + offsetX;
                            double clampedX = modifiedX.Clamp(StartPt.X, SelectionBounds.Right);

                            EndPt = new Point(clampedX, EndPt.Y);
                        }

                        SelectionRect = new Rect(StartPt, EndPt);
                        break;

                    case ResizeDirection.Invalid: // Intentional fallthrough
                    default:
                        break;
                }
            }
        }


        #region Command Functions

        /// <summary>
        /// Copy current selection to the clipboard
        /// </summary>
        private void CopySelection()
        {
            _logger.Information("User requested to copy selection");
            BitmapUtilities.CopyToClipboard(BitmapUtilities.GetCroppedBitmap(Bitmap, SelectionRect));
        }


        /// <summary>
        /// Edit current selection in default image editor
        /// </summary>
        private void EditSelection()
        {
            _logger.Information("User requested to edit selection");

            // Crop bitmap to selection area 
            CroppedBitmap finalBitmap = BitmapUtilities.GetCroppedBitmap(Bitmap, SelectionRect);

            // Save to temp file
            EncodedImage eImage = new EncodedImage(finalBitmap, Properties.Settings.Default.DefaultSaveExt);

            string tmpFileName = Properties.Settings.Default.DefaultFileName + "_" + DateTime.Now.Ticks;  // Use DateTime to avoid name collisions
            string tmpFilePath = Path.Combine(Properties.Settings.Default.AppDataLoc,
                                              tmpFileName + Properties.Settings.Default.DefaultSaveExt);

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
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception while opening image editor");

                // TODO Show notification
            }
        }


        /// <summary>
        /// Save the current selection to disk
        /// </summary>
        /// <param name="savePath">Path to save selection to</param>
        private void SaveSelection(string savePath)
        {
            _logger.Information("User requested to save selection");

            // Crop bitmap to selection area
            CroppedBitmap finalBitmap = BitmapUtilities.GetCroppedBitmap(Bitmap, SelectionRect);

            // Save as requested format
            FileInfo saveFile = new FileInfo(savePath);

            EncodedImage encodedImage = new EncodedImage(finalBitmap, saveFile.Extension);
            encodedImage.SaveToFile(savePath);

            // Update last save directory
            Properties.Settings.Default.LastSavePath = saveFile.DirectoryName;
        }


        /// <summary>
        /// Select the entire screen
        /// </summary>
        private void MaximizeSelection()
        {
            _logger.Debug("User requested to maximize selection");

            IsSelecting = false;
            IsSelected = true;
            _selection.StartPt = new Point();
            _selection.EndPt = new Point(Bitmap.Width, Bitmap.Height);
            SelectionRect = new Rect(StartPt, EndPt);
        }


        /// <summary>
        /// Clear the current selection
        /// </summary>
        private void ClearSelection()
        {
            _logger.Debug("User requested to clear selection");

            IsSelecting = false;
            IsSelected = false;
            _selection.Reset();
            SelectionRect = new Rect(StartPt, EndPt);
        }


        /// <summary>
        /// Toggles the selection border on/off
        /// </summary>
        private void ToggleBorder()
        {
            _logger.Debug("User requested to toggle border");
            Properties.Settings.Default.BorderVisibility = !Properties.Settings.Default.BorderVisibility;
        }


        /// <summary>
        /// Toggles the application hints on/off
        /// </summary>
        private void ToggleTips()
        {
            _logger.Debug("User requested to toggle tips");
            Properties.Settings.Default.TipsVisibility = !Properties.Settings.Default.TipsVisibility;
        }


        /// <summary>
        /// Handler for key down command
        /// </summary>
        private void KeyDown(KeyEventArgs e)
        {
            // Check modifier keys
            bool shiftPressed = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
            bool controlPressed = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl);

            switch (e.Key)
            {
                case Key.Up:
                    ResizeSelection(ResizeDirection.Up, shiftPressed, controlPressed);
                    break;

                case Key.Down:
                    ResizeSelection(ResizeDirection.Down, shiftPressed, controlPressed);
                    break;

                case Key.Left:
                    ResizeSelection(ResizeDirection.Left, shiftPressed, controlPressed);
                    break;

                case Key.Right:
                    ResizeSelection(ResizeDirection.Right, shiftPressed, controlPressed);
                    break;

                default:
                    // Do nothing
                    break;
            }

            e.Handled = true;
        }


        /// <summary>
        /// Handler for the mouse down command
        /// </summary>
        /// <param name="e">Arguments for the mouse down event</param>
        private void MouseDown(MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(_inputOwner);
            StartSelection(position);

            e.Handled = true;
        }


        /// <summary>
        /// Handler for the mouse up command
        /// </summary>
        /// <param name="e">Arguments for the mouse up event</param>
        private void MouseUp(MouseButtonEventArgs e)
        {
            if (IsSelecting)
            {
                Point position = e.GetPosition(_inputOwner);
                StopSelection(position);
            }

            e.Handled = true;
        }


        /// <summary>
        /// Handler for the mouse move command
        /// </summary>
        /// <param name="e">Arguments for the mouse move event</param>
        private void MouseMove(MouseEventArgs e)
        {
            Point position = e.GetPosition(_inputOwner);
            UpdateSelection(position);

            e.Handled = true;
        }

        #endregion // Command Functions

        #endregion // Methods
    }
}
