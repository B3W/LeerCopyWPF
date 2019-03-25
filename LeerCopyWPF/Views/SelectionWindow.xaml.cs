﻿using LeerCopyWPF.Controllers;
using LeerCopyWPF.Enums;
using LeerCopyWPF.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        /// Dictionary for quick lookup of key mappings
        /// </summary>
        private IDictionary<Key, Actions.ActionEnum> keyMappings;
        /// <summary>
        /// Flag to set when screen switch requested
        /// </summary>
        private bool switchFlag;
        /// <summary>
        /// Flag indicating whether switch is possible
        /// </summary>
        private bool switchValid;
        /// <summary>
        /// Flag to prevent loaded event from firing multiple times
        /// </summary>
        private bool winLoaded = false;

        private Rect screenBounds;

        public event EventHandler SignalMain;

        public SelectionWindow(Rect bounds, ref bool switchFlag, bool switchValid)
        {
            // Register window lifetime events
            this.Loaded += SelectionWindow_Loaded;

            InitializeComponent();

            this.switchFlag = switchFlag;
            this.switchValid = switchValid;
            this.screenBounds = bounds;

            // Place form on correct screen
            this.Left = bounds.Left;
            this.Top = bounds.Top;

            // Bind keys to actions
            InitKeyMappings();

            // Register event handlers
            this.PreviewKeyDown += SelectionWindow_PreviewKeyDown;
            this.PreviewMouseLeftButtonDown += SelectionWindow_MouseLeftButtonDown;
            this.PreviewMouseLeftButtonUp += SelectionWindow_MouseLeftButtonUp;
            this.PreviewMouseMove += SelectionWindow_MouseMove;
        } // SelectionWindow


        private void InitKeyMappings()
        {
            // Retrieve app settings
            Properties.Settings settings = Properties.Settings.Default;
            // Populate key mappings into dictionary
            keyMappings = new Dictionary<Key, Actions.ActionEnum>();
            string[] keyNames = settings.KeyNames;
            KeyConverter converter = new KeyConverter();
            foreach (string name in keyNames)
            {
                try
                {
                    keyMappings.Add((Key)converter.ConvertFromString((string)settings[name]), ActionConverter.KeyStrToEnum(name));
                } catch (SettingsPropertyNotFoundException)
                {
                    // TODO exception logging
                    throw;
                }
            }
        } // InitKeyMappings


        private void UpdateDisplayedImage()
        {
            SelectionImg.Clip = selectControl.GetSelectionGeometry();
        } // UpdateDisplayedImage


        private void RaiseSignal()
        {
            this.SignalMain?.Invoke(this, EventArgs.Empty);
        } // RaiseSignal


        private void SwitchScreens()
        {
            if (switchValid)
            {
                switchFlag = true;
                this.Hide();
                RaiseSignal();
            }
        } // SwitchScreens


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

                selectControl = new SelectControl(bitmap);

                winLoaded = true;
            }
        } // SelectionWindow_Loaded


        private void SelectionWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Actions.ActionEnum action;

            if (keyMappings.ContainsKey(e.Key))
            {
                action = keyMappings[e.Key];
                switch (action)
                {
                    case Actions.ActionEnum.Invalid:
                        break;
                    case Actions.ActionEnum.Copy:
                        // Copy selection to the clipboard
                        selectControl.CopySelection();
                        break;
                    case Actions.ActionEnum.Edit:
                        // Edit the selection in default image editor
                        selectControl.EditSelection();
                        break;
                    case Actions.ActionEnum.Save:
                        // Save the selection to disk
                        selectControl.SaveSelection(this);
                        break;
                    case Actions.ActionEnum.SelectAll:
                        // Select the entire screen
                        selectControl.MaximizeSelection();
                        UpdateDisplayedImage();
                        break;
                    case Actions.ActionEnum.Clear:
                        // Clear the current selection
                        selectControl.ClearSelection();
                        UpdateDisplayedImage();
                        break;
                    case Actions.ActionEnum.New:
                        // Reset for new Leer
                        // TODO
                        break;
                    case Actions.ActionEnum.Settings:
                        // Open up settings window
                        // TODO
                        break;
                    case Actions.ActionEnum.Tips:
                        // Show/Hide tip labels
                        LabelPanel.Visibility = (LabelPanel.Visibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
                        break;
                    case Actions.ActionEnum.Switch:
                        SwitchScreens();
                        break;
                    case Actions.ActionEnum.Quit:
                        // Quit selection
                        switchFlag = false;
                        RaiseSignal();
                        this.Close();
                        break;
                    default:
                        break;
                }
            }
        } // SelectionWindow_PreviewKeyDown


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
    }
}
