using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TestGameManager : MonoBehaviourPunCallbacks
{
    private PlayerSetting ps;
    [SerializeField]
    private GameObject canvas;
    // Start is called before the first frame update
    void Awake()
    {
        ps = GameObject.Find("PlayerSetting").GetComponent<PlayerSetting>();
        Screen.SetResolution(1280, 720, false);
        if(ps.team == "Blue")
        {
            var clone = PhotonNetwork.Instantiate(ps.champName, new Vector3(5f, 0f, 5f), Quaternion.identity);
            clone.name = ps.champName;
        }
        else
        {
            var clone = PhotonNetwork.Instantiate(ps.champName, new Vector3(15f, 0f, 15f), Quaternion.identity);
            clone.name = ps.champName;
        }
    }
    public void levelUpButton()
    {
        GameObject.Find(ps.champName).GetComponent<Status>().LevelUp();
    } 
}
