using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Wolf_AI : Monster_AI
{
    [SerializeField]
    private Avatar dieAvatar;
    [SerializeField]
    private Avatar idleAvatar;
    [SerializeField]
    private Avatar runAvatar;
    [SerializeField]
    private Avatar attackAvatar;
    protected override void Start()
    {
        base.Start();
        anim.avatar = idleAvatar;
    }
    protected override void Update()
    {
        base.Update();
        if (status.isDead)
        {
            anim.avatar = dieAvatar;
        }
    }
    protected override IEnumerator AttackCouroutine()
    {
            anim.avatar = attackAvatar;
            Look(target.transform.position);
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(0.1f);
            currentAnimTime = anim.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(currentAnimTime - 0.1f);
            target.GetComponent<Status>().Damage(status.attackDamage);
            isAttack = false;
            currentAttackCoolTime = 0f;
            anim.avatar = idleAvatar;
            Debug.Log("°ø°Ý³¡");

    }

    protected override void GoToOriginPos()
    {
        anim.avatar = runAvatar;
        base.GoToOriginPos();
    }

    protected override void GoToTarget()
    {
        anim.avatar = runAvatar;
        base.GoToTarget();
    }
    protected override void MoveCancel()
    {
        anim.avatar = idleAvatar;
        base.MoveCancel();
    }
}
