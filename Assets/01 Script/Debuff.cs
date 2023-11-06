using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff
{
    public string debuffName;
    public float debuffTime;
    public int debuffType;
    public float debuffFigure;
    public float dotDamage;
    public enum DebuffType
    {
        DOT = 0 , SLOW =1, CC = 2, HITATTACk = 3,RESTRICTION =4, AIRBORNE = 5
    }
}
