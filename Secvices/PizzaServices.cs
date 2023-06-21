using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pizza.Models;

namespace Pizza.Secvices
{
    public class PizzaServices
    {
        static PizzaServices instance;
        public static PizzaServices Instance
        {
            get
            {
                if(instance == null) instance= new PizzaServices();
                return instance;
            }
        }

        public Clients Client { get; set; } = new Clients();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Orders> AllOrders { get; set; } = new List<Orders>();

        protected PizzaServices() { }

        internal bool CheckInCart(Product product)
        {
            return Client.Cart.Count(p => product.ID == p.ID) > 0;
        }

        internal async Task LoadAllOrders()
        {
            AllOrders = await Orders.Load();
        }

        internal async Task LoadClientOrders()
        {
            var temp = await Orders.GetByProperty("ClientID", Client.ID);
            Client.Orders = temp;
        }

        internal void Clear()
        {
            Client = new Clients();
            AllOrders.Clear();
            Products.Clear();
        }
    }
}