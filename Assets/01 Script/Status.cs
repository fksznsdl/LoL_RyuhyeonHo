using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static Barrier;
using static Status;

public class Barrier
{
    public string barrierName;
    public float barrier;
    public float barrierTime;
    public float barrierDecreaseTime;
    public int decreaseType;

    public enum DecreaseType
    {
        NONE =0,DECREASETIME=1,INFINITE=2
    }

}

public class Status : MonoBehaviourPunCallbacks ,IPunObservable
{
    private string champName;
    public PhotonView pv;
    [SerializeField]
    State state;
    [SerializeField]
    private Image hpBar;
    [SerializeField]
    private Image mpBar;
    [SerializeField]
    private Image barrierBar;
    [SerializeField]
    private Transform chTransform;
    [SerializeField]
    private GameObject wallCheck;
    [SerializeField]
    private Animator anim;
    public Status_UI status_UI;
    [SerializeField]
    private GameObject ccText;
    public int team;
    [Header ("state")]
    public float maxHp;
    public float currentHp;
    public float maxMp;
    public float currentMp;
    public float originMoveSpeed;
    public float addMoveSpeed;
    public float currentMoveSpeed;
    public float armor;
    public float spellBlock;
    public float attackRange;
    public float hpRegen;
    public float mpRegen;
    public float crit;
    public float originAD;
    public float attackDamage;
    public float originAttackSpeed;
    public float addAttackSpeed;
    public float attackSpeed;
    public float spell;
    public float sightRange;
    public float killExp;
    public float killgold;
    public float killExpperlevel;
    public float adVamp;
    public float spellAcceleration;
    public float currentExp;
    public float maxExp;
    public float ADperPenetration;
    public float ADpenetration;
    public float APperPenetration;
    public float APpenetration;
    public float currentbarrier;

    public float currentGold;
    public int currentKillMinion;
    public int currentKillChamp;
    private List<Barrier> barriers;

    public int level;

    public int[] skillsLevel;
    public float[] skillCoolTimes;

    public List<Debuff> debuffs;
    private float goldTime;
    private float currentTime;
    private float currentslowTime;
    private float currentCCTime;
    private float currentAirborneTime;
    private float currentRegenTime;
    private float slowPer;
    private float KnockBackTime;
    private float champAttackResetTime;

    public bool isDead;
    public bool isHitAniviaQR;
    private bool isDebuff;
    public bool isCC;
    public bool isAirborne;
    private bool isSlow;
    private bool isSameVarrier;
    private bool isEndAirborne;
    private bool isKnockBack;
    public bool isNearWall;
    public bool isChampAttack;
    public bool isbeHit;
    public bool isShop;

    private float force;

    private Vector3 originPos;
    private Vector3 AirbornePos;
    private Vector3 KnockBackPos;

    public GameObject beHitTarget;
    [SerializeField]
    public SkillState[] skillStates;

    [SerializeField]
    public string minionType;
    public enum Team
    {
        NONE =0,RED=1,BLUE=2,NEUTRAL=3
    };

    public enum AttackType
    {
        AD = 0, AP =1
    }

