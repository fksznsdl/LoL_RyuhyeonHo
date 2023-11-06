using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class NetworkManager : MonoBehaviour,IConnectionCallbacks
{
    public static NetworkManager Instance {  get; private set; }
    private readonly LoadBalancingClient client = new LoadBalancingClient();
    public LoadBalancingClient Getclient { get { return client; } }
    public bool IsMasterClient { get { return client.LocalPlayer.IsMasterClient; } }
    #region
    public void OnConnected()
    {
    }

    public void OnConnectedToMaster()
    {
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
    }

    public void OnDisconnected(DisconnectCause cause)
    {
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
    }
    #endregion
    private void Awake()
    {
        Instance = this;
        Screen.SetResolution(1920, 1080, true);
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        client.AddCallbackTarget(this);
        client.ClientType = ClientAppType.Realtime;
        client.ConnectUsingSettings(new AppSettings() { AppIdRealtime = "d9185cbe-622c-496d-a184-843dd1453698", FixedRegion = "kr" });
    }

    // Update is called once per frame
    void Update()
    {
        client.Service();
    }
    private void OnDisable()
    {
        client.RemoveCallbackTarget(this);
    }
}
