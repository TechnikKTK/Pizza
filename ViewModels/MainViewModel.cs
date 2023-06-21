using Pizza.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pizza.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<Models.Product> FilterProducts { get; set; } = new ObservableCollection<Models.Product>();
        public Pizza.Secvices.PizzaServices Service => Pizza.Secvices.PizzaServices.Instance;

        public Clients Client => Service.Client;

        public ICommand AddToCartCommand { get; set; }
        public ICommand GoToCartCommand { get; set; }
        public ICommand PaymentCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand SelectCommand { get; set; }
        public MainViewModel()
        {
            AddToCartCommand = new Command((product) => AddToCart((Product)product));
            GoToCartCommand = new Command(() => GoToCart());
            FilterCommand = new Command(() => Filter());
            SearchCommand = new Command((text) => Search(text));
            SelectCommand = new Command((type) => Select((int)type));
            MessagingCenter.Subscribe<Clients>(Client, "clear_cart", client =>
            {
                foreach (var item in Service.Products)
                {
                    item.CountInCart = 0;
                    item.Update();
                }
                Update();
            });

            LoadProducts();
        }

        public void Select(params int[] type)
        {
            var all = new List<Product>();
            foreach (var item in type)
            {
                var ptype = (TypeProduct)item;
                all.AddRange(Service.Products.Where(p => p.TypeProduct == ptype));
            }

            FilterProducts = new ObservableCollection<Product>(all);
            OnPropertyChanged(nameof(FilterProducts));
        }

        private async void GoToCart()
        {
            await Shell.Current.GoToAsync("//cart",true);
        }

        private void Search(object text)
        {
            var search = text.ToString().ToLower();
            var result = Service.Products.Where(p => 
            p.Name.ToLower().StartsWith(search) || 
            p.Name.ToLower().Contains(search));

            FilterProducts = new ObservableCollection<Product>(result);
            OnPropertyChanged(nameof(FilterProducts));
        }

        private async void Filter()
        {
            List<string> data = new List<string>(new string[]
            {
                "Сначала дешевле",
                "Сначала дороже",
            });

            var result = await Shell.Current.DisplayActionSheet("", "Отмена", null, data.ToArray());
            if (data.Contains(result))
            {
                if (data.IndexOf(result) == 0)
                {
                    FilterProducts = new ObservableCollection<Product>(Service.Products.OrderBy(x => x.Cost));
                }
                else
                {
                    FilterProducts = new ObservableCollection<Product>(Service.Products.OrderByDescending(x => x.Cost));
                }

                OnPropertyChanged(nameof(FilterProducts));
            }
        }

        private void AddToCart(Product product)
        {
            if(Client.Cart.Count(p=> p.ID == product.ID) ==0)
            
            Client.Cart.Add(product);

            product.CountInCart++;
            product.Update();
            Update();

            MessagingCenter.Send(Client, "update", product);
        }

        private async void LoadProducts()
        {
            var temp = await Models.Product.Load();
            Service.Products = (temp);
            FilterProducts = new ObservableCollection<Models.Product>(Service.Products);

            OnPropertyChanged(nameof(FilterProducts));
        }

        public void Update()
        {
            OnPropertyChanged(nameof(Client));
            Client.Update();
        }
    }
}
