using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AniCollectionCtrl : MonoBehaviour
{
    string key = "";

    public AniCollectInfo[] aniCollectInfo; // 동물 콜렉션 리스트
    public GameObject[] AnimalList; // 동물 오브젝트 리스트

    public GameObject AniCollectPanel;
    public Text AniCollectCountText;
    public GameObject PuppyAnimalRoot; // 동물전체 최상위 루트

    [Header("[수집완료 관리]")]
    public GameObject starbar;
    public GameObject starfinger;
    public Text StarCountText;

    [Header("[수집가능 리스트]")]
    public List<AniCollectInfo> randomList;

    // 현재 수집
    public int CurrentAniCollect
    {
        get { return PlayerPrefs.GetInt("CurrentAniCollect_" + key, 1); }
        set { PlayerPrefs.SetInt("CurrentAniCollect_" + key, value); }
    }
    // 보유 별 동물 모두획득 시 증가
    public int CollectStar
    {
        get { return PlayerPrefs.GetInt("CollectStar_" + key, 0); }
        set { PlayerPrefs.SetInt("CollectStar_" + key, value); }
    }

    public static AniCollectionCtrl instance;
    void Awake()
    {
        //싱글턴 인스턴스 할당
        instance = this;
        key = DataController.Instance.key;
    }

    void Start()
    {
        randomList = new List<AniCollectInfo>();
        countTextInput();

        Invoke("RandomList_Start", 1f);
    }
    public void countTextInput()
    {
        AniCollectCountText.text = CurrentAniCollect + "/" + aniCollectInfo.Length;
        StarCountText.text = CollectStar.ToString();
    }

    // 수집리스트 추리기
    public void RandomList_Start()
    {
        RandomList_Clear();
        for (int i = 0; i < aniCollectInfo.Length; i++)
        {
            AniCollectInfo obj = aniCollectInfo[i];
            if (obj.isPurchased == false) randomList.Add(obj);
        }

        // 동물 업데이트 시 동물 획득 버튼 켜기
        if (CollectStar > 0 && randomList.Count > 0)
        {
            starfinger.SetActive(true);
        }
        else
        {
            starfinger.SetActive(false);
            starbar.SetActive(false);
        }
    }
    void RandomList_Clear()
    {
        if (randomList.Count > 0)
        {
            randomList.Clear();
        }
    }
    // 신규동물 등장
    public void AnimalCreate(int num)
    {
        if (randomList.Count == 0)
        {
            CollectStar++;
            countTextInput();
            //모든 동물수집 완료
            return;
        }

        int ran = num;
        randomList[ran].isPurchased = true;
        SaveAniCollButton(randomList[ran]);
        randomList[ran].Start();

        RandomList_Start();

        CurrentAniCollect++;
        countTextInput();
    }

    // 동물 업데이트 시 동물 획득 버튼 작동
    public void OnClickButton_UpdateStarCheck()
    {
        if (CollectStar > 0 && randomList.Count > 0)
        {
            CollectStar--;
            countTextInput();
            AniCollectPanel.SetActive(true);
        }
        else
        {
            // 모두수집하였습니다.
        }
    }

    // 게임 오브젝트 켜기
    public void AnimalObject_True(int number)
    {
        AnimalList[number].SetActive(true);
    }

    public void LoadAniCollButton(AniCollectInfo aniCollectInfo)
    {
        string name = aniCollectInfo.SaveName;

        if (PlayerPrefs.GetInt(name + key + "_isPurchased") == 1)
        {
            aniCollectInfo.isPurchased = true;
        }
        else
        {
            aniCollectInfo.isPurchased = false;
        }
    }

    public void SaveAniCollButton(AniCollectInfo aniCollectInfo)
    {
        string name = aniCollectInfo.SaveName;

        if (aniCollectInfo.isPurchased == true)
        {
            PlayerPrefs.SetInt(name + key + "_isPurchased", 1);
        }
        else
        {
            PlayerPrefs.SetInt(name + key + "_isPurchased", 0);
        }
    }


    // Hexa or 메인화면 이동시 동물이동 리스트 초기화
    public void ScreenChangeAnimalReset()
    {
        if (PuppyAnimalRoot.activeSelf)
        {
            GameManager.instance.gameType = GameType.Hexa;
            PuppyAnimalRoot.SetActive(false);
        }
        else
        {
            GameManager.instance.gameType = GameType.Main;
            PuppyAnimalRoot.SetActive(true);
            for (int i = 0; i < AnimalList.Length; i++)
            {
                if (AnimalList[i].activeSelf) AnimalList[i].GetComponent<AnimalPatrolCtrl>().Start();
            }
        } 
    }
}
