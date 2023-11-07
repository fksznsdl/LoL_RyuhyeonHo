using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionWizard_Attack : MonoBehaviour,IPunObservable
{
    private PhotonView pv;
    public Minion_AI minion;
    private Vector3 originPos;
    private Vector3 targetPos;
    private float currentTime;
    private bool isReady;
    [SerializeField]
    private GameObject model;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        isReady = false;
    }

    public void Initialize(Minion_AI ai, Vector3 pos)
    {
        minion = ai;
        originPos = ai.chTransform.position;
        if (minion.target == null)
        {
            string team = null;
            if (minion.status.team == 2)
            {
                team = "Blue";
            }
            else
            {
                team = "Red";
            }
            ObjectPoolManager.ReturnObject(this, team);
        }
        targetPos = minion.target.transform.position;
        SetPos(pos,targetPos);
        currentTime = 0f;
        Show(true);
    }

    private void Update()
    {
        if (pv.IsMine && isReady)
        {
            if (minion.target == null || minion.target.GetComponent<Status>().currentHp <= 0f)
            {
                isReady = false;
                string team = null;
                if (minion.status.team == 2)
                {
                    team = "Blue";
                }
                else
                {
                    team = "Red";
                }
                ObjectPoolManager.ReturnObject(this, team);
                return;
            }
            targetPos = minion.target.transform.position;
            currentTime += Time.deltaTime;
            this.transform.position = Vector3.Lerp(originPos, targetPos, currentTime);
            if (Vector3.Distance(this.transform.position, targetPos) <= 0.1f)
            {
                isReady = false;
                minion.target.GetComponent<Status>().Damage(minion.status.attackDamage);
                string team = null;
                if (minion.status.team == 2)
                {
                    team = "Blue";
                }
                else
                {
                    team = "Red";
                }
                ObjectPoolManager.ReturnObject(this, team);
                return;
            }
        }
        else if(!pv.IsMine && isReady)
        {
            currentTime += Time.deltaTime;
            if (currentTime <= 1f)
            {
                this.transform.position = Vector3.Lerp(originPos, targetPos, currentTime);
            }
        }

    }
    public void SetPos(Vector3 pos,Vector3 _targetPos)
    {
        pv.RPC("RSetPos", RpcTarget.All, pos,_targetPos);
    }
    [PunRPC]
    public void RSetPos(Vector3 pos, Vector3 _targetPos)
    {
        this.transform.position = pos;
        if (!pv.IsMine)
        {
            currentTime =0f;
            originPos = pos;
            targetPos = _targetPos;
        }
    }
    public void Show(bool a)
    {
        pv.RPC("RShow", RpcTarget.All, a);
    }
    [PunRPC]
    public void RShow(bool a)
    {
        if (a)
        {
            isReady = true;
        }
        else if (!a)
        {
            isReady = false;
        }
        model.gameObject.SetActive(a);
    }
    public void SetParent(bool a)
    {
        pv.RPC("RSetParent", RpcTarget.All, a);
    }
    [PunRPC]
    public void RSetParent(bool a)
    {
        if (a)
        {
            this.transform.SetParent(ObjectPoolManager.Instance.gameObject.transform);
        }
        else if (!a)
        {
            this.transform.SetParent(null);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(targetPos);
        }
        else
        {
            targetPos = (Vector3)stream.ReceiveNext();
        }
    }
}