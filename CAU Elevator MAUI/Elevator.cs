using static CAU_Elevator_MAUI.Floor;

namespace CAU_Elevator_MAUI;

public struct Floor : IComparable
{
    public const int B1 = -1;
    public const int B2 = -2;
    public const int B3 = -3;
    public const int B4 = -4;
    public const int B5 = -5;
    public const int B6 = -6;
    public const int F1 = 1;
    public const int F2 = 2;
    public const int F3 = 3;
    public const int F4 = 4;
    public const int F5 = 5;
    public const int F6 = 6;
    public const int F7 = 7;
    public const int F8 = 8;
    public const int F9 = 9;
    public const int F10 = 10;
    public const int F11 = 11;
    public const int F12 = 12;

    public int Value { get; set; }

    public Floor(int floor) => Value = floor;
    public static implicit operator Floor(int floor) => new(floor);
    public static implicit operator int(Floor f) => f.Value;

    int IComparable.CompareTo(object obj)
    => Value.CompareTo(obj as IComparable);

    public override string ToString()
    {
        return Value > 0 ? $"{Value}F" : $"B{-Value}";
    }

    public List<Floor> NearbyFloors(int n)
    {
        HashSet<Floor> result = new();
        for (Floor f = this - n; f <= this + n; f++)
            result.Add(f);
        return result.ToList();
    }

    public static Floor operator ++(Floor f)
    {
        f.Value = f.Value switch
        {
            F12 => f.Value,
            B1 => F1,
            _ => f.Value + 1
        };
        return f;
    }

    public static Floor operator --(Floor f)
    {
        f.Value = f.Value switch
        {
            B6 => f.Value,
            F1 => B1,
            _ => f.Value + 1
        };
        return f;
    }

    public static int operator -(Floor left, Floor right)
    {
        int count = 0;
        for (Floor f = right; f < left; f++)
            count++;

        return count;
    }

    public static Floor operator +(Floor now, int count)
    {
        return new { now, count } switch
        {
            { now: var n, count: var y } when n > 0 && n.Value + y > 12 => new(F12),
            { now.Value: >= 0 } => new(now.Value + count),
            { now: var n, count: var y } when n.Value + y < 0 => new(n.Value + y),
            { now: var n, count: var y } when n < 0 && n.Value + y > 0 => new(n.Value + y + 1),
            _ => throw new IndexOutOfRangeException($"now: {now}, count: {count}")
        };
    }

    public static Floor operator -(Floor now, int count)
    {
        return new { now, count } switch
        {
            { now: var n, count: var y } when n < 0 && n.Value - y < -6 => new(B6),
            { now.Value: <= 0 } => new(now.Value - count),
            { now: var n, count: var y } when n.Value - y > 0 => new(n.Value - y),
            { now: var n, count: var y } when n > 0 && n.Value - y < 0 => new(n.Value - y - 1),
            _ => throw new IndexOutOfRangeException($"now: {now}, count: {count}")
        };
    }
}
public enum MovingProperty
{
    Ground,
    Underground,
    Both
}
public class Elevator
{
    public int Id { get; }
    public (double X, double Y) Point { get; }

    public List<Floor> Floors { get; }
    public string FloorsRange { get; }

    public MovingProperty Moving { get; }
    // Maximum floors that user can go up/down using stairs,
    // The default value is 1.
    public int StairsRange { get; set; } = 1;

    public Elevator(int id, (double x, double y) point, List<Floor> floors, string floorsRange, MovingProperty moving = MovingProperty.Both)
    {
        Id = id;
        Point = point;
        Floors = floors;
        FloorsRange = floorsRange;
        Moving = moving;
    }

