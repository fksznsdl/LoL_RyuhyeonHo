using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private GameObject ShopPanel;

    [SerializeField]
    private int team;

    private Status status;
    private Transform pos;
    private void Update()
    {
        if(status != null)
        {
            UpdateShop();
        }
    }
    private void UpdateShop()
    {
        if(Vector3.Distance(this.transform.position,pos.position) >= 15f)
        {
            status.isShop = false;
            status = null;
            pos = null;
            ShopPanel.gameObject.SetActive(false);
        }
    }
    public void ShowShop(Status _status, Transform _pos)
    {
        if(team == _status.team)
        {
            if(Vector3.Distance(this.transform.position,_pos.position) <= 15f)
            {
                status = _status;
                pos = _pos;
                status.isShop = true;
                ShopPanel.gameObject.SetActive(true);
            }
        }
    }
}
