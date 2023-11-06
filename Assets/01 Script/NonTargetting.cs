using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonTargetting : MonoBehaviour
{
    [SerializeField]
    private Camera playercamera;

    private Vector3 mousePoint;

    private RaycastHit hitInfo;

    private float range;

    private Vector3 scale;

    private void OnEnable()
    {
        GetComponentInParent<Controller>().isShowRange = true;
        range = GetComponentInParent<Controller>().currentRange;
        scale = GetComponentInParent<Controller>().currentScale;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancleShowRange();
        }
        ShowRange();
    }
    private void CancleShowRange()
    {
        GetComponentInParent<Controller>().isShowRange = false;
        this.gameObject.SetActive(false);
    }
    private void ShowRange()
    {
        int layerMask = ((1 << LayerMask.NameToLayer("Ignore Raycast")) | (1 << LayerMask.NameToLayer("TargetExclusion")));
        layerMask = ~layerMask;
        Physics.Raycast(playercamera.ScreenPointToRay(Input.mousePosition),out hitInfo, 1000f,layerMask);
        mousePoint = new Vector3(hitInfo.point.x - this.transform.position.x, 0.1f, hitInfo.point.z - this.transform.position.z);
        this.transform.forward = mousePoint;
        this.transform.localScale = new Vector3(scale.x/2f, this.transform.localScale.y, range / 4f);
    }
}
