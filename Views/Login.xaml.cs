using Pizza.ViewModels;

namespace Pizza.Views;

public partial class Login : ContentPage
{
    AuthViewModel Context { get; set; }

    public Login()
	{
		InitializeComponent();

        Context = (AuthViewModel)this.BindingContext;
    }

    protected override void OnAppearing()
    {
        Context.Update();
    }
}