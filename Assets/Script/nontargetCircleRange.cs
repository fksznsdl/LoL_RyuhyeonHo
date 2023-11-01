using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nontargetCircleRange : MonoBehaviour
{
    private Controller championController;
    [SerializeField]
    private GameObject onMouseCircleRange;
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
        onMouseCircleRange.transform.localScale = new Vector3(championController.currentOnMouseCircleRange / 2f, championController.currentOnMouseCircleRange / 2f, 1f);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancleShowRange();
        }
        Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out HitInfo);
        onMouseCircleRange.transform.position = new Vector3(HitInfo.point.x, 0.1f, HitInfo.point.z);
    }

    private void CancleShowRange()
    {
        championController.isShowRange = false;
        this.gameObject.SetActive(false);
        onMouseCircleRange.gameObject.SetActive(false);
    }
}
