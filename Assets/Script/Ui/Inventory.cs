using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Status status;
    [SerializeField]
    private Image[] item_image;

    public Item[] item;

    private void Start()
    {
        StartCoroutine(StayCoroutine());
    }
    IEnumerator StayCoroutine()
    {
        yield return new WaitForSeconds(4.1f);
        status = GameObject.Find("PlayerCanvas").transform.Find("Status_UI").GetComponent<Status_UI>().status;
        item = new Item[6] { null, null, null, null, null, null };
    }
    public void BuyItem(Item _item)
    {
        int disCountGold =0;
        List<Item> disCountItem = new List<Item>();
        if (_item.underitem != null)
        {
            foreach(var clone in _item.underitem)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    if (item[i] == clone)
                    {
                        disCountGold += clone.gold;
                        disCountItem.Add(clone);
                        break;
                    }
                }
            }
        }
        if (item[0] != null && item[1] != null && item[2] != null && item[3] != null && item[4] != null && item[5] != null)
        {
            Debug.Log("인벤토리가 가득찼습니다.");
            return;
        }

        if (_item.gold <= (status.currentGold + disCountGold) && disCountGold >0)
        {
            foreach(var clone in disCountItem)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    if (item[i] == clone)
                    {
                        status.SellItem(item[i]);
                        item[i] = null;
                        item_image[i].sprite = null;
                        break;
                    }
                }
            }
            status.currentGold += disCountGold;
            status.currentGold -= _item.gold;
            AddItem(_item);
            Debug.Log("Buy");
        }
        else if (_item.gold <= status.currentGold)
        {
            status.currentGold -= _item.gold;
            AddItem(_item);
            Debug.Log("Buy");
        }
    }
    public void AddItem(Item _item)
    {
        for (int i = 0; i < item.Length; i++)
        {
            if (item[i] == null)
            {
                item[i] = _item;
                item_image[i].sprite = _item.Icon;
                AddState(_item);
                break;
            }
        }
    }
    public void SellItem(int _itemNumber)
    {
        status.currentGold += (item[_itemNumber].gold * 0.6f);
        status.SellItem(item[_itemNumber]);
        item[_itemNumber] = null;
        item_image[_itemNumber].sprite = null;
    }
    private void AddState(Item _item)
    {
        status.GetItem(_item);
    }
}
