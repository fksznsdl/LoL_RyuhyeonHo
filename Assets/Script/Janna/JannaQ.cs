using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JannaQ : MonoBehaviour
{
    public PhotonView pv;
    public GameObject Janna;
    private Vector3 originPos;
    private Vector3 originRot;
    private Vector3 destination;
    private float currentTime;
    private int currentsecond;
    public float damage;
    private float secondDamage;
    public string debuffName;
    public int debuffType;
    public float CCTime;

    private bool isInputQ;

    [SerializeField]
    private GameObject HitZone;
    [SerializeField]
    private GameObject model;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            this.enabled = false;
        }
        Janna = GameObject.Find("Janna");
        originPos = this.transform.position;
        originRot = Janna.GetComponent<Janna_Controller>().chTransform.forward;
        destination = originRot * Janna.GetComponent<Janna_Controller>().JannaQRange + originPos;
        currentTime = 0f;
        damage = 35f + (25f * Janna.GetComponent<Status>().skillsLevel[1]) + (Janna.GetComponent<Status>().spell * 0.35f);
        secondDamage = 10f + (5f * Janna.GetComponent<Status>().skillsLevel[1]) + (Janna.GetComponent<Status>().spell * 0.1f);
        debuffName = "¿ïºÎÂ¢´Â µ¹Ç³";
        debuffType = (int)Debuff.DebuffType.AIRBORNE;
        CCTime = 0.5f;
        currentsecond = 0;
        isInputQ = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        this.transform.Rotate(new Vector3(this.transform.rotation.x, this.transform.rotation.y + 1f, this.transform.rotation.z));
        if (isInputQ)
        {
            this.transform.position = Vector3.Lerp(originPos, destination, currentTime);
            if (Vector3.Distance(this.transform.position, destination) <= 0.1f && pv.IsMine)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
        if(currentTime >=1f && currentsecond == 0f &&!isInputQ)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x+0.25f, this.transform.localScale.y, this.transform.localScale.z+0.25f);
            destination = (originRot * (Janna.GetComponent<Janna_Controller>().JannaQRange+3.6f)) + originPos;
            damage += secondDamage;
            CCTime += 0.25f;
            currentsecond += 1;
        }
        else if (currentTime >= 2f && currentsecond == 1f && !isInputQ)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x + 0.25f, this.transform.localScale.y, this.transform.localScale.z + 0.25f);
            destination = (originRot * (Janna.GetComponent<Janna_Controller>().JannaQRange + 7.2f)) + originPos;
            damage += secondDamage;
            CCTime += 0.25f;
            currentsecond += 1;
        }
        else if (currentTime >= 3f && currentsecond == 2f && !isInputQ)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x + 0.25f, this.transform.localScale.y, this.transform.localScale.z + 0.25f);
            destination = (originRot * (Janna.GetComponent<Janna_Controller>().JannaQRange + 10.8f)) + originPos;
            damage += secondDamage;
            CCTime += 0.25f;
            currentsecond += 1;
        }
        if ((Input.GetKeyUp(KeyCode.Q) || currentsecond >=3) && !isInputQ)
        {
            Janna.GetComponent<Janna_Controller>().isSkill[0] = false;
            HitZone.gameObject.SetActive(true);
            isInputQ = true;
            currentsecond = 0;
            currentTime = 0f;
        }
    }
}
