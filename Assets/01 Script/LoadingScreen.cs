using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    private PlayerSetting ps;
    [SerializeField]
    private string team;
    [SerializeField]
    private Sprite[] image;
    private void Start()
    {
        ps = GameObject.Find("PlayerSetting").GetComponent<PlayerSetting>();
        ShowChampPick(team);
    }
    private IEnumerator StayCoroutine()
    {
        yield return new WaitForSeconds(4.5f);
        this.transform.parent.parent.gameObject.SetActive(false);
    }

    private void ShowChampPick(string _team)
    {
        if(_team == "Blue")
        {
            switch (ps.blueChampPick)
            {
                case "Veiga":
                    GetComponent<Image>().sprite = image[0];
                    break;
                case "Janna":
                    GetComponent<Image>().sprite = image[1];
                    break;
                case "Lux":
                    GetComponent<Image>().sprite = image[2];
                    break;
                case "Anivia":
                    GetComponent<Image>().sprite = image[3];
                    break;
            }
        }
        else if(_team == "Red")
        {
            switch (ps.redChampPick)
            {
                case "Veiga":
                    GetComponent<Image>().sprite = image[0];
                    break;
                case "Janna":
                    GetComponent<Image>().sprite = image[1];
                    break;
                case "Lux":
                    GetComponent<Image>().sprite = image[2];
                    break;
                case "Anivia":
                    GetComponent<Image>().sprite = image[3];
                    break;
            }
        }
        StartCoroutine(StayCoroutine());
    }
}
