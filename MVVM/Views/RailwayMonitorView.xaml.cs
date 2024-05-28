﻿using System;
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
using railway_monitor.Components.ToolButtons;
using railway_monitor.MVVM.ViewModels;
using railway_monitor.Tools;

namespace railway_monitor.MVVM.Views
{
    /// <summary>
    /// Interaction logic for RailwayMonitorView.xaml
    /// </summary>
    public partial class RailwayMonitorView : UserControl
    {
        public RailwayMonitorView()
        {
            InitializeComponent();

            Loaded += (x, y) => Keyboard.Focus(railwayCanvas); 
        }

        private void FocusCanvas(object sender, KeyEventArgs e)
        {
            if (!railwayCanvas.IsFocused)
            {
                railwayCanvas.Focus();
            }
        }

        private void OnCanvasKeyDown(object sender, KeyEventArgs e)
        {
            RailwayMonitorViewModel contex = (RailwayMonitorViewModel)DataContext;
            switch (e.Key)
            {
                case Key.Escape:
                    contex.EscapeCommand.Execute(Tuple.Create(contex.RailwayCanvas, new Point(0, 0)));
                    return;
            }
        }
    }
}
