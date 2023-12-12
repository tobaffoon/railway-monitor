using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace railway_monitor.MVVM.ViewModels
{
    class ToolButtonsViewModel : DependencyObject
    {
        private static readonly DependencyPropertyKey ToolButtonsPropertyKey;
        public static readonly DependencyProperty ToolButtonsProperty;

        static ToolButtonsViewModel()
        {

            List<RadioButton> toolButtons = new List<RadioButton>();

            RadioButton srtButton = new RadioButton();
            srtButton.GroupName = "Tools";
            srtButton.Content = "SRT";
            RadioButton switchButton = new RadioButton();
            switchButton.GroupName = "Tools";
            switchButton.Content = "Switch";

            toolButtons.Add(srtButton);
            toolButtons.Add(switchButton);

            ToolButtonsPropertyKey =
                DependencyProperty.RegisterReadOnly(
                    "ToolButtons",
                    typeof(List<RadioButton>),
                    typeof(ToolButtonsViewModel),
                    new FrameworkPropertyMetadata(toolButtons)
                );

            ToolButtonsProperty = ToolButtonsPropertyKey.DependencyProperty;
        }

        public List<RadioButton> ToolButtons => (List<RadioButton>)GetValue(ToolButtonsProperty);
    }
}
