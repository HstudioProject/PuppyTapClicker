using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Keiwando.BigInteger;


public class ClickButton : MonoBehaviour {

    public Text TapText;//탭당 증가값 표기
    public Text LevelText; // 레벨 표기
    public Text upgradeBuy;

    public string upgradeName; // 저장용 네임

    public bool startcheck; // 시작시 체크한다.

    public GameObject UpgradeClear;

    public BigInteger currentCost = 1;
    public int startCurrentCost = 1;

    [HideInInspector]
    public int level = 1;
    public int maxlevel; // 레벨 최대치
    public int costPowInt; //BigInteger는 인스펙터에 노출이 안되므로 여기에 넣은값을 Start()에서 costPow에 넣어준다 

    public Button upgradeBtn;
    public Color[] BtnColorList;

    public BigInteger costPow = 2; //3

    public bool TapItembool; // 탭당 10배 아이템 사용

    public ClickSlider clickSlider;

    void Start()
    {
        DataController.Instance.LoadClickButton(this);

        UpdateUI();
        startcheck = true;
        StartCoroutine(this.UpdateLoop()); // 0.5초 마다 반복
        costPow = costPowInt;

        LevelText.text = level.ToString();

        GameManager.instance.HexaButton_Active(); // 3마리 이상 수집 시 활성화
    }

    public void OnClick()
    {
        if (TapItembool == false)
        {
            string str = DataController.Instance.GetGoldTapClick().ToString(); // 탭당 증가
            CoinsManager.instance.MissionCoinList_call("coin", 0, str, gameObject.transform);
        }
        else
        {
            BigInteger BigTapItem = DataController.Instance.GetGoldTapClick() * MultiplyValueCtrl.instance.TapMultiply;
            string str = BigTapItem.ToString();
            CoinsManager.instance.MissionCoinList_call("coin", 0, str, gameObject.transform);
        }

        RewardBoxCtrl.instance.RewardBoxCount(); // 상자보상 탭 카운트증가
        MyLevelCtrl.instance.PurchaseItem(); // My레벨 탭 경험치 증가
    }

    public void PurchaseUpgrade()
    {
        if (DataController.Instance.gold >= currentCost)
        {
            if (maxlevel > level)
            {
                level++;
                LevelText.text = level.ToString();
                clickSlider.ClickSliderButton(); // 슬라이더 증가
                ReviewCheck(); // 리뷰 유도 체크

                DataController.Instance.gold -= currentCost;

                UpdateUpgrade();
                UpdateUI();
                DataController.Instance.SaveClickButton(this);

                //튜토 체크
                if (GameManager.instance.TutoCheck == 0)
                {
                    if (GuideQuest_Ctrl.instance.GuideTutoLevel == 1) GuideQuest_Ctrl.instance.GQcurrentPlus(2);
                    if (GuideQuest_Ctrl.instance.GuideTutoLevel == 0) GuideQuest_Ctrl.instance.GQcurrentPlus(1);
                }
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
        currentCost = currentCost + currentCost / 100 * 95; // 95% 증가
        DataController.Instance.goldTapClick = DataController.Instance.goldTapClick + DataController.Instance.goldTapClick/100 * 60; // 60% 증가
    }

    public void UpdateUI()
    {
        string currentCostStr = currentCost.ToString();

        upgradeBuy.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(currentCostStr);

        // 탭당 증가값 표기
        BigInteger BigTap = DataController.Instance.goldTapClick + DataController.Instance.goldTapClick/100 * 60; // 60% 증가 표기


        string Tapstr = BigTap.ToString();
        TapText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(Tapstr) + " / " + Language_Ctrl.instance.tapstr;


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

    void ReviewCheck()
    {
        GameManager.instance.HexaButton_Active(); // 3마리 이상 수집 시 활성화

        if (level == 15) // 리뷰 유도 (4마리 획득)
        {
            if (GameManager.instance.ReviewYesCheck == 0)
            {
                GameManager.instance.OnClickPanel_On(GameManager.instance.ReviewPanel);
            }
        }
    }

}
