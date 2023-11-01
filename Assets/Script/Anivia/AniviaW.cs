using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniviaW : MonoBehaviour
{
    private PhotonView pv;
    private GameObject Anivia;
    private float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            this.enabled = false;
        }
        currentTime = 0f;
        Anivia = GameObject.Find("Anivia");
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position. y + 1f, this.transform.position.z);
        this.transform.localScale = new Vector3(Anivia.GetComponent<Anivia_Controller>().AniviaWRange_OnMouse / 2f, this.transform.localScale.y, this.transform.localScale.z);
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= 5f)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
