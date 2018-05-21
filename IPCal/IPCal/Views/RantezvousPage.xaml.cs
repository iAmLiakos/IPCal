using IPCal.Data;
using IPCal.Services;
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
    public partial class RantezvousPage : TabbedPage
    {
        public RantezvousPage ()
        {
            InitializeComponent();
            //RantezvousDataAccess da = new RantezvousDataAccess();
            BindingContext = new RantezvousViewModel();
            //Kentro.ItemsSource = da.GetFilteredRantezvous("Kentro");       
            
        }

        //Works only with UWP
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = BindingContext as RantezvousViewModel;
            SearchPageRantezvousGrid.BeginRefresh();
            KentroList.BeginRefresh();
            AmpelokhpoiList.BeginRefresh();
            KaloutsianiList.BeginRefresh();
            KardamitsiaList.BeginRefresh();
            KiafaList.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue)) {
                SearchPageRantezvousGrid.ItemsSource = vm.DataAccess.Rantezvous;
                KentroList.ItemsSource = vm.SortedOCKentro;
                AmpelokhpoiList.ItemsSource = vm.SortedOCAmpelokhpoi;
                KaloutsianiList.ItemsSource = vm.SortedOCKaloutsiani;
                KardamitsiaList.ItemsSource = vm.SortedOCKardamitsia;
                KiafaList.ItemsSource = vm.SortedOCKiafa;
            }
            else
                SearchPageRantezvousGrid.ItemsSource = vm.DataAccess.Rantezvous.Where(i => i.CustomerAddress.ToLower().Contains(e.NewTextValue.ToLower()) || i.CustomerName.ToLower().Contains(e.NewTextValue.ToLower()));
                KentroList.ItemsSource = vm.SortedOCKentro.Where(i => i.CustomerAddress.ToLower().Contains(e.NewTextValue.ToLower()));
                AmpelokhpoiList.ItemsSource = vm.SortedOCAmpelokhpoi.Where(i => i.CustomerAddress.ToLower().Contains(e.NewTextValue.ToLower()));
                KaloutsianiList.ItemsSource = vm.SortedOCKaloutsiani.Where(i => i.CustomerAddress.ToLower().Contains(e.NewTextValue.ToLower()));
                KardamitsiaList.ItemsSource = vm.SortedOCKardamitsia.Where(i => i.CustomerAddress.ToLower().Contains(e.NewTextValue.ToLower()));
                KiafaList.ItemsSource = vm.SortedOCKiafa.Where(i => i.CustomerAddress.ToLower().Contains(e.NewTextValue.ToLower()));

            SearchPageRantezvousGrid.EndRefresh();
            KentroList.EndRefresh();
            AmpelokhpoiList.EndRefresh();
            KaloutsianiList.EndRefresh();
            KardamitsiaList.EndRefresh();
            KiafaList.EndRefresh();
        }
        //Non-MVVM method
        //private async void ServiceButton_Clicked(object sender, EventArgs e)
        //{
        //    EmailService emailservice = new EmailService();
        //    emailservice.SendEmailService();
        //}
    }
}