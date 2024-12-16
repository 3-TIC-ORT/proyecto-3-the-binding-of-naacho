using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FinishStats
{
    public float cronometer;
    public string[] itemsTaken;

    public FinishStats(float cronometerP, string[] itemsTakenP)
    {
        cronometer = cronometerP;
        itemsTaken = itemsTakenP;
    }
}
