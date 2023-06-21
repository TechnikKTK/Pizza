using Pizza.Models;
using Pizza.Secvices;

namespace Pizza.Views;

public partial class OrderViewPage : ContentPage
{
    public Clients Client { get; }

    public OrderViewPage()
	{
		InitializeComponent();
	}

    public OrderViewPage(Clients client):this()
    {
        Client = client;
        this.BindingContext = client;
    }
}