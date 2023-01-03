using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Keiwando.BigInteger;

public class SecUpgradeBtn : MonoBehaviour
{
    public Text upgradeDisplayer;
    public Text LevelText; // ���� ǥ��
    public Text upgradeBuy;

    public string upgradeName; // ����� ����
    public int upgradeNumber; // ���п� �ѹ�

    public GameObject UpgradeLock;
    public GameObject UpgradeClear;

    public BigInteger goldByUpgrade; //�ʴ� ��� ������
    public string startgoldByUpgrade = "1";

    public BigInteger currentCost = 1;
    public string startCurrentCost = "1";

    public int level = 1;
    public int maxlevel; // ���� �ִ�ġ

    public Button upgradeBtn;
    public GameObject Manager;
    public Color[] ManagerColor;

    public BigInteger upgradePow = 2; //3
    public BigInteger costPow = 3; //3


    public void LoadStart()
    {
        DataController.Instance.LoadSecUpgradeBtn(this);

        UpdateUI();
        StartCoroutine(this.UpdateLoop()); // 1�� ���� �ݺ�

        LevelText.text = "Lv. " + level;
        DataController.Instance.SecUpgradeBeforLevelCheck(upgradeNumber); // ���ܰ� ����üũ ��ư�� ����
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
                DataController.Instance.SecUpgradeBeforLevelCheck(upgradeNumber); // ���ܰ� ����üũ ��ư�� ����

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
        else // ������
        {
            Language_Ctrl.instance.WarningbarReset();
            Language_Ctrl.instance.Warningbar.SetActive(true);
            Language_Ctrl.instance.Warningbar.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void UpdateUpgrade()
    {
        // string ���� new BigInteger(string)�� �־��ֱ�
        BigInteger BigstartgoldByUpgrade = new BigInteger(startgoldByUpgrade);
        BigInteger BigstartCurrentCost = new BigInteger(startCurrentCost);

        goldByUpgrade = goldByUpgrade + goldByUpgrade / 100 * 60; // �ʴ����� 60%
        currentCost = currentCost + currentCost / 100 * 95; // �������� 90%
    }

    public void UpdateUI()
    {
        string goldByUpgradeStr = goldByUpgrade.ToString();
        string currentCostStr = currentCost.ToString();

        upgradeBuy.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(currentCostStr);

        if (UpgradeLock.activeSelf)
        {
            upgradeDisplayer.text = Language_Ctrl.instance.SecUpgradeBtnstr; // �����ܰ� Lv.10 �Ϸ� ��������
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

    // UpdateUI() �ݺ�üũ ����ȭ
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
