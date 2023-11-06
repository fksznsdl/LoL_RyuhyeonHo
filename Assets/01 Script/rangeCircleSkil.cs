using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangeCircleSkil : MonoBehaviour
{
    private Controller controller;

    void OnEnable()
    {
        controller = GetComponentInParent<Controller>();
        controller.isShowRange = true;
        this.transform.localScale = new Vector3(controller.currentCircleRange / 2f, controller.currentCircleRange / 2f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancleShowRange();
        }
        
    }
    private void CancleShowRange()
    {
        GetComponentInParent<Controller>().isShowRange = false;
        this.gameObject.SetActive(false);
    }
}
