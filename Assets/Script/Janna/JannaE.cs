using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JannaE : MonoBehaviour
{
    private PhotonView pv;
    private GameObject Janna;
    private float barrierTime;
    private float decreaseTime;
    private float barrierFigure;
    private string barrierName;
    private float buffad;

    private float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            this.enabled = false;
            return;
        }
        else if (pv.IsMine)
        {
            Janna = GameObject.Find("Janna");
            pv = GetComponent<PhotonView>();
            this.transform.parent = Janna.GetComponent<Janna_Controller>().HitInfo.transform;
            this.transform.position = new Vector3(this.GetComponentInParent<Controller>().chTransform.position.x,
                this.GetComponentInParent<Controller>().chTransform.position.y + 2f, this.GetComponentInParent<Controller>().chTransform.position.z);
            barrierName = "ÆøÇ³ÀÇ ´«";
            barrierFigure = 50f + (Janna.GetComponent<Status>().skillsLevel[3] * 25f) + (Janna.GetComponent<Status>().spell * 0.55f);
            buffad = 5f + (Janna.GetComponent<Status>().skillsLevel[3] * 5f) + (Janna.GetComponent<Status>().spell * 0.1f);
            barrierTime = 4f;
            decreaseTime = 1.5f;
            currentTime = 0f;
            Buff();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= barrierTime)
            {
                GetComponentInParent<Status>().attackDamage -= buffad;
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
    private void Buff()
    {
        GetComponentInParent<Status>().attackDamage += buffad;
        GetComponentInParent<Status>().GetBarrier(barrierName, barrierFigure,barrierTime,decreaseTime,(int)Barrier.DecreaseType.DECREASETIME);
    }
}
