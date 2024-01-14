using CAU_Elevator_MAUI.ViewModels;

namespace CAU_Elevator_MAUI;

public partial class ElevatorsPage : ContentPage
{
    public static ElevatorVM ViewModel { get; } = new();
    public ElevatorsPage()
	{
		InitializeComponent();

        BindingContext = ViewModel;
    }
}