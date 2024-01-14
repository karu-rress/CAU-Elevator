using static CAU_Elevator_MAUI.Elevators;

namespace CAU_Elevator_MAUI.ViewModels;

public class ElevatorVM
{
    // TODO: IEnumerable
    public List<Elevator> Lists { get; } = new() { No1, No2, No3, No4, No5, No6, No7, No8, No9, No10, No11, No12, No13, };
    public Command ListCommand { get; } = new(async _ => await Shell.Current.GoToAsync(nameof(ElevatorsPage)));
}
