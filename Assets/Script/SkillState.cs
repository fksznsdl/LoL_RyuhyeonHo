using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "New Skill/Skill")]
public class SkillState : ScriptableObject
{
    public string skill_Name;
    public string skill_Type;
    public int skill_MaxLevel;
    public Sprite skill_Icon;
    public string skill_Description;
    [TextArea]
    public string skill_AddDescription;
    [TextArea]
    public string skill_UpDescription;
    public float skill_originMana;
    public float skill_levelMana;

    public float skill_CoolDownTime;
    public float skill_levelCoolDownTime;

    public float skill_Scale;

}
