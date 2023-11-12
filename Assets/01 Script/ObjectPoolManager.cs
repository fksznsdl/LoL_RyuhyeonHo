using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private PhotonView pv;
    public static ObjectPoolManager Instance;
    [SerializeField]
    private string redMinionAttackPrefab;
    [SerializeField]
    private string blueMinionAttackPrefab;

    private bool isMaster;

    Queue<MinionWizard_Attack> redPoolingMinionAttackQueue = new Queue<MinionWizard_Attack>();
    Queue<MinionWizard_Attack> bluePoolingMinionAttackQueue = new Queue<MinionWizard_Attack>();

    private readonly string RED = "Red", BLUE = "Blue";
    private void Awake()
    {
        Instance = this;
        pv = GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            Initialize(20);
        }
    }

    private void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            redPoolingMinionAttackQueue.Enqueue(CreateNewObject(RED, this.transform.position));
            //레드팀 미니언 공격 이펙트전용 큐
            bluePoolingMinionAttackQueue.Enqueue(CreateNewObject(BLUE, this.transform.position));
            //블루팀 미니언 공격 이펙트전용 큐
        }
    }

    public static MinionWizard_Attack GetObject(string team)
    {
        if(team == Instance.RED)
        {
            if (Instance.redPoolingMinionAttackQueue.Count > 0)
            {
                var obj = Instance.redPoolingMinionAttackQueue.Dequeue();
                obj.GetComponent<MinionWizard_Attack>().SetParent(false);
                return obj;
            }
            else
            {
                var newObj = Instance.CreateNewObject(Instance.RED, new Vector3(0,0,0));
                newObj.GetComponent<MinionWizard_Attack>().SetParent(false);
                return newObj;
            }
        }
        else
        {
            if (Instance.bluePoolingMinionAttackQueue.Count > 0)
            {
                var obj = Instance.bluePoolingMinionAttackQueue.Dequeue();
                obj.GetComponent<MinionWizard_Attack>().SetParent(false);
                return obj;
            }
            else
            {
                var newObj = Instance.CreateNewObject(Instance.BLUE, new Vector3(0, 0, 0));
                newObj.GetComponent<MinionWizard_Attack>().SetParent(false);
                return newObj;
            }
        }
    }

    public static void ReturnObject(MinionWizard_Attack obj,string team)
    {
        obj.Show(false);
        obj.SetParent(true); // 비활성화 후 오브젝트 풀 오브젝트 자식화
        if(team == Instance.RED)
        {
            Instance.redPoolingMinionAttackQueue.Enqueue(obj);
        }
        else if(team == Instance.BLUE)
        {
            Instance.bluePoolingMinionAttackQueue.Enqueue(obj);
        }
    }


    private MinionWizard_Attack CreateNewObject(string team,Vector3 pos)
    {
        string prefab = null;
        if(team == "Red")
        {
            prefab = redMinionAttackPrefab;
        }
        else if(team == "Blue")
        {
            prefab = blueMinionAttackPrefab;
        }
        var newObj = PhotonNetwork.Instantiate(prefab, pos, Quaternion.identity).GetComponent<MinionWizard_Attack>();
        newObj.Show(false);
        newObj.SetParent(true);
        return newObj;
    }
   
}
