namespace Pizza.Views;

public partial class AdminPanel : ContentPage
{
	public AdminPanel()
	{
		InitializeComponent();
        Shell.SetTabBarIsVisible(this, false);
        Shell.SetNavBarIsVisible(this, false);
    }

    private void ExitClick(object sender, EventArgs e)
    {
        Pizza.Secvices.PizzaServices.Instance.Clear();
        Shell.Current.Navigation.PopAsync();
        Shell.Current.GoToAsync("login");
    }
}