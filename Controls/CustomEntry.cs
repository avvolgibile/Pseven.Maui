using System.Windows.Input;

namespace Pseven.Controls
{
    public class CustomEntry : Entry
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

        public CustomEntry()
        {
            TextChanged += CustomEntry_TextChanged;
            Focused += CustomEntry_Focused;
        }

        private void CustomEntry_Focused(object? sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            entry.CursorPosition = 0;
            entry.SelectionLength = entry.Text == null ? 0 : entry.Text.Length;
        }

        private void CustomEntry_TextChanged(object? sender, TextChangedEventArgs e)
        {
            Command?.Execute(CommandParameter);
        }
    }
}
