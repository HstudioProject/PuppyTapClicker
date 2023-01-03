using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AniCollectInfo : MonoBehaviour
{
    public bool isPurchased; // ���ȴ��� üũ��
    public int Number; //�ѹ��� ������Ʈ �ҷ��ö� ���
    public string SaveName; // ����� ����
    public Image image;

    public void Start()
    {
        if(Number == 0) AniCollectionCtrl.instance.SaveAniCollButton(this); // ó�� �⺻ 1���� 

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
