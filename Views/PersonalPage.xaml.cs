namespace Pizza.Views;

public partial class PersonalPage : ContentPage
{
	public PersonalPage()
	{
		InitializeComponent();
	}

    private void ExitClick(object sender, EventArgs e)
    {
        var Service = Pizza.Secvices.PizzaServices.Instance;

        Service.Client = new Models.Clients();
        foreach (var item in Service.Products)
        {
            item.CountInCart = 0;
            item.Update();
        }

        Application.Current.MainPage = new LoginShell();
    }
}