    // Start is called before the first frame update
    private void Awake()
    {
        champName = state.chName;
        pv = GetComponent<PhotonView>();
        maxHp = state.hp;
        currentHp = state.hp;
        maxMp = state.mp;
        currentMp = state.mp;
        currentMoveSpeed = state.movespeed/60f;
        armor = state.armor;
        spellBlock = state.spellblock;
        attackRange = state.attackrange / 60f;
        hpRegen = state.hpregen;
        mpRegen = state.mpregen;
        crit = state.crit;
        attackDamage = state.attackdamage;
        originAttackSpeed = state.attackspeed;
        addAttackSpeed = 0f;
        attackSpeed = state.attackspeed;
        spell = state.spell;
        currentbarrier = 0f;
        if (this.transform.tag == "Champion")
        {
            if (pv.IsMine)
            {
                if(GameObject.Find("PlayerSetting").transform.GetComponent<PlayerSetting>().team == "Blue")
                {
                    team = (int)Team.BLUE;
                }
                else
                {
                    team = (int)Team.RED;
                }
            }
        }
        else
        {
            team = state.team;
        }
        sightRange = state.sightRange / 60f;
        killExp = state.killExp;
        killgold = state.killgold;
        killExpperlevel = state.killExpperlevel;
        level = 1;
        isDead = false;
        isCC = false;
        isAirborne = false;
        isEndAirborne = false;
        isKnockBack = false;
        isNearWall = false;
        isChampAttack = false;
        isbeHit = false;
        currentCCTime = 0f;
        currentslowTime = 0f;
        currentRegenTime = 0f;
        originMoveSpeed = currentMoveSpeed;

        originAD = attackDamage;

        debuffs = new List<Debuff>();

        barriers = new List<Barrier>();

        champAttackResetTime = 0f;

        ADpenetration = 0f;
        ADperPenetration = 0f;
        APpenetration = 0f;
        APperPenetration = 0f;
        adVamp = 0f;
        spellAcceleration = 0f;

        currentExp = 0f;
        maxExp = 280f;
        currentGold = 500f;
        currentKillMinion = 0;
        currentKillChamp = 0;

        skillsLevel = new int[5] { 0,0,0,0,0 };
        skillCoolTimes = new float[5] { 0, 0, 0, 0, 0 };
        if (this.transform.tag == "Champion" && pv.IsMine)
        {
            StartCoroutine(StayCoroutine());
        }
        UpdateStatus();
        isShop = false;
    }
    private IEnumerator StayCoroutine() {
        yield return new WaitForSeconds(4f);
        status_UI = GameObject.Find("PlayerCanvas").transform.Find("Status_UI").GetComponent<Status_UI>();
        status_UI.OnSkillLevelUpButton();
    }
    void Update()
    {
        goldTime += Time.deltaTime;
        if(goldTime >= 1f)
        {
            goldTime = 0f;
            currentGold += 1.5f;
        }
        if (isChampAttack || isbeHit)
        {
            champAttackReset();
        }
        RegenHpMp();
        if (isKnockBack)
        {
            PlayKnockBack();
        }
        if (debuffs.Count >= 1f)
        {
            currentdebuff();
        }
        if (isSlow)
        {
            Slow();
        }
        if (barriers.Count > 0)
        {
            CurrentBarrier();
        }
    }
    private void UpdateStatus()
    {
        currentHp = Mathf.Floor(currentHp * 100f) / 100f;
        currentMp = Mathf.Floor(currentMp * 100f) / 100f;
        currentMoveSpeed = Mathf.Floor(currentMoveSpeed * 100f) / 100f;
        attackRange = Mathf.Floor(attackRange * 100f) / 100f;
        attackSpeed = Mathf.Floor(attackSpeed * 1000f) / 1000f;
    }

    public void GetExp(float _getExp)
    {
        currentExp += _getExp;
        if(currentExp >= maxExp)
        {
            currentExp -= maxExp;
            LevelUp();
        }
    }

    public void LevelUp()
    {
        if (this.transform.tag == "Champion")
        {
            if (level >= 18)
            {
                return;
            }
        }
        level++;
        maxHp += state.hpperlevel;
        currentHp += state.hpperlevel;
        maxMp += state.mpperlevel;
        currentMp += state.mpperlevel;
        maxMp += state.mpperlevel;
        armor += state.armorperlevel;
        spellBlock += state.spellblockperlevel;
        hpRegen += state.hpregenperlevel;
        mpRegen += state.mpregenperlevel;
        attackDamage += state.attackdamageperlevel;
        addAttackSpeed += state.attackspeedperlevel;
        attackSpeed = originAttackSpeed+(originAttackSpeed * (addAttackSpeed / 100f));
        killExp += state.killExpperlevel;
        if (level >= 18)
        {
            maxExp = 100000f;
        }
        else
        {
            maxExp = 280f + ((level - 1) * 100f);
        }
        if (this.transform.tag == "Champion" && pv.IsMine)
        {
            status_UI.OnSkillLevelUpButton();
            UpdateMpBar();
        }
        UpdateHpBar();
        UpdateStatus();
    }

