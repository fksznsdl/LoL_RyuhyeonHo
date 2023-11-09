using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PickWindowManager : MonoBehaviour,IOnEventCallback
{
    [SerializeField]
    private Text remainTimetext;
    [SerializeField]
    private Image bluePick;
    [SerializeField]
    private Image redPick;
    [SerializeField]
    private Sprite[] champSprites;
    [SerializeField]
    private PlayerSetting ps;
    [SerializeField]
    private Text currentTurnText;
    private bool isReady;
    private string currentTurn;
    private int turnCount;
    private float currentRemainTime;

    private string champPick;
    private string redPickName;
    private string bluePickName;
    public void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code == (byte)EventCodeValues.EventCode.BLUE_TURN)
        {
            currentTurn = "Blue";
            currentRemainTime = 60f;
            turnCount = 1;
            isReady = false;
        }
        else if (photonEvent.Code == (byte)EventCodeValues.EventCode.RED_TURN)
        {
            currentTurn = "Red";
            currentRemainTime = 60f;
            turnCount = 2;
            isReady = false;
        }
        else if(photonEvent.Code == (byte)EventCodeValues.EventCode.READY)
        {
            isReady = true;
            StartCoroutine(ReadyCoroutine());
        }
        else if(photonEvent.Code == (byte)EventCodeValues.EventCode.PICK_CHANGE)
        {
            string name = (string)((ExitGames.Client.Photon.Hashtable)photonEvent.CustomData)["ChampName"];
            if(currentTurn == "Blue")
            {
                bluePickName = name;
                ps.blueChampPick = name;
            }
            else if(currentTurn== "Red")
            {
                redPickName = name;
                ps.redChampPick = name;
            }
            UpdatePickImage(name);
        }
    }
   IEnumerator ReadyCoroutine()
    {
        remainTimetext.text = string.Format("로딩중");
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("IngameScene");
    }
    // Start is called before the first frame update
    void Awake()
    {
        isReady = true;
        turnCount = 0;
        NetworkManager.Instance.Getclient.AddCallbackTarget(this);
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
        raiseEventOptions.Receivers = ReceiverGroup.All;
        if (NetworkManager.Instance.Getclient.LocalPlayer.IsMasterClient)
        {
            ps.team = "Blue";
        }
        else
        {
            ps.team = "Red";
        }
        NetworkManager.Instance.Getclient.OpRaiseEvent((byte)EventCodeValues.EventCode.BLUE_TURN, 0, raiseEventOptions, SendOptions.SendReliable);
    }
    void OnDisable()
    {
        NetworkManager.Instance.Getclient.RemoveCallbackTarget(this);
    }
    // Update is called once per frame
    void Update()
    {
        if (!isReady)
        {
            currentRemainTime -= Time.deltaTime;
            UpdateRemainTimeText();
            if (currentRemainTime <= 0f)
            {
                NextTurn();
            }
        }
        CheckTurn();
    }
    private void NextTurn()
    {
        isReady = true;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
        raiseEventOptions.Receivers = ReceiverGroup.All;
        if(champPick == null)
        {
            while (champPick == null)
            {
                int number = Random.Range(0, 4);
                switch (number) { 
                    case 0:
                        champPick = "Veiga";
                        break;
                    case 1:
                        champPick = "Lux";
                        break;
                    case 2:
                        champPick = "Anivia";
                        break;
                    case 3:
                        champPick = "Janna";
                        break;
                }
                if (champPick == redPickName || champPick == bluePickName)
                {
                    champPick = null;
                }
            }
            UpdatePickImage(champPick);
        }
        if(ps.team == currentTurn)
        {
            ps.champName = champPick;
        }
        if(turnCount >= 2)
        {
            NetworkManager.Instance.Getclient.OpRaiseEvent((byte)EventCodeValues.EventCode.READY, 0, raiseEventOptions, SendOptions.SendReliable);
        }
        else if (currentTurn == "Blue")
        {
            
            champPick = null;
            NetworkManager.Instance.Getclient.OpRaiseEvent((byte)EventCodeValues.EventCode.RED_TURN, 0, raiseEventOptions, SendOptions.SendReliable);
        }
        else if(currentTurn == "Red")
        {
            
            champPick = null;
            NetworkManager.Instance.Getclient.OpRaiseEvent((byte)EventCodeValues.EventCode.BLUE_TURN, 0, raiseEventOptions, SendOptions.SendReliable);
        }
    }
    private void UpdateRemainTimeText()
    {
        remainTimetext.text = Mathf.FloorToInt(currentRemainTime).ToString();
    }
    public void ReadyButton()
    {
        if (ps.team == currentTurn)
        {
            if(champPick != null)
            {
                NextTurn();
            }
        }
    }
    private void UpdatePickImage(string _name)
    {
        int number = 4;
        switch (_name)
        {
            case "Veiga":
                number = 0;
                break;
            case "Lux":
                number = 1;
                break;
            case "Anivia":
                number = 2;
                break;
            case "Janna":
                number = 3;
                break;
        }
        if (currentTurn == "Blue")
        {
            bluePick.sprite = champSprites[number];
        }
        else if (currentTurn == "Red")
        {
            redPick.sprite = champSprites[number];
        }
    }
    public void ClickToChamp(string _name)
    {
        if (ps.team == currentTurn)
        {
            if(bluePickName == _name)
            {
                return;
            }
            else if (redPickName == _name)
            {
                return;
            }
            champPick = _name;
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash["ChampName"] = _name; // 선택한 챔피언 이름을 보내주는 해시테이블
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
            raiseEventOptions.Receivers = ReceiverGroup.All;
            NetworkManager.Instance.Getclient.OpRaiseEvent((byte)EventCodeValues.EventCode.PICK_CHANGE, hash, raiseEventOptions, SendOptions.SendReliable);
        }
    }
    public void CheckTurn()
    {
        if(currentTurn ==ps.team)
        {
            currentTurnText.text = "내 턴";
        }
        else
        {
            currentTurnText.text = "적 턴";
        }
    }
}
