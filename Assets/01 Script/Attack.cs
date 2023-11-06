using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Attack : MonoBehaviour
{
    protected PhotonView pv;
    protected Vector3 orginPos;
    protected Vector3 targetPos;
    protected float currentTime;
    protected RaycastHit hitInfo;
    protected Status status;

    protected virtual void Start()
    {
        pv = GetComponent<PhotonView>();
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z);
        orginPos = this.transform.position;
    }
     protected virtual void Update()
    {
        if (pv.IsMine)
        {
            Status _status = hitInfo.transform.GetComponent<Status>();
            if (_status != null && _status.isDead)
            {
                PhotonNetwork.Destroy(this.gameObject);
                return;
            }
            currentTime += Time.deltaTime;
            Move();
        }
    }
    protected abstract void Move();
}
