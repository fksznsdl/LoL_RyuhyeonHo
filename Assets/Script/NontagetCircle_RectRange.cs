using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NontargetCircle_RectRange : MonoBehaviour
{
    private Controller championController;
    
    [SerializeField]
    private GameObject onMouseRectRange;
    [SerializeField]
    private Camera playerCamera;

    // Start is called before the first frame update
    private float range;
    private float targetCircleRange;
    private RaycastHit HitInfo;
    void OnEnable()
    {
        championController = GetComponentInParent<Controller>();
        championController.isShowRange = true;
        this.transform.localScale = new Vector3(championController.currentCircleRange / 2f, championController.currentCircleRange / 2f, 1f);
        onMouseRectRange.transform.localScale = new Vector3(championController.currentOnMouseRectRange/2f, 1f, 1f);
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancleShowRange();
        }
        Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out HitInfo);
        onMouseRectRange.transform.position = new Vector3(HitInfo.point.x, 0.1f, HitInfo.point.z);
        onMouseRectRange.transform.forward = new Vector3(HitInfo.point.x-championController.chTransform.position.x, 0.1f, HitInfo.point.z-championController.chTransform.position.z);

    }

    private void CancleShowRange()
    {
        championController.isShowRange = false;
        this.gameObject.SetActive(false);
        onMouseRectRange.gameObject.SetActive(false);
    }
}
