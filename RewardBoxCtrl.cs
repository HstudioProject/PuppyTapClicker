using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Keiwando.BigInteger;

public class RewardBoxCtrl : MonoBehaviour
{
    string key = "";
    public int current
    {
        get { return PlayerPrefs.GetInt("Reward_current" + key, 0); }
        set { PlayerPrefs.SetInt("Reward_current" + key, value); }
    }

    [Header("[상자보상 슬라이더]")]
    public int max;
    public Text countText;
    public Slider slider;

    [Header("[상자 리스트]")]
    public BoxListCtrl boxListCtrl;

    //싱글턴 인스턴스 선언
    public static RewardBoxCtrl instance = null;

    void Awake()
    {
        //싱글턴 인스턴스 할당
        instance = this;
        key = DataController.Instance.key;
    }

    void Start()
    {
        countText.text = current + "/" + max;
        slider.value = (float)current / (float)max;
    }
    // 상자보상 탭 카운트증가
    public void RewardBoxCount()
    {
        if (current < max - 1) { current++; }
        else // 상자보상
        {
            current = 0;
            boxListCtrl.OnClickCreateBox();
        }

        countText.text = current + "/" + max;
        slider.value = (float)current / (float)max;
    }

    // 상자 오픈 (아이템, 골드, 다이아) 
    public void OnClickButton_BoxReward(GameObject obj)
    {
        RandomSelect.instance.StartRandomCall(obj);
    }

    public void BoxRandomResult(int type, GameObject obj)
    {
        if(TapCollectionCtrl.instance.CurrentTapCollect == 0) // 첫상자는 무조건 수집품 뽑기
        {
            TapCollectionCtrl.instance.BoxTapRandomResult(obj);
            obj.SetActive(false);
            return;
        }

        if (type == 1) //골드 보상
        {
            BigInteger gold = DataController.Instance.GetGoldTapClick();
            gold = gold * 50; // 탭수익 x 보상
            string Bigstr = gold.ToString(); //100.000AA 표기는 CoinValue.cs/MissionClearCoin()에서 알아서 표기해줌
            CoinsManager.instance.MissionCoinList_call("coin", 15, Bigstr, obj.transform);
        }
        else if (type == 2) // 다이아 보상
        {
            CoinsManager.instance.MissionCoinList_call("dia", 3, "3", obj.transform);
        }
        else // 아이템 뽑기
        {
            TapCollectionCtrl.instance.BoxTapRandomResult(obj);
        }
        obj.SetActive(false);
    }

}
