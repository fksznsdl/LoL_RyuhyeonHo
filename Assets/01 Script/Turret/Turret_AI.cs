using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Turret_AI : MonoBehaviourPunCallbacks
{
    private PhotonView pv;
    public Status status;
    public float sightRange;
    [SerializeField]
    private GameObject FoundTarget;
    [SerializeField]
    private GameObject SearchZone;
    [SerializeField]
    private GameObject CheckZone;
    [SerializeField]
    private string attackEffect;
    public Collider target;

    private bool isAttack;

    private float attackCoolTime;
    private float currentAttackCoolTime;

    private IEnumerator coroutine;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        status = GetComponent<Status>();
        sightRange = status.attackRange;
        FoundTarget.transform.localScale = new Vector3(sightRange / 2f, sightRange / 2f, 1f);
        FoundTarget.gameObject.SetActive(false);
        if (pv.IsMine)
        {
            CheckZone.gameObject.SetActive(true);
            SearchZone.gameObject.SetActive(true);
        }
        target = null;
        isAttack = false;

        attackCoolTime = 0f;
        currentAttackCoolTime = 0f;
    }
    private void Update()
    {
        if (pv.IsMine)
        {
            currentAttackCoolTime += Time.deltaTime;
            if (!isAttack)
            {
                if (target != null)
                {
                    if (target.GetComponent<Status>().isDead)
                    {
                        ResetTarget();
                        return;
                    }
                    if (Vector3.Distance(this.transform.position, target.transform.position) > sightRange)
                    {
                        ResetTarget();
                        return;
                    }
                    TryAttack();
                }
            }
        }
    }
    private void ResetTarget()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        FoundTarget.gameObject.SetActive(false);
        isAttack = false;
        target = null;
        SearchZone.gameObject.SetActive(true);
    }
    private void TryAttack()
    {
        attackCoolTime = 1f / status.attackSpeed;
        coroutine = AttackCouroutine();
        StartCoroutine(coroutine);
    }
    public void ChangeTarget(Collider other)
    {
        target = other;
        if (isAttack)
        {
            isAttack = false;
            StopCoroutine(coroutine);
        }
    } 

    private IEnumerator AttackCouroutine()
    {
        if (currentAttackCoolTime >= attackCoolTime)
        {
            isAttack = true;
            var clone = PhotonNetwork.Instantiate(attackEffect,this.transform.position,Quaternion.identity);
            clone.GetComponent<TurretAttack>().turret_AI = this;
            currentAttackCoolTime = 0f;
            isAttack = false;
            coroutine = null;
        }
        else
        {
            yield return new WaitForSeconds(attackCoolTime - currentAttackCoolTime);
            StartCoroutine(AttackCouroutine());
        }
    }
    public void GetTarget(Collider other)
    {
        target = other;
        FoundTarget.gameObject.SetActive(true);
    }

}
