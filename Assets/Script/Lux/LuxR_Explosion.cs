using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuxR_Explosion : MonoBehaviour
{
    [SerializeField]
    private PhotonView pv;
    private LuxR LuxR;
    private BoxCollider col;
    void Start()
    {
        col = GetComponent<BoxCollider>();
        if (!pv.IsMine)
        {
            col.enabled = false;
            this.enabled = false;
        }
        LuxR = GetComponentInParent<LuxR>();  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pv.IsMine)
        {
            if (other.transform.tag == "Champion" || other.transform.tag == "Minion" || other.transform.tag == "Monster")
            {
                if (other.GetComponent<Status>().team != LuxR.Lux.GetComponent<Status>().team)
                {
                    other.GetComponent<Status>().Damage(LuxR.damage, LuxR.Lux.transform.name, Status.AttackType.AP, LuxR.Lux.GetComponent<Status>().APperPenetration, LuxR.Lux.GetComponent<Status>().APpenetration);
                    if (other.GetComponent<Status>().currentHp >= 0f)
                    {
                        LuxR.Lux.GetComponent<Lux_Controller>().HitPassive(other);
                        LuxR.Lux.GetComponent<Lux_Controller>().Passive(other);
                    }
                }
            }
        }
    }
}