    private void RegenHpMp()
    {
        currentRegenTime += Time.deltaTime;
        if (currentRegenTime >= 1f)
        {
            currentHp += hpRegen / 5f;
            currentMp += mpRegen / 5f;
            currentRegenTime = 0f;
            if(currentHp> maxHp)
            {
                currentHp = maxHp;
            }
            if(currentMp > maxMp)
            {
                currentMp = maxMp;
            }
        }
        UpdateStatus();
        UpdateHpBar();
        if(this.transform.tag == "Champion")
        UpdateMpBar();
    }
    private void champAttackReset()
    {
        champAttackResetTime += Time.deltaTime;
        if (champAttackResetTime >= 0.5f)
        {
            isbeHit = false;
            isChampAttack = false;
            champAttackResetTime = 0f;
            beHitTarget = null;
        }
        
    }
    public void Respawn()
    {
        pv.RPC("PRespawn", RpcTarget.All);
    }
    [PunRPC]
    public void PRespawn()
    {
        this.GetComponent<NavMeshAgent>().enabled = false;
        if (team == 2)
        {
            this.transform.position = new Vector3(5f, 0f, 5f);
        }
        else if (team == 1)
        {
            this.transform.position = new Vector3(205f, 0f, 205f);
        }
        
        isDead = false;
        ResetDebuff();
        currentHp = maxHp;
        currentMp = maxMp;
        UpdateStatus();
        UpdateHpBar();
        UpdateMpBar();
        currentTime = 0f;
        RespawnCheck();
        this.GetComponent<NavMeshAgent>().enabled = true;
        this.transform.GetComponent<BoxCollider>().enabled = true;
        anim.SetTrigger("Idle");
    }
    private void RespawnCheck()
    {
       this.transform.position = GameObject.Find("PlayerSetting").GetComponent<PlayerSetting>().PlayerPos();
    }
    private void CurrentBarrier()
    {
        for (int i = 0; i < barriers.Count; i++)
        {
            barriers[i].barrierTime -= Time.deltaTime;
            if (barriers[i].decreaseType == (int)Barrier.DecreaseType.DECREASETIME)
            {
                if (barriers[i].barrierTime <= barriers[i].barrierDecreaseTime)
                {
                    currentbarrier -= barriers[i].barrier * Time.deltaTime;
                    barriers[i].barrier -= barriers[i].barrier * Time.deltaTime; 
                }
                
            }
            if (barriers[i].barrierTime <= 0f)
            {
                currentbarrier -= barriers[i].barrier;
                //Debug.Log(barriers[i].barrierName + " " + barriers[i].barrier +" 해체");
                barriers.Remove(barriers[i]);
                i--;
            }
        }
        UpdateHpBar();
    }
    private void Slow()
    {
        if (addMoveSpeed < slowPer)
        {
            currentMoveSpeed = originMoveSpeed * (1 - ((slowPer - addMoveSpeed) / 100f));
        }
        else
        {
            currentMoveSpeed = originMoveSpeed * (1 + ((addMoveSpeed - slowPer) / 100f));
        }
    }
    public void ResetSlow()
    {
        currentMoveSpeed = originMoveSpeed + (originMoveSpeed * (addMoveSpeed / 100));
        slowPer = 0f;
        isSlow = false;
    }
    private void SetAirborne()
    {
        originPos = chTransform.transform.position;
        AirbornePos = new Vector3(chTransform.transform.position.x, chTransform.transform.position.y+(currentAirborneTime*5f), chTransform.transform.position.z);
    }
    private void Airborne()
    {
        if(!isEndAirborne)
            chTransform.transform.position = Vector3.Lerp(originPos, AirbornePos, currentTime);
        else if(isEndAirborne)
            chTransform.transform.position = Vector3.Lerp(AirbornePos, originPos, currentTime);
        if (currentTime >= 0.99f && !isEndAirborne)
        {
            //Debug.Log("위");
            currentTime = 0f;
            isEndAirborne = true;
        }
        else if (currentTime >= 0.99f && isEndAirborne)
        {
            chTransform.transform.position = originPos;
            this.transform.position = new Vector3(originPos.x,0.1f,originPos.z);
            //Debug.Log("아래");
            currentAirborneTime = 0f;
            isEndAirborne = false;
            isAirborne = false;
        }
    }
    private void currentdebuff()
    {
        currentCCTime -= Time.deltaTime;
        currentslowTime -= Time.deltaTime;
        if(isAirborne)
        {
            currentTime += (Time.deltaTime * (1/(currentAirborneTime/2f)));
            Airborne();
        }
        if (currentCCTime <= 0f && isCC)
        {
            isCC = false;
        }
        else if(currentslowTime <= 0f && isSlow)
        {
            ResetSlow();
        }
        for (int i = 0; i < debuffs.Count; i++)
        {
            debuffs[i].debuffTime -= Time.deltaTime;
            if (debuffs[i].debuffTime <= 0)
            {
                //Debug.Log(debuffs[i].debuffName + " 해체");
                if (debuffs[i].debuffType == (int)Debuff.DebuffType.SLOW)
                {
                    slowPer -= debuffs[i].debuffFigure;
                    if(slowPer <= 0)
                    {
                        slowPer = 0;
                    }
                }
                if (debuffs[i].debuffName == "동상" || debuffs[i].debuffName == "얼음 폭풍")
                    isHitAniviaQR = false;
                debuffs.RemoveAt(i);
                i--;
            }
        }

        if (debuffs.Count >= 1)
        {
            foreach (var item in debuffs)
            {
                if (item.debuffName == "동상" || item.debuffName == "얼음 폭풍")
                {
                    isHitAniviaQR = true;
                }
            }
        }
    }
    
