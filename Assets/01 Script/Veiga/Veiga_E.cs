using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SearchService;

public class Veiga_E : MonoBehaviour
{
    private PhotonView pv;
    private GameObject Veiga;

    private Status status;
    private List<Collider> targets;
    private float currentDuration;
    private float ccRange;
    private bool isOn;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            GetComponent<SphereCollider>().enabled = false;
            this.enabled = false;
            
        }
        isOn = false;
        currentDuration = 0f;
        Veiga = GameObject.Find("Veiga");
        status = Veiga.GetComponent<Status>();
        ccRange = Veiga.GetComponent<Veiga_Controller>().VeigaERange_OnMouse;
        targets = new List<Collider>();
        this.transform.localScale = new Vector3(Veiga.GetComponent<Veiga_Controller>().VeigaERange_OnMouse / 2f, Veiga.GetComponent<Veiga_Controller>().VeigaERange_OnMouse / 2f, Veiga.GetComponent<Veiga_Controller>().VeigaERange_OnMouse / 2f);
    }

    // Update is called once per frame
    void Update()
    {
        currentDuration += Time.deltaTime;
        if(currentDuration >= 0.5f && !isOn)
        {
            OnCyllinders();
        }
        if(currentDuration >= 3f)
        {
           PhotonNetwork.Destroy(this.gameObject);
        }
    }
    private void OnCyllinders()
    {
        isOn = true;
        if (pv.IsMine)
        {
            this.transform.GetComponent<SphereCollider>().enabled = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Champion" || other.transform.tag == "Minion" || other.transform.tag == "Monster")
        {
            if(other.GetComponent<Status>().team != Veiga.GetComponent<Status>().team)
            {
                if (Vector3.Distance(other.transform.position, this.transform.position) <= ccRange-1.1f)
                {
                    return;
                }
                else
                {
                    if (targets.Count >= 1)
                    {
                        foreach(var item in targets)
                        {
                            if(other == item)
                            {
                                return;
                            }
                        }
                    }
                    DamageAndDebuffSkillState skillState = (DamageAndDebuffSkillState)status.skillStates[3];
                    other.GetComponent<Status>().HitDebuff(skillState.skill_Name, skillState.skill_CCTime + (skillState.skill_levelCCTime * status.skillsLevel[3]),
                        (int)Debuff.DebuffType.CC);
                    targets.Add(other);
                }
            }
        }
        
    }
}
