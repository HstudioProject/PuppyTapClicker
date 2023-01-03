using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Text;
using Keiwando.BigInteger;


public class DataController : MonoBehaviour {

    public string key = "PuppyTapClicker";

    private static DataController instance;

    public ClickButton clickButton;

    //public TapPerNonBtn[] tapPerNonBtns; // 탭당 퍼센트증가 1회성 
    public SecUpgradeBtn[] secUpgradeBtns; // 초당 업그레이드 이전 레벨체크용
    public SecPerNonBtn[] secPerNonBtns; // 초당 퍼센트증가 1회성

    public bool SecItembool; // 초당 10배 아이템 사용

    public static DataController Instance
    {
        get{
            if (instance == null)
            {
                instance = FindObjectOfType<DataController>();
                if (instance == null)
                {
                    GameObject container = new GameObject("DataCountroller");
                    instance = container.AddComponent<DataController>();
                }
            }
            return instance;
        }
    }

    public BigInteger gold
    {
        get
        {
            if (!PlayerPrefs.HasKey("Gold" + key))
            {
                return 50000;
            }

            string tmpGold = PlayerPrefs.GetString("Gold" + key);
            return new BigInteger(tmpGold);
        }
        set
        {
            PlayerPrefs.SetString("Gold" + key, value.ToString());
        }
    }

    public BigInteger goldSecClick
    {
        get
        {
            if (!PlayerPrefs.HasKey("GoldSecClick" + key))
            {
                return 1000;
            }

            string tmpGoldSec = PlayerPrefs.GetString("GoldSecClick" + key);
            return new BigInteger(tmpGoldSec);
        }
        set
        {
            PlayerPrefs.SetString("GoldSecClick" + key, value.ToString());
        }
    }

    public BigInteger goldSecClickUpgrade
    {
        get
        {
            if (!PlayerPrefs.HasKey("GoldSecClickUpgrade" + key)){ return 0; }
            string tmpGoldSecUp = PlayerPrefs.GetString("GoldSecClickUpgrade" + key);
            return new BigInteger(tmpGoldSecUp);
        }
        set
        {
            PlayerPrefs.SetString("GoldSecClickUpgrade" + key, value.ToString());
        }
    }

    public BigInteger goldTapClick
    {
        get
        {
            if (!PlayerPrefs.HasKey("GoldTapClick" + key))
            {
                return 1000;
            }

            string tmpGoldTap = PlayerPrefs.GetString("GoldTapClick" + key);
            return new BigInteger(tmpGoldTap);
        }
        set
        {
            PlayerPrefs.SetString("GoldTapClick" + key, value.ToString());
        }
    }

    // 탭당 퍼센트 종합 증가값 1회성
    public int TotalTapPercent_non
    {
        get { return PlayerPrefs.GetInt("TotalTapPercent_non" + key, 0); }
        set { PlayerPrefs.SetInt("TotalTapPercent_non" + key, value); }
    }
    // 탭당 퍼센트 종합 증가값 수집형
    public int TotalTapPercent_collect
    {
        get { return PlayerPrefs.GetInt("TotalTapPercent_collect" + key, 0); }
        set { PlayerPrefs.SetInt("TotalTapPercent_collect" + key, value); }
    }

    // 초당 퍼센트 종합 증가값 1회성
    public int TotalSecPercent_non
    {
        get { return PlayerPrefs.GetInt("TotalSecPercent_non" + key, 0); }
        set { PlayerPrefs.SetInt("TotalSecPercent_non" + key, value); }
    }

    // Dia 개수 //TEST용 1000개 = 0개로 출시전 초기화 해줄것
    public int Dia
    {
        get { return PlayerPrefs.GetInt("DiaCount_" + key, 3); } 
        set { PlayerPrefs.SetInt("DiaCount_" + key, value); }
    }


    void Start()
    {
        // 초당 수익 증가 반복
        StartCoroutine(this.GoldPerSecLoop());
    }

