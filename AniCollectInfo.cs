using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AniCollectInfo : MonoBehaviour
{
    public bool isPurchased; // 열렸는지 체크용
    public int Number; //넘버링 오브젝트 불러올때 사용
    public string SaveName; // 저장용 네임
    public Image image;

    public void Start()
    {
        if(Number == 0) AniCollectionCtrl.instance.SaveAniCollButton(this); // 처음 기본 1마리 

        AniCollectionCtrl.instance.LoadAniCollButton(this);

        if (isPurchased)
        {
            gameObject.gameObject.SetActive(true);
            AniCollectionCtrl.instance.AnimalObject_True(Number);
        }
        else
        {
            gameObject.gameObject.SetActive(false);
        }
    }
}
