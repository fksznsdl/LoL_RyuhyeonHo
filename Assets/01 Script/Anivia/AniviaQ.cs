using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniviaQ : MonoBehaviour
{
    private PhotonView pv;
    public GameObject Anivia;
    public Status status;
    public float damage;
    public float explosionDamage;
    public float slowTime;
    public float CCTime;

    private Vector3 orginPoint;
    private Vector3 movePoint;

    private float range;
    private bool isInputQ;
    private float currentMoveTime;
    private float currentTime;
    [SerializeField]
    private GameObject skillQ;
    [SerializeField]
    private GameObject explosionQ;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            Anivia = GameObject.Find("Anivia");
            currentMoveTime = 0f;
            currentTime = 0f;
            slowTime = 3f;
            CCTime = 1f + (Anivia.GetComponent<Status>().skillsLevel[1] * 0.1f);
            isInputQ = false;
            status = Anivia.GetComponent<Status>();
            damage = 30f + (Anivia.GetComponent<Status>().skillsLevel[1] * 20f) + (Anivia.GetComponent<Status>().spell * 0.25f);
            explosionDamage = 35f + (Anivia.GetComponent<Status>().skillsLevel[1] * 25f) + (Anivia.GetComponent<Status>().spell * 0.45f);
            orginPoint = this.transform.position;
            movePoint = Anivia.GetComponent<Anivia_Controller>().chTransform.forward * Anivia.GetComponent<Anivia_Controller>().AniviaQRange + orginPoint;
            movePoint = new Vector3(movePoint.x, orginPoint.y, movePoint.z);
            range = Anivia.GetComponent<Anivia_Controller>().AniviaQRange;
        }
    }

    void Update()
    {
        if (pv.IsMine)
        {
            if (!isInputQ)
            {
                currentMoveTime += Time.deltaTime * 0.5f;
                this.transform.position = Vector3.Lerp(orginPoint, movePoint, currentMoveTime);
                if (Input.GetKeyDown(KeyCode.Q) || Vector3.Distance(this.transform.position, orginPoint) >= (range - 0.01f))
                {
                    skillQ.gameObject.SetActive(false);
                    explosionQ.gameObject.SetActive(true);
                    Anivia.GetComponent<Anivia_Controller>().isSkill[0] = false;
                    isInputQ = true;
                }
            }
            else if (isInputQ)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= 1f)
                {
                    PhotonNetwork.Destroy(this.gameObject);
                }
            }
        }
    }
}
