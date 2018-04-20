using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IPCal.Behaviors
{
    class EntryValidator : Behavior<Entry>
    {
        public static readonly BindableProperty ValueProperty = BindableProperty.Create<EntryValidator, int>(p => p.Value, 0, propertyChanged: OnValueChange, defaultBindingMode: BindingMode.TwoWay);
        public static readonly BindableProperty FormatProperty = BindableProperty.Create<EntryValidator, string>(p => p.Format, string.Empty, propertyChanged: OnFormatChange);

        private static void OnValueChange(BindableObject bindable, int oldvalue, int newvalue)
        {
            var e = bindable as EntryValidator;

            if (e == null)
                throw new Exception("ExtendedEntry.OnValueChange bindable == null");

            e.Value = newvalue;
        }

        private static void OnFormatChange(BindableObject bindable, string oldvalue, string newvalue)
        {
            var e = bindable as EntryValidator;

            if (e == null)
                throw new Exception("ExtendedEntry.OnFormatChange bindable == null");

            e.Format = newvalue;
        }

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        protected override void OnAttachedTo(Entry entry)
        {
            entry.Focused += OnFocused;
            entry.Unfocused += OnUnfocused;
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.Focused -= OnFocused;
            entry.Unfocused -= OnUnfocused;
        }

        private void OnFocused(object sender, FocusEventArgs e)
        {
            Entry entry = sender as Entry;

            if (entry == null)
                throw new Exception("ExtendedEntry.OnFocus sender == null");

            entry.Text = entry.Text.Remove.Format;
        }

        private void OnUnfocused(object sender, FocusEventArgs e)
        {
            Entry entry = sender as Entry;

            if (entry == null)
                throw new Exception("ExtendedEntry.OnUnfocused sender == null");

            var unformattedValue = entry.Text.RemoveFormat(Format);
            Value = Convert.ToInt32(unformattedValue);
            entry.Text = unformattedValue.ApplyFormat(Format, Value);
        }

    }
}
