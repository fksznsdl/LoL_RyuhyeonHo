using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuxR : MonoBehaviour
{

    private PhotonView pv;
    public GameObject Lux;
    public float damage;
    private float currentTime;
    [SerializeField]
    private GameObject LuxR_Explosion;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            this.enabled = false;
        }
        Lux = GameObject.Find("Lux");
        damage = 200f + (Lux.GetComponent<Status>().skillsLevel[4] * 100f) + (Lux.GetComponent<Status>().spell * 1.2f);
        currentTime = 0f;
        this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, Lux.GetComponent<Lux_Controller>().LuxR_Range);
        this.transform.forward = Lux.GetComponent<Lux_Controller>().chTransform.forward;
        this.transform.position += (Lux.GetComponent<Lux_Controller>().chTransform.forward * (Lux.GetComponent<Lux_Controller>().LuxR_Range/2f));


    }

    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= 1f)
        {
            LuxR_Explosion.gameObject.SetActive(true);
        }
        if(currentTime >= 1.2f)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
