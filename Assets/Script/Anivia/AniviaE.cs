using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniviaE : MonoBehaviour
{
    private GameObject Anivia;
    private PhotonView pv;
    private Vector3 originPos;
    private Vector3 movePos;
    private float currentTime;
    private RaycastHit hitInfo;
    private float damage;
    // Start is called before the first frame update

    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            this.enabled = false;
        }
        Anivia = GameObject.Find("Anivia");
        hitInfo = Anivia.GetComponent<Anivia_Controller>().HitInfo;
        this.transform.forward = hitInfo.point - this.transform.position;
        originPos = this.transform.position;
        movePos = hitInfo.point;
    }
    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime * 1.5f;
        this.transform.position = Vector3.Lerp(originPos, movePos, currentTime);
        if (Vector3.Distance(movePos, this.transform.position) <= 1f)
        {
            if (hitInfo.transform.GetComponent<Status>().isHitAniviaQR)
            {
                damage = ((Anivia.GetComponent<Status>().skillsLevel[3] * 25f) + 25f +(Anivia.GetComponent<Status>().spell*0.5f)) * 2f;
            }
            else
            {
                damage = ((Anivia.GetComponent<Status>().skillsLevel[3] * 25f) + 25f + (Anivia.GetComponent<Status>().spell * 0.5f));
            }
            hitInfo.transform.GetComponent<Status>().Damage(damage,Anivia.transform.name,Status.AttackType.AP, Anivia.GetComponent<Status>().APperPenetration, Anivia.GetComponent<Status>().APpenetration);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
