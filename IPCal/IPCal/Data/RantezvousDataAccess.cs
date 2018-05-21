
using IPCal.Models;
using IPCal.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace IPCal.Data
{
    public class RantezvousDataAccess : INotifyPropertyChanged
    {
        public SQLiteConnection database;
        private static object collisionLock = new object();

        public ObservableCollection<Rantezvous> Rantezvous { get; set; }

        public RantezvousDataAccess()
        {
            database = DependencyService.Get<IDatabaseConnection>().DbConnection();
            database.CreateTable<Rantezvous>();

            Rantezvous = new ObservableCollection<Rantezvous>(database.Table<Rantezvous>());

            // If the table is empty, initialize the collection
            if (!database.Table<Rantezvous>().Any())
            {
                InitRantezvous();
            }
        }

        public void InitRantezvous()
        {
            Rantezvous rn = new Rantezvous
            {
                CustomerName = "Ηλίας Παπανικολάου",
                CustomerAddress = "Δ.Χατζή 32",
                CustomerPhone = 265136913,
                AreaName = "Αμπελόκηποι",
                AppointmentDate = new DateTime().Date,
                FrequencyOfCleaning = 12
            };
            database.Insert(rn);
            this.Rantezvous.Add(rn);
            OnPropertyChanged();

        }

        // Use LINQ to query and filter data
        public ObservableCollection<Rantezvous> GetFilteredRantezvous(string area)
        {
            ObservableCollection<Rantezvous> queried = new ObservableCollection<Rantezvous>();

            // Use locks to avoid database collitions
            lock (collisionLock)
            {
                var query = from cust in database.Table<Rantezvous>()
                            where cust.AreaName == area
                            select cust;
                foreach (var item in query)
                {
                    queried.Add(item);
                }
                return queried;
                //public IEnumerable<Rantezvous>
                //return query.AsEnumerable();
            }
        }

        // Use SQL queries against data
        public IEnumerable<Rantezvous> GetFilteredRantezvous10DaysNear()
        {
            lock (collisionLock)
            {
                //var query = from c in data.Rantezvous
                //            orderby c.AppointmentDate
                //            where c.AppointmentDate >= DateTime.Now && c.AppointmentDate <= DateTime.Now.AddDays(10)
                //            select new { c.CustomerName, c.CustomerAddress, c.AppointmentDate, c.DateTrimmed };
                //var results = query.ToList();
                return database.
                    Query<Rantezvous>
                    ("SELECT * FROM Rantezvous WHERE AppointmentDate >= DATETIME('now', '-7 day');").AsEnumerable();
            }

        }
            public Rantezvous GetRantezvous(int id)
        {
            lock (collisionLock)
            {
                return database.Table<Rantezvous>().
                    FirstOrDefault(rantezvous => rantezvous.Id == id);
            }
        }

        public int SaveRantezvous(Rantezvous rantezvousInstance)
        {
            lock (collisionLock)
            {
                if (rantezvousInstance.Id != 0)
                {
                    database.Update(rantezvousInstance);
                    return rantezvousInstance.Id;
                }
                else
                {
                    database.Insert(rantezvousInstance);
                    return rantezvousInstance.Id;
                }
            }
        }

        public void SaveAllRantezvous()
        {
            lock (collisionLock)
            {
                foreach (var rantezvousInstance in this.Rantezvous)
                {
                    if (rantezvousInstance.Id != 0)
                    {
                        database.Update(rantezvousInstance);
                    }
                    else
                    {
                        database.Insert(rantezvousInstance);
                    }
                }
            }
        }

        public async void DeleteRantezvous(Rantezvous rantezvousInstance)
        {
            var id = rantezvousInstance.Id;
            if (id != 0)
            {
                var result = await Application.Current.MainPage.DisplayAlert("Confirmation", "Are you sure? This cannot be undone", "OK", "Cancel");
                if (result == true)
                {
                    lock (collisionLock)
                    {
                        database.Delete<Rantezvous>(id);
                        this.Rantezvous.Remove(rantezvousInstance);
                        //OnPropertyChanged();
                    }
                }
            }
        }

        public async void DeleteAllRantezvous()
        {
            try
            {
                var result = await Application.Current.MainPage.DisplayAlert("Μήνυμα επιβεβαίωσης", "Είστε σίγουρος/η; Αυτή η ενέργεια είναι μόνιμη και δε θα μπορέσετε να επαναφέρετε τα δεδομένα!", "Είμαι σίγουρος", "Όχι, ακύρωση.");
                if (result == true)
                {
                    database.DropTable<Rantezvous>();
                    database.CreateTable<Rantezvous>();
                    Rantezvous.Clear();
                    await Application.Current.MainPage.DisplayAlert("Μήνυμα Συστήματος", "Έγινε διαγραφή όλων", "OK");
                }
                //this.Rantezvous = null;
                //this.Rantezvous = new ObservableCollection<Rantezvous>
                //(database.Table<Rantezvous>());
                //Rantezvous.Clear();

            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("error", "error deleting", "OK");
                throw e;
            }

        }

        #region PropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
