using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lux_Controller : Controller
{
    [SerializeField]
    protected GameObject nonTargetRange; // 논타겟 스킬
    [SerializeField]
    protected GameObject nonTargetCircleRange; //논타겟 서클 스킬
    [SerializeField]
    protected GameObject onMouseCircleRange; // 마우스 위 서클

    [SerializeField]
    private GameObject LuxQ;
    [SerializeField]
    private GameObject LuxW;
    [SerializeField]
    private GameObject LuxE;
    [SerializeField]
    private GameObject LuxR;

    private bool isNotReachRange = false;

    public float LuxQRange;
    public float LuxWRange;
    public float LuxERange;
    public float LuxERange_OnMouse;
    public float LuxR_Range;

    private string passiveName;
    private float passiveTime;
    private float passvieDamage;

    protected override void Start()
    {
        base.Start();
        LuxQRange = 1175f / 60f;
        LuxWRange = 1150f / 60f;
        LuxERange = 1100f / 60f;
        LuxERange_OnMouse = 200f / 60f;
        LuxR_Range = 3340f / 60f;

        passiveName = "광채";
        passiveTime = 6f;
        passvieDamage = 10f + (10f * status.level) + (status.spell * 0.2f);
    }
    protected override void Update()
    {
        if (!status.isDead)
        {
            base.Update();
            if (pv.IsMine)
            {
                if (isNotReachRange)
                {
                    if (Input.GetMouseButtonUp(1))
                    {
                        isNotReachRange = false;
                        MoveCancel();
                    }
                    else if (Vector3.Distance(HitInfo.point, chTransform.position) <= LuxERange && currentSkil == (int)CurretSkil.E)
                    {
                        isNotReachRange = false;
                        StartCoroutine(SkilECouroutine());
                    }
                }
                if (isShowRange)
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        switch (currentSkil)
                        {
                            case (int)CurretSkil.Q:
                                isShowRange = false;
                                nonTargetRange.gameObject.SetActive(false);
                                Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out HitInfo);
                                StartCoroutine(SkilQCouroutine());
                                break;
                            case (int)CurretSkil.W:
                                isShowRange = false;
                                nonTargetRange.gameObject.SetActive(false);
                                Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out HitInfo);
                                StartCoroutine(SkilWCoroutine());
                                break;
                            case (int)CurretSkil.E:
                                isShowRange = false;
                                nonTargetCircleRange.gameObject.SetActive(false);
                                onMouseCircleRange.gameObject.SetActive(false);
                                Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out HitInfo);
                                StartCoroutine(SkilECouroutine());
                                break;
                            case (int)CurretSkil.R:
                                isShowRange = false;
                                nonTargetRange.gameObject.SetActive(false);
                                Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out HitInfo);
                                StartCoroutine(SkilRCouroutine());
                                break;
                        }

                    }
                }
            }
        }
    }

    protected override void TrySkilE()
    {
        currentSkil = (int)CurretSkil.E;
        currentCircleRange = LuxERange;
        currentOnMouseCircleRange = LuxERange_OnMouse;
        nonTargetCircleRange.gameObject.SetActive(true);
        onMouseCircleRange.gameObject.SetActive(true);
    }

    protected override void TrySkilQ()
    {
        currentSkil = (int)CurretSkil.Q;
        currentRange = LuxQRange;
        currentScale = LuxQ.transform.localScale;
        nonTargetRange.gameObject.SetActive(true);
    }
    protected override IEnumerator SkilQCouroutine()
    {
        if (isMove)
        {
            MoveCancel();
        }
        isAttack = true;
        isSkillAttack = true;
        Look(HitInfo.point);
        SetCoolTimeAndMp(1);
        var cloneQ = PhotonNetwork.Instantiate("LuxQ", new Vector3(chTransform.position.x, chTransform.position.y + 1f, chTransform.position.z), Quaternion.identity);
        anim.SetTrigger("Spell");
        yield return new WaitForSeconds(0.01f);
        currentAnimationTime = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(currentAnimationTime - 0.1f);
        isSkillAttack = false;
        isAttack = false;
    }

    protected override void TrySkilR()
    {
        currentSkil = (int)CurretSkil.R;
        currentRange = LuxR_Range;
        currentScale = LuxR.transform.localScale;
        nonTargetRange.gameObject.SetActive(true);
    }


    protected override void TrySkilW()
    {
        currentSkil = (int)CurretSkil.W;
        currentRange = LuxWRange;
        currentScale = LuxW.transform.localScale;
        nonTargetRange.gameObject.SetActive(true);
    }

    protected override IEnumerator SkilWCoroutine()
    {

        if (isMove)
        {
            MoveCancel();
        }
        isAttack = true;
        isSkillAttack = true;
        Look(HitInfo.point);
        SetCoolTimeAndMp(2);
        var cloneW = PhotonNetwork.Instantiate("LuxW", new Vector3(chTransform.position.x, chTransform.position.y + 1f, chTransform.position.z), Quaternion.identity);
        anim.SetTrigger("Spell");
        yield return new WaitForSeconds(0.01f);
        currentAnimationTime = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(currentAnimationTime - 0.1f);
        isSkillAttack = false;
        isAttack = false;
    }
    private void goToRange()
    {
        isNotReachRange = true;
        SetDestination(HitInfo.point);
    }

    protected override IEnumerator SkilECouroutine()
    {
        if (isMove)
        {
            MoveCancel();
        }
        if (Vector3.Distance(chTransform.position, HitInfo.point) >= LuxERange)
        {
            goToRange();
            yield break;
        }
        isAttack = true;
        isSkillAttack = true;
        HitInfo.point = new Vector3(HitInfo.point.x, 1f, HitInfo.point.z);
        SetCoolTimeAndMp(3);
        Look(HitInfo.point);
        var cloneE = PhotonNetwork.Instantiate("LuxE", chTransform.position, Quaternion.identity);
        isSkill[(int)CurretSkil.E] = true;
        anim.SetTrigger("Spell");
        yield return new WaitForSeconds(0.01f);
        currentAnimationTime = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(currentAnimationTime - 0.1f);
        isSkillAttack = false;
        isAttack = false;
    }

    protected override IEnumerator SkilRCouroutine()
    {
        if (isMove)
        {
            MoveCancel();
        }
        isAttack = true;
        isSkillAttack = true;
        Look(HitInfo.point);
        SetCoolTimeAndMp(4);
        var cloneR = PhotonNetwork.Instantiate("LuxR", new Vector3(chTransform.position.x, chTransform.position.y + 1f, chTransform.position.z), Quaternion.identity);
        anim.SetTrigger("Spell");
        yield return new WaitForSeconds(1f);
        isSkillAttack = false;
        isAttack = false;
    }
    public void Passive(Collider other)
    {
        bool ispassive = false;
        for (int i = 0; i < other.GetComponent<Status>().debuffs.Count; i++)
        {
            if (other.GetComponent<Status>().debuffs[i].debuffName == passiveName)
            {
                other.GetComponent<Status>().debuffs[i].debuffTime = passiveTime;
                ispassive = true;
            }
        }
        if (!ispassive)
            other.GetComponent<Status>().HitDebuff(passiveName, passiveTime, (int)Debuff.DebuffType.HITATTACk);
    }
    public void HitPassive(Collider other)
    {
        foreach (var item in other.GetComponent<Status>().debuffs)
        {
            if (item.debuffName == passiveName)
            {
                other.GetComponent<Status>().Damage(passvieDamage, this.gameObject.name, Status.AttackType.AP, status.APperPenetration, status.APpenetration);
                other.GetComponent<Status>().debuffs.Remove(item);
                break;
            }
        }
    }
    public void HitPassive(RaycastHit _hitInfo)
    {
        foreach (var item in _hitInfo.transform.GetComponent<Status>().debuffs)
        {
            if (item.debuffName == passiveName)
            {
                _hitInfo.transform.GetComponent<Status>().Damage(passvieDamage, this.gameObject.name, Status.AttackType.AP, status.APperPenetration, status.APpenetration);
                _hitInfo.transform.GetComponent<Status>().debuffs.Remove(item);
                break;
            }
        }
    }
}
