using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Anivia_Controller : Controller
{
    [SerializeField]
    protected GameObject nonTargetRange; // 논타겟 스킬
    [SerializeField]
    protected GameObject nonTargetCircleRange; //논타겟 서클 스킬
    [SerializeField]
    private GameObject nonTargetCircle_RectRange; // 논타겟 서클 in 사각형 스킬
    [SerializeField]
    protected GameObject onMouseCircleRange; // 마우스 위 서클
    [SerializeField]
    private GameObject onMouseRectRange; // 마우스 위 사각형
    [SerializeField]
    protected GameObject targetCircleRange; // 타겟팅 스킬

    [SerializeField]
    private GameObject AniviaPassive;
    [SerializeField]
    private GameObject AniviaQ;
    [SerializeField]
    private GameObject AniviaW;
    [SerializeField]
    private GameObject AniviaE;
    [SerializeField]
    private GameObject AniviaR;

    private bool isNotReachRange = false;

    public float AniviaQRange;
    public float AniviaWRange;
    public float AniviaWRange_OnMouse;
    public float AniviaERange;
    public float AniviaR_Range;
    public float AniviaR_Range_OnMouse;

    protected override void Start()
    {
        base.Start();
        AniviaQRange = 1075f / 60f;
        AniviaWRange = 1000f / 60f;
        AniviaWRange_OnMouse = ((status.skillsLevel[2] * 100f)+300f) / 60f / 2f;
        AniviaERange = 650f / 60f;
        AniviaR_Range = 750f / 60f;
        AniviaR_Range_OnMouse = ((status.skillsLevel[4] * 100f) + 200f) / 60f;
    }
    protected override void Update()
    {
        base.Update();

        if (status.isCC || status.isAirborne)
        {
            MoveCancel();
            return;
        }
        if (pv.IsMine)
        {
            if (isNotReachRange)
            {
                if (Input.GetMouseButtonUp(1))
                {
                    isNotReachRange = false;
                    MoveCancel();
                }
                if (Vector3.Distance(HitInfo.point, chTransform.position) <= AniviaWRange && currentSkil == (int)CurretSkil.W)
                {
                    isNotReachRange = false;
                    StartCoroutine(SkilWCoroutine());
                }
                else if (Vector3.Distance(HitInfo.point, chTransform.position) <= AniviaERange && currentSkil == (int)CurretSkil.E)
                {
                    isNotReachRange = false;
                    StartCoroutine(SkilECouroutine());
                }
                else if (Vector3.Distance(HitInfo.point, chTransform.position) <= AniviaR_Range && currentSkil == (int)CurretSkil.R)
                {
                    isNotReachRange = false;
                    StartCoroutine(SkilRCouroutine());
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
                            nonTargetCircle_RectRange.gameObject.SetActive(false);
                            onMouseRectRange.gameObject.SetActive(false);
                            Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out HitInfo);
                            StartCoroutine(SkilWCoroutine());
                            break;
                        case (int)CurretSkil.E:
                            Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out HitInfo);
                            if (Vector3.Distance(chTransform.position, HitInfo.point) <= AniviaERange)
                            {
                                if (HitInfo.transform.tag == "Champion" || HitInfo.transform.tag == "Monster" || HitInfo.transform.tag == "Minion")
                                {
                                    if (status.team != HitInfo.transform.GetComponent<Status>().team)
                                    {
                                        isShowRange = false;
                                        targetCircleRange.SetActive(false);
                                        StartCoroutine(SkilECouroutine());
                                    }
                                }
                            }
                            break;
                        case (int)CurretSkil.R:
                            isShowRange = false;
                            nonTargetCircleRange.gameObject.SetActive(false);
                            onMouseCircleRange.gameObject.SetActive(false);
                            Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out HitInfo);
                            StartCoroutine(SkilRCouroutine());
                            break;
                    }

                }
            }
        }
    }

    protected override void TrySkilE()
    {
        currentSkil = (int)CurretSkil.E;
        currentCircleRange = AniviaERange;
        targetCircleRange.gameObject.SetActive(true);
    }

    protected override void TrySkilQ()
    {
        currentSkil = (int)CurretSkil.Q;
        currentRange = AniviaQRange;
        currentScale = AniviaQ.transform.localScale;
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
        SetCoolTimeAndMp(1);
        Look(HitInfo.point);
        var cloneQ = PhotonNetwork.Instantiate("AniviaQ", new Vector3(chTransform.position.x, chTransform.position.y + 1f, chTransform.position.z), Quaternion.identity);
        isSkill[(int)CurretSkil.Q] = true;
        anim.SetFloat("AttackSpeed", 1f);
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
        currentCircleRange = AniviaR_Range;
        currentOnMouseCircleRange = AniviaR_Range_OnMouse;
        nonTargetCircleRange.gameObject.SetActive(true);
        onMouseCircleRange.gameObject.SetActive(true);
    }


    protected override void TrySkilW()
    {
        currentSkil = (int)CurretSkil.W;
        currentCircleRange = AniviaWRange;
        currentOnMouseRectRange = AniviaWRange_OnMouse;
        nonTargetCircle_RectRange.gameObject.SetActive(true);
        onMouseRectRange.gameObject.SetActive(true);
    }

    protected override IEnumerator SkilWCoroutine()
    {

        if (isMove)
        {
            MoveCancel();
        }
        if (Vector3.Distance(chTransform.position, HitInfo.point) >= AniviaWRange)
        {
            goToRange();
            yield break;
        }
        isAttack = true;
        isSkillAttack = true;
        Look(HitInfo.point);
        SetCoolTimeAndMp(2);
        var cloneW = PhotonNetwork.Instantiate("AniviaW", HitInfo.point, Quaternion.identity);
        cloneW.transform.forward = onMouseRectRange.transform.forward;
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
        if (Vector3.Distance(chTransform.position, HitInfo.point) >= AniviaERange)
        {
            goToRange();
            yield break;
        }
        isAttack = true;
        isSkillAttack = true;
        HitInfo.point = new Vector3(HitInfo.point.x, 1f, HitInfo.point.z);
        SetCoolTimeAndMp(3);
        Look(HitInfo.point);
        var cloneE = PhotonNetwork.Instantiate("AniviaE", chTransform.position, Quaternion.identity);
        anim.SetFloat("AttackSpeed", 1f);
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
        if (Vector3.Distance(chTransform.position, HitInfo.point) >= AniviaR_Range)
        {
            goToRange();
            yield break;
        }
        isAttack = true;
        isSkillAttack = true;
        Look(HitInfo.point);
        SetCoolTimeAndMp(4);
        var cloneR = PhotonNetwork.Instantiate("AniviaR", HitInfo.point, Quaternion.identity);
        isSkill[(int)CurretSkil.R] = true;
        anim.SetTrigger("Spell");
        yield return new WaitForSeconds(0.01f);
        currentAnimationTime = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(currentAnimationTime - 0.1f);
        isSkillAttack = false;
        isAttack = false;
    }
}
