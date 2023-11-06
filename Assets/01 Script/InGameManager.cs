
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class InGameManager : MonoBehaviour, IPunObservable, IOnEventCallback
{
    private PhotonView pv;
    private PlayerSetting player;
    [SerializeField]
    private Text blueTeamKill_txt;
    [SerializeField]
    private Text redTeamKill_txt;
    [SerializeField]
    private Text inGameTimer_txt;

    [SerializeField]
    private GameObject[] canvas;
    [SerializeField]
    private GameObject[] completeGamePanel;


    [SerializeField]
    private string[] blueTeamMinion;
    [SerializeField]
    private string[] redTeamMinion;
    [SerializeField]
    private string[] monsters;
    private bool[] isSurviveMonsters;
    [SerializeField]
    private Vector3[] monstersSpawnSpots;
    [SerializeField]
    private Vector3[] blueMinionsSpawnSpots;
    [SerializeField]
    private Vector3[] redMinionsSpawnSpots;

    private float[] monstersReSpawnTime;
    private float[] monsterFirstSpawnTime;
    private float[] monsterSpawnTimer;

    public static int BlueGolem = 0, Dragon = 1, Gromp = 2, CrimsonRaptor = 3, Raptor = 4, RedGolem = 5, RockTurtle = 6,
        SmallRockTurtle = 7, Wolf = 8;
    public int red_inBlueGoelm = 9, red_inGromp = 10, red_inCrimsonRaptor = 11, red_inRaptor = 12, red_inRedGolem = 13, red_inRockTurtle = 14,
        red_inSmallRockTurtle = 15, red_inWolf = 16;

    public float ingameTime;
    public int ingameSec;
    public int ingameMin;
    private int ingameTotalSec;
    private float inGameStartTime;

    private int minionSpawnCount;

    public int blueTeamKill;
    public int redTeamKill;

    private bool isMaster;
    private bool isComplete;
    private void Awake()
    {
        NetworkManager.Instance.Getclient.AddCallbackTarget(this);
        pv = GetComponent<PhotonView>();
        player = GameObject.Find("PlayerSetting").GetComponent<PlayerSetting>();
        player.PlayerSpawn();
        isMaster = PhotonNetwork.IsMasterClient;
        inGameStartTime = Time.time;
        ingameTime = 0f;
        ingameMin = 0;
        ingameTotalSec = 0;
        ingameSec = 0;

        isComplete = false;

        blueTeamKill = 0;
        redTeamKill = 0;

        minionSpawnCount = 0;

        isSurviveMonsters = new bool[17] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        monstersReSpawnTime = new float[17] { 300f, 360f, 135f, 135f, 135f, 300f, 135f, 135f, 135f, 300f, 135f, 135f, 135f, 300f, 135f, 135f, 135f };
        monsterFirstSpawnTime = new float[17] { 89f, 150f, 102f, 90f, 91f, 92f, 103f, 102f, 93f, 94f, 104f, 95f, 96f, 97f, 105f, 106f, 98f };
        monsterSpawnTimer = new float[17];
        monsterSpawnTimer = monsterFirstSpawnTime;
    }
    private void Update()
    {
        if (!isComplete)
        {
            if (isMaster)
            {
                ingameTime = Time.time - inGameStartTime;
                UpdateSpawn();
                UpdateTimer();
            }
            else
            {
                inGameTimer_txt.text = string.Format("{0} : {1}", ingameMin, ingameSec);
            }
        }
    }

    private void OnDisable()
    {
        NetworkManager.Instance.Getclient.RemoveCallbackTarget(this);
    }
    private void UpdateTimer()
    {
        if (ingameTime >= ingameTotalSec + 1)
        {
            ingameSec++;
            ingameTotalSec++;
        }
        if (ingameSec >= 60)
        {
            ingameMin++;
            ingameSec = 0;
        }
        inGameTimer_txt.text = string.Format("{0} : {1}", ingameMin, ingameSec);
    }
    private void UpdateSpawn()
    {
        for (int i = 0; i < monsterSpawnTimer.Length; i++)
        {
            if (isSurviveMonsters[i] == false)
            {
                monsterSpawnTimer[i] -= Time.deltaTime;
                if (monsterSpawnTimer[i] <= 0f)
                {
                    MonsterSpawn(i);
                }
            }
        }
        if (ingameTime >= 65f)
        {
            if ((minionSpawnCount * 30) < (ingameTime - 65f) || minionSpawnCount == 0)
            {
                minionSpawnCount++;
                StartCoroutine(MinionSpawnCoroutine());
            }
        }
    }
    private IEnumerator MinionSpawnCoroutine()
    {

        for (int i = 0; i < 3; i++) {
            var clone = PhotonNetwork.Instantiate(blueTeamMinion[0], blueMinionsSpawnSpots[0], Quaternion.identity);
            var clone1 = PhotonNetwork.Instantiate(blueTeamMinion[1], blueMinionsSpawnSpots[1], Quaternion.identity);
            var clone2 = PhotonNetwork.Instantiate(blueTeamMinion[2], blueMinionsSpawnSpots[2], Quaternion.identity);
            var clone3 = PhotonNetwork.Instantiate(redTeamMinion[0], redMinionsSpawnSpots[0], Quaternion.identity);
            var clone4 = PhotonNetwork.Instantiate(redTeamMinion[1], redMinionsSpawnSpots[1], Quaternion.identity);
            var clone5 = PhotonNetwork.Instantiate(redTeamMinion[2], redMinionsSpawnSpots[2], Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
        for (int i = 0; i < 3; i++)
        {
            var clone = PhotonNetwork.Instantiate(blueTeamMinion[3], blueMinionsSpawnSpots[0], Quaternion.identity);
            var clone1 = PhotonNetwork.Instantiate(blueTeamMinion[4], blueMinionsSpawnSpots[1], Quaternion.identity);
            var clone2 = PhotonNetwork.Instantiate(blueTeamMinion[5], blueMinionsSpawnSpots[2], Quaternion.identity);
            var clone3 = PhotonNetwork.Instantiate(redTeamMinion[3], redMinionsSpawnSpots[0], Quaternion.identity);
            var clone4 = PhotonNetwork.Instantiate(redTeamMinion[4], redMinionsSpawnSpots[1], Quaternion.identity);
            var clone5 = PhotonNetwork.Instantiate(redTeamMinion[5], redMinionsSpawnSpots[2], Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
    }
    public void MonsterSpawn(int number)
    {
        var clone = PhotonNetwork.Instantiate(monsters[number], monstersSpawnSpots[number], Quaternion.identity);
        isSurviveMonsters[number] = true;
    }
    public void SetSpawnTime(int number)
    {
        isSurviveMonsters[number] = false;
        monsterSpawnTimer[number] = monstersReSpawnTime[number];
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

            stream.SendNext(ingameMin);
            stream.SendNext(ingameSec);
        }
        else
        {

            ingameMin = (int)stream.ReceiveNext();
            ingameSec = (int)stream.ReceiveNext();

        }
    }
    private void UpdateKillScore()
    {
        blueTeamKill_txt.text = blueTeamKill.ToString();
        redTeamKill_txt.text = redTeamKill.ToString();
    }

    public void LevelUpButton()
    {
        GameObject.Find(player.champName).GetComponent<Status>().LevelUp();
    }

    public void CompleteGame(int _team)
    {
        pv.RPC("PComplete", RpcTarget.All, _team);
    }
    [PunRPC]
    public void PComplete(int _team)
    {
        isComplete = true;
        string team = null;
        if (_team == (int)Status.Team.RED)
        {
            team = "Red";
        }
        else if (_team == (int)Status.Team.BLUE)
        {
            team = "Blue";
        }
        canvas[0].gameObject.SetActive(false);
        canvas[1].gameObject.SetActive(true);
        if (player.team != team)
        {
            completeGamePanel[0].gameObject.SetActive(true);
        }
        else
        {
            completeGamePanel[1].gameObject.SetActive(true);
        }
    }
    public void RedTeamDead()
    {
        pv.RPC("RedDead", RpcTarget.All);
    }
    [PunRPC]
    public void RedDead()
    {
        blueTeamKill++;
        blueTeamKill_txt.text = blueTeamKill.ToString();
        if(player.team == "Blue")
        {
            GameObject.Find(player.champName).GetComponent<Status>().GetGold(300f);
        }
    }
    public void BlueTeamDead()
    {
        pv.RPC("BlueDead", RpcTarget.All);
    }
    [PunRPC]
    public void BlueDead()
    {
        redTeamKill++;
        redTeamKill_txt.text = redTeamKill.ToString();
        if (player.team == "Red")
        {
            GameObject.Find(player.champName).GetComponent<Status>().GetGold(300f);
        }
    }
    public void ExitGame()
    {
        StartCoroutine(StayCoroutine());
    }
    public IEnumerator StayCoroutine()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
        raiseEventOptions.Receivers = ReceiverGroup.Others;
        NetworkManager.Instance.Getclient.OpRaiseEvent((byte)EventCodeValues.EventCode.EXIT_GAME, 0, raiseEventOptions, SendOptions.SendReliable);
        yield return new WaitForSeconds(0.3f);
        PhotonNetwork.LeaveRoom();
        NetworkManager.Instance.Getclient.OpLeaveRoom(false);
        Application.Quit();
    }
    public void QuitGame()
    {
        PhotonNetwork.LeaveRoom();
        NetworkManager.Instance.Getclient.OpLeaveRoom(false);
        Application.Quit();
    }

    public void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code == (byte)EventCodeValues.EventCode.EXIT_GAME)
        {
            canvas[0].gameObject.SetActive(false);
            canvas[1].gameObject.SetActive(true);
            completeGamePanel[2].gameObject.SetActive(true);
            GameObject.Find(player.champName).GetComponent<BoxCollider>().enabled = false;
            GameObject.Find(player.champName).GetComponent<Controller>().enabled = false;
        }
    }
}
