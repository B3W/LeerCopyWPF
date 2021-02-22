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

using LeerCopyWPF.Constants;
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
        #region Fields

        private readonly SelectionViewModel _selectionViewModel;

        private SelectionViewModel _tmpVM;

        /// <summary>
        /// Dictionary for quick lookup of key up mappings
        /// </summary>
        private IDictionary<Key, KeyUpAction> _keyUpMappings = new Dictionary<Key, KeyUpAction>(10);

        /// <summary>
        /// Dictionary containing mappings between KeyUpAction enum and string values
        /// </summary>
        private readonly IDictionary<string, KeyUpAction> _keyUpStringMappings;

        /// <summary>
        /// Flag indicating whether switch is possible
        /// </summary>
        private bool _switchValid;

        /// <summary>
        /// Flag to prevent loaded event from firing multiple times
        /// </summary>
        private bool _winLoaded = false;

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Event for signaling the MainWindow
        /// </summary>
        public event EventHandler<FlagEventArgs> SignalMain;

        #endregion // Properties

        #region Constructors
        public SelectionWindow(Rect bounds, bool switchValid)
        {
            // Register window lifetime events
            this.Loaded += SelectionWindow_Loaded;

            InitializeComponent();

            // Construct conversion mapping
            _keyUpStringMappings = new Dictionary<string, KeyUpAction>
            {
                { SettingsConstants.CopySettingName, KeyUpAction.Copy },
                { SettingsConstants.EditSettingName, KeyUpAction.Edit },
                { SettingsConstants.SaveSettingName, KeyUpAction.Save },
                { SettingsConstants.ClearSettingName, KeyUpAction.Clear },
                { SettingsConstants.SelectAllSettingName, KeyUpAction.SelectAll },
                { SettingsConstants.BorderSettingName, KeyUpAction.Border },
                { SettingsConstants.TipsSettingName, KeyUpAction.Tips },
                { SettingsConstants.SwtchScrnSettingName, KeyUpAction.Switch },
                { SettingsConstants.SettingsSettingName, KeyUpAction.Settings },
                { SettingsConstants.QuitSettingName, KeyUpAction.Quit }
            };

            // Set DataContext
            _selectionViewModel = new SelectionViewModel(this, bounds);
            _selectionViewModel.OpenSettingsEvent += (s, eargs) => new SettingsWindow().ShowDialog();
            _selectionViewModel.KeyBindingsChangedEvent += (s, eargs) => KeyMappingsChanged();
            DataContext = _selectionViewModel;

            _switchValid = switchValid;

            // Place form on correct screen
            this.Left = bounds.Left;
            this.Top = bounds.Top;

            // Bind keys to actions
            _selectionViewModel.RefreshKeyBindings();

            // Register event handlers
            this.PreviewKeyUp += SelectionWindow_PreviewKeyUp;
        } // SelectionWindow
        #endregion // Constructors

        #region Methods
        /// <summary>
        /// Switch screen if possible
        /// </summary>
        private void SwitchScreens()
        {
            if (_switchValid)
            {
                this.Hide();
                RaiseSignal(true);
            }
        } // SwitchScreens
        #endregion // Methods

        #region EventHandlers
        /// <summary>
        /// EventHandler for updating the key up bindings (called by the ViewModel)
        /// </summary>
        private void KeyMappingsChanged()
        {
            SelectionViewModel _tmpVM = DataContext as SelectionViewModel;
            IDictionary<string, string> keyMappings = _tmpVM.KeyMappings;
            KeyConverter converter = new KeyConverter();
            Key key;

            // Update mappings
            foreach (KeyValuePair<string, string> mapping in keyMappings)
            {
                key = (Key)converter.ConvertFromString(mapping.Value);
                if (_keyUpStringMappings.TryGetValue(mapping.Key, out KeyUpAction value))
                {
                    _keyUpMappings[key] = value;
                }
                else
                {
                    throw new KeyNotFoundException("KeyMappings from SelectionViewModel contain invalid key: " + mapping.Key);
                }
            }

        } // InitKeyUpMappings


        /// <summary>
        /// Raise the EventHandler to signal MainWindow
        /// </summary>
        private void RaiseSignal(bool switchFlag)
        {
            this.SignalMain?.Invoke(this, new FlagEventArgs { Flag = switchFlag });
        } // RaiseSignal


        private void SelectionWindow_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (_keyUpMappings.ContainsKey(e.Key))
            {
                _tmpVM = DataContext as SelectionViewModel;
                KeyUpAction action = _keyUpMappings[e.Key];
                Visibility vis;

                switch (action)
                {
                    case KeyUpAction.Copy:
                        // Copy selection to the clipboard
                        if (_tmpVM.CopyCommand.CanExecute(null))
                        {
                            _tmpVM.CopyCommand.Execute(null);
                        }
                        break;
                    case KeyUpAction.Edit:
                        // Edit the selection in default image editor
                        if (_tmpVM.EditCommand.CanExecute(null))
                        {
                            _tmpVM.EditCommand.Execute(null);
                        }
                        break;
                    case KeyUpAction.Save:
                        // Save the selection to disk
                        if (_tmpVM.SaveCommand.CanExecute(null))
                        {
                            _tmpVM.SaveCommand.Execute(null);
                        }
                        break;
                    case KeyUpAction.SelectAll:
                        // Select the entire screen
                        if (_tmpVM.MaximizeCommand.CanExecute(null))
                        {
                            _tmpVM.MaximizeCommand.Execute(null);
                        }
                        break;
                    case KeyUpAction.Clear:
                        // Clear the current selection
                        if (_tmpVM.ClearCommand.CanExecute(null))
                        {
                            _tmpVM.ClearCommand.Execute(null);
                        }
                        break;
                    case KeyUpAction.Border:
                        // Show/Hide border
                        vis = (Properties.Settings.Default.BorderVisibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
                        Properties.Settings.Default.BorderVisibility = vis;
                        break;
                    case KeyUpAction.Tips:
                        // Show/Hide tip labels
                        vis = (Properties.Settings.Default.TipsVisibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
                        Properties.Settings.Default.TipsVisibility = vis;
                        break;
                    case KeyUpAction.Switch:
                        e.Handled = true;
                        SwitchScreens();
                        break;
                    case KeyUpAction.Settings:
                        // Open up settings window
                        if (_tmpVM.SettingsCommand.CanExecute(null))
                        {
                            _tmpVM.SettingsCommand.Execute(null);
                        }
                        break;
                    case KeyUpAction.Quit:
                        // Quit selection
                        RaiseSignal(false);
                        this.Close();
                        break;
                    case KeyUpAction.Invalid:
                    default:
                        break;
                }
            }
            e.Handled = true;
        } // SelectionWindow_PreviewKeyDown


        private void SelectionWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_winLoaded)
            {
                // Don't show the tip if you cannot switch screens
                if (!_switchValid)
                {
                    LabelPanel.Children.Remove(SwitchLblPanel);
                }

                _winLoaded = true;
            }

            this.WindowState = WindowState.Maximized;
        } // SelectionWindow_Loaded


        protected override void OnClosing(CancelEventArgs e)
        {
            // Save settings
            Properties.Settings.Default.Save();

            base.OnClosing(e);
        }
        #endregion // EventHandlers
    }
}
