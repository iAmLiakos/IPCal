using System;
using System.Collections.Generic;
using System.Text;

namespace IPCal.Data
{
    public interface IDatabaseConnection
    {
        SQLite.SQLiteConnection DbConnection();
    }
}
