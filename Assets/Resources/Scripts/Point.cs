using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point //좌표 계산 구조체 사용할 오퍼레이터 별로 반환값이 달라짐.
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x,int y)
    {
        this.X = x;
        this.Y = y;
    }
    public static bool operator == (Point first, Point second)
    {
        return first.X == second.X && first.Y == second.Y;
    }
    public static bool operator !=(Point first, Point second)
    {
        return first.X != second.X || first.Y != second.Y;
    }
    public static Point operator -(Point x, Point y)
    {
        return new Point(x.X - y.X, x.Y - y.Y);
    }
    public bool Equals(Point other)
    {
        return X == other.X && Y == other.Y;
    }
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is Point && Equals((Point)obj);
    }
    public override int GetHashCode()
    {
        return 0;
    }
}
