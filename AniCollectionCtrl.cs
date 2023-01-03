using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AniCollectionCtrl : MonoBehaviour
{
    string key = "";

    public AniCollectInfo[] aniCollectInfo; // ���� �ݷ��� ����Ʈ
    public GameObject[] AnimalList; // ���� ������Ʈ ����Ʈ

    public GameObject AniCollectPanel;
    public Text AniCollectCountText;
    public GameObject PuppyAnimalRoot; // ������ü �ֻ��� ��Ʈ

    [Header("[�����Ϸ� ����]")]
    public GameObject starbar;
    public GameObject starfinger;
    public Text StarCountText;

    [Header("[�������� ����Ʈ]")]
    public List<AniCollectInfo> randomList;

    // ���� ����
    public int CurrentAniCollect
    {
        get { return PlayerPrefs.GetInt("CurrentAniCollect_" + key, 1); }
        set { PlayerPrefs.SetInt("CurrentAniCollect_" + key, value); }
    }
    // ���� �� ���� ���ȹ�� �� ����
    public int CollectStar
    {
        get { return PlayerPrefs.GetInt("CollectStar_" + key, 0); }
        set { PlayerPrefs.SetInt("CollectStar_" + key, value); }
    }

    public static AniCollectionCtrl instance;
    void Awake()
    {
        //�̱��� �ν��Ͻ� �Ҵ�
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

    // ��������Ʈ �߸���
    public void RandomList_Start()
    {
        RandomList_Clear();
        for (int i = 0; i < aniCollectInfo.Length; i++)
        {
            AniCollectInfo obj = aniCollectInfo[i];
            if (obj.isPurchased == false) randomList.Add(obj);
        }

        // ���� ������Ʈ �� ���� ȹ�� ��ư �ѱ�
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
    // �űԵ��� ����
    public void AnimalCreate(int num)
    {
        if (randomList.Count == 0)
        {
            CollectStar++;
            countTextInput();
            //��� �������� �Ϸ�
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

    // ���� ������Ʈ �� ���� ȹ�� ��ư �۵�
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
            // ��μ����Ͽ����ϴ�.
        }
    }

    // ���� ������Ʈ �ѱ�
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


    // Hexa or ����ȭ�� �̵��� �����̵� ����Ʈ �ʱ�ȭ
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
