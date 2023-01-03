using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelect : MonoBehaviour
{
    public List<Card> deck = new List<Card>();  // 카드 덱
    public int total = 0;  // 카드들의 가중치 총 합

    public static RandomSelect instance;
    void Awake()
    {
        //싱글턴 인스턴스 할당
        instance = this;
    }

    public List<Card> result = new List<Card>(); // 랜덤하게 선택된 카드를 담을 리스트

    bool Runbool = false; // 동시실행 방지용

    // 확률에 랜덤값 호출
    public void StartRandomCall(GameObject obj)
    {
        if (Runbool) return;
        Runbool = true;
        total = 0; // 이거 초기화 중요!!
        result.Clear(); // 모든결과 리셋

        for (int i = 0; i < deck.Count; i++)
        {
            // 스크립트가 활성화 되면 덱의 모든 총 가중치를 구함
            total += deck[i].weight;
        }
        // 실행
        ResultSelect(obj);
    }


    // 결과값 
    public void ResultSelect(GameObject obj)
    {
        result.Add(RandomCard());
        Runbool = false;
        string valuestr = result[0].name;

        //Debug.Log("ResultSelect: " + valuestr);
        if (valuestr.Contains("item"))
        {
            RewardBoxCtrl.instance.BoxRandomResult(0, obj);
        }
        if (valuestr.Contains("gold"))
        {
            RewardBoxCtrl.instance.BoxRandomResult(1, obj);
        }
        if (valuestr.Contains("dia"))
        {
            RewardBoxCtrl.instance.BoxRandomResult(2, obj);
        }
    }


    // 가중치 랜덤의 설명은 영상을 참고.
    public Card RandomCard()
    {
        int weight = 0;
        int selectNum = 0;

        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));

        for (int i = 0; i < deck.Count; i++)
        {
            weight += deck[i].weight;
            if (selectNum <= weight)
            {
                Card temp = new Card(deck[i]);
                return temp;
            }
        }
        return null;
    }

}
