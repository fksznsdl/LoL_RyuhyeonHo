using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public abstract class Controller : MonoBehaviour
{
    [SerializeField]
    protected GameObject returnEffect;
    protected GameObject canvas;
    [SerializeField]
    protected PhotonView pv;
    [SerializeField]
    public Camera playerCamera;
    [SerializeField]
    public Transform chTransform;
    [SerializeField]
    protected NavMeshAgent agent;
    protected Animator anim;
    protected Status status;
    protected SkillState[] skillStates;
    public float[] skillCoolTimes;

    [SerializeField]
    protected string attackeffect;
    [SerializeField]
    protected GameObject rangeCircle;

    protected GameObject reCallPanel;
    protected Text[] warningTexts;

    protected Vector3 mousepoint;
    public RaycastHit HitInfo;

    public bool isMove;
    public bool isSkillAttack;
    public bool isShowRange;
    public bool isAttack;
    public bool isReturn;
    public bool isMiniMap;

    protected bool isNotReachAttack;

    protected float attackCoolTime;
    protected float currentAttackCoolTime;
    protected float currentAnimationTime;
    public float currentRange; // 사용할 공격의 사정거리
    public float currentCircleRange; // 사용할 자기중심 공격의 사정거리
    public float currentOnMouseCircleRange; // 사용할 논타겟 써클 공격의 지름
    public float currentOnMouseRectRange; // 사용할 논타겟 네모공격의 가로

    public Vector3 currentScale;
    protected Vector3 destination;

    protected int currentSkil;
    protected float returnTime;

    public bool[] isSkill;
    protected enum CurretSkil
    {
        Q=0,W=1,E=2,R=3,NONE=4
    }

    protected virtual void Start()
    {
        isSkill = new bool[4] { false, false, false, false };
        agent.acceleration = 1000f;
        status = GetComponent<Status>();
        anim = GetComponentInChildren<Animator>();
        skillStates = status.skillStates;
        skillCoolTimes = status.skillCoolTimes;
        if (pv.IsMine)
        {
            playerCamera.enabled = true;
        }
        agent.updateRotation = false;
        agent.acceleration = 100;

        isSkillAttack = false;
        isAttack = false;
        isShowRange = false;
        isMove = false;
        isReturn = false;
        isNotReachAttack = false;
        currentAttackCoolTime = 0;
        if (pv.IsMine)
        {
            canvas = GameObject.Find("PlayerCanvas");
            reCallPanel = GameObject.Find("ReCall");
            reCallPanel.gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        currentAttackCoolTime += Time.deltaTime;
        UpdateCoolTime();
        if (pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.S) && isAttack && !isSkillAttack)
            {
                StopAllCoroutines();
                isAttack = false;
            }
            if (isMove)
            {
                if (status.isAirborne || status.isCC || Vector3.Distance(chTransform.position, agent.destination) <= 0.1f)
                {
                    MoveCancel();
                }
            }
            if (!isAttack && !status.isAirborne && !status.isCC)
            {
                if (isNotReachAttack)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        isNotReachAttack = false;
                        MoveCancel();
                    }
                    else if (Vector3.Distance(HitInfo.point, chTransform.position)<= status.attackRange)
                    {
                        isNotReachAttack = false;
                        TryAttack(HitInfo);
                    }
                }
                if(Input.GetKey(KeyCode.B))
                {
                    TryReturn();
                }
                if (Input.GetKey(KeyCode.A))
                {
                    InputA();
                }
                if (!isShowRange)
                {
                    if (Input.GetKeyDown(KeyCode.Q) && isSkill[(int)CurretSkil.Q] == false)
                    {
                        if (status.skillsLevel[1] < 1)
                        {
                            canvas.transform.GetChild(4).gameObject.SetActive(true);
                        }
                        else if (status.currentMp < (skillStates[1].skill_originMana + (status.skillsLevel[1] * skillStates[1].skill_levelMana)))
                        {
                            canvas.transform.GetChild(2).gameObject.SetActive(true);
                        }
                        else if (skillCoolTimes[1] > 0f)
                        {
                            canvas.transform.GetChild(3).gameObject.SetActive(true);
                        }
                        else
                        {
                            TrySkilQ();
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.W) && isSkill[(int)CurretSkil.W] == false)
                    {
                        if (status.skillsLevel[2] < 1)
                        {
                            canvas.transform.GetChild(4).gameObject.SetActive(true);
                        }
                        else if (status.currentMp < (skillStates[2].skill_originMana + (status.skillsLevel[2] * skillStates[2].skill_levelMana)))
                        {
                            canvas.transform.GetChild(2).gameObject.SetActive(true);
                        }
                        else if (skillCoolTimes[2] > 0f)
                        {
                            canvas.transform.GetChild(3).gameObject.SetActive(true);
                        }
                        else
                        {
                            TrySkilW();
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.E) && isSkill[(int)CurretSkil.E] == false)
                    {
                        if (status.skillsLevel[3] < 1)
                        {
                            canvas.transform.GetChild(4).gameObject.SetActive(true);
                        }
                        else if (status.currentMp < (skillStates[3].skill_originMana + (status.skillsLevel[3] * skillStates[3].skill_levelMana)))
                        {
                            canvas.transform.GetChild(2).gameObject.SetActive(true);
                        }
                        else if (skillCoolTimes[3] > 0f)
                        {
                            canvas.transform.GetChild(3).gameObject.SetActive(true);
                        }
                        else
                        {
                            TrySkilE();
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.R) && isSkill[(int)CurretSkil.R] == false)
                    {
                        if (status.skillsLevel[4] < 1)
                        {
                            canvas.transform.GetChild(4).gameObject.SetActive(true);
                        }
                        else if (status.currentMp < (skillStates[4].skill_originMana + (status.skillsLevel[4] * skillStates[4].skill_levelMana)))
                        {
                            canvas.transform.GetChild(2).gameObject.SetActive(true);
                        }
                        else if (skillCoolTimes[4] > 0f)
                        {
                            canvas.transform.GetChild(3).gameObject.SetActive(true);
                        }
                        else
                        {
                            TrySkilR();
                        }
                    }
                }
                if (Input.GetMouseButtonUp(1))
                {
                    RightClick();
                }
            }
            else if(isAttack)
            {
                if (Input.GetMouseButtonDown(1)) { 
                    StopAllCoroutines();
                    isAttack = false;
                    anim.SetBool("AttackBool", false);
                }
            }
        }
    }

    private void TryReturn()
    {
        returnTime = 0f;
        MoveCancel();
        StartCoroutine(ReturnCoroutine());
    }
    private IEnumerator ReturnCoroutine()
    {
        if (!isReturn)
        {
            Image _image = reCallPanel.transform.Find("ReCallImage").GetComponent<Image>();
            isReturn = true;
            reCallPanel.gameObject.SetActive(true);
            returnEffect.gameObject.SetActive(true);
            while (returnTime <= 8f)
            {
                returnTime += Time.deltaTime;
                _image.fillAmount = returnTime / 8f;
                yield return new WaitForEndOfFrame();
                if (isAttack || isMove || status.isbeHit)
                {
                    if(status.isbeHit)
                    status.isbeHit = false;
                    break;
                }
            }
            returnEffect.gameObject.SetActive(false);
            reCallPanel.gameObject.SetActive(false);
            if (returnTime >= 8f)
            {
                this.GetComponent<NavMeshAgent>().enabled = false;
                if (status.team == 2)
                {
                    this.transform.position = new Vector3(5f,0f,5f);
                }
                else if (status.team == 1 )
                {
                    this.transform.position = new Vector3(205f, 0f, 205f);
                }
                this.GetComponent<NavMeshAgent>().enabled = true;
            }
            isReturn = false;
        }
    }
    private void UpdateCoolTime()
    {
        for (int i = 0; i < skillCoolTimes.Length; i++)
        {
            if (skillCoolTimes[i]>0f)
            {
                skillCoolTimes[i] -= Time.deltaTime;
                if (skillCoolTimes[i] < 0f)
                {
                    skillCoolTimes[i] = 0f;
                }
            }
        }
    }
    protected void SetCoolTimeAndMp(int _skillNumber)
    {
        skillCoolTimes[_skillNumber] = skillStates[_skillNumber].skill_CoolDownTime - (status.skillsLevel[_skillNumber] * skillStates[_skillNumber].skill_levelCoolDownTime);
        status.currentMp -= (skillStates[_skillNumber].skill_originMana + (skillStates[_skillNumber].skill_levelMana * status.skillsLevel[_skillNumber]));
    }
    private void InputA()
    {
        currentCircleRange = status.attackRange;
        rangeCircle.gameObject.SetActive(true);
    }
    
    protected abstract void TrySkilQ();

    protected abstract IEnumerator SkilQCouroutine();
    protected abstract void TrySkilW();

    protected abstract IEnumerator SkilWCoroutine();

    protected abstract void TrySkilE();

    protected abstract IEnumerator SkilECouroutine();
    protected abstract void TrySkilR();

    protected abstract IEnumerator SkilRCouroutine();
    public void MoveCancel()
    {
        isMove = false;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        anim.SetBool("Run",false);
    }
    private void RightClick()
    {
        isMiniMap = false;
        if (Input.mousePosition.x>=(Screen.width*0.8) && Input.mousePosition.y<=(Screen.height * 0.35))
        {
            isMiniMap = true;
        }
        if (!isMiniMap) {
            if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out HitInfo))
            {
                if (HitInfo.transform.tag == "Champion" || HitInfo.transform.tag == "Minion" || HitInfo.transform.tag == "Building" || HitInfo.transform.tag == "Monster")
                {
                    if (Vector3.Distance(HitInfo.point, chTransform.position) <= status.attackRange)
                    {
                        if (status.team != HitInfo.transform.GetComponent<Status>().team)
                        {
                            TryAttack(HitInfo);
                        }
                    }
                    else
                    {
                        if (status.team != HitInfo.transform.GetComponent<Status>().team)
                        {
                            isNotReachAttack = true;
                            SetDestination(HitInfo.point);
                        }
                    }
                }
                else if (HitInfo.transform.tag == "shop")
                {
                    Debug.Log(HitInfo.transform.name);
                    HitInfo.transform.GetComponent<Shop>().ShowShop(status, chTransform);
                }
                else
                {
                    SetDestination(HitInfo.point);
                }
            }
        }
    }
    public void SetDestination(Vector3 _hitInfo)
    {
        if(status.isDead ||status.isAirborne || status.isCC || isAttack)
        {
            return;
        }
        if (isMove)
        {
            agent.velocity = Vector3.zero;
        }
        isMove = true;
        agent.isStopped = false;
        anim.SetBool("Run", true);
        agent.speed = status.currentMoveSpeed;
        agent.SetDestination(_hitInfo);
        destination = _hitInfo;
        Look(_hitInfo);
    }
    private void TryAttack(RaycastHit _hitInfo)
    {
        if (isMove)
        {
            MoveCancel();
        }

        attackCoolTime = 1f / status.attackSpeed;
        isAttack = true;
        Look(_hitInfo.point);
        if (status.attackSpeed > 1) anim.SetFloat("AttackSpeed", status.attackSpeed);
        else anim.SetFloat("AttackSpeed", 1);
        //StartCoroutine(AttackCouroutine(_hitInfo));
        StartCoroutine(AttackBoolCouroutine(_hitInfo));
    }
    protected virtual IEnumerator AttackBoolCouroutine(RaycastHit _hitInfo)
    {
        if (currentAttackCoolTime >= attackCoolTime)
        {
            anim.SetBool("AttackBool", true);
            yield return new WaitForSeconds(0.1f);
            currentAnimationTime = anim.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(currentAnimationTime - 0.2f);
            anim.SetBool("AttackBool", false);
            isAttack = false;
            currentAttackCoolTime = 0;
        }
        else
        {
            yield return new WaitForSeconds(attackCoolTime - currentAttackCoolTime);
            StartCoroutine(AttackBoolCouroutine(_hitInfo));
        }
    }
    protected virtual IEnumerator AttackCouroutine(RaycastHit _hitInfo)
    {
        if (currentAttackCoolTime >= attackCoolTime)
         {
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(0.1f);
            currentAnimationTime = anim.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(currentAnimationTime-0.6f);
            var clone = PhotonNetwork.Instantiate(attackeffect, chTransform.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.4f);
            isAttack = false;
            currentAttackCoolTime = 0;
            
        }
        else
        {
            yield return new WaitForSeconds(attackCoolTime-currentAttackCoolTime);
            StartCoroutine(AttackCouroutine(_hitInfo));
        }
    }
    public void Look(Vector3 _hitInfo)
    {
        var movePoint = (new Vector3(_hitInfo.x - chTransform.position.x, chTransform.position.y, _hitInfo.z - chTransform.position.z));
        chTransform.forward = movePoint;
    }
}
