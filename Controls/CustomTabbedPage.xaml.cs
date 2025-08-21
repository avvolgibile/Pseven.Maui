

using Microsoft.Maui.Graphics;

namespace Pseven.Controls;

public partial class CustomTabbedPage : ContentView
{
    #region BindableProperty
    public static readonly BindableProperty Pulsante1TextProperty = BindableProperty.Create(nameof(Pulsante1Text), typeof(string), typeof(CustomTabbedPage), string.Empty, BindingMode.TwoWay, propertyChanged: OnPulsante1TextChanged);
	public static readonly BindableProperty Pulsante2TextProperty = BindableProperty.Create(nameof(Pulsante2Text), typeof(string), typeof(CustomTabbedPage), string.Empty, BindingMode.TwoWay, propertyChanged: OnPulsante2TextChanged);
	public static readonly BindableProperty Pulsante3TextProperty = BindableProperty.Create(nameof(Pulsante3Text), typeof(string), typeof(CustomTabbedPage), string.Empty, BindingMode.TwoWay, propertyChanged: OnPulsante3TextChanged);
    public static readonly BindableProperty SelectedBackgroundColorProperty = BindableProperty.Create(nameof(SelectedBackgroundColor), typeof(Color), typeof(CustomTabbedPage), Color.FromRgba("#0000ff"), BindingMode.TwoWay);
    public static readonly BindableProperty UnselectedBackgroundColorProperty = BindableProperty.Create(nameof(UnselectedBackgroundColor), typeof(Color), typeof(CustomTabbedPage), Color.FromRgba("#5f9ea0"), BindingMode.TwoWay);
    public static readonly BindableProperty SelectedTextColorProperty = BindableProperty.Create(nameof(SelectedTextColor), typeof(Color), typeof(CustomTabbedPage), Color.FromRgba("#FFF"), BindingMode.TwoWay);
    public static readonly BindableProperty UnselectedTextColorProperty = BindableProperty.Create(nameof(UnselectedTextColor), typeof(Color), typeof(CustomTabbedPage), Color.FromRgba("#FFF"), BindingMode.TwoWay);
    public static readonly BindableProperty Drawable1SourceProperty = BindableProperty.Create(nameof(Drawable1Source), typeof(IDrawable), typeof(CustomTabbedPage), null, BindingMode.TwoWay, propertyChanged: OnDrawable1SourceChanged);
    public static readonly BindableProperty Drawable2SourceProperty = BindableProperty.Create(nameof(Drawable2Source), typeof(IDrawable), typeof(CustomTabbedPage), null, BindingMode.TwoWay, propertyChanged: OnDrawable2SourceChanged);
    public static readonly BindableProperty Drawable3SourceProperty = BindableProperty.Create(nameof(Drawable3Source), typeof(IDrawable), typeof(CustomTabbedPage), null, BindingMode.TwoWay, propertyChanged: OnDrawable3SourceChanged);
    public static readonly BindableProperty Drawable4SourceProperty = BindableProperty.Create(nameof(Drawable4Source), typeof(IDrawable), typeof(CustomTabbedPage), null, BindingMode.TwoWay, propertyChanged: OnDrawable4SourceChanged);
    public static readonly BindableProperty Drawable5SourceProperty = BindableProperty.Create(nameof(Drawable5Source), typeof(IDrawable), typeof(CustomTabbedPage), null, BindingMode.TwoWay, propertyChanged: OnDrawable5SourceChanged);
    public static readonly BindableProperty Drawable6SourceProperty = BindableProperty.Create(nameof(Drawable6Source), typeof(IDrawable), typeof(CustomTabbedPage), null, BindingMode.TwoWay, propertyChanged: OnDrawable6SourceChanged);
    public static readonly BindableProperty Drawable7SourceProperty = BindableProperty.Create(nameof(Drawable7Source), typeof(IDrawable), typeof(CustomTabbedPage), null, BindingMode.TwoWay, propertyChanged: OnDrawable7SourceChanged);
    public static readonly BindableProperty Drawable8SourceProperty = BindableProperty.Create(nameof(Drawable8Source), typeof(IDrawable), typeof(CustomTabbedPage), null, BindingMode.TwoWay, propertyChanged: OnDrawable8SourceChanged);
    public static readonly BindableProperty Drawable9SourceProperty = BindableProperty.Create(nameof(Drawable9Source), typeof(IDrawable), typeof(CustomTabbedPage), null, BindingMode.TwoWay, propertyChanged: OnDrawable9SourceChanged);
    #endregion