    public bool Goes(Floor floor) => Floors.Contains(floor);
    public bool GoesNearby(Floor floor) => Floors.Any(f => floor.NearbyFloors(StairsRange).Any(f2 => f2 == f));
    public int GoesThrough(Floor depart, Floor dest)
    {
        int result = 0;
        if (dest > depart)
        {
            for (Floor f = depart; f < dest; f++)
                if (Goes(f))
                    result++;
        }
        else
        {
            for (Floor f = dest; f > depart; f--)
                if (Goes(f))
                    result++;
        }
        return result;
    }
    /*
     *
     *  엘리베이터가 빠를 조건
     *
     *
     *
     *  운행층이 적을수록 빠르다.
     *  운행 범위가 좁을수록 빠르다.
     *  
     *  
     *  
     *  
     *  
     *  
     */
}

public static class Elevators
{
    public static IEnumerable<Elevator> EVList
    {
        get
        {
            yield return No1;
            yield return No2;
            yield return No3;
            yield return No4;
            yield return No5;
            yield return No6;
            yield return No7;
            yield return No8;
            yield return No9;
            yield return No10;
            yield return No11;
            yield return No12;
            yield return No13;
        }
    }

    public static Elevator No1 { get; } = new(1, (37.5038152, 126.9561715), new() { F1, F3, F5, F7, F9 }, "1F, 3F, 5F, 7F, 9F", MovingProperty.Ground);
    public static Elevator No2 { get; } = new(2, (37.5038152, 126.9561715), new() { F1, F2, F4, F6, F8 }, "1F, 2F, 4F, 6F, 8F", MovingProperty.Ground);
    public static Elevator No3 { get; } = new(3, (37.5038152, 126.9561715), new() { B6, B5, B4, B3, F1, F3, F6, F9 }, "B6~B3, 1F, 3F, 6F, 9F", MovingProperty.Both);
    public static Elevator No4 { get; } = new(4, (37.5038152, 126.9561715), new() { B6, B5, B4, B3, B2, B1, F1, F2, F3, F4, F5, F6, F7, F8, F9 }, "B6~B1, F1~F9", MovingProperty.Both);
    public static Elevator No5 { get; } = new(5, (37.5037410, 126.9561775), new() { B6, B5, B4, B3, B2, B1, F1, F2, F3, F4, F5, F6, F7, F8, F9 }, "B6~B1, F1~F9", MovingProperty.Both);
    public static Elevator No6 { get; } = new(6, (37.5034780, 126.9559351), new() { B3, B2, B1, F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12 }, "B3~B1, F1~F12", MovingProperty.Both);
    public static Elevator No7 { get; } = new(7, (37.5034780, 126.9559351), new() { B3, B2, B1, F1, F3, F6, F9, F12 }, "B3~B1, F1, F3, F6, F9, F12", MovingProperty.Both);
    public static Elevator No8 { get; } = new(8, (37.5034780, 126.9559351), new() { F1, F5, F8, F11 }, "F1, F5, F8, F11", MovingProperty.Ground);
    public static Elevator No9 { get; } = new(9, (37.5034780, 126.9559351), new() { F1, F4, F7, F10 }, "F1, F4, F7, F10", MovingProperty.Ground);
    public static Elevator No10 { get; } = new(10, (37.5035189, 126.9559496), new() { B3, B2, B1, F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12 }, "B3~B1, F1~F12", MovingProperty.Both);
    public static Elevator No11 { get; } = new(11, (37.5035963, 126.9560827), new() { B3, B2, B1, F1, F3, F6, F9 }, "B3~B1, F1, F3, F6, F9", MovingProperty.Both);
    public static Elevator No12 { get; } = new(12, (37.5035963, 126.9560827), new() { B3, B2, B1, F1, F2, F3, F4, F5, F6, F7, F8, F9 }, "B3~B1, F1~F9", MovingProperty.Both);

    // 13~14호기 위치 확인 불가 (1~10층에 없음)
    public static Elevator No13 { get; } = new(13, (37.5040990, 126.9566181), new() { B6, B5 }, "B6~B5 (대형강의실용)", MovingProperty.Underground);    }