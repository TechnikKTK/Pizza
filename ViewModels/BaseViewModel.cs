using Pizza.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Formats.Asn1;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pizza.ViewModels
{
    public class NotifyClass : INotifyPropertyChanged
    {
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }


    public class BaseViewModel : INotifyPropertyChanged
    {
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class AuthViewModel : BaseViewModel
    {
        public Pizza.Secvices.PizzaServices Service => Pizza.Secvices.PizzaServices.Instance;
        public Clients Client => Service.Client;
        public ICommand AuthCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

        public AuthViewModel()
        {
            AuthCommand = new Command(() => Auth(Client));
            RegisterCommand = new Command(() => Register(Client));
        }

        private async void Register(Clients client)
        {
            this.Title = "Регистрация";

            Service.Client = client;
            await Client.Save();

            Service.Client = new Clients();
            OnPropertyChanged(nameof(Client));
            
            await Application.Current.MainPage.DisplayAlert(this.Title, "Вы успешно зарегистрировались", "OK");
            await Shell.Current.GoToAsync("login", true);
        }

        private async void Auth(Clients client)
        {
            this.Title = "Авторизация";
            if(string.IsNullOrEmpty(client.Password) || string.IsNullOrEmpty(client.Phone))
            {
                await Application.Current.MainPage.DisplayAlert(this.Title, "Не заполнены логи и/или пароль", "OK");
                return;
            }

            var temp = await Clients.GetOnceByProperty(nameof(client.Phone), client.Phone);
            if (temp != null)
            {
                if (temp.Password == client.Password)
                {
                    Service.Client = temp;
                    OnPropertyChanged(nameof(Client));
                    await Application.Current.MainPage.DisplayAlert(this.Title, "Вы успешно авторизовались", "OK");

                    if (Client.TypeClient == TypeClient.Клиент)
                    {
                        await Service.LoadClientOrders();
                        Application.Current.MainPage = new AppShell();
                    }
                    else
                    {
                        await Service.LoadAllOrders();
                        Service.Client.Orders.Clear();
                        await Shell.Current.Navigation.PushAsync(new Views.AdminPanel());
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(this.Title, "Неверный пароль", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(this.Title, "Неверные логин и/или пароль", "OK");
            }
        }

        internal void Update()
        {
            OnPropertyChanged(nameof(Client));
        }
    }
}
