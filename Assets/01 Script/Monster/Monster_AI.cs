using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster_AI : MonoBehaviour
{
    protected PhotonView pv;
    public Status status;
    protected NavMeshAgent nav_agent;
    [SerializeField]
    protected Animator anim;
    [SerializeField]
    protected Transform chTransform;
    [SerializeField]
    protected int monsterNumber;
    protected bool dead;

    public bool isAttack;
    protected bool isOriginPos;

    protected float currentAttackCoolTime;
    protected float attackCoolTime;
    protected float currentAnimTime;

    protected float aggroTime;

    public GameObject target;

    protected Vector3 originPos;

    protected float originDistance;
    protected float distance;
    protected virtual void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
            this.enabled = false;
        status = GetComponent<Status>();
        nav_agent = GetComponent<NavMeshAgent>();
        nav_agent.acceleration = 1000f;
        isAttack = false;
        target = null;
        originPos = this.transform.position;
        isOriginPos = true;
        currentAttackCoolTime = 0f;
        dead = false;
        UpdateLevel();
    }

    protected virtual void UpdateLevel()
    {
        float IngameTime = GameObject.Find("InGameManager").GetComponent<InGameManager>().ingameTime;
        int level = Mathf.FloorToInt((IngameTime - 89f) / 60f);
        if (level >= 1)
        {
            if (level >= 18)
            {
                level = 18;
            }
            for (int i = 0; i < level; i++)
            {
                status.LevelUp();
            }
        }
    }

    protected virtual void Update()
    {
        currentAttackCoolTime += Time.deltaTime;
        if (status.isDead && !dead)
        {
            IsDead();
        }
        if(!isAttack && !status.isCC && !status.isAirborne){
            if(target != null)
            {
                aggroTime += Time.deltaTime;
                originDistance = Vector3.Distance(this.transform.position, originPos);
                if (target.GetComponent<Status>().isDead || originDistance>20f)
                {
                    RemoveTarget();
                    GoToOriginPos();
                    return;
                }
                distance = Vector3.Distance(this.transform.position, target.transform.position);
                if(distance <= status.attackRange)
                {
                    TryAttack();
                }
                else if(distance <= status.sightRange || aggroTime <5f)
                {
                    GoToTarget();
                }
                else if(distance > status.sightRange && aggroTime >= 5f)
                {
                    RemoveTarget();
                    GoToOriginPos();
                    return;
                }
            }
            else if (status.isbeHit)
            {
                target = status.beHitTarget;
                status.isbeHit = false;
                status.beHitTarget = null;
            }
            else if (!isOriginPos)
            {
                distance = Vector3.Distance(this.transform.position, originPos);
                if (distance <= 0.5f)
                {
                    isOriginPos = true;
                    MoveCancel();
                }
            }
        }
    }
    protected void TryAttack()
    {
        MoveCancel();
        attackCoolTime = 1f / status.attackSpeed;
        if (currentAttackCoolTime < attackCoolTime)
        {
            return;
        }
        isAttack = true;
        StartCoroutine(AttackCouroutine());
    }
    protected virtual IEnumerator AttackCouroutine()
    {
            Look(target.transform.position);
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(0.1f);
            currentAnimTime = anim.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(currentAnimTime-0.1f);
            target.GetComponent<Status>().Damage(status.attackDamage);
            isAttack = false;
            currentAttackCoolTime = 0f;
    }
    protected virtual void MoveCancel()
    {
        nav_agent.isStopped = true;
        nav_agent.velocity = Vector3.zero;
        anim.SetBool("Run", false);
    }
    protected void IsDead()
    {
        dead = true;
        GameObject.Find("InGameManager").transform.GetComponent<InGameManager>().SetSpawnTime(monsterNumber);
    }
    protected virtual void GoToOriginPos()
    {
        isOriginPos = false;
        Look(originPos);
        nav_agent.isStopped = false;
        nav_agent.speed = status.currentMoveSpeed;
        anim.SetBool("Run", true);
        nav_agent.SetDestination(originPos);
    }
    protected virtual void GoToTarget()
    {
        Look(target.transform.position);
        nav_agent.isStopped = false;
        nav_agent.speed = status.currentMoveSpeed;
        anim.SetBool("Run", true);
        nav_agent.SetDestination(target.transform.position);
    }
    protected  void Look(Vector3 point)
    {
        chTransform.forward = point - chTransform.position;
    }
    protected void RemoveTarget()
    {
        target = null;
        aggroTime = 0f;
    }
}
