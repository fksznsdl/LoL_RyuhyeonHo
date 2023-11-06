using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHealZone : MonoBehaviour
{
    [SerializeField]
    private int team;

    private float currenthealTime;

    private Collider colliders;
    private Status status;
    private void Start()
    {
        colliders = null;
        currenthealTime = 0f;
    }
    private void Update()
    {
        if(colliders != null)
        {
            if (Vector3.Distance(colliders.transform.position, this.transform.position) >= 6f)
            {
                Debug.Log("°ÅÁ¡Èú ³ª°¨");
                colliders = null;
                status = null;
            }
            Heal();
        }
    }

    private void Heal()
    {
        currenthealTime += Time.deltaTime;
        if(currenthealTime >= 0.75f)
        {
            currenthealTime = 0f;
            if (status != null || colliders != null)
            {
                if (status.currentHp < status.maxHp || status.currentMp < status.maxMp)
                {
                    status.currentHp += 100f;
                    status.currentMp += 100f;
                    if (status.maxHp < status.currentHp)
                    {
                        status.currentHp = status.maxHp;
                    }
                    else if (status.maxMp < status.currentMp)
                    {
                        status.currentMp = status.maxMp;
                    }
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Champion")
        {
            if(other.GetComponent<Status>().team == team)
            {
                colliders = other;
                status = other.GetComponent<Status>();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Champion") {
            if (other == colliders)
            {
                colliders = null;
                status = null;
                Debug.Log("°ÅÁ¡Èú ³ª°¨");
            }
        }
    }
}
