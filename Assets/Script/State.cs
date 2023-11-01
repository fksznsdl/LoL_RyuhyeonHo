using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "New State/State")]
public class State : ScriptableObject
{
    public string chName;
    public float hp;
    public float hpperlevel;
    public float mp;
    public float mpperlevel;
    public float movespeed;
    public float armor;
    public float armorperlevel;
    public float spellblock;
    public float spellblockperlevel;
    public float attackrange;
    public float hpregen;
    public float hpregenperlevel;
    public float mpregen;
    public float mpregenperlevel;
    public float crit;
    public float critperlevel;
    public float attackdamage;
    public float attackdamageperlevel;
    public float attackspeed;
    public float attackspeedperlevel;
    public float spell;

    public int team;

    public float killgold;
    public float killExp;
    public float killExpperlevel;
    public float sightRange;
}
