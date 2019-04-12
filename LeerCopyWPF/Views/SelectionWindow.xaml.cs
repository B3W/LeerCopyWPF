using LeerCopyWPF.Controllers;
using LeerCopyWPF.Enums;
using LeerCopyWPF.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LeerCopyWPF
{
    /// <summary>
    /// Interaction logic for SelectionWindow.xaml
    /// </summary>
    public partial class SelectionWindow : Window
    {
        /// <summary>
        /// Controller for making selections
        /// </summary>
        private SelectControl selectControl;
        /// <summary>
        /// Bitmap of the screen
        /// </summary>
        private BitmapSource bitmap;
        /// <summary>
        /// Dictionary for quick lookup of key up mappings
        /// </summary>
        private IDictionary<Key, KeyActions.KeyUp> keyUpMappings;
        /// <summary>
        /// Dictionary for quick lookup of key up mappings
        /// </summary>
        private IDictionary<Key, KeyActions.KeyDown> keyDownMappings;
        /// <summary>
        /// Flag indicating whether switch is possible
        /// </summary>
        private bool switchValid;
        /// <summary>
        /// Flag to prevent loaded event from firing multiple times
        /// </summary>
        private bool winLoaded = false;
        /// <summary>
        /// Bounds of the screen this window is placed on
        /// </summary>
        private Rect screenBounds;
        /// <summary>
        /// Event for signaling the MainWindow
        /// </summary>
        public event EventHandler<FlagEventArgs> SignalMain;


        public SelectionWindow(Rect bounds, bool switchValid)
        {
            // Register window lifetime events
            this.Loaded += SelectionWindow_Loaded;

            InitializeComponent();

            this.switchValid = switchValid;
            this.screenBounds = bounds;

            // Place form on correct screen
            this.Left = bounds.Left;
            this.Top = bounds.Top;

            // Bind keys to actions
            InitKeyUpMappings();
            InitKeyDownMappings();

            // Register event handlers
            this.PreviewKeyUp += SelectionWindow_PreviewKeyUp;
            this.PreviewKeyDown += SelectionWindow_PreviewKeyDown;
            this.PreviewMouseLeftButtonDown += SelectionWindow_MouseLeftButtonDown;
            this.PreviewMouseLeftButtonUp += SelectionWindow_MouseLeftButtonUp;
            this.PreviewMouseMove += SelectionWindow_MouseMove;
        } // SelectionWindow


        /// <summary>
        /// Initialize key bindings for the key up event
        /// </summary>
        private void InitKeyUpMappings()
        {
            // Retrieve app settings
            Properties.Settings settings = Properties.Settings.Default;

            // Populate key mappings into dictionary
            keyUpMappings = new Dictionary<Key, KeyActions.KeyUp>();
            string[] keyUpNames = settings.KeyUpNames;
            KeyConverter converter = new KeyConverter();

            foreach (string name in keyUpNames)
            {
                try
                {
                    keyUpMappings.Add((Key)converter.ConvertFromString((string)settings[name]), ActionConverter.KeyUpStrToEnum(name));
                } catch (SettingsPropertyNotFoundException)
                {
                    // TODO exception logging
                    throw;
                }
            }
        } // InitKeyUpMappings


        /// <summary>
        /// Initialize key bindings for the key down event
        /// </summary>
        private void InitKeyDownMappings()
        {
            // Populate key mappings into dictionary
            keyDownMappings = new Dictionary<Key, KeyActions.KeyDown>
            {
                // Add arrow keys
                { Key.Up, KeyActions.KeyDown.Up },
                { Key.Down, KeyActions.KeyDown.Down },
                { Key.Left, KeyActions.KeyDown.Left },
                { Key.Right, KeyActions.KeyDown.Right }
            };

        } // InitKeyDownMappings


        /// <summary>
        /// 'Repaint' the updated portion of the selected image
        /// </summary>
        private void UpdateDisplayedImage()
        {
            RectangleGeometry selectionArea = selectControl.GetSelectionGeometry();

            // Clip displayed image
            SelectionImg.Clip = selectionArea;

            // Set border outside of the clipped image
            double offset = BorderRect.StrokeThickness;
            Canvas.SetLeft(BorderRect, selectionArea.Rect.Left - offset);
            Canvas.SetTop(BorderRect, selectionArea.Rect.Top - offset);
            BorderRect.Width = selectionArea.Rect.Width + (offset * 2);
            BorderRect.Height = selectionArea.Rect.Height + (offset * 2);
        } // UpdateDisplayedImage


        /// <summary>
        /// Raise the event to signal MainWindow
        /// </summary>
        private void RaiseSignal(bool switchFlag)
        {
            this.SignalMain?.Invoke(this, new FlagEventArgs { Flag = switchFlag });
        } // RaiseSignal


        /// <summary>
        /// Switch screen if possible
        /// </summary>
        private void SwitchScreens()
        {
            if (switchValid)
            {
                this.Hide();
                RaiseSignal(true);
            }
        } // SwitchScreens


        private void SelectionWindow_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            KeyActions.KeyUp action;
            Visibility vis;

            if (keyUpMappings.ContainsKey(e.Key))
            {
                action = keyUpMappings[e.Key];
                switch (action)
                {
                    case KeyActions.KeyUp.Copy:
                        // Copy selection to the clipboard
                        selectControl.CopySelection();
                        break;
                    case KeyActions.KeyUp.Edit:
                        // Edit the selection in default image editor
                        selectControl.EditSelection();
                        break;
                    case KeyActions.KeyUp.Save:
                        // Save the selection to disk
                        selectControl.SaveSelection(this);
                        break;
                    case KeyActions.KeyUp.SelectAll:
                        // Select the entire screen
                        selectControl.MaximizeSelection();
                        UpdateDisplayedImage();
                        break;
                    case KeyActions.KeyUp.Clear:
                        // Clear the current selection
                        selectControl.ClearSelection();
                        UpdateDisplayedImage();
                        break;
                    case KeyActions.KeyUp.Border:
                        // Show/Hide border
                        vis = (Properties.Settings.Default.BorderVisibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
                        Properties.Settings.Default.BorderVisibility = vis;
                        break;
                    case KeyActions.KeyUp.Tips:
                        // Show/Hide tip labels
                        vis = (Properties.Settings.Default.TipsVisibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
                        Properties.Settings.Default.TipsVisibility = vis;
                        break;
                    case KeyActions.KeyUp.Switch:
                        e.Handled = true;
                        SwitchScreens();
                        break;
                    case KeyActions.KeyUp.Settings:
                        // Open up settings window
                        // TODO
                        break;
                    case KeyActions.KeyUp.Quit:
                        // Quit selection
                        RaiseSignal(false);
                        this.Close();
                        break;
                    case KeyActions.KeyUp.Invalid:
                    default:
                        break;
                }
            }
        } // SelectionWindow_PreviewKeyDown


        /// <summary>
        /// Resizes the user's selection based on arrow key presses
        /// </summary>
        /// <param name="dir"></param>
        private void ResizeSelection(KeyActions.KeyDown dir)
        {
            if (selectControl.IsSelected && !selectControl.IsSelecting)
            {
                double shftMod = 2.0;
                bool vert = false;
                double offsetX = 0.0;
                double offsetY = 0.0;

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

                selectControl.Selection.Resize(offsetX, offsetY, dir, screenBounds);
                UpdateDisplayedImage();
            }
        } // ResizeSelection


        private void SelectionWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyActions.KeyDown action;

            if (keyDownMappings.ContainsKey(e.Key))
            {
                action = keyDownMappings[e.Key];
                switch (action)
                {
                    case KeyActions.KeyDown.Invalid:
                        break;
                    default:
                        // Arrow keys pressed
                        ResizeSelection(action);
                        break;
                }
            }
        } // SelectionWindow_PreviewKeyDown


        private void SelectionWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!winLoaded)
            {
                // Capture screen
                bitmap = BitmapUtilities.CaptureRect(screenBounds);

                // Initialize user selection view
                ScreenImg.Source = bitmap;
                SelectionImg.Source = bitmap;
                SelectionImg.Clip = new RectangleGeometry();
                SelectionImg.Visibility = Visibility.Visible;

                selectControl = new SelectControl(bitmap, screenBounds);

                // Don't show the tip if you cannot switch screens
                if (!switchValid)
                {
                    LabelPanel.Children.Remove(SwitchLblPanel);
                }

                winLoaded = true;
            }

            this.WindowState = WindowState.Maximized;
        } // SelectionWindow_Loaded


        private void SelectionWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectControl.StartSelection(e.GetPosition(this));
            UpdateDisplayedImage();
        } // SelectionWindow_MouseLeftButtonDown


        private void SelectionWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            selectControl.StopSelection(e.GetPosition(this));
        } // SelectionWindow_MouseLeftButtonUp


        private void SelectionWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectControl.IsSelecting)
            {
                selectControl.UpdateSelection(e.GetPosition(this));
                UpdateDisplayedImage();
            }
        } // SelectionWindow_MouseMove


        protected override void OnClosing(CancelEventArgs e)
        {
            // Save settings
            Properties.Settings.Default.Save();

            base.OnClosing(e);
        }
    }
}