    // 초당 업그레이드 전단계 레벨체크 버튼락 해제
    public void SecUpgradeBeforLevelCheck(int num)
    {
        if (secUpgradeBtns[num].level >= 11)
        {
            if(num + 1 < secUpgradeBtns.Length)
            {
                secUpgradeBtns[num + 1].UpgradeLock.SetActive(false);
            }
        }
    }

    public void LoadClickButton(ClickButton clickButton)
    {
        string name = clickButton.upgradeName;
        string currentCost;

        clickButton.level = PlayerPrefs.GetInt(name + key + "_level", 1);
        currentCost = PlayerPrefs.GetString(name + key + "_cost", clickButton.startCurrentCost.ToString());

        clickButton.currentCost = new BigInteger(currentCost);
    }
    public void SaveClickButton(ClickButton clickButton)
    {
        string name = clickButton.upgradeName;
        string currentCost = clickButton.currentCost.ToString();

        PlayerPrefs.SetInt(name + key + "_level", clickButton.level);
        PlayerPrefs.SetString(name + key + "_cost", currentCost);
    }


    public void LoadTapPerNonBtn(TapPerNonBtn tapPerNonBtn)
    {
        string name = tapPerNonBtn.upgradeName;
        string currentCost;

        tapPerNonBtn.level = PlayerPrefs.GetInt(name + key + "_level", 1);
        currentCost = PlayerPrefs.GetString(name + key + "_cost", tapPerNonBtn.startCurrentCost.ToString());

        tapPerNonBtn.currentCost = new BigInteger(currentCost);
    }

    public void SaveTapPerNonBtn(TapPerNonBtn tapPerNonBtn)
    {
        string name = tapPerNonBtn.upgradeName;
        string currentCost = tapPerNonBtn.currentCost.ToString();

        PlayerPrefs.SetInt(name + key + "_level", tapPerNonBtn.level);
        PlayerPrefs.SetString(name + key + "_cost", currentCost);
    }
    public void LoadSecUpgradeBtn(SecUpgradeBtn secUpgradeBtn)
    {
        string name = secUpgradeBtn.upgradeName;
        string goldByUpgrade;
        string currentCost;

        secUpgradeBtn.level = PlayerPrefs.GetInt(name + key + "_level",1);
        goldByUpgrade = PlayerPrefs.GetString(name + key + "_goldByUpgrade", secUpgradeBtn.startgoldByUpgrade.ToString());
        currentCost = PlayerPrefs.GetString(name + key + "_cost", secUpgradeBtn.startCurrentCost.ToString());

        secUpgradeBtn.goldByUpgrade = new BigInteger(goldByUpgrade);
        secUpgradeBtn.currentCost = new BigInteger(currentCost);
    }
    public void SaveSecUpgradeBtn(SecUpgradeBtn secUpgradeBtn)
    {
        string name = secUpgradeBtn.upgradeName;
        string goldByUpgrade = secUpgradeBtn.goldByUpgrade.ToString();
        string currentCost = secUpgradeBtn.currentCost.ToString();

        PlayerPrefs.SetInt(name + key + "_level", secUpgradeBtn.level);
        PlayerPrefs.SetString(name + key + "_goldByUpgrade", goldByUpgrade);
        PlayerPrefs.SetString(name + key + "_cost", currentCost);
    }

    public void LoadSecPerNonBtn(SecPerNonBtn secPerNonBtn)
    {
        string name = secPerNonBtn.upgradeName;
        string currentCost;

        secPerNonBtn.level = PlayerPrefs.GetInt(name + key + "_level", 1);
        currentCost = PlayerPrefs.GetString(name + key + "_cost", secPerNonBtn.startCurrentCost.ToString());

        secPerNonBtn.currentCost = new BigInteger(currentCost);
    }
    public void SaveSecPerNonBtn(SecPerNonBtn secPerNonBtn)
    {
        string name = secPerNonBtn.upgradeName;
        string currentCost = secPerNonBtn.currentCost.ToString();

        PlayerPrefs.SetInt(name + key + "_level", secPerNonBtn.level);
        PlayerPrefs.SetString(name + key + "_cost", currentCost);
    }

