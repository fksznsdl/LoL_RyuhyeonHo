using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Skill", menuName = "New Skill/DamageAndDebuffSkill")]
public class DamageAndDebuffSkillState : DamageSkillState
{
    public float skill_DebuffTime;
    public float skill_DebuffDamage;
    public float skill_DebuffAmount;
    public float skill_DebuffCoeffcient;

    public float skill_levelDebuffDamage;
    public float skill_levelDebuffAmount;
    public float skill_levelDebuffTime;
    public float skill_CCTime;
    public float skill_levelCCTime;
}
