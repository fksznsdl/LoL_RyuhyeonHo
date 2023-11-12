using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dragon_AI : Monster_AI
{

    private bool isFly;
    [SerializeField]
    private GameObject KnockBackZone;
    [SerializeField]
    private string AttackEffect;
    // Start is called before the first frame update
    protected override void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            this.enabled = false;
            return;
        }
        else {
            status = GetComponent<Status>();
            nav_agent = GetComponent<NavMeshAgent>();
            nav_agent.acceleration = 1000f;
            nav_agent.updateRotation = false;
            isAttack = false;
            target = null;
            originPos = this.transform.position;
            isOriginPos = true;
            currentAttackCoolTime = 0f;
            dead = false;
            isFly = false;
        }
    }

    protected override void Update()
    {
        currentAttackCoolTime += Time.deltaTime;
        UpdateLevel();
        if(status.isDead)
        {
            IsDead();
        }
        else if (!isAttack)
        {
            status.ResetSlow();
            if (target != null && isFly)
            {
                aggroTime += Time.deltaTime;
                originDistance = Vector3.Distance(this.transform.position, originPos);
                if (target.GetComponent<Status>().isDead || originDistance > 20f)
                {
                    RemoveTarget();
                    GoToOriginPos();
                    return;
                }
                distance = Vector3.Distance(this.transform.position, target.transform.position);
                if (distance <= status.attackRange)
                {
                    TryAttack();
                }
                else if (distance <= status.sightRange || aggroTime < 5f)
                {
                    GoToTarget();
                }
                else if (distance > status.sightRange && aggroTime >= 5f)
                {
                    RemoveTarget();
                    GoToOriginPos();
                    return;
                }
            }
            else if (status.isbeHit && !isFly)
            {
                target = status.beHitTarget;
                status.isbeHit = false;
                status.beHitTarget = null;
                StartCoroutine(FlyCoroutine());
            }
            else if (!isOriginPos && isFly)
            {
                distance = Vector3.Distance(this.transform.position, originPos);
                if (distance <= 1f)
                {
                    isOriginPos = true;
                    MoveCancel();
                    originState();
                }
            }
        }
    }
    protected override void UpdateLevel()
    {
        float IngameTime = GameObject.Find("InGameManager").GetComponent<InGameManager>().ingameTime;
        int level = Mathf.FloorToInt((IngameTime - 149f) / (60f*status.level));
        if (level >= 1 && status.level <18)
        {
            status.LevelUp();
        }
    }
    private void originState()
    {
        isFly = false;
        isOriginPos = true;
        anim.SetTrigger("Idle");
    }
    private IEnumerator FlyCoroutine()
    {
        anim.SetTrigger("Fly");
        KnockBackZone.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        currentAnimTime = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(0.9f);
        KnockBackZone.gameObject.SetActive(false);
        yield return new WaitForSeconds(currentAnimTime - 1f);
        isFly = true;
        if (target == null)
        {
            originState();
        }
    }

    protected override IEnumerator AttackCouroutine()
    {
        Look(target.transform.position);
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.1f);
        currentAnimTime = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(currentAnimTime - 0.1f);
        var clone = PhotonNetwork.Instantiate(AttackEffect, this.transform.position, Quaternion.identity);
        clone.GetComponent<DragonAttack>().dragon = this;
        isAttack = false;
        currentAttackCoolTime = 0f;
    }

}
