#if ANDROID
using Android.Gms.Ads;
using Android.OS;
#endif

using static CAU_Elevator_MAUI.Floor;

namespace CAU_Elevator_MAUI;


public partial class MainPage : ContentPage
{
    private enum Status
    {
        Normal,
        Error,
        Info
    }

    public MainPage()
    {
        InitializeComponent();
        BindingContext = ElevatorsPage.ViewModel;
    }

    private Status CurrentStatus
    {
        set
        {
            switch (value)
            {
                case Status.Normal:
                    ErrorLabel.IsVisible = false;
                    return;

                case Status.Error:
                    ErrorLabel.IsVisible = true;
                    ErrorLabel.TextColor = Colors.Red;
                    return;

                case Status.Info:
                    ErrorLabel.IsVisible = true;
                    ErrorLabel.TextColor = Colors.Blue;
                    return;
            }
        }
    }

    private readonly int[] floorArray = { F12, F11, F10, F9, F8, F7, F6, F5, F4, F3, F2, F1, B1, B2, B3, B4, B5, B6 };
    private Floor DepartFloor => floorArray[DepartPicker.SelectedIndex];
    private Floor DestFloor => floorArray[DestPicker.SelectedIndex];

    // 버튼 클릭 후 오류메시지. 그리고 C#으로 리스트 추가 구현하기.
    private void Button_Clicked(object sender, EventArgs e)
    {
        if (DepartPicker.SelectedIndex is -1 || DestPicker.SelectedIndex is -1)
        {
            CurrentStatus = Status.Error;
            ErrorLabel.Text = "출발 층과 목적지를 선택해주세요.";
            return;
        }

        MainStackLayout.Children.Clear();
        if (DepartPicker.SelectedIndex == DestPicker.SelectedIndex)
        {
            CurrentStatus = Status.Error;
            ErrorLabel.Text = "같은 층인데 뭐하러 엘리베이터를 타요";
            return;
        }

        CurrentStatus = Status.Normal;
        if (Math.Abs(DepartPicker.SelectedIndex - DestPicker.SelectedIndex) is 1)
        {
            CurrentStatus = Status.Info;
            ErrorLabel.Text = "한 층 정도는 걸어가는 게 더 빠를 수도 있어요";
        }

        List<Elevator> ResultList = (from ev in Elevators.EVList
                                     where ev.Goes(DepartFloor) && ev.Goes(DestFloor)
                                     orderby ev.GoesThrough(DepartFloor, DestFloor) ascending,
                                      ev.Floors.Count() ascending,
                                      ev.Id switch { 13 => 0, 5 or 10 or 11 or 12 => 1, _ => 2 }
                                     select ev).ToList();

        if (ResultList.Count is 0)
        {
            CurrentStatus = Status.Error;
            ErrorLabel.Text = "한 번에 가는 엘리베이터가 없습니다.";
            return;
        }


        MainStackLayout.Children.Add(new Label
        {
            Text = $"검색 결과: {ResultList.Count}건 (추천: {ResultList[0].Id}호기)",
            Margin = new(20, 13, 0, 13),
            FontSize = 20,
            FontAttributes = FontAttributes.Bold
        });

        foreach (var ev in ResultList)
        {
            DrawBar();

            Grid grid = new()
            {
                RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(38) },
                        new RowDefinition { Height = new GridLength(38) },
                    },
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(0, GridUnitType.Auto) }
                    }
            };

            grid.Add(new Label { Text = $"{ev.Id}호기", Margin = new(11, 13, 0, 0), FontAttributes = FontAttributes.Bold }, 1, 0);
            grid.Add(new Label { Text = $"운행층: {ev.FloorsRange}", Margin = new(11, 5, 0, 0), FontSize = 12 }, 1, 1);

            MainStackLayout.Children.Add(grid);
        }
    }

    private void DrawBar()
    {
        MainStackLayout.Children.Add(new BoxView
        {
            Margin = new(10, 0, 10, 0),
            Color = Color.FromArgb("#CCCCCCCC"),
            HeightRequest = 1
        });
    }
}