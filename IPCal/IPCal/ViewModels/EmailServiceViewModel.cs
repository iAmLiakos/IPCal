using IPCal.Data;
using IPCal.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IPCal.ViewModels
{
    public class EmailServiceViewModel
    {
        public RantezvousDataAccess DataAccess = new RantezvousDataAccess();
        public Command SaveUser { get; }
        bool _EmailReminder;
        string _User_Name;
        string _User_Email;
        string _EmailPassword;
        int _DaysForReminder;

        public int DaysForReminder
        {
            get
            {
                if (App.Current.Properties.ContainsKey("Days"))
                {
                    int days = (int)App.Current.Properties["Days"];
                    return days;
                }
                return _DaysForReminder;
            }

            set
            {
                _DaysForReminder = value;
            }
        }

        public string EmailPassword
        {
            get
            {
                if (App.Current.Properties.ContainsKey("EmailPassword"))
                {
                    string pass = (string)App.Current.Properties["EmailPassword"];
                    return pass;
                }
                return _EmailPassword;
            }
            set
            {
                _EmailPassword = value;
            }
        }

        public bool EmailReminder
        {
            get
            {
                if (App.Current.Properties.ContainsKey("EmailReminder"))
                {
                    bool reminder = (bool)App.Current.Properties["EmailReminder"];
                    return reminder;
                }
                return _EmailReminder;
            }
            set
            {
                _EmailReminder = value;
            }
        }

        public string User_Name
        {
            get
            {
                if (App.Current.Properties.ContainsKey("Name"))
                {
                    string name = (string)App.Current.Properties["Name"];
                    return name;
                }
                return _User_Name;
            }
            set
            {
                _User_Name = value;
            }
        }

        public string User_Email
        {
            get
            {
                if (App.Current.Properties.ContainsKey("Email"))
                {
                    string email = (string)App.Current.Properties["Email"];
                    return email;
                }
                return _User_Email;
            }
            set
            {
                _User_Email = value;
            }
        }

        public EmailServiceViewModel()
        {
            
            SaveUser = new Command(async () => await SaveUserItems());
        }

        private async Task SaveUserItems()
        {
            User newUser = new User();
            newUser.User_Name = _User_Name;
            newUser.User_Email = _User_Email;
            newUser.EmailReminder = _EmailReminder;
            newUser.EmailPassword = _EmailPassword;
            newUser.DaysForReminder = _DaysForReminder;
            App.Current.Properties["Name"] = newUser.User_Name;
            App.Current.Properties["Email"] = newUser.User_Email;
            App.Current.Properties["EmailReminder"] = newUser.EmailReminder;
            App.Current.Properties["EmailPassword"] = newUser.EmailPassword;
            App.Current.Properties["Days"] = newUser.DaysForReminder;
            //await Application.Current.MainPage.DisplayAlert("System Message", "Done", "OK");
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public ObservableCollection<Rantezvous> AllRantezvousData
        {
            get
            {
                return DataAccess.Rantezvous;
            }

        }
    }
}
