using Pizza.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Pizza.ViewModels
{
    public class CartViewModel : BaseViewModel
    {
        public Pizza.Secvices.PizzaServices Service => Pizza.Secvices.PizzaServices.Instance;        
        public Clients Client => Service.Client;      

        public bool CanCreateOrder => Service.Client.Cart.Count != 0;
        public bool CartIsEmpty => Service.Client.Cart.Count == 0;
        public bool IsEditableproduct { get; set; } = true;

        public float OrderSum => Client.Cart.Sum(p => p.CountInCart * p.Cost);

        public ICommand CreateOrderCommand { get; set; }
        public ICommand ConfirmOrderCommand { get; set; }
        public ICommand RemoveFromCartCommand { get; set; }
        public ICommand SetPaymentTypeCommand { get; set; }
        public ICommand ShowOrdersCommand { get; set; }

        public CartViewModel()
        {
            RemoveFromCartCommand = new Command((product) => RemoveFromCart((Product)product));
            CreateOrderCommand = new Command(() => OrderDetails(Client));
            ConfirmOrderCommand = new Command(() => ConfirmOrder(Client));
            SetPaymentTypeCommand = new Command(() => SetPayment());
            ShowOrdersCommand = new Command(() => ShowOrders());

            MessagingCenter.Subscribe<Clients, Product>(Client, "update", (client,product) =>
            {
                if (product != null)
                {
                    product.Update();
                    Update();
                }
            });
        }

        private async void ShowOrders()
        {
            await Shell.Current.Navigation.PushAsync(new Views.OrderViewPage(Client));
        }

        private async void SetPayment()
        {
            List<string> data = new List<string>(new string[]
            {
                "Наличные",
                "Картой при получении",
            });

            var result = await Shell.Current.DisplayActionSheet("Выберите способ оплаты", "Отмена", null, data.ToArray());
            if (data.Contains(result))
            {
                if (data.IndexOf(result) == 0)
                {
                    Client.CurrentOrder.TypePayment = OrderPayment.Наличные;
                }
                else
                {
                    Client.CurrentOrder.TypePayment = OrderPayment.Карта;
                }


                Update();
            }
        }

        private async void ConfirmOrder(Clients client)
        {
            Client.CurrentOrder.ClientID = client.ID;
            Client.CurrentOrder.Products = Client.Cart.ToList();
            Client.CurrentOrder.Amount = OrderSum;
            Client.CurrentOrder.OrderState = OrderState.Создан;
            await Client.CurrentOrder.Save();
            await Shell.Current.DisplayAlert("Информация", "Заказ оформлен. Ожидайте звонка оператора для подтверждения", "ОК");

            Client.Orders.Add(client.CurrentOrder);
            var temp = Client.Orders;
            Client.Cart.Clear();
            Client.CurrentOrder = null;
            Client.Orders.Clear();
            await Client.Save();

            Client.Orders = temp;

            foreach (var item in Service.Products)
            {
                item.CountInCart = 0;
                item.Update();
            }
            Update();


            await Shell.Current.Navigation.PopAsync();
            await Shell.Current.GoToAsync("//menu", true);
        }


        private async void OrderDetails(Clients client)
        {
            IsEditableproduct = false;
            Update();
            await Shell.Current.Navigation.PushAsync(new Views.OrderDetails(this));
        }

        private void RemoveFromCart(Product product)
        {
            product.CountInCart = 0;
            Client.Cart.Remove(product);
            product.Update();
            Update();
        }

        public void Update()
        {
            OnPropertyChanged(nameof(Client));
            OnPropertyChanged(nameof(CartIsEmpty));
            OnPropertyChanged(nameof(CanCreateOrder));
            OnPropertyChanged(nameof(OrderSum));
            Client.Update();
        }
    }
}
