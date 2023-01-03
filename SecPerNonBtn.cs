using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Keiwando.BigInteger;

public class SecPerNonBtn : MonoBehaviour
{
    public int PercentInt;  // %값 넣기
    public Text PercentText;//증가값 표기
    public Text upgradeBuy;
    public Text titeText; 

    public string upgradeName; // 저장용 네임

    public GameObject UpgradeClear;

    public BigInteger currentCost = 1;
    public string startCurrentCost = "1";

    [HideInInspector]
    public int level = 1;
    public int maxlevel; // 레벨 최대치
    public int costPowInt; //BigInteger는 인스펙터에 노출이 안되므로 여기에 넣은값을 Start()에서 costPow에 넣어준다 

    public Button upgradeBtn;
    public Color[] BtnColorList;

    public BigInteger costPow = 3; //3


    public void LoadStart()
    {
        DataController.Instance.LoadSecPerNonBtn(this);

        UpdateUI();
        if (level < 2) StartCoroutine(this.UpdateLoop()); // 1회성 체크 0.5초 마다 반복
        costPow = costPowInt;

        //시작 시 증가값 표기 
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

                //값 증가
                DataController.Instance.TotalSecPercent_non += PercentInt;

                UpdateUpgrade();
                UpdateUI();
                DataController.Instance.SaveSecPerNonBtn(this);

                Loopbool = false;
                Stop_CoroutineMethod(); // 코루틴 중지
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
        //goldByUpgrade = startgoldByUpgrade * (int) Mathf.Pow(upgradePow, level); // 기본형이라 그냥 남겨둠 

        // string 변수 new BigInteger(string)로 넣어주기
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
    //코루틴 정지
    public void Stop_CoroutineMethod()
    {
        IEnumerator updateloop = UpdateLoop();
        if (updateloop != null)
        {
            StopCoroutine(updateloop);
        }
    }

    // UpdateUI() 반복체크 최적화
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
