using LeerCopyWPF.Models;
using LeerCopyWPF.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LeerCopyWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// All screens in user's environment
        /// </summary>
        private List<ExtBitmapSource> screens;
        /// <summary>
        /// Currently displayed screen for capture
        /// </summary>
        private int curScreenIndex = 0;
        /// <summary>
        /// Selection windows for each screen
        /// </summary>
        private SelectionWindow[] selectionWindows;
        /// <summary>
        /// Flag to prevent loaded event handler logic firing multiple times
        /// </summary>
        private bool winLoaded = false;
        /// <summary>
        /// Flag indicating if screen switch should occur
        /// </summary>
        private bool switchFlag = false;


        public MainWindow()
        {
            // Register window lifetime event handlers
            this.Loaded += MainWindow_Loaded;

            InitializeComponent();

            // Initialize AppData setting
            try
            {
                Properties.Settings.Default.AppDataLoc = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                        System.IO.Path.DirectorySeparatorChar + Application.ResourceAssembly.GetName().Name + System.IO.Path.DirectorySeparatorChar;
                Properties.Settings.Default.Save();
            }
            catch (PlatformNotSupportedException)
            {
                // TODO exception handling
            }
            catch (InvalidOperationException)
            {
                // TODO exception handling
            }

            // Capture all screens
            screens = BitmapUtilities.CaptureScreens();

            // Initialize container for selection windows
            selectionWindows = new SelectionWindow[screens.Count];

            // Register event handlers
            this.Closing += MainWindow_Closing;
        }


        /// <summary>
        /// Determines which screen the MainWindow is located on
        /// </summary>
        /// <returns>Index within 'screens' containing MainWindow if found, 0 otherwise</returns>
        private int GetMainScreenIndex()
        {
            int i;
            bool found = false;

            for (i = 0; i < screens.Count; i++)
            {
                if (screens[i].Bounds.Contains(new Point(this.Left, this.Top)))
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                return i;
            }
            return 0;
        } // GetMainScreenIndex


        /// <summary>
        /// Initializes a selection window covering the given bounds
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        private SelectionWindow InitSelectWindow(Rect bounds)
        {
            SelectionWindow sWin = new SelectionWindow(bounds, ref switchFlag, (screens.Count > 1));
            sWin.SignalMain += Selection_SignalMain;
            sWin.Owner = this;

            return sWin;
        } // InitSelectWindow


        /// <summary>
        /// Event for child SelectionWindow to signal MainWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Selection_SignalMain(object sender, EventArgs e)
        {
            if (switchFlag)
            {
                // Increment screen index
                curScreenIndex = (curScreenIndex + 1) % screens.Count;
                // Create new selection window or show existing
                if (selectionWindows[curScreenIndex] == null)
                {
                    selectionWindows[curScreenIndex] = InitSelectWindow(screens[curScreenIndex].Bounds);
                }
                selectionWindows[curScreenIndex].Show();
                selectionWindows[curScreenIndex].Activate();
            }
            else
            {
                // Clear all selections when selection capture is quit
                for (int i = 0; i < selectionWindows.Length; i++)
                {
                    selectionWindows[i]?.Close();
                    selectionWindows[i] = null;
                }

                // Show MainWindow
                this.Show();
                this.WindowState = WindowState.Normal;
            }
        } // Selection_SignalMain


        private void SelectCaptureBtn_Click(object sender, RoutedEventArgs e)
        {
            // Determine screen MainWindow is located on
            // ***Needs to be done before window is minimized***
            curScreenIndex = GetMainScreenIndex();

            // Hide MainWindow
            this.WindowState = WindowState.Minimized;
            this.Hide();

            // Open SelectionWindow
            selectionWindows[curScreenIndex] = InitSelectWindow(screens[curScreenIndex].Bounds);
            selectionWindows[curScreenIndex].Show();
            selectionWindows[curScreenIndex].Activate();
        } // SelectCaptureBtn_Click


        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        } // ExitMenuItem_Click


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!winLoaded)
            {
                // Restore window location if possible
                double tmpL = Properties.Settings.Default.MainWinX;
                double tmpT = Properties.Settings.Default.MainWinY;
                System.Drawing.Point pt = new System.Drawing.Point((int)tmpL, (int)tmpT);

                // Check if a screen can contain window location
                foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
                {
                    if (screen.Bounds.Contains(pt))
                    {
                        this.Left = tmpL;
                        this.Top = tmpT;
                        break;
                    }
                }

                winLoaded = true;
            }
        } // MainWindow_Loaded


        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Save window location 
            Properties.Settings.Default.MainWinX = this.Left;
            Properties.Settings.Default.MainWinY = this.Top;
            Properties.Settings.Default.Save();
        } // MainWindow_Closing
    }
}
