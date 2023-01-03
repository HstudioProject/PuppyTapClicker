using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppyScrollCtrl : MonoBehaviour
{
    public GetpuppyInfo pick;
    public GameObject MoveScroll;
    public Transform ScrollEnd;

    public GameObject OkBtn;
    public GetpuppyInfo[] getpuppyInfo;
    public bool movebool;
    public float speed;

    public static PuppyScrollCtrl instance;
    void Awake()
    {
        //싱글턴 인스턴스 할당
        instance = this;
    }

    void OnEnable()
    {
        if (AniCollectionCtrl.instance.randomList.Count == 0) return;

        int RanMaxcount = AniCollectionCtrl.instance.randomList.Count;
        for (int i = 0; i < getpuppyInfo.Length; i++)
        {
            int Rancount = Random.Range(0, RanMaxcount);
            getpuppyInfo[i].image.sprite = AniCollectionCtrl.instance.randomList[Rancount].image.sprite;
            getpuppyInfo[i].value = Rancount;
        }

        MoveScroll.transform.localPosition = new Vector3(0f, 0f, 0f);
        movebool = true;
    }

    void Update()
    {
        if (movebool)
        {
            MoveScroll.transform.position = Vector2.Lerp(MoveScroll.transform.position, ScrollEnd.transform.position, speed * Time.deltaTime);
        }

        if (Vector2.Distance(MoveScroll.transform.position, ScrollEnd.transform.position) < 0.01f)
        {
            // 움직임 정지 1번만 실행
            if (movebool == true)
            {
                movebool = false;
                MoveScroll.transform.position = ScrollEnd.transform.position;
                OkBtn.SetActive(true);
            }
        }
    }

    public void AniCollectPanel_Ok()
    {
        OkBtn.SetActive(false);
        AniCollectionCtrl.instance.AnimalCreate(pick.value);
        AniCollectionCtrl.instance.AniCollectPanel.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("scrollColl"))
        {
            pick = coll.gameObject.GetComponent<GetpuppyInfo>();
        }
    }
}
