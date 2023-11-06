using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuxW : MonoBehaviour
{
    private PhotonView pv;
    private SphereCollider col;
    private GameObject Lux;
    private float varrier;
    private float varrierTime;
    private string varrierName;
    private float currentTime;
    private Vector3 originPos;
    private Vector3 destination;

    private bool isTurnPoint;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<SphereCollider>();
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            col.enabled = false;
            this.enabled = false;
        }
        else
        {
            Lux = GameObject.Find("Lux");
            varrier = 25f + (Lux.GetComponent<Status>().skillsLevel[2] * 25f) + (Lux.GetComponent<Status>().spell * 0.3f);
            varrierName = "프리즘 보호막";
            currentTime = 0f;
            originPos = this.transform.position;
            destination = Lux.GetComponent<Lux_Controller>().LuxWRange * Lux.GetComponent<Lux_Controller>().chTransform.forward + originPos;
            isTurnPoint = false;
            varrierTime = 2.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            currentTime += Time.deltaTime;
            this.transform.Rotate(new Vector3(this.transform.rotation.x, this.transform.rotation.y + 1f, this.transform.rotation.y));
            if (isTurnPoint)
            {
                this.transform.position = Vector3.Lerp(destination, Lux.GetComponent<Lux_Controller>().chTransform.position, currentTime);
                if (Vector3.Distance(this.transform.position, Lux.GetComponent<Lux_Controller>().chTransform.position) <= 0.1f)
                {
                    PhotonNetwork.Destroy(this.gameObject);
                }
            }
            else
            {
                this.transform.position = Vector3.Lerp(originPos, destination, currentTime);
            }
            if (Vector3.Distance(this.transform.position, destination) <= 0.1f && !isTurnPoint)
            {
                currentTime = 0f;
                isTurnPoint = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        pv = GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            if (other.transform.tag == "Champion")
            {
                if (other.GetComponent<Status>().team == Lux.GetComponent<Status>().team)
                {
                    other.GetComponent<Status>().GetBarrier(varrierName, varrier, varrierTime);
                }
            }
        }
        else if (!pv.IsMine)
        {
            GetComponent<SphereCollider>().enabled = false;
            this.enabled = false;
        }
    }
}
