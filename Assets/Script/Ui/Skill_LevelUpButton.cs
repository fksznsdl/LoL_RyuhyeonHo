using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Skill_LevelUpButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private SkillButton skillButton;
    private Status status;
    private int skillNumber;
    [SerializeField]
    public Text panel_levelDes;
    private void OnEnable()
    {
        status = this.transform.parent.parent.GetComponentInParent<Status_UI>().status;
        skillNumber = skillButton.skillNumber;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        skillButton.OnEnablePanel();
        skillButton.panel_AddDes.gameObject.SetActive(false);
        panel_levelDes.gameObject.SetActive(true);
        SetUpDescription();
    }
    public void ClickLevelUpButton(int _number)
    {
        status.skillsLevel[_number]++;
        status.status_UI.OffSkillLevelUpButton();
    }

    private void SetUpDescription()
    {
        string arg, arg1, arg2, arg3, arg4, arg5, arg6, arg7;
        arg = (status.skillStates[skillNumber].skill_originMana + (status.skillStates[skillNumber].skill_levelMana * status.skillsLevel[skillNumber])).ToString();
        arg1 = (status.skillStates[skillNumber].skill_originMana + (status.skillStates[skillNumber].skill_levelMana * 
            (status.skillsLevel[skillNumber]+1))).ToString();
        arg2 = (status.skillStates[skillNumber].skill_CoolDownTime - (status.skillStates[skillNumber].skill_levelCoolDownTime * status.skillsLevel[skillNumber])).ToString();
        arg3 = (status.skillStates[skillNumber].skill_CoolDownTime - (status.skillStates[skillNumber].skill_levelCoolDownTime * 
            (status.skillsLevel[skillNumber]+1))).ToString();
        if (status.skillStates[skillNumber].skill_Type == "DamageSkill")
        {
            DamageSkillState skillState = (DamageSkillState)status.skillStates[skillNumber];
            arg4 = (skillState.skill_damage + (skillState.skill_leveldamage * status.skillsLevel[skillNumber])).ToString();
            arg5 = (skillState.skill_damage + (skillState.skill_leveldamage * (status.skillsLevel[skillNumber]+1))).ToString();
            panel_levelDes.text = string.Format(skillState.skill_UpDescription, arg, arg1, arg2, arg3, arg4, arg5);
        }
        else if (status.skillStates[skillNumber].skill_Type == "Damage&Debuff")
        {
            DamageAndDebuffSkillState skillState = (DamageAndDebuffSkillState)status.skillStates[skillNumber];
            arg4 = (skillState.skill_damage + (skillState.skill_leveldamage * status.skillsLevel[skillNumber])).ToString();
            arg5 = (skillState.skill_damage + (skillState.skill_leveldamage * (status.skillsLevel[skillNumber] + 1))).ToString();
            arg6 = (skillState.skill_DebuffAmount + (skillState.skill_levelDebuffAmount * (status.skillsLevel[skillNumber]))).ToString();
            arg7 = (skillState.skill_DebuffAmount + (skillState.skill_levelDebuffAmount * (status.skillsLevel[skillNumber]+1))).ToString();
            panel_levelDes.text = string.Format(skillState.skill_UpDescription, arg, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        else if (status.skillStates[skillNumber].skill_Type == "Damage&CCskill")
        {
            DamageAndDebuffSkillState skillState = (DamageAndDebuffSkillState)status.skillStates[skillNumber];
            arg4 = (skillState.skill_damage + (skillState.skill_leveldamage * status.skillsLevel[skillNumber])).ToString();
            arg5 = (skillState.skill_damage + (skillState.skill_leveldamage * (status.skillsLevel[skillNumber] + 1))).ToString();
            arg6 = (skillState.skill_CCTime + (skillState.skill_levelCCTime * (status.skillsLevel[skillNumber]))).ToString();
            arg7 = (skillState.skill_CCTime + (skillState.skill_levelCCTime * (status.skillsLevel[skillNumber]+1))).ToString();
            panel_levelDes.text = string.Format(skillState.skill_UpDescription, arg, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        else if (status.skillStates[skillNumber].skill_Type == "ProtectSkill")
        {
            BuffSkillState skillState = (BuffSkillState)status.skillStates[skillNumber];
            arg4 = (skillState.skill_BuffAmount + (skillState.skill_levelBuffAmount * (status.skillsLevel[skillNumber]))).ToString();
            arg5 = (skillState.skill_BuffAmount + (skillState.skill_levelBuffAmount * (status.skillsLevel[skillNumber]+1))).ToString();
            arg6 = (skillState.skill_BuffTime + (skillState.skill_levelBuffTime * (status.skillsLevel[skillNumber]))).ToString();
            arg7 = (skillState.skill_BuffTime + (skillState.skill_levelBuffTime * (status.skillsLevel[skillNumber]+1))).ToString();
            panel_levelDes.text = string.Format(skillState.skill_UpDescription, arg, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        else if (status.skillStates[skillNumber].skill_Type == "CCSkill")
        {
            DamageAndDebuffSkillState skillState = (DamageAndDebuffSkillState)status.skillStates[skillNumber];
            arg4 = (skillState.skill_CCTime + (skillState.skill_levelCCTime * (status.skillsLevel[skillNumber]))).ToString();
            arg5 = (skillState.skill_CCTime + (skillState.skill_levelCCTime * (status.skillsLevel[skillNumber]+1))).ToString();
            panel_levelDes.text = string.Format(skillState.skill_UpDescription, arg, arg1, arg2, arg3, arg4, arg5);
        }
        else if (status.skillStates[skillNumber].skill_Type == "Barrier&Buff")
        {
            BarrierAndBuffSkillState skillState = (BarrierAndBuffSkillState)status.skillStates[skillNumber];
            arg4 = (skillState.skill_BarrierAmount + (skillState.skill_levelBarrierAmount * (status.skillsLevel[skillNumber]))).ToString();
            arg5 = (skillState.skill_BarrierAmount + (skillState.skill_levelBarrierAmount * (status.skillsLevel[skillNumber]+1))).ToString();
            arg6 = (skillState.skill_BuffAmount + (skillState.skill_levelBuffAmount * (status.skillsLevel[skillNumber]))).ToString();
            arg7 = (skillState.skill_BuffAmount + (skillState.skill_levelBuffAmount * (status.skillsLevel[skillNumber]+1))).ToString();
            panel_levelDes.text = string.Format(skillState.skill_UpDescription, arg, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

        }
        else if (status.skillStates[skillNumber].skill_Type == "TwoTouchSecSkill")
        {
            TwoTouchSkillState skillState = (TwoTouchSkillState)status.skillStates[skillNumber];
            arg4 = (skillState.skill_damage + (skillState.skill_leveldamage * status.skillsLevel[skillNumber])).ToString();
            arg5 = (skillState.skill_damage + (skillState.skill_leveldamage * (status.skillsLevel[skillNumber] + 1))).ToString();
            if (skillState.skill_DebuffAmount > 0f)
            {
                arg6 = (skillState.skill_DebuffAmount + (skillState.skill_levelDebuffAmount * (status.skillsLevel[skillNumber]))).ToString();
                arg7 = (skillState.skill_DebuffAmount + (skillState.skill_levelDebuffAmount * (status.skillsLevel[skillNumber]+1))).ToString();
                panel_levelDes.text = string.Format(skillState.skill_UpDescription, arg, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            }
            else
            {
                panel_levelDes.text = string.Format(skillState.skill_UpDescription, arg, arg1, arg2, arg3, arg4, arg5);
            }
            
        }
        else if (status.skillStates[skillNumber].skill_Type == "HealSkill")
        {
            BuffSkillState skillState = (BuffSkillState)status.skillStates[skillNumber];
            arg4 = (skillState.skill_BuffAmount + (skillState.skill_levelBuffAmount * (status.skillsLevel[skillNumber]))).ToString();
            arg5 = (skillState.skill_BuffAmount + (skillState.skill_levelBuffAmount * (status.skillsLevel[skillNumber]+1))).ToString();
            panel_levelDes.text = string.Format(skillState.skill_UpDescription, arg, arg1, arg2, arg3, arg4, arg5);
        }
        else if (status.skillStates[skillNumber].skill_Type == "TwoTouchSkill")
        {
            TwoTouchSkillState skillState = (TwoTouchSkillState)status.skillStates[skillNumber];
            arg4 = (skillState.skill_damage + (skillState.skill_leveldamage * status.skillsLevel[skillNumber])).ToString();
            arg5 = (skillState.skill_damage + (skillState.skill_leveldamage * (status.skillsLevel[skillNumber] + 1))).ToString();
            arg6 = (skillState.skill_TwoTouchDamage + (skillState.skill_levelTowTouchDamage * (status.skillsLevel[skillNumber]))).ToString();
            arg7 = (skillState.skill_TwoTouchDamage + (skillState.skill_levelTowTouchDamage * (status.skillsLevel[skillNumber]+1))).ToString();
            if (skillState.skill_CCTime > 0f)
            {
                string arg8, arg9;
                arg8 = (skillState.skill_CCTime + (skillState.skill_levelCCTime * (status.skillsLevel[skillNumber]))).ToString();
                arg9 = (skillState.skill_CCTime + (skillState.skill_levelCCTime * (status.skillsLevel[skillNumber]+1))).ToString();
                panel_levelDes.text = string.Format(status.skillStates[skillNumber].skill_AddDescription, arg, arg1, arg2,arg3,arg4,arg5,arg6,arg7,arg8,arg9);
            }
            else
            {
                panel_levelDes.text = string.Format(skillState.skill_UpDescription, arg, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        skillButton.panel.gameObject.SetActive(false);
    }
}
