using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Keiwando.BigInteger;

public class SecUpgradeBtn : MonoBehaviour
{
    public Text upgradeDisplayer;
    public Text LevelText; // 레벨 표기
    public Text upgradeBuy;

    public string upgradeName; // 저장용 네임
    public int upgradeNumber; // 구분용 넘버

    public GameObject UpgradeLock;
    public GameObject UpgradeClear;

    public BigInteger goldByUpgrade; //초당 골드 증가값
    public string startgoldByUpgrade = "1";

    public BigInteger currentCost = 1;
    public string startCurrentCost = "1";

    public int level = 1;
    public int maxlevel; // 레벨 최대치

    public Button upgradeBtn;
    public GameObject Manager;
    public Color[] ManagerColor;

    public BigInteger upgradePow = 2; //3
    public BigInteger costPow = 3; //3


    public void LoadStart()
    {
        DataController.Instance.LoadSecUpgradeBtn(this);

        UpdateUI();
        StartCoroutine(this.UpdateLoop()); // 1초 마다 반복

        LevelText.text = "Lv. " + level;
        DataController.Instance.SecUpgradeBeforLevelCheck(upgradeNumber); // 전단계 레벨체크 버튼락 해제
    }

    public void PurchaseUpgrade()
    {
        if (UpgradeLock.activeSelf) return;

        if (DataController.Instance.gold >= currentCost)
        {
            if (maxlevel > level)
            {
                DataController.Instance.gold -= currentCost;
                level++;
                LevelText.text = "Lv. " + level;
                DataController.Instance.SecUpgradeBeforLevelCheck(upgradeNumber); // 전단계 레벨체크 버튼락 해제

                DataController.Instance.goldSecClickUpgrade += goldByUpgrade;

                UpdateUpgrade();
                UpdateUI();
                DataController.Instance.SaveSecUpgradeBtn(this);
            }
            else
            {
                UpgradeClear.SetActive(true);
            }
        }
        else // 골드부족
        {
            Language_Ctrl.instance.WarningbarReset();
            Language_Ctrl.instance.Warningbar.SetActive(true);
            Language_Ctrl.instance.Warningbar.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void UpdateUpgrade()
    {
        // string 변수 new BigInteger(string)로 넣어주기
        BigInteger BigstartgoldByUpgrade = new BigInteger(startgoldByUpgrade);
        BigInteger BigstartCurrentCost = new BigInteger(startCurrentCost);

        goldByUpgrade = goldByUpgrade + goldByUpgrade / 100 * 60; // 초당증가 60%
        currentCost = currentCost + currentCost / 100 * 95; // 가격증가 90%
    }

    public void UpdateUI()
    {
        string goldByUpgradeStr = goldByUpgrade.ToString();
        string currentCostStr = currentCost.ToString();

        upgradeBuy.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(currentCostStr);

        if (UpgradeLock.activeSelf)
        {
            upgradeDisplayer.text = Language_Ctrl.instance.SecUpgradeBtnstr; // 이전단계 Lv.10 완료 번역적용
            Manager.GetComponent<Image>().color = ManagerColor[0];
        }
        else
        {
            upgradeDisplayer.text = "+" + BigIntegerCtrl_global.bigInteger.ChangeMoney(goldByUpgradeStr);
            Manager.GetComponent<Image>().color = ManagerColor[1];
        }

        if (DataController.Instance.gold >= currentCost)
        {
            upgradeBtn.interactable = true;
        }
        else
        {
            upgradeBtn.interactable = false;
        }

        if (maxlevel <= level)
        {
            UpgradeClear.SetActive(true);
        }
    }

    // UpdateUI() 반복체크 최적화
    private IEnumerator UpdateLoop()
    {
        WaitForSeconds delay = new WaitForSeconds(0.5f);

        while (true)
        {
            yield return delay;
            UpdateUI();
        }
    }
}
