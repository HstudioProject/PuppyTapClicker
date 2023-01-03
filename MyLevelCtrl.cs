using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Keiwando.BigInteger;


public class MyLevelCtrl : MonoBehaviour {

    public Slider slider;

    public int level;

    public BigInteger current;
    public BigInteger max;
    public int startMax = 3; // 시작 시 개수

    public string myname; // 저장용 네임
    public string currentString;
    public Text levelText;

    [Header("[레벨업 보상창]")]
    public Text LevelUpText;
    public GameObject LevelUpPanel;

    public BigInteger Bigvalue; // 바에 백분율 적용
    public string Bigvaluestring;
    int RewardDia;

    [Header("[레벨패스 리스트 체크]")]
    public LvPassInfo[] lvPassInfo;


    public static MyLevelCtrl instance;
    void Awake()
    {
        //싱글턴 인스턴스 할당
        instance = this;
    }

    void Start()
    {
        DataController.Instance.LoadMyLevel(this);
        UpdateUI();
    }

    public void PurchaseItem()
    {
        if (current >= max)
        {
            level++;
            current++;
            UpdateUpgrade();
            DataController.Instance.SaveMyLevel(this);

            LevelUpText.text = "Level " + level;
            LvupPopup(); //여기서 팝업창 띄움

            MyLevelPassCheck(); // 레벨 패스 체크
        }
        else
        {
            current++;
            DataController.Instance.SaveMyLevel(this);
        }

        UpdateUI();
    }

    // max의 4/1 증가
    public void UpdateUpgrade()
    {
        current = 0;
        max = max + max / 4;
    }

    public void UpdateUI()
    {
        currentString = current+ "/" + max;
        levelText.text = "Lv" + level.ToString();

        slider.minValue = 0;
        slider.maxValue = 100;

        // BigInteger 게이지바로 표현하기
        Bigvalue = current * 100 / max; // 현재값 * 100 / 최대값(업글가능 금액)
        Bigvaluestring = Bigvalue.ToString();

        slider.value = int.Parse(Bigvaluestring); // 현재 백분율 값
    }

    void LvupPopup()
    {
        LevelUpPanel.SetActive(true);
    }

    // 기본 보상 받기 OK버튼
    public void LvUpReward_OkBtn()
    {
        Transform center = GameManager.instance.gameObject.transform;
        CoinsManager.instance.MissionCoinList_call("dia", 3, "3", center.transform);
        LevelUpPanel.SetActive(false);
    }
    // 광고 보상 버튼 선택 --> 광고 호출하기
    public void LvUpReward_AdsBtn()
    {
        LvUpReward_ReOkBtn();
    }

    // 광고 보상 완료 버튼
    public void LvUpReward_ReOkBtn()
    {
        Transform center = GameManager.instance.gameObject.transform;
        CoinsManager.instance.MissionCoinList_call("dia", 15, "15", center.transform);
        LevelUpPanel.SetActive(false);
    }

    // 레벨 패스 체크
    public void MyLevelPassCheck()
    {
        for(int i = 0; i < lvPassInfo.Length; i++)
        {
            lvPassInfo[i].Start();
        }
    }
}
