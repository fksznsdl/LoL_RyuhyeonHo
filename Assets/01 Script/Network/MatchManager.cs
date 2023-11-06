using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class MatchManager : MonoBehaviour , IOnEventCallback , IInRoomCallbacks,IMatchmakingCallbacks
{
    protected int currentPlayerCount = 1;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    // 规 积己
    public virtual void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        EnterRoomParams enterRoomParams = new EnterRoomParams();
        enterRoomParams.RoomOptions = roomOptions;

        NetworkManager.Instance.Getclient.OpCreateRoom(enterRoomParams);
    }
    // 规 曼啊
    public void JoinRoom(string name)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        TypedLobby typedLobby = new TypedLobby(name, LobbyType.SqlLobby);

        NetworkManager.Instance.Getclient.OpJoinLobby(typedLobby);
    }
    
    public IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("PickWindowScene");
    }

    public void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code == (byte)EventCodeValues.EventCode.START_GAME)
        {
            Debug.Log("StartGame");
            StartCoroutine(StartGameCoroutine());
        }
    }

    protected void Subscribe()
    {
        NetworkManager.Instance.Getclient.AddCallbackTarget(this);
    }

    protected void UnSubscribe()
    {
        NetworkManager.Instance.Getclient.RemoveCallbackTarget(this);
    }

    #region IInRoomCallbacks
    public virtual void OnPlayerEnteredRoom(Player newPlayer)
    {

    }

    public virtual void OnPlayerLeftRoom(Player otherPlayer)
    {

    }

    public virtual void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {

    }

    public virtual void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {

    }

    public virtual void OnMasterClientSwitched(Player newMasterClient)
    {

    }

    public virtual void OnFriendListUpdate(List<FriendInfo> friendList)
    {

    }
    #endregion

    #region IMatchmakingCallbacks
    public virtual void OnCreatedRoom()
    {
    }

    public virtual void OnCreateRoomFailed(short returnCode, string message)
    {

    }

    public virtual void OnJoinedRoom()
    {

    }

    public virtual void OnJoinRoomFailed(short returnCode, string message)
    {

    }

    public virtual void OnJoinRandomFailed(short returnCode, string message)
    {

    }

    public virtual void OnLeftRoom()
    {

    }
    #endregion
}
