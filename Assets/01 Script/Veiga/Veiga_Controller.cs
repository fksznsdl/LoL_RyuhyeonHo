using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Veiga_Controller : Controller
{
    [SerializeField]
    private GameObject nonTargetRange;
    [SerializeField]
    private GameObject nonTargetCircleRange;
    [SerializeField]
    private GameObject onMouseCircleRange;
    [SerializeField]
    private GameObject targetCircleRange;

    [SerializeField]
    private GameObject VeigaQ;
    [SerializeField]
    private GameObject VeigaW;
    [SerializeField]
    private GameObject VeigaE;
    [SerializeField]
    private GameObject VeigaR;

    private bool isNotReachRange = false;

    public float VeigaQRange;
    public float VeigaWRange;
    public float VeigaWRange_OnMouse;
    public float VeigaERange;
    public float VeigaERange_OnMouse;
    public float VeigaR_Range;

    protected override void Start()
    {
        base.Start();
        VeigaQRange = 900f / 60f;
        VeigaWRange = 900f / 60f;
        VeigaWRange_OnMouse = 150f / 60f;
        VeigaERange = 725f / 60f;
        VeigaERange_OnMouse = 300f / 60f;
        VeigaR_Range = 650f / 60f;
    }
    protected override void Update()
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
                if (Vector3.Distance(HitInfo.point, chTransform.position) <= VeigaWRange && currentSkil == (int)CurretSkil.W)
                {
                    isNotReachRange = false;
                    StartCoroutine(SkilWCoroutine());
                }
                else if (Vector3.Distance(HitInfo.point, chTransform.position) <= VeigaERange && currentSkil == (int)CurretSkil.E)
                {
                    isNotReachRange = false;
                    StartCoroutine(SkilECouroutine());
                }
                else if (Vector3.Distance(HitInfo.point, chTransform.position) <= VeigaR_Range && currentSkil == (int)CurretSkil.R)
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
                            nonTargetCircleRange.gameObject.SetActive(false);
                            onMouseCircleRange.gameObject.SetActive(false);
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
                            Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out HitInfo);
                            if (Vector3.Distance(chTransform.position, HitInfo.point) <= VeigaR_Range)
                            {
                                if (HitInfo.transform.tag == "Champion")
                                {
                                    if (status.team != HitInfo.transform.GetComponent<Status>().team)
                                    {
                                        isShowRange = false;
                                        targetCircleRange.gameObject.SetActive(false);
                                        StartCoroutine(SkilRCouroutine());
                                    }
                                }
                            }
                            break;
                    }

                }
            }
        }
    }

    protected override void TrySkilE()
    {
        currentSkil = (int)CurretSkil.E;
        currentCircleRange = VeigaERange;
        currentOnMouseCircleRange = VeigaERange_OnMouse;
        nonTargetCircleRange.gameObject.SetActive(true);
        onMouseCircleRange.gameObject.SetActive(true);
    }

    protected override void TrySkilQ()
    {
        currentSkil = (int)CurretSkil.Q;
        currentRange = VeigaQRange;
        currentScale = VeigaQ.transform.localScale;
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
        var cloneQ = PhotonNetwork.Instantiate("VeigaQ", this.transform.position, Quaternion.identity);
        anim.SetFloat("AttackSpeed", 1f);
        anim.SetTrigger("Spell3");
        yield return new WaitForSeconds(0.01f);
        currentAnimationTime = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(currentAnimationTime - 0.1f);
        isSkillAttack = false;
        isAttack = false;
    }

    protected override void TrySkilR()
    {
        currentSkil = (int)CurretSkil.R;
        currentCircleRange = VeigaR_Range;
        targetCircleRange.gameObject.SetActive(true);
    }


    protected override void TrySkilW()
    {
        currentSkil = (int)CurretSkil.W;
        currentCircleRange = VeigaWRange;
        currentOnMouseCircleRange = VeigaWRange_OnMouse;
        nonTargetCircleRange.gameObject.SetActive(true);
        onMouseCircleRange.gameObject.SetActive(true);
    }

    protected override IEnumerator SkilWCoroutine()
    {

        if (isMove)
        {
            MoveCancel();
        }
        if (Vector3.Distance(chTransform.position, HitInfo.point) >= VeigaWRange)
        {
            goToRange();
            yield break;
        }
        isAttack = true;
        isSkillAttack = true;
        Look(HitInfo.point);
        SetCoolTimeAndMp(2);
        var cloneW = PhotonNetwork.Instantiate("VeigaW", HitInfo.point, Quaternion.identity);
        cloneW.GetComponent<Veiga_W>().hitInfo = HitInfo;
        anim.SetTrigger("Spell2");
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
        if (Vector3.Distance(chTransform.position, HitInfo.point) >= VeigaERange)
        {
            goToRange();
            yield break;
        }
        isAttack = true;
        isSkillAttack = true;
        Look(HitInfo.point);
        SetCoolTimeAndMp(3);
        HitInfo.point = new Vector3(HitInfo.point.x, 0.1f, HitInfo.point.z);
        var cloneE = PhotonNetwork.Instantiate("VeigaE", HitInfo.point, Quaternion.identity);
        anim.SetTrigger("Spell3");
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
        if (Vector3.Distance(chTransform.position, HitInfo.point) >= VeigaR_Range)
        {
            goToRange();
            yield break;
        }
        isAttack = true;
        isSkillAttack = true;
        Look(HitInfo.point);
        SetCoolTimeAndMp(4);
        var cloneR = PhotonNetwork.Instantiate("VeigaR", this.transform.position, Quaternion.identity);
        anim.SetFloat("AttackSpeed", 1f);
        anim.SetTrigger("Spell3");
        yield return new WaitForSeconds(0.01f);
        currentAnimationTime = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(currentAnimationTime - 0.1f);
        isSkillAttack = false;
        isAttack = false;
    }
}
