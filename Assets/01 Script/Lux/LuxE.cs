using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuxE : MonoBehaviour
{
    private PhotonView pv;
    public GameObject Lux;
    private Vector3 originPos;
    private Vector3 destination;
    private Vector3 range;
    public float damage;
    public float slowper;
    private float currentTIme;
    private bool isDestination;

    [SerializeField]
    private GameObject slowRange;
    [SerializeField]
    private GameObject explosionRange;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            this.enabled = false;
        }
        Lux = GameObject.Find("Lux");
        originPos = this.transform.position;
        destination = new Vector3(Lux.GetComponent<Lux_Controller>().HitInfo.point.x, originPos.y, Lux.GetComponent<Lux_Controller>().HitInfo.point.z);
        range = Lux.GetComponent<Lux_Controller>().chTransform.forward * (Lux.GetComponent<Lux_Controller>().LuxERange ) + originPos;
        range = new Vector3(range.x, originPos.y, range.z);
        damage = 15f + (Lux.GetComponent<Status>().skillsLevel[3] * 45f) + (Lux.GetComponent<Status>().spell * 0.6f);
        slowper = 20f + Lux.GetComponent<Status>().skillsLevel[3] * 5f;
        currentTIme = 0f;
        
        this.transform.localScale = new Vector3(Lux.GetComponent<Lux_Controller>().LuxERange_OnMouse / 2f, 1f, Lux.GetComponent<Lux_Controller>().LuxERange_OnMouse / 2f);
    }

    // Update is called once per frame
    void Update()
    {
        currentTIme += Time.deltaTime * 0.75f;
        if (!isDestination)
        {
            this.transform.position = Vector3.Lerp(originPos, range, currentTIme);
            if (Vector3.Distance(destination, this.transform.position) <= 0.3f)
            {
                isDestination = true;
                slowRange.gameObject.SetActive(true);
                currentTIme = 0f;
            }
            else if (currentTIme >= 1f)
            {
                isDestination = true;
                slowRange.gameObject.SetActive(true);
                currentTIme = 0f;
            }
        }
        else if (isDestination)
        {
            if (currentTIme >= 5f || Input.GetKey(KeyCode.E))
            {
                Lux.GetComponent<Lux_Controller>().isSkill[2] = false;
                slowRange.gameObject.SetActive(false);
                explosionRange.gameObject.SetActive(true);
            }
        }
    }
}
