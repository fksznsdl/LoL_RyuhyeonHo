using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerSetting : MonoBehaviourPunCallbacks,IConnectionCallbacks
{
    public string team;
    public string champName;

    public string blueChampPick;
    public string redChampPick;

    private readonly string RED = "Red", BLUE = "Blue";

    private readonly string RoomName = "A";

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        NetworkManager.Instance.Getclient.AddCallbackTarget(this);
        PhotonNetwork.AddCallbackTarget(this);
        PhotonNetwork.AutomaticallySyncScene = true;
        JoinRoom();
    }
    void JoinRoom()
    {
        PhotonNetwork.ConnectUsingSettings(); // Pun 辑滚 立加
        StartCoroutine(StayCoroutine());
    }
    private IEnumerator StayCoroutine()
    {
        yield return new WaitForSeconds(5f);
        if(team == RED)
        {
            PhotonNetwork.CreateRoom(RoomName, new RoomOptions { MaxPlayers = 2 }); // 规 积己
        }
        else
        {
            yield return new WaitForSeconds(5f);
            PhotonNetwork.JoinRoom(RoomName); // 规 立加
        }
    }
    public override void OnDisable()
    {
        NetworkManager.Instance.Getclient.RemoveCallbackTarget(this);
        PhotonNetwork.RemoveCallbackTarget(this);
        PhotonNetwork.Disconnect();
    }
    public void PlayerSpawn()
    {
        if (team == BLUE)
        {
            var clone = PhotonNetwork.Instantiate(champName, PlayerPos(), Quaternion.identity);
            clone.name = champName;
        }
        else
        {
            var clone = PhotonNetwork.Instantiate(champName, PlayerPos(), Quaternion.identity);
            clone.name = champName;
        }
    }
    public Vector3 PlayerPos()
    {
        Vector3 pos = new Vector3();
        if (team == BLUE)
        {
            pos = new Vector3(5f, 0f, 5f);
        }
        else if(team == RED)
        {
            pos = new Vector3(205f, 0f, 205f);
        }
        return pos;
    }
}
