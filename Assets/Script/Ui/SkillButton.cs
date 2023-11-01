using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public Status status;

    private Image image;

    [SerializeField]
    public int skillNumber;
    [SerializeField]
    public GameObject panel;
    [SerializeField]
    private Image panel_Icon;
    [SerializeField]
    private Text panel_Des;
    [SerializeField]
    public Text panel_AddDes;
    [SerializeField]
    private Text panel_levelDes;
    [SerializeField]
    private Text panel_Mana;
    [SerializeField]
    private Text panel_CoolTime;

    private void Start()
    {
        StartCoroutine(StayCoroutine());
    }
    private IEnumerator StayCoroutine()
    {
        yield return new WaitForSeconds(3.5f);
        status = this.transform.parent.parent.parent.GetComponent<Status_UI>().status;
        image = GetComponent<Image>();
        image.sprite = status.skillStates[skillNumber].skill_Icon;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        panel_levelDes.gameObject.SetActive(false);
        OnEnablePanel();
        if (skillNumber != 0)
        {
            panel_AddDes.gameObject.SetActive(true);
            SetAddDescription();
        }
        else
        {
            panel_AddDes.gameObject.SetActive(false);
        }
    }
    public void OnEnablePanel()
    {
        panel.gameObject.SetActive(true);
        panel_Icon.sprite = status.skillStates[skillNumber].skill_Icon;
        panel_Des.text = status.skillStates[skillNumber].skill_Description;
        panel_Mana.text = (status.skillStates[skillNumber].skill_originMana + (status.skillStates[skillNumber].skill_levelMana * status.skillsLevel[skillNumber])).ToString();
        panel_CoolTime.text = (status.skillStates[skillNumber].skill_CoolDownTime - (status.skillStates[skillNumber].skill_levelCoolDownTime * status.skillsLevel[skillNumber])).ToString();
    }

    private void SetAddDescription()
    {
        string arg, arg1, arg2, arg3;

        if (status.skillStates[skillNumber].skill_Type == "DamageSkill")
        {
            DamageSkillState skillState = (DamageSkillState)status.skillStates[skillNumber];
            arg = (skillState.skill_damage + " + " + skillState.skill_leveldamage * status.skillsLevel[skillNumber] + " + " + status.spell + " * " +
                skillState.skill_damageCoefficient + " = " + (status.spell * skillState.skill_damageCoefficient + skillState.skill_damage
                + (skillState.skill_leveldamage * status.skillsLevel[skillNumber]))).ToString();
            panel_AddDes.text = string.Format(status.skillStates[skillNumber].skill_AddDescription, arg);
        }
        else if (status.skillStates[skillNumber].skill_Type == "Damage&Debuff")
        {
            DamageAndDebuffSkillState skillState = (DamageAndDebuffSkillState)status.skillStates[skillNumber];
            arg = (skillState.skill_damage + " + " + skillState.skill_leveldamage * status.skillsLevel[skillNumber] + " + " + status.spell + " * " +
                skillState.skill_damageCoefficient + " = " + (status.spell * skillState.skill_damageCoefficient + skillState.skill_damage
                + (skillState.skill_leveldamage * status.skillsLevel[skillNumber]))).ToString();
            arg1 = (skillState.skill_DebuffAmount + " + " + skillState.skill_levelDebuffAmount * status.skillsLevel[skillNumber] + " + " + status.spell + " * " +
                skillState.skill_DebuffCoeffcient + " = " + (status.spell * skillState.skill_DebuffCoeffcient + skillState.skill_DebuffAmount
                + (skillState.skill_levelDebuffAmount * status.skillsLevel[skillNumber]))).ToString();
            panel_AddDes.text = string.Format(status.skillStates[skillNumber].skill_AddDescription, arg, arg1);
        }
        else if (status.skillStates[skillNumber].skill_Type == "Damage&CCskill")
        {
            DamageAndDebuffSkillState skillState = (DamageAndDebuffSkillState)status.skillStates[skillNumber];
            arg = (skillState.skill_damage + " + " + skillState.skill_leveldamage * status.skillsLevel[skillNumber] + " + " + status.spell + " * " +
               skillState.skill_damageCoefficient + " = " + (status.spell * skillState.skill_damageCoefficient + skillState.skill_damage
               + (skillState.skill_leveldamage * status.skillsLevel[skillNumber]))).ToString();
            arg1 = (skillState.skill_CCTime + " + " + skillState.skill_levelCCTime * status.skillsLevel[skillNumber] + " = " +
                (skillState.skill_CCTime + (skillState.skill_levelCCTime * status.skillsLevel[skillNumber]))).ToString();
            panel_AddDes.text = string.Format(status.skillStates[skillNumber].skill_AddDescription, arg, arg1);
        }
        else if (status.skillStates[skillNumber].skill_Type == "ProtectSkill")
        {
            BuffSkillState skillState = (BuffSkillState)status.skillStates[skillNumber];
            arg = (skillState.skill_BuffAmount + " + " + skillState.skill_levelBuffAmount * status.skillsLevel[skillNumber] + " + " + status.spell + " * " +
                skillState.skill_Buffcoefficient + " = " + (status.spell * skillState.skill_Buffcoefficient + skillState.skill_BuffAmount
                + (skillState.skill_levelBuffAmount * status.skillsLevel[skillNumber]))).ToString();
            arg1 = (skillState.skill_BuffTime + " + " + skillState.skill_levelBuffTime * status.skillsLevel[skillNumber] + " = " +
                (skillState.skill_BuffTime + (skillState.skill_levelBuffTime * status.skillsLevel[skillNumber]))).ToString();
            panel_AddDes.text = string.Format(status.skillStates[skillNumber].skill_AddDescription, arg, arg1);
        }
        else if (status.skillStates[skillNumber].skill_Type == "CCSkill")
        {
            DamageAndDebuffSkillState skillState = (DamageAndDebuffSkillState)status.skillStates[skillNumber];
            arg = (skillState.skill_CCTime + " + " + skillState.skill_levelCCTime * status.skillsLevel[skillNumber] + " = " +
                (skillState.skill_CCTime + (skillState.skill_levelCCTime * status.skillsLevel[skillNumber]))).ToString();
            panel_AddDes.text = string.Format(status.skillStates[skillNumber].skill_AddDescription, arg);
        }
        else if (status.skillStates[skillNumber].skill_Type == "Barrier&Buff")
        {
            BarrierAndBuffSkillState skillState = (BarrierAndBuffSkillState)status.skillStates[skillNumber];
            arg = (skillState.skill_BarrierAmount + " + " + skillState.skill_levelBarrierAmount * status.skillsLevel[skillNumber] + " + " +
                status.spell * skillState.skill_BarrierCoefficient +
                " = " + (skillState.skill_BarrierCoefficient + (skillState.skill_levelBarrierAmount * status.skillsLevel[skillNumber]) + (status.spell *
                skillState.skill_BarrierCoefficient))).ToString();
            arg1 = (skillState.skill_BuffAmount + " + " + skillState.skill_levelBuffAmount * status.skillsLevel[skillNumber] + " + " +
                status.spell * skillState.skill_Buffcoefficient + " = " + (skillState.skill_BuffAmount + (skillState.skill_levelBuffAmount * status.skillsLevel[skillNumber]
                ) + (status.spell * skillState.skill_Buffcoefficient))).ToString();
            arg2 = (skillState.skill_BuffTime + " + " + skillState.skill_levelBuffTime * status.skillsLevel[skillNumber] + " = " +
                (skillState.skill_BuffTime + (skillState.skill_levelBuffTime * status.skillsLevel[skillNumber]))).ToString();
            panel_AddDes.text = string.Format(status.skillStates[skillNumber].skill_AddDescription, arg, arg1, arg2);

        }
        else if (status.skillStates[skillNumber].skill_Type == "TwoTouchSecSkill")
        {
            TwoTouchSkillState skillState = (TwoTouchSkillState)status.skillStates[skillNumber];
            arg = (skillState.skill_damage + " + " + skillState.skill_leveldamage * status.skillsLevel[skillNumber] + " + " + status.spell + " * " +
                skillState.skill_damageCoefficient + " = " + (status.spell * skillState.skill_damageCoefficient + skillState.skill_damage
                + (skillState.skill_leveldamage * status.skillsLevel[skillNumber]))).ToString();
            arg1 = (skillState.skill_TwoTouchDamage + " + " + skillState.skill_levelTowTouchDamage * status.skillsLevel[skillNumber] + " + " + status.spell + " * " +
                skillState.skill_TwoTouchCoefficient + " = " + (status.spell * skillState.skill_TwoTouchCoefficient + skillState.skill_TwoTouchDamage) +
                (skillState.skill_levelTowTouchDamage * status.skillsLevel[skillNumber])).ToString();
            arg2 = null;
            arg3 = null;
            if (skillState.skill_CCTime > 0f)
            {
                arg2 = (skillState.skill_CCTime).ToString();
                arg3 = (skillState.skill_CCTime + (skillState.skill_levelCCTime * 3f)).ToString();
            }
            else if (skillState.skill_DebuffAmount >0f)
            {
                arg2 = (skillState.skill_DebuffAmount + " + " + skillState.skill_levelDebuffAmount * status.skillsLevel[skillNumber] + " = " +
                    (skillState.skill_DebuffAmount + (skillState.skill_levelDebuffAmount * status.skillsLevel[skillNumber]))).ToString();
                arg3 = ("\"" + skillState.skill_DebuffAmount + " + " + skillState.skill_levelDebuffAmount * status.skillsLevel[skillNumber] + "\" * " + 1.5 + " = " +
                    (skillState.skill_DebuffAmount + (skillState.skill_levelDebuffAmount * status.skillsLevel[skillNumber])) * 1.5f).ToString();
            }
            panel_AddDes.text = string.Format(status.skillStates[skillNumber].skill_AddDescription, arg, arg1, arg2,arg3);
        }
        else if (status.skillStates[skillNumber].skill_Type == "HealSkill")
        {
            BuffSkillState skillState = (BuffSkillState)status.skillStates[skillNumber];
            arg = (skillState.skill_BuffAmount + " + " + skillState.skill_levelBuffAmount * status.skillsLevel[skillNumber] + " + " + status.spell + " * " +
                skillState.skill_Buffcoefficient + " = " + (skillState.skill_BuffAmount + (skillState.skill_levelBuffAmount * status.skillsLevel[skillNumber]) +
                (status.spell * skillState.skill_Buffcoefficient))).ToString();
            panel_AddDes.text = string.Format(status.skillStates[skillNumber].skill_AddDescription, arg);
        }
        else if (status.skillStates[skillNumber].skill_Type == "TwoTouchSkill")
        {
            TwoTouchSkillState skillState = (TwoTouchSkillState)status.skillStates[skillNumber];
            arg = (skillState.skill_damage + " + " + skillState.skill_leveldamage * status.skillsLevel[skillNumber] + " + " + status.spell + " * " +
                skillState.skill_damageCoefficient + " = " + (skillState.skill_damage + (skillState.skill_leveldamage * status.skillsLevel[skillNumber]) +
                (status.spell * skillState.skill_damageCoefficient))).ToString();
            arg1 = (skillState.skill_TwoTouchDamage + " + " + skillState.skill_levelTowTouchDamage * status.skillsLevel[skillNumber] + " + " + status.spell + " * " +
               skillState.skill_TwoTouchCoefficient + " = " + (status.spell * skillState.skill_TwoTouchCoefficient + skillState.skill_TwoTouchDamage) +
               (skillState.skill_levelTowTouchDamage * status.skillsLevel[skillNumber])).ToString();
            if (skillState.skill_CCTime > 0f)
            {
                arg2 = (skillState.skill_CCTime + " + " + skillState.skill_levelCCTime * status.skillsLevel[skillNumber] + " = " + (skillState.skill_CCTime +
                    (skillState.skill_levelCCTime * status.skillsLevel[skillNumber]))).ToString();
                panel_AddDes.text = string.Format(status.skillStates[skillNumber].skill_AddDescription, arg, arg1, arg2);
            }
            else
            {
                panel_AddDes.text = string.Format(status.skillStates[skillNumber].skill_AddDescription, arg, arg1);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panel.gameObject.SetActive(false);
    }
}