    public void Damage(float _damage,string name, Status.AttackType attackType , float _perPenetrat , float _Penetrat)
    {
        if (currentHp <= 0f)
        {
            return;
        }
        pv.RPC("ChampDamage", RpcTarget.All,_damage,name,(int)attackType,_perPenetrat,_Penetrat);
    }
    [PunRPC]
    public void ChampDamage(float _damage, string _name, int attackType, float _perPenetrat, float _Penetrat)
    {
        GameObject _gameObject = GameObject.Find(_name);
        if (attackType == 0)
        {
            _damage = (_damage * (1 - (((100 * ((armor * (1 - (_perPenetrat / 100))) - _Penetrat)) /
                (100 + ((armor * (1 - (_perPenetrat / 100))) - _Penetrat))) / 100)));
        }
        else if (attackType == 1)
        {
            _damage = (_damage * (1 - (((100 * ((spellBlock * (1 - (_perPenetrat / 100))) - _Penetrat)) /
                (100 + ((spellBlock * (1 - (_perPenetrat / 100))) - _Penetrat))) / 100)));
        }
        if (this.transform.tag == "Champion" && _gameObject.transform.tag == "Champion")
        {
            _gameObject.GetComponent<Status>().isChampAttack = true;
            champAttackResetTime = 0f;
        }
        if (currentbarrier > 0f)
        {
            for (int i = 0; i < barriers.Count; i++)
            {
                if (_damage >= barriers[i].barrier)
                {
                    _damage -= barriers[i].barrier;
                    currentbarrier -= barriers[i].barrier;
                    //Debug.Log(this.gameObject.name + " 의hp " + currentbarrier);
                    //Debug.Log(barriers[i].barrierName + "해체");
                    barriers.Remove(barriers[i]);
                    i--;
                }
                else
                {
                    barriers[i].barrier -= _damage;
                    currentbarrier -= _damage;
                    _damage = 0f;
                    break;
                }
            }
        }
        if (_damage > 0f)
        {
            isbeHit = true;
            beHitTarget = _gameObject;
            currentHp -= _damage;
            //Debug.Log(this.gameObject.name + " 의hp " + currentHp);
        }
        UpdateHpBar();
        if (currentHp <= 0)
        {
            if (this.transform.tag == "Minion" && _gameObject.transform.tag == "Champion")
            {
                _gameObject.GetComponent<Status>().currentKillMinion += 1;
            }
            else if (this.transform.tag == "Champion" && _gameObject.transform.tag == "Champion")
            {
                _gameObject.GetComponent<Status>().currentKillChamp += 1;
            }
            if(this.transform.name == "BlueNexus" || this.transform.name == "RedNexus")
            {

            }
            else
            {
                _gameObject.GetComponent<Status>().GetGold(killgold);
            }
            Dead();
        }
    }
    public void GetGold(float _killGold)
    {
        if(this.transform.tag != "Champion")
        {
            return;
        }
        pv.RPC("GetKillGold", RpcTarget.All, _killGold);
    }
    [PunRPC]
    public void GetKillGold(float _killGold)
    {
        if (pv.IsMine)
        {
            currentGold += _killGold;
            status_UI.ShowGetGoldUI(_killGold);
        }
    }
    public void Damage(float _damage)
    {
        if (currentHp <= 0f)
        {
            return;
        }
        pv.RPC("MinionDamage",RpcTarget.All,_damage);
    }
    [PunRPC]
    public void MinionDamage(float _damage)
    {
        _damage = _damage * (1 - (((100 * armor) / (100 + armor)) / 100));
        if (currentbarrier > 0f)
        {
            for (int i = 0; i < barriers.Count; i++)
            {
                if (_damage >= barriers[i].barrier)
                {
                    _damage -= barriers[i].barrier;
                    currentbarrier -= barriers[i].barrier;
                    //Debug.Log(this.gameObject.name + " 의hp " + currentbarrier);
                    //Debug.Log(barriers[i].barrierName + "해체");
                    barriers.Remove(barriers[i]);
                    i--;
                }
                else
                {
                    barriers[i].barrier -= _damage;
                    currentbarrier -= _damage;
                    _damage = 0f;
                    break;
                }
            }
        }
        if (_damage > 0f)
        {
            currentHp -= _damage;
        }
        UpdateHpBar();
        if (currentHp <= 0)
        {
            Dead();
        }
    }
    private void ResetDebuff()
    {
        isDebuff = false;
        isHitAniviaQR = false;
        isCC = false;
        ResetSlow();
        currentslowTime = 0f;
        currentCCTime = 0f;
        debuffs.Clear();
    }
   
