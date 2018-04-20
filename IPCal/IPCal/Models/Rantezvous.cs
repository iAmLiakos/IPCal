using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace IPCal.Models
{
    [Table ("Rantezvous")]
    public class Rantezvous
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public int CustomerPhone { get; set; }
        public string AreaName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime NextAppointmentDate { get; set; }
        public int FrequencyOfCleaning { get; set; }
        public string Details { get; set; }
        
    }
}