    #region Properties
    public string Pulsante1Text
	{
        get => (string)GetValue(Pulsante1TextProperty);
        set => SetValue(Pulsante1TextProperty, value);
    }
	public string Pulsante2Text
	{
        get => (string)GetValue(Pulsante2TextProperty);
        set => SetValue(Pulsante2TextProperty, value);
    }
	public string Pulsante3Text
	{
		get => (string)GetValue(Pulsante3TextProperty);
		set => SetValue(Pulsante3TextProperty, value);
	}
    public IDrawable Drawable1Source
    {
        get => (IDrawable)GetValue(Drawable1SourceProperty);
        set => SetValue(Drawable1SourceProperty, value);
    }
    public IDrawable Drawable2Source
    {
        get => (IDrawable)GetValue(Drawable2SourceProperty);
        set => SetValue(Drawable2SourceProperty, value);
    }
    public IDrawable Drawable3Source
    {
        get => (IDrawable)GetValue(Drawable3SourceProperty);
        set => SetValue(Drawable3SourceProperty, value);
    }
    public IDrawable Drawable4Source
    {
        get => (IDrawable)GetValue(Drawable4SourceProperty);
        set => SetValue(Drawable4SourceProperty, value);
    }
    public IDrawable Drawable5Source
    {
        get => (IDrawable)GetValue(Drawable5SourceProperty);
        set => SetValue(Drawable5SourceProperty, value);
    }
    public IDrawable Drawable6Source
    {
        get => (IDrawable)GetValue(Drawable6SourceProperty);
        set => SetValue(Drawable6SourceProperty, value);
    }
    public IDrawable Drawable7Source
    {
        get => (IDrawable)GetValue(Drawable7SourceProperty);
        set => SetValue(Drawable7SourceProperty, value);
    }
    public IDrawable Drawable8Source
    {
        get => (IDrawable)GetValue(Drawable8SourceProperty);
        set => SetValue(Drawable8SourceProperty, value);
    }
    public IDrawable Drawable9Source
    {
        get => (IDrawable)GetValue(Drawable9SourceProperty);
        set => SetValue(Drawable9SourceProperty, value);
    }

    public Color SelectedBackgroundColor
    {
        get => (Color)GetValue(SelectedBackgroundColorProperty);
        set => SetValue(SelectedBackgroundColorProperty, value);
    }
    public Color UnselectedBackgroundColor
    {
        get => (Color)GetValue(UnselectedBackgroundColorProperty);
        set => SetValue(UnselectedBackgroundColorProperty, value);
    }
    public Color SelectedTextColor
    {
        get => (Color)GetValue(SelectedTextColorProperty);
        set => SetValue(SelectedTextColorProperty, value);
    }
    public Color UnselectedTextColor
    {
        get => (Color)GetValue(UnselectedTextColorProperty);
        set => SetValue(UnselectedTextColorProperty, value);
    }
    #endregion

