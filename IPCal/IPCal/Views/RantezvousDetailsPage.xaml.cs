using IPCal.Models;
using IPCal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IPCal.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RantezvousDetailsPage : ContentPage
	{
		public RantezvousDetailsPage ()
		{
			InitializeComponent ();
            BindingContext = new RantezvousViewModel();
        }

        public RantezvousDetailsPage(Rantezvous currentRantezvous)
        {
            InitializeComponent();
            BindingContext = new RantezvousViewModel(currentRantezvous);
        }
	}
}