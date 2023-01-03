using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapCollectionCtrl : MonoBehaviour
{
    string key = "";

    private Sprite[] SpriteList;
    public TapCollectInfo[] tapCollectInfo;

    public Text TapCollectCountText; // �������� ǥ�� 0/10

    // ���� ����
    public int CurrentTapCollect
    {
        get { return PlayerPrefs.GetInt("CurrentTapCollect_" + key, 0); }
        set { PlayerPrefs.SetInt("CurrentTapCollect_" + key, value); }
    }

    public static TapCollectionCtrl instance;
    void Awake()
    {
        //�̱��� �ν��Ͻ� �Ҵ�
        instance = this;
        key = DataController.Instance.key;
    }

    void Start()
    {
        SpriteList = new Sprite[tapCollectInfo.Length];
        for (int i = 0; i < tapCollectInfo.Length; i++)
        {
            SpriteList[i] = Resources.Load<Sprite>("Puppy_Props/Puppy_Props_" + i);
            tapCollectInfo[i].image.sprite = SpriteList[i];
        }

        TapCollectCountText.text = CurrentTapCollect + "/" + tapCollectInfo.Length;
    }

    // ���ڻ̱� ���
    public void BoxTapRandomResult(GameObject objpos)
    {
        int ran = Random.Range(0, tapCollectInfo.Length); // ���� �̱�

        TapPopupCtrl.instance.BoxItemPopup(ran, objpos); // ���ڻ̱� ������ǥ��

        // �ű� üũ
        if (tapCollectInfo[ran].isPurchased == false)
        {
            CurrentTapCollect++;
            TapCollectCountText.text = CurrentTapCollect + "/" + tapCollectInfo.Length;

            tapCollectInfo[ran].count++;
            tapCollectInfo[ran].countTextInput();

            tapCollectInfo[ran].gameObject.SetActive(true);
            tapCollectInfo[ran].isPurchased = true;

            DataController.Instance.TotalTapPercent_collect += 10;

            ResetNewIcon(); // �űԾ����� ǥ�� �ʱ�ȭ
            tapCollectInfo[ran].newIcon.SetActive(true);

            // ������ ��� ���� �ƴѰ��
            if (GameManager.instance.AutoItem.canSlider == false && GameManager.instance.TapItem.canSlider == false)
            {
                TapPopupCtrl.instance.PopupPenel_open();
            }
        }
        else
        {
            // �ߺ�
            DataController.Instance.TotalTapPercent_collect += 1;

            tapCollectInfo[ran].count++;
            tapCollectInfo[ran].countTextInput();
        }

        SaveTapCollButton(tapCollectInfo[ran]); // ����
    }

    // �űԾ����� ǥ�� �ʱ�ȭ
    public void ResetNewIcon()
    {
        for (int i = 0; i < tapCollectInfo.Length; i++)
        {
            tapCollectInfo[i].newIcon.SetActive(false);
        }
    }

    public void LoadTapCollButton(TapCollectInfo tapCollectInfo)
    {
        string name = tapCollectInfo.SaveName;

        tapCollectInfo.count = PlayerPrefs.GetInt(name + key + "_count", 0);

        if (PlayerPrefs.GetInt(name + key + "_isPurchased") == 1)
        {
            tapCollectInfo.isPurchased = true;
        }
        else
        {
            tapCollectInfo.isPurchased = false;
        }
    }

    public void SaveTapCollButton(TapCollectInfo tapCollectInfo)
    {
        string name = tapCollectInfo.SaveName;

        PlayerPrefs.SetInt(name + key + "_count", tapCollectInfo.count);

        if (tapCollectInfo.isPurchased == true)
        {
            PlayerPrefs.SetInt(name + key + "_isPurchased", 1);
        }
        else
        {
            PlayerPrefs.SetInt(name + key + "_isPurchased", 0);
        }
    }
}
