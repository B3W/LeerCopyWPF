/*
 *  Leer Copy - Quick and Accurate Screen Capturing Application
 *  Copyright (C) 2019  Weston Berg
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */

using LeerCopyWPF.Enums;
using LeerCopyWPF.Utilities;
using LeerCopyWPF.ViewModels;
using LeerCopyWPF.Views;
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
        private SelectionViewModel _selectionViewModel;
        private SelectionViewModel tmpVM;
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
        /// Event for signaling the MainWindow
        /// </summary>
        public event EventHandler<FlagEventArgs> SignalMain;


        public SelectionWindow(Rect bounds, bool switchValid)
        {
            // Register window lifetime events
            this.Loaded += SelectionWindow_Loaded;

            InitializeComponent();

            _selectionViewModel = new SelectionViewModel(bounds);
            _selectionViewModel.OpenSettingsEvent += (s, eargs) => new SettingsWindow().ShowDialog();
            DataContext = _selectionViewModel;

            this.switchValid = switchValid;

            // Place form on correct screen
            this.Left = bounds.Left;
            this.Top = bounds.Top;

            // Bind keys to actions
            InitKeyUpMappings();
            keyDownMappings = new Dictionary<Key, KeyActions.KeyDown>
            {
                // Add arrow keys
                { Key.Up, KeyActions.KeyDown.Up },
                { Key.Down, KeyActions.KeyDown.Down },
                { Key.Left, KeyActions.KeyDown.Left },
                { Key.Right, KeyActions.KeyDown.Right }
            };

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
                tmpVM = DataContext as SelectionViewModel;
                action = keyUpMappings[e.Key];
                switch (action)
                {
                    case KeyActions.KeyUp.Copy:
                        // Copy selection to the clipboard
                        if (tmpVM.CopyCommand.CanExecute(null))
                        {
                            tmpVM.CopyCommand.Execute(null);
                        }
                        break;
                    case KeyActions.KeyUp.Edit:
                        // Edit the selection in default image editor
                        if (tmpVM.EditCommand.CanExecute(null))
                        {
                            tmpVM.EditCommand.Execute(null);
                        }
                        break;
                    case KeyActions.KeyUp.Save:
                        // Save the selection to disk
                        if (tmpVM.SaveCommand.CanExecute(null))
                        {
                            tmpVM.SaveCommand.Execute(null);
                        }
                        break;
                    case KeyActions.KeyUp.SelectAll:
                        // Select the entire screen
                        if (tmpVM.MaximizeCommand.CanExecute(null))
                        {
                            tmpVM.MaximizeCommand.Execute(null);
                        }
                        break;
                    case KeyActions.KeyUp.Clear:
                        // Clear the current selection
                        if (tmpVM.ClearCommand.CanExecute(null))
                        {
                            tmpVM.ClearCommand.Execute(null);
                        }
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
                        if (tmpVM.SettingsCommand.CanExecute(null))
                        {
                            tmpVM.SettingsCommand.Execute(null);
                        }
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
            e.Handled = true;
        } // SelectionWindow_PreviewKeyDown
        

        private void SelectionWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (keyDownMappings.ContainsKey(e.Key))
            {
                tmpVM = DataContext as SelectionViewModel;
                KeyActions.KeyDown dir = keyDownMappings[e.Key];

                tmpVM.ResizeSelection(dir);                
            }
            e.Handled = true;
        } // SelectionWindow_PreviewKeyDown


        private void SelectionWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!winLoaded)
            {
                // Final configuration of user selection view
                // SelectionImg.Visibility = Visibility.Visible;

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
            tmpVM = DataContext as SelectionViewModel;
            tmpVM.StartSelection(e.GetPosition(this));

            e.Handled = true;
        } // SelectionWindow_MouseLeftButtonDown


        private void SelectionWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            tmpVM?.StopSelection(e.GetPosition(this));

            e.Handled = true;
        } // SelectionWindow_MouseLeftButtonUp


        private void SelectionWindow_MouseMove(object sender, MouseEventArgs e)
        {
            tmpVM?.UpdateSelection(e.GetPosition(this));

            e.Handled = true;
        } // SelectionWindow_MouseMove


        protected override void OnClosing(CancelEventArgs e)
        {
            // Save settings
            Properties.Settings.Default.Save();

            base.OnClosing(e);
        }
    }
}
