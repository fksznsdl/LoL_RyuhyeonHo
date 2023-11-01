using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JannaR : MonoBehaviour
{
    public PhotonView pv;
    public GameObject Janna;
    public float heal;
    public float healTime;
    public float currentTime;

    public float KnockBackForce;

    private bool isKnockBackZone;

    [SerializeField]
    private GameObject KnockBackZone;
    [SerializeField]
    private GameObject healZone; 
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if(!pv.IsMine)
            this.enabled= false;
        Janna = GameObject.Find("Janna");
        this.transform.localScale = new Vector3(Janna.GetComponent<Janna_Controller>().JannaR_Range / 2f, 1f, Janna.GetComponent<Janna_Controller>().JannaR_Range / 2f);
        heal = 50f + (Janna.GetComponent<Status>().skillsLevel[4] * 50f) + (Janna.GetComponent<Status>().spell * 0.5f);
        healTime = 3f;
        currentTime = 0f;
        KnockBackForce = 875f / 60f;
        isKnockBackZone = true;
    }
    public void Update()
    {
        currentTime += Time.deltaTime;
        healTime -= Time.deltaTime;
        if(currentTime >= 0.1f &&isKnockBackZone)
        {
            KnockBackZone.gameObject.SetActive(false);
            isKnockBackZone = false;
        }
        if(currentTime >= 1f)
        {
            healZone.gameObject.SetActive(true);
            currentTime = 0f;
        }
        if (pv.IsMine)
        {
            if (healTime <= 0f || Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}

