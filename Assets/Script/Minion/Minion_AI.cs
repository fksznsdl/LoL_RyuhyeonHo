using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion_AI : MonoBehaviour
{
    private PhotonView pv;
    public Status status;
    private NavMeshAgent nav_agent;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Transform chTransform;
    [SerializeField]
    public float sightRange;
    [SerializeField]
    private GameObject sightObject;
    [SerializeField]
    private string attackEffect;
    
    public Collider target;

    private float currentAnimationTime;
    private float attackCoolTime;
    private float currentAttackCoolTime;

    private bool isAttack;
    private bool isMove;

    private float distance;
    [SerializeField]
    private Vector3[] destination;
    private int destinationNumber;
    private float IngameTime;
    private int level;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        nav_agent = GetComponent<NavMeshAgent>();
        if (!pv.IsMine)
        {
            
        }
        else
        {
            status = GetComponent<Status>();
            nav_agent.acceleration = 1000f;
            sightRange /= 60f;
            currentAttackCoolTime = 0f;
            isAttack = false;
            sightObject.gameObject.SetActive(true);
            destinationNumber = 0;
            UpdateStatus();
            isMove = false;
        }
    }
    private void Update()
    {
        if (pv.IsMine)
        {
            currentAttackCoolTime += Time.deltaTime;
            if (status.isDead)
            {
                GetComponent<BoxCollider>().enabled = false;
            }
            if (!isAttack && !status.isCC && !status.isAirborne && !status.isDead)
            {
                if (target != null)
                {
                    distance = Vector3.Distance(target.transform.position, this.transform.position);
                    if (distance <= sightRange)
                    {
                        if (distance <= status.attackRange)
                        {
                            TryAttack();
                        }
                        else
                        {
                            MoveTarget();
                        }
                    }
                    else
                    {
                        target = null;
                        SearchTarget();
                        Move();
                    }
                }
                else
                {
                    Move();
                }
            }
        }
    }
    private void UpdateStatus()
    {
        IngameTime = GameObject.Find("InGameManager").GetComponent<InGameManager>().ingameTime;
        level = Mathf.FloorToInt((IngameTime - 64f) / 90f);
        if (level >= 1)
        {
            if (level >= 23)
            {
                level = 23;
            }
            for (int i = 0; i < level; i++)
            {
                status.LevelUp();
            }
        }
    }
    private void MoveTarget()
    {
        nav_agent.isStopped = false;
        if (isMove == false)
        {
            isMove = true;
            pv.RPC("AnimSetBool", RpcTarget.All, "Run", true);
        }
        nav_agent.speed = status.currentMoveSpeed;
        Look(target.transform.position);
        nav_agent.SetDestination(target.transform.position);
    }
    private void Move()
    {
        Vector3 currentDestination;
        nav_agent.isStopped = false;
        if (isMove == false)
        {
            isMove = true;
            pv.RPC("AnimSetBool", RpcTarget.All, "Run", true);
        }

        nav_agent.speed = status.currentMoveSpeed;
        currentDestination = SerachDestination();
        Look(currentDestination);
        nav_agent.SetDestination(currentDestination);
    }
    private Vector3 SerachDestination()
    {
        if (Vector3.Distance(this.transform.position, destination[destinationNumber]) <= 3f)
        {
            if(destinationNumber == destination.Length)
            {
                return destination[destinationNumber];
            }
            destinationNumber++;
        }
        return destination[destinationNumber];
    }
    private void MoveCancel()
    {
        nav_agent.isStopped = true;
        nav_agent.velocity = Vector3.zero;
        if (isMove == true)
        {
            isMove = false;
            pv.RPC("AnimSetBool", RpcTarget.All, "Run", false);
        }
    }
    private void TryAttack()
    {
        isAttack = true;
        attackCoolTime = 1f / status.attackSpeed;
        MoveCancel();
        Look(target.transform.position);
        if (status.attackSpeed > 1f) pv.RPC("AnimSetFloat", RpcTarget.All, "AttackSpeed", status.attackSpeed);
        else pv.RPC("AnimSetFloat", RpcTarget.All, "AttackSpeed", status.attackSpeed);
        StartCoroutine(AttackCouroutine());
    }
    private IEnumerator AttackCouroutine()
    {
        if (currentAttackCoolTime >= attackCoolTime)
        {
            currentAttackCoolTime = 0f;
            pv.RPC("AnimSetTrigger", RpcTarget.All, "Attack");
            yield return new WaitForSeconds(0.03f);
            currentAnimationTime = anim.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(currentAnimationTime);
            if (status.attackRange > 9f)
            {
                var clone = PhotonNetwork.Instantiate(attackEffect,chTransform.position,Quaternion.identity);
                clone.GetComponent<MinionWizard_Attack>().minion = this;
                if (target == null || target.GetComponent<Status>().isDead) {
                        PhotonNetwork.Destroy(clone);
                }
            }
            else
            {
                if(target != null)
                target.GetComponent<Status>().Damage(status.attackDamage);
            }
            if (target == null)
            {
                target = null;
                SearchTarget();
            }
            else if (target.GetComponent<Status>().isDead)
            {
                target = null;
                SearchTarget();
            }
            isAttack = false;
        }
        else
        {
            yield return new WaitForSeconds(attackCoolTime - currentAttackCoolTime);
            StartCoroutine(AttackCouroutine());
        }
    }
    private void SearchTarget()
    {
        sightObject.gameObject.SetActive(true);
    }

    private void GoToTarget()
    {
        nav_agent.isStopped = false;
        if (isMove == false)
        {
            isMove = true;
            pv.RPC("AnimSetBool", RpcTarget.All, "Run", true);
        }
        nav_agent.speed = status.currentMoveSpeed;
        nav_agent.SetDestination(target.transform.position);
    }

    public void Look(Vector3 point)
    {
        chTransform.forward = point - chTransform.position;
    }

    public void GetTarget(Collider other)
    {
        if (target == null)
        {
            sightObject.gameObject.SetActive(false);
            target = other;
            Look(other.transform.position);
            GoToTarget();
        }
        else
        {
            return;
        }
    }
    [PunRPC]
    public void AnimSetTrigger(string name)
    {
        anim.SetTrigger(name);
    }
    [PunRPC]
    public void AnimSetBool(string name,bool a)
    {
        anim.SetBool(name, a);
    }
    [PunRPC]
    public void AnimSetFloat(string name,float speed)
    {
        anim.SetFloat(name, speed);
    }
}
