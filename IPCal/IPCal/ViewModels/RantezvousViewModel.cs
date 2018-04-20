using IPCal.Data;
using IPCal.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IPCal.ViewModels
{
    public class RantezvousViewModel: INotifyPropertyChanged
    {
        bool _isBusy = false;
        public Command SaveButtonCommand { get; }
        public RantezvousDataAccess DataAccess = new RantezvousDataAccess();
        

        public RantezvousViewModel()
        {
            SaveButtonCommand = new Command(async () => await SaveAppointment());
        }

        public ObservableCollection<Rantezvous> DisplayRantezvousList
        {
            get { return DataAccess.Rantezvous; }
        }

        public bool isBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        #region MODEL AND VIEWMODEL properties
        int _Id;
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
        string _CustomerName;
        string _CustomerAddress;
        int _CustomerPhone;
        string _AreaName;
        DateTime _AppointmentDate;
        DateTime _NextAppointmentDate;
        int _FrequencyOfCleaning;
        string _Details;

        public string CustomerName
        {
            get { return _CustomerName; }
            set {
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
            get { return _AppointmentDate; }
            set
            {
                _AppointmentDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime NextAppointmentDate
        {
            get { return _NextAppointmentDate; }
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
            "Kentro",
            "Kaloutsiani",
            "Kardamitsia",
            "Ampelokhpoi",
            "Kiafa",
            "Ioannina"
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

        #region Save Rantezvous
        public Task SaveAppointment()
        {
            Rantezvous newRantezvous = new Rantezvous();
            newRantezvous.CustomerName = _CustomerName;
            newRantezvous.CustomerAddress = _CustomerAddress;
            newRantezvous.CustomerPhone = _CustomerPhone;
            newRantezvous.AppointmentDate = _AppointmentDate;
            newRantezvous.AreaName = SelectedAreaPicker;
            newRantezvous.FrequencyOfCleaning = SelectedFrequencyPicker;
            TimeSpan months = new System.TimeSpan(SelectedFrequencyPicker * 30, 0, 0, 0);
            newRantezvous.NextAppointmentDate = _AppointmentDate.Add(months);
            newRantezvous.Details = _Details;
            isBusy = true;
            DataAccess.SaveRantezvous(newRantezvous);
            isBusy = false;
            return Application.Current.MainPage.DisplayAlert("System Message", "New Appointment saved!", "OK");
            //await Application.Current.MainPage.DisplayAlert("Loading Alert", "Doing some background job", "Cancel?");
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
