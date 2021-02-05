﻿using System;
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

namespace livelywpf.Views
{
    /// <summary>
    /// Interaction logic for ScreenLayoutView.xaml
    /// </summary>
    public partial class ScreenLayoutView : Window
    {
        private readonly List<ScreenLabelView> screenLabels = new List<ScreenLabelView>();

        public ScreenLayoutView()
        {
            InitializeComponent();
            this.DataContext = new ScreenLayoutViewModel();
            CreateLabelWindows();

            ScreenHelper.DisplayUpdated += ScreenHelper_DisplayUpdated;
        }

        private void ScreenLayoutControl_ChildChanged(object sender, EventArgs e)
        {
            // Hook up x:Bind source.
            global::Microsoft.Toolkit.Wpf.UI.XamlHost.WindowsXamlHost windowsXamlHost =
                sender as global::Microsoft.Toolkit.Wpf.UI.XamlHost.WindowsXamlHost;
            var control = windowsXamlHost.GetUwpInternalObject() as global::livelyscreenlayout.ScreenLayoutView;

            if (control != null)
            {

            }
        }

        private void ScreenHelper_DisplayUpdated(object sender, EventArgs e)
        {
            //Windows will move the label window if property change.
            //This is a lazy fix if display disconnect/reconnect.
            this.Dispatcher.BeginInvoke(new Action(() => {
                CloseLabelWindows();
                CreateLabelWindows();
            }));
        }

        private void CreateLabelWindows()
        {
            var screens = ScreenHelper.GetScreen();
            if (screens.Count > 1)
            {
                screens.ForEach(screen =>
                {
                    var labelWindow = new ScreenLabelView(screen.DeviceNumber, screen.Bounds.Left + 10, screen.Bounds.Top + 10);
                    labelWindow.Show();
                    screenLabels.Add(labelWindow);
                });
            }
        }

        private void CloseLabelWindows()
        {
            screenLabels.ForEach(x => x.Close());
            screenLabels.Clear();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ScreenHelper.DisplayUpdated -= ScreenHelper_DisplayUpdated;
            CloseLabelWindows();
        }
    }
}