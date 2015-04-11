using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class VectorExtensions
{
    public static Vector2 Multiply(this Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x * v2.x, v1.y * v2.y);
    }
}   
