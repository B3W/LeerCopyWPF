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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LeerCopyWPF.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        #region Fields

        /// <summary>
        /// Converts key codes to/from strings
        /// </summary>
        private readonly KeyConverter _keyConverter;

        #endregion // Fields

        #region Constructors

        public SettingsView()
        {
            InitializeComponent();

            _keyConverter = new KeyConverter();

            // This makes sure the first UserControl in the tab order gets focus when the view is opened
            // https://stackoverflow.com/a/818536
            Loaded += (sender, e) => MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        #endregion // Constructors

        #region EventHandlers

        /// <summary>
        /// Update the text within the text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyBindingTxtBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox keyBindTxtBx = (TextBox)sender;
            Key key = e.Key;

            if ((key >= Key.Space && key <= Key.Home)       // Space, PageUp, PageDown, Home, End
                || key == Key.Insert
                || key == Key.Delete
                || (key >= Key.D0 && key <= Key.Z)          // 0, 1, 2, ..., x, y, z
                || (key >= Key.NumPad0 && key <= Key.F24))  // 0, 1, 2, ..., *, +, ..., F22, F23, F24
            {
                keyBindTxtBx.Text = _keyConverter.ConvertToString(key);
            }
        }


        /// <summary>
        /// Closes the dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.DialogResult = false;
                parentWindow.Close();
            }
        }

        #endregion // EventHandlers
    }
}
