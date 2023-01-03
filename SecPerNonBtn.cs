using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Keiwando.BigInteger;

public class SecPerNonBtn : MonoBehaviour
{
    public int PercentInt;  // %�� �ֱ�
    public Text PercentText;//������ ǥ��
    public Text upgradeBuy;
    public Text titeText; 

    public string upgradeName; // ����� ����

    public GameObject UpgradeClear;

    public BigInteger currentCost = 1;
    public string startCurrentCost = "1";

    [HideInInspector]
    public int level = 1;
    public int maxlevel; // ���� �ִ�ġ
    public int costPowInt; //BigInteger�� �ν����Ϳ� ������ �ȵǹǷ� ���⿡ �������� Start()���� costPow�� �־��ش� 

    public Button upgradeBtn;
    public Color[] BtnColorList;

    public BigInteger costPow = 3; //3


    public void LoadStart()
    {
        DataController.Instance.LoadSecPerNonBtn(this);

        UpdateUI();
        if (level < 2) StartCoroutine(this.UpdateLoop()); // 1ȸ�� üũ 0.5�� ���� �ݺ�
        costPow = costPowInt;

        //���� �� ������ ǥ�� 
        PercentTextInput(PercentInt);
    }

    void PercentTextInput(int value)
    {
        PercentText.text = value + "%";
    }


    public void PurchaseUpgrade()
    {
        if (DataController.Instance.gold >= currentCost)
        {
            if (maxlevel > level)
            {
                level++;
                DataController.Instance.gold -= currentCost;

                //�� ����
                DataController.Instance.TotalSecPercent_non += PercentInt;

                UpdateUpgrade();
                UpdateUI();
                DataController.Instance.SaveSecPerNonBtn(this);

                Loopbool = false;
                Stop_CoroutineMethod(); // �ڷ�ƾ ����
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
        //goldByUpgrade = startgoldByUpgrade * (int) Mathf.Pow(upgradePow, level); // �⺻���̶� �׳� ���ܵ� 

        // string ���� new BigInteger(string)�� �־��ֱ�
        BigInteger BigstartCurrentCost = new BigInteger(startCurrentCost);
        currentCost = BigstartCurrentCost * BigInteger.Pow(costPow, level);
    }

    public void UpdateUI()
    {
        string currentCostStr = currentCost.ToString();

        upgradeBuy.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(currentCostStr);

        if (DataController.Instance.gold >= currentCost)
        {
            upgradeBtn.interactable = true;
            upgradeBtn.GetComponent<Image>().color = BtnColorList[0];
        }
        else
        {
            upgradeBtn.interactable = false;
            upgradeBtn.GetComponent<Image>().color = BtnColorList[1];
        }

        if (maxlevel <= level)
        {
            UpgradeClear.SetActive(true);
        }
    }
    bool Loopbool = true;
    //�ڷ�ƾ ����
    public void Stop_CoroutineMethod()
    {
        IEnumerator updateloop = UpdateLoop();
        if (updateloop != null)
        {
            StopCoroutine(updateloop);
        }
    }

    // UpdateUI() �ݺ�üũ ����ȭ
    private IEnumerator UpdateLoop()
    {
        WaitForSeconds delay = new WaitForSeconds(0.5f);

        while (Loopbool)
        {
            yield return delay;
            UpdateUI();
        }
    }
}
