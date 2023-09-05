using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Perk : MonoBehaviour
{
    public string title;
    [TextArea]
    public string description;
    public Color cardColor = Color.white;
    public UnityEvent connection;
    public int maxRepeatsOfPerk;
    public int MiniumLevel;
    public Perk[] prerequisitePerks;
    public int weigh = 10;
}
