using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Janna_Controller : Controller
{
    [SerializeField]
    protected GameObject nonTargetRange; // ≥Ì≈∏∞Ÿ Ω∫≈≥

    [SerializeField]
    protected GameObject targetCircleRange; // ≈∏∞Ÿ∆√ Ω∫≈≥

    [SerializeField]
    private GameObject JannaPassive;
    [SerializeField]
    private GameObject JannaQ;
    [SerializeField]
    private GameObject JannaW;
    [SerializeField]
    private GameObject JannaE;
    [SerializeField]
    private GameObject JannaR;

    private bool isNotReachRange = false;

    public float JannaQRange;
    public float JannaWRange;
    public float JannaERange;
    public float JannaR_Range;

    protected override void Start()
    {
        base.Start();
        JannaQRange = 1100f / 60f;
        JannaWRange = 650f / 60f;
        JannaERange = 800f / 60f;
        JannaR_Range = 725f / 60f;
    }
    protected override void Update()
    {
        if (!status.isDead)
        {

            base.Update();
            if (pv.IsMine)
            {
                if (currentSkil == (int)CurretSkil.R)
                {
                    if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                    {
                        StopCoroutine(SkilRCouroutine());
                        anim.SetBool("Spell4", false);
                        isAttack = false;
                        isSkillAttack = false;
                        currentSkil = (int)CurretSkil.NONE;
                    }
                }
                if (isNotReachRange)
                {
                    if (Input.GetMouseButtonUp(1))
                    {
                        isNotReachRange = false;
                        MoveCancel();
                    }
                    if (Vector3.Distance(HitInfo.point, chTransform.position) <= JannaWRange && currentSkil == (int)CurretSkil.W)
                    {
                        isNotReachRange = false;
                        StartCoroutine(SkilWCoroutine());
                    }
                    else if (Vector3.Distance(HitInfo.point, chTransform.position) <= JannaERange && currentSkil == (int)CurretSkil.E)
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
                                Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out HitInfo);

                                if (HitInfo.transform.tag == "Champion" || HitInfo.transform.tag == "Minion" || HitInfo.transform.tag == "Monster")
                                {
                                    if (status.team != HitInfo.transform.GetComponent<Status>().team)
                                    {
                                        isShowRange = false;
                                        targetCircleRange.SetActive(false);
                                        StartCoroutine(SkilWCoroutine());
                                    }
                                }
                                break;
                            case (int)CurretSkil.E:
                                Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out HitInfo);

                                if (HitInfo.transform.tag == "Champion" || HitInfo.transform.tag == "Building")
                                {
                                    if (status.team == HitInfo.transform.GetComponent<Status>().team)
                                    {
                                        isShowRange = false;
                                        targetCircleRange.SetActive(false);
                                        StartCoroutine(SkilECouroutine());
                                    }
                                }

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
        currentCircleRange = JannaERange;
        targetCircleRange.gameObject.SetActive(true);
    }

    protected override void TrySkilQ()
    {
        currentSkil = (int)CurretSkil.Q;
        currentRange = JannaQRange;
        currentScale = JannaQ.transform.localScale;
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
        var cloneQ = PhotonNetwork.Instantiate("JannaQ", new Vector3(chTransform.position.x, chTransform.position.y + 1f, chTransform.position.z), Quaternion.identity);
        isSkill[(int)CurretSkil.Q] = true;
        anim.SetTrigger("Spell1");
        yield return new WaitForSeconds(0.03f);
        currentAnimationTime = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(currentAnimationTime - 0.1f);
        isSkillAttack = false;
        isAttack = false;
    }

    protected override void TrySkilR()
    {
        currentSkil = (int)CurretSkil.R;
        currentCircleRange = JannaR_Range;
        StartCoroutine(SkilRCouroutine());
    }


    protected override void TrySkilW()
    {
        currentSkil = (int)CurretSkil.W;
        currentCircleRange = JannaWRange;
        targetCircleRange.gameObject.SetActive(true);
    }

    protected override IEnumerator SkilWCoroutine()
    {

        if (isMove)
        {
            MoveCancel();
        }
        if (Vector3.Distance(chTransform.position, HitInfo.point) >= JannaWRange)
        {
            goToRange();
            yield break;
        }
        isAttack = true;
        isSkillAttack = true;
        Look(HitInfo.point);
        SetCoolTimeAndMp(2);
        var cloneW = PhotonNetwork.Instantiate("JannaW", chTransform.position, Quaternion.identity);
        anim.SetTrigger("Spell1");
        yield return new WaitForSeconds(0.03f);
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
        if (Vector3.Distance(chTransform.position, HitInfo.point) >= JannaERange)
        {
            goToRange();
            yield break;
        }
        isAttack = true;
        isSkillAttack = true;
        Look(HitInfo.point);
        SetCoolTimeAndMp(3);
        var cloneE = PhotonNetwork.Instantiate("JannaE", new Vector3(HitInfo.point.x, HitInfo.point.y + 2f, HitInfo.point.z), Quaternion.identity);
        anim.SetTrigger("Spell1");
        yield return new WaitForSeconds(0.03f);
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
        anim.SetBool("Spell4", true);
        SetCoolTimeAndMp(4);
        var cloneR = PhotonNetwork.Instantiate("JannaR", new Vector3(chTransform.position.x, chTransform.position.y + 1f, chTransform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(3f);
        isAttack = false;
        isSkillAttack = false;
        anim.SetBool("Spell4", false);
    }
}
