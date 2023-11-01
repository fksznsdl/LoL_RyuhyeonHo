using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "New Skill/DamageSkill")]
public class DamageSkillState : SkillState
{
    public float skill_damageCoefficient;
    public float skill_damage;
    public float skill_leveldamage;
}
