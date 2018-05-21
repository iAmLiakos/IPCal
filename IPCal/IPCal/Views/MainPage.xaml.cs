using IPCal.Data;
using IPCal.Models;
using IPCal.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IPCal
{
	public partial class MainPage : ContentPage
	{
        public MainPage()
		{
			InitializeComponent();
            BindingContext = new RantezvousViewModel();
        }

    }
}
