using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    public uint ShipsSunk { get; set; } = 0;
    public uint DamageDealt { get; set; } = 0;
    public uint DamageTaken { get; set; } = 0;
    public uint PlanksPickedUp { get; set; } = 0;
    public uint PlanksLost { get; set; } = 0;
    public uint ArrowsFired { get; set; } = 0;
}
