using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<byte,byte> BuildingCreated;

    public static void OnBuidingCreated(byte SciencePoint, byte PhysicalPoint)
    {
        BuildingCreated?.Invoke(SciencePoint, PhysicalPoint);
    }
}
