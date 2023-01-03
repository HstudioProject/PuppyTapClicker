using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxListCtrl : MonoBehaviour
{
    public GameObject root;
    public GameObject prefab;
    public List<GameObject> objectList;


    void Start()
    {
        objectList = new List<GameObject>();

        for (int i = 0; i < 30; i++)
        {
            GameObject obj = (GameObject)Instantiate(prefab);
            obj.transform.SetParent(root.transform);
            obj.transform.localScale = new Vector3(1f, 1f, 1f);
            obj.SetActive(false);
            objectList.Add(obj);
        }
    }

    public GameObject GetPoolObj_A()
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            if (!objectList[i].activeInHierarchy)
            {
                return objectList[i];
            }
        }
        return null;
    }

    // 보상 상자 등장
    public void OnClickCreateBox()
    {
        GameObject obj = GetPoolObj_A();
        if (obj == null) return; //불러올 오브젝트가 없다면 리턴

        obj.SetActive(true);
    }

}
