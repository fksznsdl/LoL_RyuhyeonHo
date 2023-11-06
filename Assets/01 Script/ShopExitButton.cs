using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopExitButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject shopPanel;
    private Status status;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            status = GameObject.Find("PlayerCanvas").transform.Find("Status_UI").GetComponent<Status_UI>().status;
            status.isShop = false;
            shopPanel.gameObject.SetActive(false);
        }
    }
}
