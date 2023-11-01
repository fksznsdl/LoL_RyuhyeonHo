using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniviaR : MonoBehaviour
{
    private GameObject Anivia;
    private SphereCollider col;
    private PhotonView pv;
    private float damage;
    private float slowper;
    private float completionTime;
    private float currentTime;
    private float currentTriggerTime;
    private float currentManaTime;
    private string debuffname;
    private Vector3 completionScale;
    private Vector3 originScale;
    private bool iscomplete;
    private Status status;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        col = GetComponent<SphereCollider>();
        if (!pv.IsMine)
        {
            col.enabled = false;
            this.enabled = false;
            return;
        }
        currentManaTime = 0f;
        Anivia = GameObject.Find("Anivia");
        status = Anivia.GetComponent<Status>();
        debuffname = "¾óÀ½ ÆøÇ³";
        completionTime = 1.5f;
        completionScale = new Vector3(Anivia.GetComponent<Anivia_Controller>().AniviaR_Range_OnMouse / 2f, 1f, Anivia.GetComponent<Anivia_Controller>().AniviaR_Range_OnMouse / 2f);
        originScale = this.transform.localScale;
        currentTime = 0f;currentTriggerTime = 0f;
        damage = (Anivia.GetComponent<Status>().skillsLevel[4] * 15f + 15f) + (Anivia.GetComponent<Status>().spell * 0.125f);
        slowper = Anivia.GetComponent<Status>().skillsLevel[4] * 10f + 10f;
        this.transform.position = new Vector3(this.transform.position.x, 0.1f, this.transform.position.z);
        iscomplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentTriggerTime += Time.deltaTime;
        currentManaTime += Time.deltaTime;
        if(currentManaTime >= 1f)
        {
            if(!col.enabled)
            col.enabled = true;
            currentManaTime = 0f;
            status.currentMp -= (status.skillsLevel[4] * 10 + 25);
        }
        if(currentTriggerTime >= 1.05f)
        {
            col.enabled = false;
            currentTriggerTime = 0f;
        }
        if (!iscomplete)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= completionTime)
            {
                iscomplete = true;
                currentTime = completionTime;
            }
            this.transform.localScale = Vector3.Lerp(originScale, completionScale, currentTime);
        }
        if(Input.GetKey(KeyCode.R)||status.currentMp < (status.skillsLevel[4]*10 + 25))
        {
            Anivia.GetComponent<Anivia_Controller>().isSkill[3] = false;
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GetComponent<PhotonView>().IsMine)
        {
            return;
        }
        else if (other.transform.tag == "Champion" || other.transform.tag == "Minion")
        {
            if (other.GetComponent<Status>().team != Anivia.GetComponent<Status>().team)
            {
                if (iscomplete)
                {
                    other.GetComponent<Status>().Damage(damage * 3f, Anivia.transform.name, Status.AttackType.AP, Anivia.GetComponent<Status>().APperPenetration, Anivia.GetComponent<Status>().APpenetration);
                    other.GetComponent<Status>().HitDebuff(debuffname, 1f, slowper * 1.5f, (int)Debuff.DebuffType.SLOW);
                }
                else
                {
                    other.GetComponent<Status>().Damage(damage, Anivia.transform.name, Status.AttackType.AP, Anivia.GetComponent<Status>().APperPenetration, Anivia.GetComponent<Status>().APpenetration);
                    other.GetComponent<Status>().HitDebuff(debuffname, 1f, slowper, (int)Debuff.DebuffType.SLOW);
                }

            }
        }
    }
}