    #region PropertyChanged
    private static void OnPulsante1TextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomTabbedPage)bindable;
		control.Pulsante1.Text = (string)newValue;
    }
	private static void OnPulsante2TextChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (CustomTabbedPage)bindable;
		control.Pulsante2.Text = (string)newValue;
	}
	private static void OnPulsante3TextChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (CustomTabbedPage)bindable;
		control.Pulsante3.Text = (string)newValue;
	}

    private static void OnDrawable1SourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomTabbedPage)bindable;
        control.Immagine1.Drawable = (IDrawable)newValue;
    }
    private static void OnDrawable2SourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomTabbedPage)bindable;
        control.Immagine2.Drawable = (IDrawable)newValue;
    }
    private static void OnDrawable3SourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomTabbedPage)bindable;
        control.Immagine3.Drawable = (IDrawable)newValue;
    }
    private static void OnDrawable4SourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomTabbedPage)bindable;
        control.Immagine4.Drawable = (IDrawable)newValue;
    }
    private static void OnDrawable5SourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomTabbedPage)bindable;
        control.Immagine5.Drawable = (IDrawable)newValue;
    }
    private static void OnDrawable6SourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomTabbedPage)bindable;
        control.Immagine6.Drawable = (IDrawable)newValue;
    }
    private static void OnDrawable7SourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomTabbedPage)bindable;
        control.Immagine7.Drawable = (IDrawable)newValue;
    }
    private static void OnDrawable8SourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomTabbedPage)bindable;
        control.Immagine8.Drawable = (IDrawable)newValue;
    }
    private static void OnDrawable9SourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomTabbedPage)bindable;
        control.Immagine9.Drawable = (IDrawable)newValue;
    }
    #endregion

    #region Event
    private void Pulsante1_Clicked(object? sender, EventArgs e)
    {
        GridTab1.IsVisible = true;
        GridTab2.IsVisible = false;
        GridTab3.IsVisible = false;
        Pulsante1.BackgroundColor = SelectedBackgroundColor;
        Pulsante2.BackgroundColor = UnselectedBackgroundColor;
        Pulsante3.BackgroundColor = UnselectedBackgroundColor;
        Pulsante1.TextColor = SelectedTextColor;
        Pulsante2.TextColor = UnselectedTextColor;
        Pulsante3.TextColor = UnselectedTextColor;
    }
    private void Pulsante2_Clicked(object? sender, EventArgs e)
    {
        GridTab1.IsVisible = false;
        GridTab2.IsVisible = true;
        GridTab3.IsVisible = false;
        Pulsante1.BackgroundColor = UnselectedBackgroundColor;
        Pulsante2.BackgroundColor = SelectedBackgroundColor;
        Pulsante3.BackgroundColor = UnselectedBackgroundColor;
        Pulsante1.TextColor = UnselectedTextColor;
        Pulsante2.TextColor = SelectedTextColor;
        Pulsante3.TextColor = UnselectedTextColor;
    }
    private void Pulsante3_Clicked(object? sender, EventArgs e)
    {
        GridTab1.IsVisible = false;
        GridTab2.IsVisible = false;
        GridTab3.IsVisible = true;
        Pulsante1.BackgroundColor = UnselectedBackgroundColor;
        Pulsante2.BackgroundColor = UnselectedBackgroundColor;
        Pulsante3.BackgroundColor = SelectedBackgroundColor;
        Pulsante1.TextColor = UnselectedTextColor;
        Pulsante2.TextColor = UnselectedTextColor;
        Pulsante3.TextColor = SelectedTextColor;
    }
    #endregion

    #region Init
    private void Init()
    {
        Pulsante1.Clicked += Pulsante1_Clicked;
        Pulsante2.Clicked += Pulsante2_Clicked;
        Pulsante3.Clicked += Pulsante3_Clicked;
        GridTab1.IsVisible = true;
        GridTab2.IsVisible = false;
        GridTab3.IsVisible = false;
        Pulsante1.BackgroundColor = SelectedBackgroundColor;
        Pulsante2.BackgroundColor = UnselectedBackgroundColor;
        Pulsante3.BackgroundColor = UnselectedBackgroundColor;
        Pulsante1.TextColor = SelectedTextColor;
        Pulsante2.TextColor = UnselectedTextColor;
        Pulsante3.TextColor = UnselectedTextColor;

    }
    #endregion
    public CustomTabbedPage()
	{
		InitializeComponent();
        Init();
	}
}