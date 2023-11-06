using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuxE_Explosion : MonoBehaviour
{
    [SerializeField]
    private PhotonView pv;
    [SerializeField]
    private GameObject model;
    [SerializeField]
    private GameObject LuxE;
    private LuxE luxE;

    private float currentTime;

    private void OnEnable()
    {
        if (!pv.IsMine)
        {
            this.enabled = false; return;
        }
        luxE = LuxE.GetComponent<LuxE>();
        currentTime = 0f;
        model.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!pv.IsMine)
        {
            GetComponent<SphereCollider>().enabled = false; return;
        }
        if (pv.IsMine)
        {
            if (other.transform.tag == "Champion" || other.transform.tag == "Minion" || other.transform.tag == "Monster")
            {
                if (other.GetComponent<Status>().team != luxE.Lux.GetComponent<Status>().team)
                {
                    other.GetComponent<Status>().Damage(luxE.damage, luxE.Lux.transform.name, Status.AttackType.AP, luxE.Lux.GetComponent<Status>().APperPenetration, luxE.Lux.GetComponent<Status>().APpenetration);
                    luxE.Lux.GetComponent<Lux_Controller>().Passive(other);
                    other.GetComponent<Status>().currentMoveSpeed = other.GetComponent<Status>().originMoveSpeed;
                }
            }
        }
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= 0.3f)
        {
            PhotonNetwork.Destroy(this.transform.parent.gameObject);
        }
    }
}
