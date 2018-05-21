using System;
using System.Collections.Generic;
using System.Text;

namespace IPCal.Models
{
    public class User
    {
        public bool EmailReminder { get; set; }
        public string User_Name { get; set; }
        public string User_Email { get; set; }
        public string EmailPassword { get; set; }
        public int DaysForReminder { get; set; }
    }
}
