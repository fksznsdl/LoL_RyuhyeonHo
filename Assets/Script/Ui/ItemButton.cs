using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemButton : MonoBehaviour, IPointerClickHandler
{
    private Status status;
    private Inventory inventory;
    [SerializeField]
    private int itemNumber;
    public void OnPointerClick(PointerEventData eventData)
    {
        inventory = GetComponentInParent<Inventory>();
        status = inventory.status;
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (status.isShop)
            {
                inventory.SellItem(itemNumber);
            }
        }
    }
}
