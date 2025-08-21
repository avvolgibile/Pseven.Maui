using Syncfusion.Maui.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pseven.Controls
{
    public class CustomSfComboBox : SfComboBox
    {
        public static readonly BindableProperty SelectionChangedCommandProperty = BindableProperty.Create(nameof(SelectionChangedCommand), typeof(ICommand), typeof(CustomSfComboBox), null);
        public static readonly BindableProperty SelectionChangedCommandParameterProperty = BindableProperty.Create(nameof(SelectionChangedCommandParameter), typeof(object), typeof(CustomSfComboBox), null);

        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }
        public object SelectionChangedCommandParameter
        {
            get { return (ICommand)GetValue(SelectionChangedCommandParameterProperty); }
            set { SetValue(SelectionChangedCommandParameterProperty, value); }
        }

        public CustomSfComboBox()
        {
            SelectionChanged += SfComboBox_SelectionChanged;
        }

        private void SfComboBox_SelectionChanged(object? sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
        {
            SelectionChangedCommand?.Execute(SelectionChangedCommandParameter);
        }
    }
}
