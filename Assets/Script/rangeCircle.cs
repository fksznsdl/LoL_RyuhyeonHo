using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangeCircle : MonoBehaviour
{
    public Controller controller;
    // Update is called once per frame
    void Start()
    {
        controller = GetComponentInParent<Controller>();
        
        transform.localScale = new Vector3(controller.currentCircleRange / 2f, controller.currentCircleRange / 2f,0f);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.gameObject.SetActive(false);
        }
    }
}
