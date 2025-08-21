namespace Pseven.Controls
{
    public class AkjbaButton : Button
    {
        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(AkjbaButton), false, BindingMode.TwoWay, propertyChanged: OnIsCheckedChanged);
        public static readonly BindableProperty CheckedBackgroundColorProperty = BindableProperty.Create(nameof(CheckedBackgroundColor), typeof(Color), typeof(AkjbaButton), propertyChanged: OnCheckedBackgroundColorChanged);
        public static readonly BindableProperty UncheckedBackgroundColorProperty = BindableProperty.Create(nameof(UncheckedBackgroundColor), typeof(Color), typeof(AkjbaButton), propertyChanged: OnUncheckedBackgroundColorChanged);
        public static readonly BindableProperty CheckedTextColorProperty = BindableProperty.Create(nameof(CheckedTextColor), typeof(Color), typeof(AkjbaButton), propertyChanged: OnCheckedTextColorChanged);
        public static readonly BindableProperty UncheckedTextColorProperty = BindableProperty.Create(nameof(UncheckedTextColor), typeof(Color), typeof(AkjbaButton), propertyChanged: OnUncheckedTextColorChanged);
        public static readonly BindableProperty CheckedBorderColorProperty = BindableProperty.Create(nameof(CheckedBorderColor), typeof(Color), typeof(AkjbaButton), propertyChanged: OnCheckedBorderColorChanged);
        public static readonly BindableProperty UncheckedBorderColorProperty = BindableProperty.Create(nameof(UncheckedBorderColor), typeof(Color), typeof(AkjbaButton), propertyChanged: OnUncheckedBorderColorChanged);

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }
        public Color CheckedBackgroundColor
        {
            get => (Color)GetValue(CheckedBackgroundColorProperty);
            set => SetValue(CheckedBackgroundColorProperty, value);
        }
        public Color UncheckedBackgroundColor
        {
            get => (Color)GetValue(UncheckedBackgroundColorProperty);
            set => SetValue(UncheckedBackgroundColorProperty, value);
        }
        public Color CheckedTextColor
        {
            get => (Color)GetValue(CheckedTextColorProperty);
            set => SetValue(CheckedTextColorProperty, value);
        }

        public Color UncheckedTextColor
        {
            get => (Color)GetValue(UncheckedTextColorProperty);
            set => SetValue(UncheckedTextColorProperty, value);
        }

        public Color CheckedBorderColor
        {
            get => (Color)GetValue(CheckedBorderColorProperty);
            set => SetValue(CheckedBorderColorProperty, value);
        }
        public Color UncheckedBorderColor
        {
            get => (Color)GetValue(UncheckedBorderColorProperty);
            set => SetValue(UncheckedBorderColorProperty, value);
        }


        private static void OnIsCheckedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (AkjbaButton)bindable;
            if (control.CheckedBackgroundColor is not null && control.UncheckedBackgroundColor is not null)
            {
                control.BackgroundColor = (bool)newValue ? control.CheckedBackgroundColor : control.UncheckedBackgroundColor;
            }
            if (control.CheckedTextColor is not null && control.UncheckedTextColor is not null)
            {
                control.TextColor = (bool)newValue ? control.CheckedTextColor : control.UncheckedTextColor;
            }
            if (control.CheckedBorderColor is not null && control.UncheckedBorderColor is not null)
            {
                control.BorderColor = (bool)newValue ? control.CheckedBorderColor : control.UncheckedBorderColor;
            }
        }
        private static void OnCheckedBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (AkjbaButton)bindable;
            control.BackgroundColor = (Color)newValue;
        }
        private static void OnUncheckedBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (AkjbaButton)bindable;
            control.BackgroundColor = (Color)newValue;
        }
        private static void OnCheckedTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (AkjbaButton)bindable;
            control.TextColor = (Color)newValue;
        }
        private static void OnUncheckedTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (AkjbaButton)bindable;
            control.TextColor = (Color)newValue;
        }
        private static void OnCheckedBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (AkjbaButton)bindable;
            control.BorderColor = (Color)newValue;
        }
        private static void OnUncheckedBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (AkjbaButton)bindable;
            control.BorderColor = (Color)newValue;
        }

        public AkjbaButton()
        {
            Loaded += CustomButton_Loaded;
            Clicked += CustomButton_Clicked;
        }

        private void CustomButton_Loaded(object sender, EventArgs e)
        {
            if (CheckedBackgroundColor is not null && UncheckedBackgroundColor is not null)
            {
                BackgroundColor = IsChecked ? CheckedBackgroundColor : UncheckedBackgroundColor;
            }
            if (CheckedTextColor is not null && UncheckedTextColor is not null)
            {
                TextColor = IsChecked ? CheckedTextColor : UncheckedTextColor;
            }
            if (CheckedBorderColor is not null && UncheckedBorderColor is not null)
            {
                BorderColor = IsChecked ? CheckedBorderColor : UncheckedBorderColor;
            }
        }

        private void CustomButton_Clicked(object sender, EventArgs e)
        {
            IsChecked = !IsChecked;
            OnIsCheckedChanged(this, !IsChecked, IsChecked);
        }
    }
}
