using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pseven.Controls
{
    public class CustomCheckBox : CheckBox
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CustomEntry), null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CustomEntry), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public object CommandParameter
        {
            get { return (ICommand)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public CustomCheckBox()
        {
            CheckedChanged += CustomCheckBox_CheckedChanged;
        }

        private void CustomCheckBox_CheckedChanged(object? sender, CheckedChangedEventArgs e)
        {
            Command?.Execute(CommandParameter);
        }
    }
}
