using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour, IPointerClickHandler
{
    private Inventory inventory;
    [SerializeField]
    private Item item;

    [SerializeField]
    private GameObject NormalPanel;
    [SerializeField]
    private GameObject OneLinePanel;
    [SerializeField]
    private GameObject TwoLinePanel;
    [SerializeField]
    private GameObject ThreeLienPanel;

    private void OnEnable()
    {
        this.GetComponent<Image>().sprite = item.Icon;
        this.transform.GetChild(1).GetComponent<Text>().text = item.gold.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            ShowDescript();
        }
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            BuyItem();
        }
    }

    private void BuyItem()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        inventory.BuyItem(item);
    }
    private void ShowDescript()
    {
        NormalPanel.gameObject.SetActive(false);
        OneLinePanel.gameObject.SetActive(false);
        TwoLinePanel.gameObject.SetActive(false);
        ThreeLienPanel.gameObject.SetActive(false);
        if (item.item_Rank == "Normal")
        {
            NormalPanel.gameObject.SetActive(true);
            NormalPanel.transform.GetChild(0).GetComponent<Image>().sprite = item.Icon;
            NormalPanel.transform.GetChild(0).GetComponentInChildren<Text>().text = item.gold.ToString();
            NormalPanel.transform.GetChild(1).GetComponent<Text>().text = item.description;
        }
        else if(item.item_Rank == "Epic")
        {
            switch (item.underitem.Length)
            {
                case 1:
                    OneLinePanel.gameObject.SetActive(true);
                    OneLinePanel.transform.GetChild(1).GetComponent<Image>().sprite = item.Icon;
                    OneLinePanel.transform.GetChild(1).GetComponentInChildren<Text>().text = item.gold.ToString();
                    OneLinePanel.transform.GetChild(2).GetComponent<Image>().sprite = item.underitem[0].Icon;
                    OneLinePanel.transform.GetChild(2).GetComponentInChildren<Text>().text = item.underitem[0].gold.ToString();
                    OneLinePanel.transform.GetChild(3).GetComponent<Text>().text = item.description;
                    break;
                case 2:
                    TwoLinePanel.gameObject.SetActive(true);
                    TwoLinePanel.transform.GetChild(1).GetComponent<Image>().sprite = item.Icon;
                    TwoLinePanel.transform.GetChild(1).GetComponentInChildren<Text>().text = item.gold.ToString();
                    for (int i = 2; i <= 3; i++)
                    {
                        TwoLinePanel.transform.GetChild(i).GetComponent<Image>().sprite = item.underitem[i-2].Icon;
                        TwoLinePanel.transform.GetChild(i).GetComponentInChildren<Text>().text = item.underitem[i-2].gold.ToString();
                    }
                    TwoLinePanel.transform.GetChild(4).GetComponent<Text>().text = item.description;
                    break;
                case 3:
                    ThreeLienPanel.gameObject.SetActive(true);
                    ThreeLienPanel.transform.GetChild(1).GetComponent<Image>().sprite = item.Icon;
                    ThreeLienPanel.transform.GetChild(1).GetComponentInChildren<Text>().text = item.gold.ToString();
                    for (int i = 2; i <= 4; i++)
                    {
                        ThreeLienPanel.transform.GetChild(i).GetComponent<Image>().sprite = item.underitem[i - 2].Icon;
                        ThreeLienPanel.transform.GetChild(i).GetComponentInChildren<Text>().text = item.underitem[i - 2].gold.ToString();
                    }
                    ThreeLienPanel.transform.GetChild(5).GetComponent<Text>().text = item.description;
                    break;
            }
        }
        else if(item.item_Rank == "Legend")
        {
            switch (item.underitem.Length)
            {
                case 1:
                    OneLinePanel.gameObject.SetActive(true);
                    OneLinePanel.transform.GetChild(1).GetComponent<Image>().sprite = item.Icon;
                    OneLinePanel.transform.GetChild(1).GetComponentInChildren<Text>().text = item.gold.ToString();
                    OneLinePanel.transform.GetChild(2).GetComponent<Image>().sprite = item.underitem[0].Icon;
                    OneLinePanel.transform.GetChild(2).GetComponentInChildren<Text>().text = item.underitem[0].gold.ToString();
                    OneLinePanel.transform.GetChild(3).GetComponent<Text>().text = item.description;
                    break;
                case 2:
                    TwoLinePanel.gameObject.SetActive(true);
                    TwoLinePanel.transform.GetChild(1).GetComponent<Image>().sprite = item.Icon;
                    TwoLinePanel.transform.GetChild(1).GetComponentInChildren<Text>().text = item.gold.ToString();
                    for (int i = 2; i <= 3; i++)
                    {
                        TwoLinePanel.transform.GetChild(i).GetComponent<Image>().sprite = item.underitem[i - 2].Icon;
                        TwoLinePanel.transform.GetChild(i).GetComponentInChildren<Text>().text = item.underitem[i - 2].gold.ToString();
                    }
                    TwoLinePanel.transform.GetChild(4).GetComponent<Text>().text = item.description;
                    break;
                case 3:
                    ThreeLienPanel.gameObject.SetActive(true);
                    ThreeLienPanel.transform.GetChild(1).GetComponent<Image>().sprite = item.Icon;
                    ThreeLienPanel.transform.GetChild(1).GetComponentInChildren<Text>().text = item.gold.ToString();
                    for (int i = 2; i <= 4; i++)
                    {
                        ThreeLienPanel.transform.GetChild(i).GetComponent<Image>().sprite = item.underitem[i - 2].Icon;
                        ThreeLienPanel.transform.GetChild(i).GetComponentInChildren<Text>().text = item.underitem[i - 2].gold.ToString();
                    }
                    ThreeLienPanel.transform.GetChild(5).GetComponent<Text>().text = item.description;
                    break;
            }
        }
    }
}
