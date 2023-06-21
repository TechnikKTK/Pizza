using Pizza.Models;
using System.Windows.Input;

namespace Pizza.ViewModels
{
    public class AdminVewModel : BaseViewModel
    {
        public Pizza.Secvices.PizzaServices Service => Pizza.Secvices.PizzaServices.Instance;

        public List<Orders> ListOrders => Service.AllOrders;

        public ICommand OpenOrderCommand { get; set; }

        public AdminVewModel()
        {
            OpenOrderCommand = new Command(order => OpenOrder((Orders)order));
        }

        private void OpenOrder(Orders order)
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new Views.OrderForm(order), true);
        }
    }
}