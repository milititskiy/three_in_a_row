using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuadTypeExtensions
{
    public static bool IsWalkable(this QuadType quadType)
    {
        switch (quadType)
        {
            case QuadType.Normal:
            case QuadType.Sword:
            case QuadType.Flask:
            case QuadType.Shield:
            case QuadType.Coin:
            case QuadType.Enemy:
                //case QuadType.Door:
                return true; // These are walkable.
                             //case QuadType.Wall:
                             //case QuadType.Pit:
                             //return false; // These are not walkable.
            default:
                throw new ArgumentException($"Unknown QuadType: {quadType}");
        }
    }

}
