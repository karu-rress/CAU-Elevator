using System.Windows.Input;

namespace CAU_Elevator_MAUI;

public partial class AboutPage : ContentPage
{
    public ICommand TapCommand => new Command<string>(async url => await Launcher.OpenAsync(url));
    public AboutPage()
	{
		InitializeComponent();
	}
}