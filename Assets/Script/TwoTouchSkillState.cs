using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "New Skill/TwoTouchSkillState")]
public class TwoTouchSkillState : DamageAndDebuffSkillState
{
    public float skill_TwoTouchDamage;
    public float skill_levelTowTouchDamage;
    public float skill_TwoTouchCoefficient;
}
