namespace CAU_Elevator_MAUI;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();        
		Routing.RegisterRoute(nameof(ElevatorsPage), typeof(ElevatorsPage));
    }
}
