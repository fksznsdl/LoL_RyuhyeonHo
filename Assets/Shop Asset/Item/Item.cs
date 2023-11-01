using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/New Item")]
public class Item : ScriptableObject
{
    public string item_Name;
    public Sprite Icon;
    public string item_Type;
    public string item_Rank;
    public int gold;
    public float hp;
    public float mp;
    public float ap;
    public float ad;
    public float hpregen;
    public float mpregen;
    public float armor;
    public float spellblock;
    public float ADperPenetration;
    public float ADpenetration;
    public float APperPenetration;
    public float APpenetration;
    public float attackspeedPer;
    public float moveSpeedPer;
    public float moveSpeed;
    public float crit;
    public float critDamage;
    [TextArea]
    public string description;
    public Item[] underitem;
}
