using Pseven.Models;
using System.Diagnostics.Tracing;

namespace Pseven.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AkjbaComboBox : ContentView
{
    public bool IsOpen { get; set; } = false;
    public static readonly BindableProperty ComboTypeProperty = BindableProperty.Create(nameof(ComboType), typeof(Type), typeof(AkjbaComboBox), null, BindingMode.TwoWay);
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<object>), typeof(AkjbaComboBox), null, BindingMode.TwoWay, propertyChanged: OnItemsSourceChanged);
    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(AkjbaComboBox), null, BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);
    public static readonly BindableProperty DisplayItemProperty = BindableProperty.Create(nameof(DisplayItem), typeof(string), typeof(AkjbaComboBox), null, BindingMode.TwoWay);

    public Type ComboType
    {
        get => (Type)GetValue(ComboTypeProperty);
        set => SetValue(ComboTypeProperty, value);
    }
    public IEnumerable<object> ItemsSource
    {
        get => (IEnumerable<object>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    public string DisplayItem
    {
        get => (string)GetValue(DisplayItemProperty);
        set => SetValue(DisplayItemProperty, value);
    }

    private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (AkjbaComboBox)bindable;
        control.ComboCollection.ItemsSource = (IEnumerable<object>)newValue;
    }
    private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (AkjbaComboBox)bindable;
        control.ComboCollection.SelectedItem = (string)newValue;
    }
    public AkjbaComboBox()
    {
        InitializeComponent();
        ComboButton.Clicked += ComboButton_Clicked;
        ComboCollection.SelectionChanged += ComboCollection_SelectionChanged;
        ComboCollection.Focused += ComboCollection_Focused;
        ComboEntry.TextChanged += ComboEntry_TextChanged;
        ComboEntry.Focused += ComboEntry_Focused;
        ComboEntry.Completed += ComboEntry_Completed;
        

        ComboButton.Source = ImageSource.FromFile("arrow_down_bold.png");
        ComboCollection.IsVisible = false;
        ComboCollection.ItemTemplate = new DataTemplate(() =>
        {
            var grid = new Grid();
            var label = new Label { VerticalOptions = LayoutOptions.Center };
            label.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, DisplayItem);
            grid.Children.Add(label);
            return grid;
        });
    }

    private void ComboCollection_Focused(object? sender, FocusEventArgs e)
    {
        
    }

    private void ComboEntry_Completed(object? sender, EventArgs e)
    {
        if(ComboCollection.SelectedItem != null)
        {
            ComboEntry.Text = ((Cliente)ComboCollection.SelectedItem).RagioneSociale;
        }
    }

    private void ComboEntry_Focused(object? sender, FocusEventArgs e)
    {
        var entry = sender as Entry;
        entry.CursorPosition = 0;
        entry.SelectionLength = entry.Text == null ? 0 : entry.Text.Length;
    }

    private void ComboEntry_TextChanged(object? sender, TextChangedEventArgs e)
    {

        if(ComboCollection.SelectedItem == null)
        {
            ComboCollection.IsVisible = true;
            ComboCollection.ItemsSource = ItemsSource.Where(x => ((Cliente)x).RagioneSociale.ToLower().Contains(ComboEntry.Text.ToLower()));
        }
        else if(ComboEntry.Text == string.Empty)
        {
            ComboCollection.IsVisible = false;
            ComboCollection.SelectedItem = null;
        }
    }

    private void ComboCollection_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ComboCollection.SelectedItem != null)
        {
            ComboEntry.Text = ((Cliente)ComboCollection.SelectedItem).RagioneSociale;
        }
        
        ApriChiudiCombo();
    }

    void ComboButton_Clicked(object? sender, EventArgs e)
    {
        ApriChiudiCombo();
    }
    void ApriChiudiCombo()
    {
        IsOpen = !IsOpen;
        if (IsOpen)
        {
            ComboButton.Source = ImageSource.FromFile("arrow_up_bold.png");
            ComboCollection.IsVisible = true;
            ComboCollection.Focus();
        }
        else
        {
            ComboButton.Source = ImageSource.FromFile("arrow_down_bold.png");
            ComboCollection.IsVisible = false;
        }
    }
}