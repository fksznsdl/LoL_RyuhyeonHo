using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "New Skill/BarrierAndBuffSkillState")]
public class BarrierAndBuffSkillState : BuffSkillState
{
    public float skill_BarrierAmount;
    public float skill_levelBarrierAmount;
    public float skill_BarrierCoefficient;

    public float skill_BarrierTime;
    public float skill_levelBarrierTime;
}
