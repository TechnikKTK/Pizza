using Firebase.Database;
using Firebase.Database.Query;
using Pizza.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pizza.Models
{
    public class FirebaseObject<T>: NotifyClass where T : new()
    {
        protected T data;
        static FirebaseClient firebase;
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }

        private async static Task LoadFirebase()
        {
            firebase = await FirebaseHelper.GetFirebase();
        }

        public async Task Save()
        {
            await LoadFirebase();
            if (ID == 0) ID = await PushToBase(data);
            else await PopInBase(data);
        }

        public static async Task<List<T>> Load()
        {
            await LoadFirebase();

            var data = new T();
            return await GetAll(data);
        }

        public static async Task<T> GetByID(int ID)
        {
            await LoadFirebase();

            var data = new T();
            var type = data.GetType();
            var property = type.GetProperty("ID");

            var result = (await firebase.Child(type.Name)
                .OnceAsync<T>()).Where(elem => (int)property.GetValue(elem.Object) == ID)
                .Select(item => item.Object).FirstOrDefault();

            return result;
        }

        public static async Task<bool> DeleteByID(int ID)
        {
            await LoadFirebase();

            try
            {
                var data = new T();
                var type = data.GetType();
                var property = type.GetProperty("ID");

                var result = (await firebase.Child(type.Name)
                    .OnceAsync<T>()).Where(elem => (int)property.GetValue(elem.Object) == ID)
                    .Select(item => item).FirstOrDefault();

                await firebase.Child(type.Name).Child(result.Key).DeleteAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<List<T>> GetByProperty(string name, object value)
        {
            await LoadFirebase();

            var data = new T();
            var type = data.GetType();
            var property = type.GetProperty(name);

            var result = (await firebase.Child(type.Name)
                .OnceAsync<T>()).Where(elem => property.GetValue(elem.Object) == value)
                .Select(item => item.Object).ToList();

            return result;
        }

        public static async Task<List<T>> GetByProperty(string name, int value)
        {
            await LoadFirebase();

            var data = new T();
            var type = data.GetType();
            var property = type.GetProperty(name);

            var result = (await firebase.Child(type.Name)
                .OnceAsync<T>()).Where(elem => (int)property.GetValue(elem.Object) == value)
                .Select(item => item.Object).ToList();

            return result;
        }

        public static async Task<T> GetOnceByProperty(string name, int value)
        {
            await LoadFirebase();

            var data = new T();
            var type = data.GetType();
            var property = type.GetProperty(name);

            var result = (await firebase.Child(type.Name)
                .OnceAsync<T>()).Where(elem => (int)property.GetValue(elem.Object) == value)
                .Select(item => item.Object).FirstOrDefault();

            return result;
        }
        public static async Task<T> GetOnceByProperty(string name, string value)
        {
            await LoadFirebase();

            var data = new T();
            var type = data.GetType();
            var property = type.GetProperty(name);

            var result = (await firebase.Child(type.Name)
                .OnceAsync<T>()).Where(elem => (string)property.GetValue(elem.Object) == value)
                .Select(item => item.Object).FirstOrDefault();

            return result;
        }

        public static async Task<bool> ExistByProperty(string name, string value)
        {
            await LoadFirebase();

            var data = new T();
            var type = data.GetType();
            var property = type.GetProperty(name);

            var list = (await firebase.Child(type.Name)
                .OnceAsync<T>()).Select(item => item.Object).ToList();

            if (list == null) return false;
            var result = list.Count(elem =>
            {
                if (property.GetValue(elem) != null)
                    return (string)property.GetValue(elem) == value;
                else return false;
            })
                > 0;

            return result;
        }
        public static async Task<bool> ExistByProperty(string name, int value)
        {
            await LoadFirebase();

            var data = new T();
            var type = data.GetType();
            var property = type.GetProperty(name);

            var list = (await firebase.Child(type.Name)
                .OnceAsync<T>()).Select(item => item.Object).ToList();

            if (list == null) return false;
            var result = list.Count(elem =>
            {
                if (property.GetValue(elem) != null)
                    return (int)property.GetValue(elem) == value;
                else return false;
            })
                > 0;

            return result;
        }

        static async Task<List<T>> GetAll(T data)
        {
            await LoadFirebase();

            var type = data.GetType();
            return (await firebase.Child(type.Name)
                .OnceAsync<T>()).Select(item => item.Object).ToList();
        }

        async Task<int> PushToBase(T data)
        {
            var type = data.GetType();

            var property = type.GetProperty("CreatedAt");
            property.SetValue(data, DateTime.Now);
            property = type.GetProperty("ID");

            var fObject = await firebase.Child(type.Name).OnceAsync<T>();
            var max_id = 0;
            if (fObject != null)
            {
                if (fObject.Count > 0)
                {
                    max_id = fObject.Max(elem => (int)property.GetValue(elem.Object));
                }
            }

            var nextIndex = max_id + 1;

            property.SetValue(data, nextIndex);

            await firebase
              .Child(type.Name)
              .PostAsync<T>(data);

            return nextIndex;
        }

        async Task<int> PopInBase(T data)
        {
            var type = data.GetType();
            var property = type.GetProperty("ID");

            var result = (await firebase.Child(type.Name)
                .OnceAsync<T>()).Where(elem => (int)property.GetValue(elem.Object) == ID)
                .FirstOrDefault();

            await firebase
                    .Child(type.Name)
                    .Child(result.Key)
                    .PutAsync(data);

            return ID;
        }
    }

    public class Clients : FirebaseObject<Clients>
    {
        public Clients()
        {
            data = this;
        }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public List<Orders> Orders { get; set; } = new List<Orders>();
        public Orders CurrentOrder { get; set; } = new Orders();

        public ObservableCollection<Product> Cart { get; set; } = new ObservableCollection<Product>();

        public TypeClient TypeClient { get; set; } =  TypeClient.Клиент;

        public void Update()
        {
            if(CurrentOrder!=null)
            {
                CurrentOrder.Update();
                OnPropertyChanged(nameof(CurrentOrder));
            }
            OnPropertyChanged(nameof(FirstName));
            OnPropertyChanged(nameof(LastName));
            OnPropertyChanged(nameof(Password));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Phone));
            OnPropertyChanged(nameof(Cart));
            OnPropertyChanged(nameof(Orders));
        }
    }

    public enum TypeClient
    {
        Клиент = 1,
        Админ = 2
    }



    public class Orders : FirebaseObject<Orders>
    {
        public Orders()
        {
            data = this;
        }
        public int ClientID { get; set; }
        public DateTime PaymentAt { get; set; }
        public string DeliveryPhone { get; set; } = "";
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryDate { get; set; } = DateTime.Now;
        public TimeSpan DeliveryTime { get; set; } 
        public OrderState OrderState { get; set; }
        public OrderPayment TypePayment { get; set; } =  OrderPayment.Наличные;
        public float Amount { get; set; }
        public string PaymentText => TypePayment.ToString();

        public List<Product> Products { get; set; } = new List<Product>();
        [NotMapped]
        public bool IsExpand { get; internal set; }

        public void Update()
        {
            OnPropertyChanged(nameof(Products));
            OnPropertyChanged(nameof(PaymentText));
            OnPropertyChanged(nameof(Amount));
            OnPropertyChanged(nameof(IsExpand));
            OnPropertyChanged(nameof(OrderState));
            OnPropertyChanged(nameof(TypePayment));
        }
    }

    public enum OrderPayment
    {
        Наличные = 1,
        Карта = 2,
    }

    public enum OrderState
    {
        Создан = 1,
        Готовится = 2,
        Доставка = 3,
        Доставлен = 4,
        Оплачен = 5
    }

    public enum TypeProduct
    {
        Пицца = 1,
        Напитки = 2,
        Соусы = 3,
    }


    public class Product : FirebaseObject<Product>
    {
        public Product()
        {
            data = this;
        }
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public TypeProduct TypeProduct { get; set; } = TypeProduct.Пицца;
        public float Cost { get; set; }
        public float Summ => CountInCart * Cost;
        public int Count { get; set; }
        public int CountInCart { get; set; }
        public bool InCart => Pizza.Secvices.PizzaServices.Instance.CheckInCart(this);

        internal void Update()
        {
            OnPropertyChanged(nameof(CountInCart));
            OnPropertyChanged(nameof(Summ));
            OnPropertyChanged(nameof(InCart));
        }
    }
}
