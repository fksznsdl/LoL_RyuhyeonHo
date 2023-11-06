using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Status_UI : MonoBehaviour
{
    public Status status;
    [SerializeField]
    private GameObject deadPanel;
    [SerializeField]
    private GameObject getGoldPanel;
    [SerializeField]
    private GameObject statusCCPanel;
    [SerializeField]
    private Image[] status_gauge;
    [SerializeField]
    private Text[] status_txt;
    [SerializeField]
    private Text level;
    [SerializeField]
    private Text gold;
    [SerializeField]
    private Text killMinion;
    [SerializeField]
    private Text[] skill_CoolTimes;
    [SerializeField]
    private GameObject[] skillLevelUpButtons;
    [SerializeField]
    private GameObject skillPanel;
    [SerializeField]
    private Image champIcon;
    [SerializeField]
    private Sprite[] champIcons;
    private const int HP = 0, DP = 1, MP = 2, AD = 3, AP = 4, ADS = 5, CRI = 6, ADR = 7, MVSP = 8,
        AM = 9, SB = 10, ADP = 11, APP = 12, ADV = 13, SPA = 14;

    private bool isReady;
    private void Start()
    {
        isReady = false;
        StartCoroutine(StayCoroutine());
    }
    IEnumerator StayCoroutine()
    {
        yield return new WaitForSeconds(3f);
        string name = GameObject.Find("PlayerSetting").GetComponent<PlayerSetting>().champName;
        status = GameObject.Find(name).GetComponent<Status>();
        UpdateChampIcon(name);
        yield return new WaitForSeconds(1f);
        isReady = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            UpdateGauge();
            UpdateText();
            UpdateSkill_CoolTime();
        }
    }
    private void UpdateChampIcon(string name)
    {
        switch (name)
        {
            case "Veiga":
                champIcon.sprite = champIcons[0];
                break;
            case "Anivia":
                champIcon.sprite = champIcons[1];
                break;
            case "Lux":
                champIcon.sprite = champIcons[2];
                break;
            case "Janna":
                champIcon.sprite = champIcons[3];
                break;
        }
    }
    public void Dead()
    {
        InGameManager inGM = GameObject.Find("InGameManager").GetComponent<InGameManager>();
        deadPanel.gameObject.SetActive(true);
        float respawnTime = (float)inGM.ingameMin;
        if(status.team == (int)Status.Team.RED)
        {
            inGM.RedTeamDead();
        }
        else
        {
            inGM.BlueTeamDead();

        }
        StartCoroutine(UpdateRespawnTimeTextCoroutine(respawnTime));
    }
    private IEnumerator UpdateRespawnTimeTextCoroutine(float respawnTime)
    {
        Text txt = deadPanel.transform.Find("DeadText").GetComponent<Text>();
        while (respawnTime >= 0f)
        {
            respawnTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
            txt.text = (Mathf.FloorToInt(respawnTime)).ToString();
        }
        deadPanel.gameObject.SetActive(false);
        status.Respawn();
    }
    public void OffSkillLevelUpButton()
    {
        for (int i = 1; i <= 4; i++)
        {
            skillLevelUpButtons[i].gameObject.SetActive(false);
        }
        skillPanel.gameObject.SetActive(false);
    }
    public void OnSkillLevelUpButton()
    {
        for (int i = 1; i <= 4; i++)
        {
            if (i<= 3&& status.skillsLevel[i] <=4)
            {
                skillLevelUpButtons[i].gameObject.SetActive(true);
            }
            else if (status.level >= ((status.skillsLevel[i]+1)*5+1) && status.skillsLevel[i]<=2) 
            {
                skillLevelUpButtons[i].gameObject.SetActive(true);
            }
        }

    }
    private void UpdateSkill_CoolTime()
    {
        if (status.skillCoolTimes[0] > 0f)
        {
            skill_CoolTimes[0].gameObject.SetActive(true);
            skill_CoolTimes[0].text = (Mathf.FloorToInt(status.skillCoolTimes[0])).ToString();
        }
        else
        {
            skill_CoolTimes[0].gameObject.SetActive(false);
        }
        if (status.skillCoolTimes[1] > 0f)
        {
            skill_CoolTimes[1].gameObject.SetActive(true);
            skill_CoolTimes[1].text = (Mathf.FloorToInt(status.skillCoolTimes[1])).ToString();
        }
        else
        {
            skill_CoolTimes[1].gameObject.SetActive(false);
        }
        if (status.skillCoolTimes[2] > 0f)
        {
            skill_CoolTimes[2].gameObject.SetActive(true);
            skill_CoolTimes[2].text = (Mathf.FloorToInt(status.skillCoolTimes[2])).ToString();
        }
        else
        {
            skill_CoolTimes[2].gameObject.SetActive(false);
        }
        if (status.skillCoolTimes[3] > 0f)
        {
            skill_CoolTimes[3].gameObject.SetActive(true);
            skill_CoolTimes[3].text = (Mathf.FloorToInt(status.skillCoolTimes[3])).ToString();
        }
        else
        {
            skill_CoolTimes[3].gameObject.SetActive(false);
        }
        if (status.skillCoolTimes[4] > 0f)
        {
            skill_CoolTimes[4].gameObject.SetActive(true);
            skill_CoolTimes[4].text = (Mathf.FloorToInt(status.skillCoolTimes[4])).ToString();
        }
        else
        {
            skill_CoolTimes[4].gameObject.SetActive(false);
        }
    }
    private void UpdateText()
    {
        status_txt[HP].text = status.currentHp.ToString() + " / " + status.maxHp.ToString() + " + " + (status.hpRegen / 5f).ToString();
        if (status.currentbarrier > 0f)
        {
            status_txt[HP].text = status.currentHp.ToString() + " / " + status.maxHp.ToString() + " + " + (status.hpRegen / 5f).ToString() +" + (" + status.currentbarrier.ToString() + ")";
        }
        status_txt[MP].text = status.currentMp.ToString() + " / " + status.maxMp.ToString() + " + " + (status.mpRegen / 5f).ToString();
        status_txt[AD].text = status.attackDamage.ToString();
        status_txt[AP].text = status.spell.ToString();
        status_txt[ADS].text = status.attackSpeed.ToString();
        status_txt[CRI].text = status.crit.ToString();
        status_txt[ADR].text = status.attackRange.ToString();
        status_txt[MVSP].text = status.currentMoveSpeed.ToString();
        status_txt[AM].text = status.armor.ToString();
        status_txt[SB].text = status.spellBlock.ToString();
        status_txt[ADP].text = status.ADpenetration.ToString() + "|" + status.ADperPenetration.ToString();
        status_txt[APP].text = status.APpenetration.ToString() + "|" + status.APperPenetration.ToString();
        status_txt[ADV].text = status.adVamp.ToString();
        status_txt[SPA].text = status.spellAcceleration.ToString();
        level.text = status.level.ToString();
        gold.text = Mathf.FloorToInt(status.currentGold).ToString();
        killMinion.text = status.currentKillMinion.ToString();
    }

    private void UpdateGauge()
    {
        status_gauge[HP].fillAmount = status.currentHp / status.maxHp;
        status_gauge[MP].fillAmount = status.currentMp / status.maxMp;
        status_gauge[DP].fillAmount = status.currentbarrier / status.currentHp;
    }

    public void ShowGetGoldUI(float _gold)
    {
        StartCoroutine(StayCoroutine(_gold));
    }

    private IEnumerator StayCoroutine(float _killGold)
    {
        getGoldPanel.gameObject.SetActive(true);
        getGoldPanel.GetComponent<Text>().text = "»πµÊ«— ∞ÒµÂ : " + _killGold.ToString();
        yield return new WaitForSeconds(1.0f);
        getGoldPanel.gameObject.SetActive(false);
    }
    public void GetCC(string _cc)
    {
        statusCCPanel.gameObject.SetActive(true);
        statusCCPanel.GetComponent<Text>().text = _cc;
    }
    public void ResetCC()
    {
        statusCCPanel.gameObject.SetActive(false);
    }

}