    public void HitDebuff(string _debuffname,float _debuffTime, int _debuffType)
    {
        pv.RPC("ADebuff", RpcTarget.All, _debuffname, _debuffTime, _debuffType);
    }
    [PunRPC]
    public void ADebuff(string _debuffname, float _debuffTime, int _debuffType)
    {
        debuffs.Add(new Debuff() { debuffName = _debuffname, debuffTime = _debuffTime, debuffType = _debuffType });
        isDebuff = true;
        if (debuffs[debuffs.Count - 1].debuffType == (int)Debuff.DebuffType.CC)
        {

            if (this.transform.tag == "Champion")
            {
                StartCoroutine(DebuffCoroutine("Stun", _debuffTime));
            }
            currentCCTime = debuffs[debuffs.Count - 1].debuffTime;
            isCC = true;
        }
        else if (debuffs[debuffs.Count - 1].debuffType == (int)Debuff.DebuffType.SLOW)
        {
            currentslowTime = debuffs[debuffs.Count - 1].debuffTime;
            isSlow = true;
        }
        else if (debuffs[debuffs.Count - 1].debuffType == (int)Debuff.DebuffType.AIRBORNE)
        {

            if (this.transform.tag == "Champion")
            {
                StartCoroutine(DebuffCoroutine("Airborne", _debuffTime));
            }
            currentAirborneTime = debuffs[debuffs.Count - 1].debuffTime;
            isAirborne = true;
            SetAirborne();
        }
    }
    public void HitDebuff(string _debuffname, float _debuffTime, float _debuffFigure ,int _debuffType)
    {
        pv.RPC("ResDebuff", RpcTarget.All, _debuffname, _debuffTime, _debuffFigure, _debuffType);
    }
    [PunRPC]
    public void ResDebuff(string _debuffname, float _debuffTime, float _debuffFigure, int _debuffType)
    {
        debuffs.Add(new Debuff() { debuffName = _debuffname, debuffTime = _debuffTime, debuffType = _debuffType, debuffFigure = _debuffFigure });
        isDebuff = true;
        if (debuffs[debuffs.Count - 1].debuffType == (int)Debuff.DebuffType.CC)
        {
            if (this.transform.tag == "Champion")
            {
                StartCoroutine(DebuffCoroutine("Stun", _debuffTime));
            }
            currentCCTime = debuffs[debuffs.Count - 1].debuffTime;
            isCC = true;
        }
        else if (debuffs[debuffs.Count - 1].debuffType == (int)Debuff.DebuffType.SLOW || debuffs[debuffs.Count - 1].debuffType == (int)Debuff.DebuffType.RESTRICTION)
        {
            if (this.transform.tag == "Champion")
            {
                StartCoroutine(DebuffCoroutine("RESTRICTION", _debuffTime));
            }
            slowPer += _debuffFigure;
            currentslowTime = debuffs[debuffs.Count - 1].debuffTime;
            isSlow = true;
        }
    }
    public void GetBarrier(string _varrierName ,float _varrier,float _varrierTime , float _varrierDecreaseTime,int _decreaseType)
    {
        pv.RPC("AGetBarrier", RpcTarget.All, _varrierName, _varrier, _varrierTime, _varrierDecreaseTime, _decreaseType);
    }
    [PunRPC]
    public void AGetBarrier(string _varrierName, float _varrier, float _varrierTime, float _varrierDecreaseTime, int _decreaseType)
    {
        currentbarrier += _varrier;
        for (int i = 0; i < barriers.Count; i++)
        {
            if (barriers[i].barrierName == _varrierName)
            {
                barriers[i].barrier += _varrier;
                barriers[i].barrierTime = _varrierTime;
                isSameVarrier = true;
            }
        }
        if (!isSameVarrier)
            barriers.Add(new Barrier() { barrierName = _varrierName, barrier = _varrier, barrierTime = _varrierTime, barrierDecreaseTime = _varrierDecreaseTime, decreaseType = _decreaseType });
        isSameVarrier = false;
    }
    public void GetBarrier(string _varrierName, float _varrier, float _varrierTime)
    {
        pv.RPC("BGetBarrier", RpcTarget.All, _varrierName, _varrier, _varrierTime);
    }
    [PunRPC]
    public void BGetBarrier(string _varrierName, float _varrier, float _varrierTime)
    {
        currentbarrier += _varrier;
        for (int i = 0; i < barriers.Count; i++)
        {
            if (barriers[i].barrierName == _varrierName)
            {
                barriers[i].barrier += _varrier;
                barriers[i].barrierTime = _varrierTime;
                isSameVarrier = true;
            }
        }
        if (!isSameVarrier)
            barriers.Add(new Barrier() { barrierName = _varrierName, barrier = _varrier, barrierTime = _varrierTime });
        isSameVarrier = false;
    }
    public void Healing(float _heal)
    {
        pv.RPC("AHeal", RpcTarget.All, _heal);
    }
    [PunRPC]
    public void AHeal(float _heal)
    {
        currentHp += _heal;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }
    public void Dead()
    {
        pv.RPC("PDead", RpcTarget.All);
    }
    private IEnumerator DeadCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        this.GetComponent<BoxCollider>().enabled = false;
        if (this.transform.tag != "Minion" && this.transform.tag != "Building")
        { 
            anim.SetTrigger("Dead");
        }
        else if(this.transform.tag == "Minion")
        {
            if (pv.IsMine)
            {
                yield return new WaitForSeconds(1f);
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
        yield return new WaitForSeconds(3f);
        if (pv.IsMine)
        {
            if (this.transform.tag != "Champion")
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
    [PunRPC]
    public void PDead()
    {
        if (!isDead)
        {
            isDead = true;
            this.transform.position = new Vector3(chTransform.position.x, chTransform.position.y, chTransform.position.z);
            StartCoroutine(DeadCoroutine());
            if (this.transform.tag == "Champion")
            {
                GetComponent<Controller>().MoveCancel();
                if (pv.IsMine)
                {
                    status_UI = GameObject.Find("PlayerCanvas").transform.Find("Status_UI").GetComponent<Status_UI>();
                    status_UI.Dead();
                }
            }
            else if (this.transform.name == "BlueNexus" || this.transform.name == "RedNexus")
            {
                GameObject.Find("InGameManager").GetComponent<InGameManager>().CompleteGame(team);
            }
            currentTime = 0f;
        }
    }
    private void Look(Vector3 _target)
    {
        chTransform.forward = _target - chTransform.position;
    }
    public void RPCKnockBack(float _force, Vector3 _target)
    {
        pv.RPC("KnockBack", RpcTarget.All, _force, _target);
    }
    [PunRPC]
    public void KnockBack(float _force,Vector3 _target)
    {
        wallCheck.gameObject.SetActive(true);
        isKnockBack = true;
        Look(_target);
        force = _force;
        originPos = chTransform.position;
        KnockBackPos = chTransform.position - (chTransform.forward * force);
        KnockBackTime = 0f;
    }
    private void PlayKnockBack()
    {
        if (Vector3.Distance(chTransform.position, KnockBackPos) <= 0.1f || isNearWall)
        {
            this.transform.position = this.transform.position;
            wallCheck.gameObject.SetActive(false);
            isNearWall = false;
            isKnockBack = false;
            force = 0f;
            KnockBackTime = 0f;
            return;
        }
        KnockBackTime += Time.deltaTime * 2f;
        this.transform.position = Vector3.Lerp(originPos, KnockBackPos, KnockBackTime);
    }
    public void GetItem(Item _item)
    {
        maxHp += _item.hp;
        currentHp += _item.hp;
        currentMp += _item.mp;
        maxMp += _item.mp;
        attackDamage += _item.ad;
        spell += _item.ap;
        armor += _item.armor;
        spellBlock += _item.spellblock;
        ADpenetration += _item.ADpenetration;
        ADperPenetration += _item.ADperPenetration;
        APpenetration += _item.APpenetration;
        APperPenetration += _item.APperPenetration;
        addAttackSpeed += _item.attackspeedPer;
        addMoveSpeed += _item.moveSpeedPer;
        originMoveSpeed += (_item.moveSpeed/60f);
        crit += _item.crit;
        UpdateStatus();
        UpdateSpeed();
    }
    public void UpdateSpeed()
    {
        currentMoveSpeed = originMoveSpeed + (originMoveSpeed * (addMoveSpeed / 100));
        attackSpeed = originAttackSpeed + (originAttackSpeed * (addAttackSpeed / 100f));
        UpdateStatus();
    }
    private void UpdateHpBar()
    {
        hpBar.fillAmount = currentHp / maxHp;
        if (currentbarrier + currentHp > maxHp)
        {
            hpBar.fillAmount = currentHp / (currentHp + currentbarrier);
            barrierBar.fillAmount = 1f;
        }
        barrierBar.fillAmount = (currentHp + currentbarrier) / maxHp;
    }
    private void UpdateMpBar()
    {
        if(this.transform.tag == "Champion")
        mpBar.fillAmount = currentMp / maxMp;
    }
    public void SellItem(Item _item)
    {
        maxHp -= _item.hp;
        currentHp -= _item.hp;
        currentMp -= _item.mp;
        maxMp -= _item.mp;
        attackDamage -= _item.ad;
        spell -= _item.ap;
        armor -= _item.armor;
        spellBlock -= _item.spellblock;
        ADpenetration -= _item.ADpenetration;
        ADperPenetration -= _item.ADperPenetration;
        APpenetration -= _item.APpenetration;
        APperPenetration -= _item.APperPenetration;
        addAttackSpeed -= _item.attackspeedPer;
        addMoveSpeed -= _item.moveSpeedPer;
        originMoveSpeed -= _item.moveSpeed;
        crit -= _item.crit;
        UpdateSpeed();
    }

    private IEnumerator DebuffCoroutine(string debuffType,float debuffTime)
    {
        ccText.GetComponent<TextMeshPro>().text = debuffType;
        yield return new WaitForSeconds(debuffTime);
        ccText.GetComponent<TextMeshPro>().text = champName;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(team);
            stream.SendNext(champName);
            stream.SendNext(currentHp);
            stream.SendNext(maxHp);
            stream.SendNext(currentMp);
            stream.SendNext(maxMp);
            stream.SendNext(attackDamage);
            stream.SendNext(currentMoveSpeed);
        }
        else { 
            team = (int)stream.ReceiveNext();
            this.transform.name = (string)stream.ReceiveNext();
            currentHp = (float)stream.ReceiveNext();
            maxHp = (float)stream.ReceiveNext();
            currentMp = (float)stream.ReceiveNext();
            maxMp = (float)stream.ReceiveNext();
            attackDamage = (float)stream.ReceiveNext();
            currentMoveSpeed = (float)stream.ReceiveNext();
        }
    }
}
