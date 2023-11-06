using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    private GameObject champion;
    private bool isReady;
    private Camera cam;
    void Start()
    {
        isReady = false;
        StartCoroutine(StayCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            if (Input.GetMouseButtonUp(1))
            {
                if (!champion.GetComponent<Status>().isDead)
                {
                    if (champion.GetComponent<Controller>().isMiniMap)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
                        {
                            champion.GetComponent<Controller>().SetDestination(hit.point);
                        }
                    }
                }
            }
        }
    }
    private IEnumerator StayCoroutine()
    {
        yield return new WaitForSeconds(3f);
        cam = GetComponent<Camera>();
        string name = GameObject.Find("PlayerSetting").GetComponent<PlayerSetting>().champName;
        champion = GameObject.Find(name);
        isReady = true;
    }
}
