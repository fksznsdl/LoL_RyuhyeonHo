using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickMatch : MatchManager
{
    [SerializeField]
    private Text currentPlayerCount_text;
    [SerializeField]
    private GameObject connectPanel;
    private int MaxPlayers = 2;
    private void Start()
    {
        Subscribe();
    }
    private void OnDisable()
    {
        NetworkManager.Instance.Getclient.OpLeaveRoom(false);
        UnSubscribe();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        currentPlayerCount = NetworkManager.Instance.Getclient.CurrentRoom.PlayerCount;
        UpdateCurrentPlayerText();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        currentPlayerCount = NetworkManager.Instance.Getclient.CurrentRoom.PlayerCount;
        UpdateCurrentPlayerText();
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
    }

    public override void OnJoinedRoom()
    {
        currentPlayerCount = NetworkManager.Instance.Getclient.CurrentRoom.PlayerCount;
        UpdateCurrentPlayerText();

        if(currentPlayerCount == MaxPlayers)
        {
            RaiseEventOptions eventOptions = new RaiseEventOptions();
            eventOptions.Receivers = ReceiverGroup.All;
            NetworkManager.Instance.Getclient.OpRaiseEvent((byte)EventCodeValues.EventCode.START_GAME,0,eventOptions,SendOptions.SendReliable);
        }
    }

    public void QuickMatchConnect()
    {
        ExitGames.Client.Photon.Hashtable property = new ExitGames.Client.Photon.Hashtable();
        property["MatchType"] = "Quick";

        OpJoinRandomRoomParams opJoinRandomRoomParams = new OpJoinRandomRoomParams();
        opJoinRandomRoomParams.ExpectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        opJoinRandomRoomParams.ExpectedMaxPlayers = (byte)MaxPlayers;
        opJoinRandomRoomParams.ExpectedCustomRoomProperties = property;

        EnterRoomParams enterRoomParams = new EnterRoomParams();
        enterRoomParams.RoomOptions = new RoomOptions();
        enterRoomParams.RoomOptions.IsOpen = true;
        enterRoomParams.RoomOptions.IsVisible = true;
        enterRoomParams.RoomOptions.MaxPlayers = MaxPlayers;
        enterRoomParams.RoomOptions.CustomRoomProperties = property;
        enterRoomParams.RoomOptions.CustomRoomPropertiesForLobby = new string[]
        {
            "MatchType"
         };
        connectPanel.gameObject.SetActive(true);
        NetworkManager.Instance.Getclient.OpJoinRandomOrCreateRoom(opJoinRandomRoomParams, enterRoomParams);
    }
    public void QuickMatchDisconnect()
    {
        NetworkManager.Instance.Getclient.OpLeaveRoom(true);
        connectPanel.gameObject.SetActive(false);
    }
    public void UpdateCurrentPlayerText()
    {
        currentPlayerCount_text.text = string.Format(currentPlayerCount_text.text, currentPlayerCount, MaxPlayers);
    }
}
