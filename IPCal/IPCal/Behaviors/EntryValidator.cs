using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IPCal.Behaviors
{
    class EntryValidator : Behavior<Entry>
    {
        //Result Boolean
        private static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid",
            typeof(bool), typeof(EntryValidator), false);
        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;


        public bool IsValid
        {
            get
            {
                return (bool)GetValue(IsValidProperty);
            }
            private set
            {
                SetValue(IsValidPropertyKey, value);
            }
        }

        //Result message
        public static readonly BindableProperty MessageProperty = BindableProperty.Create("Message",
            typeof(string), typeof(EntryValidator), string.Empty);

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            private set { SetValue(MessageProperty, value); }
        }

        //Is check empty
        public static BindableProperty IsCheckEmptyProperty = BindableProperty.Create("IsCheckEmpty",
            typeof(bool), typeof(EntryValidator), false);

        public bool IsCheckEmpty
        {
            get { return (bool)GetValue(IsCheckEmptyProperty); }
            set { SetValue(IsCheckEmptyProperty, value); }
        }

        //Validate from start
        public static BindableProperty IsValidateFromStartProperty = BindableProperty.Create("ValidateFromStart",
            typeof(bool), typeof(EntryValidator), false);

        public bool ValidateFromStart
        {
            get { return (bool)GetValue(IsValidateFromStartProperty); }
            set { SetValue(IsValidateFromStartProperty, value); }
        }


        //Is check email
        public static BindableProperty IsCheckEmailProperty = BindableProperty.Create("IsCheckEmail",
            typeof(bool), typeof(EntryValidator), false);

        public bool IsCheckEmail
        {
            get { return (bool)GetValue(IsCheckEmailProperty); }
            set { SetValue(IsCheckEmailProperty, value); }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
        }

        private void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsOldTextNull = e.OldTextValue == null;

            IsValid = e.NewTextValue.Length > 4;
            if (!IsValid)
                {
                    Message = "Πρέπει να είναι πάνω απο 4 χαρακτήρες";
                    ((Entry)sender).TextColor = Color.Red;
                    if (IsOldTextNull && !ValidateFromStart)
                        Message = string.Empty;

                    return;
                }
            Message = string.Empty;
            ((Entry)sender).TextColor = Color.Black;

        }

            //TODO add more validation

            //Default
            //IsValid = true;
            //Message = string.Empty;

            
        }

}
