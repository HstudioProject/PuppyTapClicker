using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapCollectionCtrl : MonoBehaviour
{
    string key = "";

    private Sprite[] SpriteList;
    public TapCollectInfo[] tapCollectInfo;

    public Text TapCollectCountText; // 수집개수 표기 0/10

    // 현재 수집
    public int CurrentTapCollect
    {
        get { return PlayerPrefs.GetInt("CurrentTapCollect_" + key, 0); }
        set { PlayerPrefs.SetInt("CurrentTapCollect_" + key, value); }
    }

    public static TapCollectionCtrl instance;
    void Awake()
    {
        //싱글턴 인스턴스 할당
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

    // 상자뽑기 결과
    public void BoxTapRandomResult(GameObject objpos)
    {
        int ran = Random.Range(0, tapCollectInfo.Length); // 랜덤 뽑기

        TapPopupCtrl.instance.BoxItemPopup(ran, objpos); // 상자뽑기 아이템표기

        // 신규 체크
        if (tapCollectInfo[ran].isPurchased == false)
        {
            CurrentTapCollect++;
            TapCollectCountText.text = CurrentTapCollect + "/" + tapCollectInfo.Length;

            tapCollectInfo[ran].count++;
            tapCollectInfo[ran].countTextInput();

            tapCollectInfo[ran].gameObject.SetActive(true);
            tapCollectInfo[ran].isPurchased = true;

            DataController.Instance.TotalTapPercent_collect += 10;

            ResetNewIcon(); // 신규아이콘 표기 초기화
            tapCollectInfo[ran].newIcon.SetActive(true);

            // 아이템 사용 중이 아닌경우
            if (GameManager.instance.AutoItem.canSlider == false && GameManager.instance.TapItem.canSlider == false)
            {
                TapPopupCtrl.instance.PopupPenel_open();
            }
        }
        else
        {
            // 중복
            DataController.Instance.TotalTapPercent_collect += 1;

            tapCollectInfo[ran].count++;
            tapCollectInfo[ran].countTextInput();
        }

        SaveTapCollButton(tapCollectInfo[ran]); // 저장
    }

    // 신규아이콘 표기 초기화
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
