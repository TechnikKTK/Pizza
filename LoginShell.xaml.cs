namespace Pizza
{
	public partial class LoginShell : Shell
	{
		public LoginShell()
		{
			InitializeComponent();

            Routing.RegisterRoute("login", typeof(Views.Login));
            Routing.RegisterRoute("register", typeof(Views.RegisterPage));
        }
	}
}