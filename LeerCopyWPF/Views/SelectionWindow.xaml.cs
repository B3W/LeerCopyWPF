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
        #endregion // Fields


        #region Properties
        #endregion // Properties


        #region Constructors

        public SelectionWindow(Rect screenBounds)
        {
            // Register window lifetime events
            this.Loaded += SelectionWindow_Loaded;

            InitializeComponent();

            // Set DataContext
            // _selectionViewModel = new SelectionViewModel(this, bounds);
            // _selectionViewModel.OpenSettingsEvent += (s, eargs) => new SettingsWindow().ShowDialog();
            // _selectionViewModel.KeyBindingsChangedEvent += (s, eargs) => KeyMappingsChanged();
            // DataContext = _selectionViewModel;

            // Place form on correct screen
            this.Left = screenBounds.Left;
            this.Top = screenBounds.Top;

            // Bind keys to actions
            // _selectionViewModel.RefreshKeyBindings();
        } // SelectionWindow

        #endregion // Constructors


        #region EventHandlers

        private void SelectionWindow_Loaded(object sender, RoutedEventArgs e)
        {
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