    // 기본나무 심은 개수 카운트 나의 레벨 구하기
    public void LoadMyLevel(MyLevelCtrl myLevelCtrl)
    {
        string name = myLevelCtrl.myname;
        string max;
        string current;

        myLevelCtrl.level = PlayerPrefs.GetInt(name + key + "_level", 1);
        max = PlayerPrefs.GetString(name + key + "_max", myLevelCtrl.startMax.ToString());
        current = PlayerPrefs.GetString(name + key + "_current", "0");

        myLevelCtrl.max = new BigInteger(max);
        myLevelCtrl.current = new BigInteger(current);
    }

    public void SaveMyLevel(MyLevelCtrl myLevelCtrl)
    {
        string name = myLevelCtrl.myname;
        string max = myLevelCtrl.max.ToString();
        string current = myLevelCtrl.current.ToString();

        PlayerPrefs.SetInt(name + key + "_level", myLevelCtrl.level);
        PlayerPrefs.SetString(name + key + "_max", max);
        PlayerPrefs.SetString(name + key + "_current", current);
    }


    public void LoadLvPassInfo(LvPassInfo lvPassInfo)
    {
        string key = lvPassInfo.itemName;

        if (PlayerPrefs.GetInt(name + key + "_isPurchased") == 1) lvPassInfo.isPurchased = true;
        else lvPassInfo.isPurchased = false;
    }

    public void SaveLvPassInfo(LvPassInfo lvPassInfo)
    {
        string key = lvPassInfo.itemName;

        if (lvPassInfo.isPurchased == true) PlayerPrefs.SetInt(name + key + "_isPurchased", 1);
        else PlayerPrefs.SetInt(name + key + "_isPurchased", 0);
    }


    // 탭당 퍼센트 증가값 적용 (TotalgoldTapClick = 기본값 + 1회성 + 수집형)
    public BigInteger GetGoldTapClick()
    {
        BigInteger TotalgoldTapClick_non = goldTapClick / 100 * TotalTapPercent_non;
        BigInteger TotalgoldTapClick_collect = goldTapClick / 100 * TotalTapPercent_collect;
        BigInteger TotalgoldTapClick = goldTapClick + TotalgoldTapClick_non + TotalgoldTapClick_collect;

        return TotalgoldTapClick;
    }
    // 총합 초당 증가값
    public BigInteger GetGoldSecClick()
    {
        BigInteger TotalgoldSecClick = goldSecClick + goldSecClickUpgrade; // 기본초당 증가값 + 업그레이드 증가값
        BigInteger TotalgoldSecClick_non = TotalgoldSecClick / 100 * TotalSecPercent_non;
        TotalgoldSecClick = TotalgoldSecClick + TotalgoldSecClick_non;

        return TotalgoldSecClick;
    }

    // 초당 수익 증가 반복
    IEnumerator GoldPerSecLoop()
    {
        while (true)
        {
            if (SecItembool == false)
            {
                gold += GetGoldSecClick(); // 초당 증가
                string str = GetGoldSecClick().ToString();
                CoinsManager.instance.MissionCoinList_call("coin", 0, str, gameObject.transform);
            }
            else
            {
                gold += GetGoldSecClick() * 10; // 초당 증가
                BigInteger BigSecItem = GetGoldSecClick() * MultiplyValueCtrl.instance.SecMultiply;
                string str = BigSecItem.ToString();
                CoinsManager.instance.MissionCoinList_call("coin", 0, str, gameObject.transform);
            }
            ScrollButtonListCtrl.instance.ButtonListActiveCheck(); // 하단버튼 리스트 활성화표기 체크
            yield return new WaitForSeconds(1.0f);
        }
    }


    public void ResetData()
    {
#if UNITY_EDITOR
        PlayerPrefs.DeleteAll();
        Invoke("ResetScene", 1.0f);
#endif
    }

    public void ResetScene()
    {
        SceneManager.LoadScene("PuppyTapClicker_0");
    }

}