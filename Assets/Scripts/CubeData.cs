using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeData
{
    public enum Type
    {
        None = -1,
        Red,
        Green,
        Blue,
        Black,
        Grey,
        Cyan,
        White,
        Magenta,
        Yellow
    }

    public Type type;
    public Vector3 position;
    public Color color;

}
