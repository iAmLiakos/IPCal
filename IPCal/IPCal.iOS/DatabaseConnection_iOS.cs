using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using IPCal.Data;
using IPCal.iOS;
using SQLite;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection_iOS))]
namespace IPCal.iOS
{
    public class DatabaseConnection_iOS : IDatabaseConnection
    {
        public SQLiteConnection DbConnection()
        {
            var dbName = "RantezvousDb.db3";
            string personalFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string libraryFolder = Path.Combine(personalFolder, "..", "Library");
            var path = Path.Combine(libraryFolder, dbName);
            return new SQLiteConnection(path);
        }
    }
}