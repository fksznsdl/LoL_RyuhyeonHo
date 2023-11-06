using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "New Skill/BuffSkill")]
public class BuffSkillState : SkillState
{
    public float skill_BuffTime;
    public float skill_levelBuffTime;

    public float skill_BuffAmount;
    public float skill_Buffcoefficient;
    public float skill_levelBuffAmount;
    public float skill_levelBuffcoefficient;
}
