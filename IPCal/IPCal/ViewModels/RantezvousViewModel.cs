using IPCal.Data;
using IPCal.Models;
using IPCal.Services;
using IPCal.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IPCal.ViewModels
{
    public class RantezvousViewModel : INotifyPropertyChanged
    {
        bool _isBusy = false;
        public Command SaveButtonCommand { get; }
        public Command DeleteAllButtonCommand { get; }
        public Command SeeButtonCommand { get; }
        public Command DeleteOneItemCommand { get; }
        public Command DeleteOneItemCommand2 { get; }
        public Command UpdateButtonCommand { get; }
        public Command UpdateButton { get; }
        public Command SettingsButton { get; }
        public Command SendEmailButton { get; }
        public RantezvousDataAccess DataAccess = new RantezvousDataAccess();

        #region Collections to Display!
        //General Collection
        public ObservableCollection<Rantezvous> RantezvousData
        {
            get
            {
                return DataAccess.Rantezvous;
            }

        }
        public IEnumerable<Rantezvous> Near10Dates
        {
            get
            {
                return DataAccess.GetFilteredRantezvous10DaysNear();
                //var kardamitsia = DataAccess.Rantezvous.Where(x => x.AreaName == "Kardamitsia");
                //return new ObservableCollection<Rantezvous>(kardamitsia);
            }
        }

        public ObservableCollection<Rantezvous> SortedOCAmpelokhpoi
        {
            get
            {
                var ampelok = RantezvousData.Where(x => x.AreaName == "Αμπελόκηποι");
                return new ObservableCollection<Rantezvous>(ampelok);
            }
        }

        public IEnumerable<Rantezvous> SortedOCKentro
        {
            get
            {
                return DataAccess.GetFilteredRantezvous("Κέντρο").OrderBy(x => x.AppointmentDate);
            }
        }

        public IEnumerable<Rantezvous> SortedOCKaloutsiani
        {
            get
            {
                return DataAccess.GetFilteredRantezvous("Καλούτσιανη").OrderBy(x => x.AppointmentDate);
            }
        }

        public IEnumerable<Rantezvous> SortedOCKiafa
        {
            get
            {
                return DataAccess.GetFilteredRantezvous("Κιάφα").OrderBy(x => x.AppointmentDate);
            }
        }

        public IEnumerable<Rantezvous> SortedOCMakrina
        {
            get
            {
                return DataAccess.GetFilteredRantezvous("Μακρινά").OrderBy(x => x.AppointmentDate);
            }
        }

        public IEnumerable<Rantezvous> SortedOCPlateiaOmirou
        {
            get
            {
                return DataAccess.GetFilteredRantezvous("Πλατεία Ομήρου/Σπύρου Λάμπρου").OrderBy(x => x.AppointmentDate);
            }
        }
        public IEnumerable<Rantezvous> SortedOCKardamitsia
        {
            get
            {
                return DataAccess.GetFilteredRantezvous("Καρδαμίτσια/Ανατολή").OrderBy(x => x.AppointmentDate);
            }
        }

        
        

       

        

        

        #endregion

        #region 2 Constructors
        public RantezvousViewModel()
        {
            SaveButtonCommand = new Command(async () => await SaveAppointment());
            DeleteAllButtonCommand = new Command(async () => await DeleteAllAppointments());
            SeeButtonCommand = new Command(async () => await GotoRantezvousPage());

            UpdateButtonCommand = new Command(async () => await UpdateAppointment(ItemSelected));
            DeleteOneItemCommand2 = new Command(() => DeleteOne(ItemSelected));
            SettingsButton = new Command(async () => await GotoSettingsPageMD());
            //SettingsPageCommand = new Command(async () => await GotoSettingsPageMD());
            SendEmailButton = new Command(() => SendEmail());


            UpdateButton = new Command(async (e) =>
            {
                var item = (e as Rantezvous);
                await GotoRantezvousDetailPage(item);
                OnPropertyChanged();
            });

            DeleteOneItemCommand = new Command((e) =>
            {
                var item = (e as Rantezvous);
                DeleteOne(item);
                OnPropertyChanged();
            });
        }

        private void SendEmail()
        {
            isBusy = true;
            
            Task.Run(()=> { 
            EmailService es = new EmailService();
            es.SendEmailService();

                isBusy = false;
                //Device.BeginInvokeOnMainThread(() => {
                //    isBusy = false;
                //    //Application.Current.MainPage.DisplayAlert("System Message", "Το email στάλθηκε επιτυχώς!", "Συνέχεια");
                //});
            });

            

        }

        public RantezvousViewModel(Rantezvous currentRant)
        {
            Id = currentRant.Id;
            CustomerName = currentRant.CustomerName;
            CustomerAddress = currentRant.CustomerAddress;
            CustomerPhone = currentRant.CustomerPhone;
            AppointmentDate = currentRant.AppointmentDate;
            NextAppointmentDate = currentRant.NextAppointmentDate;
            Details = currentRant.Details;
            AreaName = currentRant.AreaName;
            FrequencyOfCleaning = currentRant.FrequencyOfCleaning;
            SelectedAreaPicker = AreaName;
            SelectedFrequencyPicker = FrequencyOfCleaning;

            SaveButtonCommand = new Command(async () => await SaveAppointment());
            UpdateButtonCommand = new Command(async () => await UpdateAppointment(currentRant));
            DeleteAllButtonCommand = new Command(async () => await DeleteAllAppointments());
            SeeButtonCommand = new Command(async () => await GotoRantezvousPage());

            DeleteOneItemCommand2 = new Command(() => DeleteOne(ItemSelected));
            DeleteOneItemCommand = new Command((e) => 
            {
                var item = (e as Rantezvous);
                DeleteOne(item);
                //RantezvousData.Remove(item);
                OnPropertyChanged();
            });
        }
        #endregion

        #region Navigation Pages
        private async Task GotoRantezvousDetailPage(Rantezvous rant)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RantezvousDetailsPage(rant));
        }

        private async Task GotoRantezvousPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RantezvousPage());
        }

        private async Task GotoSettingsPageMD()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage());
        }
        #endregion

        #region MODEL AND VIEWMODEL properties
        public bool isBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        private bool _isEmailEnabled;
        public bool isEmailEnabled
        {
            get
            {
                if (App.Current.Properties.ContainsKey("EmailReminder"))
                {
                    bool reminder = (bool)App.Current.Properties["EmailReminder"];
                    return reminder;
                }
                return _isEmailEnabled;
            }
            set
            {
                _isEmailEnabled = value;
            }
        }

        //Model
        int _Id;
        string _CustomerName;
        string _CustomerAddress;
        int _CustomerPhone;
        string _AreaName;
        DateTime _AppointmentDate;
        DateTime _NextAppointmentDate;
        int _FrequencyOfCleaning;
        string _Details;
        Rantezvous _ItemSelected;

        [PrimaryKey, AutoIncrement]

        public int Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged();
            }
        }

        public string CustomerName
        {
            get { return _CustomerName; }
            set
            {
                _CustomerName = value;
                OnPropertyChanged();
            }
        }

        public string CustomerAddress
        {
            get { return _CustomerAddress; }
            set
            {
                _CustomerAddress = value;
                OnPropertyChanged();
            }
        }

        public int CustomerPhone
        {
            get { return _CustomerPhone; }
            set
            {
                _CustomerPhone = value;
                OnPropertyChanged();
            }
        }

        public string AreaName
        {
            get { return _AreaName; }
            set
            {
                _AreaName = value;
                OnPropertyChanged();
            }
        }
        public DateTime AppointmentDate
        {
            get { return _AppointmentDate.Date; }
            set
            {
                _AppointmentDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime NextAppointmentDate
        {
            get
            {

                TimeSpan months = new System.TimeSpan(SelectedFrequencyPicker * 30, 0, 0, 0);
                return AppointmentDate.Add(months);
            }
            set
            {
                _NextAppointmentDate = value;
                OnPropertyChanged();
            }
        }

        public int FrequencyOfCleaning
        {
            get { return _FrequencyOfCleaning; }
            set
            {
                _FrequencyOfCleaning = value;
                OnPropertyChanged();
            }
        }

        public string Details
        {
            get { return _Details; }
            set
            {
                _Details = value;
                OnPropertyChanged();
            }
        }

        public Rantezvous ItemSelected
        {
            get
            {
                return _ItemSelected;
            }
            set
            {
                _ItemSelected = value;
                OnPropertyChanged();
            }
        }

        public string NextDateTrimmed
        {
            get { return NextAppointmentDate.ToLongDateString(); }
        }

        public string DateTrimmed
        {
            get { return AppointmentDate.ToLongDateString(); }

        }
        #endregion

        #region Populating Pickers
        List<int> _FrequencyPicker = new List<int>
        {
            2,
            4,
            6,
            8,
            12
        };
        public List<int> FrequencyPicker => _FrequencyPicker;

        List<string> _AreaPicker = new List<string>
        {
            "Αμπελόκηποι",
            "Κέντρο",
            "Καλούτσιανη",
            "Κιάφα",
            "Μακρινά",
            "Πλατεία Ομήρου/Σπύρου Λάμπρου",
            "Καρδαμίτσια/Ανατολή"

            
            
        };
        public List<string> AreaPicker => _AreaPicker;
        #endregion

        #region Pickers AREA + FREQUENCY
        public string SelectedAreaPicker
        {
            get;
            set;
        }
        public int SelectedFrequencyPicker
        {
            get;
            set;
        }
        #endregion

        #region Save/Delete Rantezvous
        public async Task SaveAppointment()
        {
            Rantezvous newRantezvous = new Rantezvous();
            newRantezvous.CustomerName = _CustomerName;
            newRantezvous.CustomerAddress = _CustomerAddress;
            newRantezvous.CustomerPhone = _CustomerPhone;
            newRantezvous.AppointmentDate = _AppointmentDate;
            TimeSpan months = new System.TimeSpan(SelectedFrequencyPicker * 30, 0, 0, 0);
            newRantezvous.NextAppointmentDate = _AppointmentDate.Add(months);
            newRantezvous.AreaName = SelectedAreaPicker;
            newRantezvous.FrequencyOfCleaning = SelectedFrequencyPicker;
            newRantezvous.Details = _Details;
            newRantezvous.DateTrimmed = DateTrimmed;
            newRantezvous.NextDateTrimmed = NextDateTrimmed;
            isBusy = true;
            DataAccess.SaveRantezvous(newRantezvous);
            isBusy = false;
            await Application.Current.MainPage.DisplayAlert("System Message", "Αποθηκεύτηκε για τη διεύθυνση: "+" "+ newRantezvous.CustomerAddress, "OK");
            await Application.Current.MainPage.Navigation.PushAsync(new RantezvousPage());
            //await Application.Current.MainPage.DisplayAlert("Loading Alert", "Doing some background job", "Cancel?");
        }

        public Task DeleteAllAppointments()
        {
            DataAccess.DeleteAllRantezvous();
            return Task.CompletedTask;
            //return Application.Current.MainPage.DisplayAlert("System Message", "Deleted Everything", "OK");
        }

        public void DeleteOne(Rantezvous item)
        {
            DataAccess.DeleteRantezvous(item);
        }

        public async Task UpdateAppointment(Rantezvous newRantezvous)
        {
            newRantezvous.CustomerName = _CustomerName;
            newRantezvous.CustomerAddress = _CustomerAddress;
            newRantezvous.CustomerPhone = _CustomerPhone;
            newRantezvous.AppointmentDate = _AppointmentDate;
            TimeSpan months = new System.TimeSpan(SelectedFrequencyPicker * 30, 0, 0, 0);
            newRantezvous.NextAppointmentDate = _AppointmentDate.Add(months);
            newRantezvous.AreaName = SelectedAreaPicker;
            newRantezvous.FrequencyOfCleaning = SelectedFrequencyPicker;
            newRantezvous.Details = _Details;
            newRantezvous.DateTrimmed = DateTrimmed;
            newRantezvous.NextDateTrimmed = NextDateTrimmed;
            isBusy = true;
            DataAccess.SaveRantezvous(newRantezvous);
            isBusy = false;
            await Application.Current.MainPage.DisplayAlert("System Message", "Updated" + " " + newRantezvous.CustomerName + "/n" + "Returning to All List".ToString(), "OK");
            await Application.Current.MainPage.Navigation.PopAsync();
            //return Application.Current.MainPage.DisplayAlert("System Message", "Updated" + " " + newRantezvous.CustomerName.ToString(), "OK");
        }
        #endregion

        #region PropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
