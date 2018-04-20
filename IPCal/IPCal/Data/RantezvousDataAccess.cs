
using IPCal.Models;
using IPCal.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace IPCal.Data
{
    public class RantezvousDataAccess
    {
        public SQLiteConnection database;
        private static object collisionLock = new object();
        public ObservableCollection<Rantezvous> Rantezvous { get; set; }

        public RantezvousDataAccess()
        {
            database =
                DependencyService.Get<IDatabaseConnection>().
                DbConnection();
            database.CreateTable<Rantezvous>();

            this.Rantezvous =
                new ObservableCollection<Rantezvous>(database.Table<Rantezvous>());

            // If the table is empty, initialize the collection
            if (!database.Table<Rantezvous>().Any())
            {
                AddNewRantezvous();
            }
        }

        public void AddNewRantezvous()
        {
            this.Rantezvous.
                Add(new Rantezvous
                {
                    CustomerName = "Init",
                    CustomerAddress = "Init",
                    CustomerPhone = 12345
                });
        }

        // Use LINQ to query and filter data
        public IEnumerable<Rantezvous> GetFilteredRantezvous(string area)
        {
            // Use locks to avoid database collitions
            lock (collisionLock)
            {
                var query = from cust in database.Table<Rantezvous>()
                            where cust.AreaName == area
                            select cust;
                return query.AsEnumerable();
            }
        }

        // Use SQL queries against data
        //public IEnumerable<RantezvousViewModel> GetFilteredRantezvous()
        //{
        //    lock (collisionLock)
        //    {
        //        return database.
        //            Query<RantezvousViewModel>
        //            ("SELECT * FROM Item WHERE Country = 'Italy'").AsEnumerable();
        //    }
        //}

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

        public int DeleteRantezvous(Rantezvous rantezvousInstance)
        {
            var id = rantezvousInstance.Id;
            if (id != 0)
            {
                lock (collisionLock)
                {
                    database.Delete<RantezvousViewModel>(id);
                }
            }
            this.Rantezvous.Remove(rantezvousInstance);
            return id;
        }

        public void DeleteAllRantezvous()
        {
            lock (collisionLock)
            {
                database.DropTable<Rantezvous>();
                database.CreateTable<Rantezvous>();
            }
            this.Rantezvous = null;
            this.Rantezvous = new ObservableCollection<Rantezvous>
                (database.Table<Rantezvous>());
        }
    }
}
