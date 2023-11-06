using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Exp_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Status status;
    [SerializeField]
    private Image currentExp_Image;
    [SerializeField]
    private GameObject panel_Exp;
    [SerializeField]
    private Text exp_txt;
    private bool isReady;
    private void Start()
    {
        isReady = false;
        StartCoroutine(StayCoroutine());
    }
    private IEnumerator StayCoroutine()
    {
        yield return new WaitForSeconds(3.3f);
        status = this.transform.parent.GetComponentInParent<Status_UI>().status;
        isReady = true;

    }
    private void Update()
    {
        if (isReady)
        {
            currentExp_Image.fillAmount = status.currentExp / status.maxExp;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        panel_Exp.gameObject.SetActive(true);
        exp_txt.text = string.Format("현재 경험치 : {0}/{1}", status.currentExp, status.maxExp);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panel_Exp.gameObject.SetActive(false);
    }
}
