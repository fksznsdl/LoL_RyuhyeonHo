using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackAnim : MonoBehaviour
{
    private PhotonView pv;
    [SerializeField]
    private string attackeffect;
    private Controller controller;
    private void Start()
    {
        pv = GetComponentInParent<PhotonView>();
        controller = GetComponentInParent<Controller>();
    }

    public void InstantiateAttack()
    {
        if (pv.IsMine)
        {
            var clone = PhotonNetwork.Instantiate(attackeffect, this.transform.position, Quaternion.identity);
            controller.currentAttackCoolTime = 0f;
        }
    }
}
