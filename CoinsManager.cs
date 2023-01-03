using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class CoinsManager : MonoBehaviour
{
    [Header("[CoinValue Object or Text Pos]")]
    public CoinValueList coinValueList; // 정보를 넘겨주기 위한 CoinValue.cs 가지고 있는 오브젝트 리스트
    public GameObject goldTextPos; // 골드바 Text 위치
    public GameObject diaTextPos; // 다이아바 Text 위치

    public GameObject CoinRoot; // 코인 상위 Root
    public GameObject DiaRoot; // 다이아 상위 Root

    //References
    [Header ("UI references")]
	//[SerializeField] TMP_Text coinUIText;
	[SerializeField] GameObject animatedCoinPrefab;
    [SerializeField] GameObject animatedDiaPrefab;
	[SerializeField] Transform cointarget;
    [SerializeField] Transform diatarget;

    [Space]
	[Header ("Available coins : (coins to pool)")]
	[SerializeField] int maxCoins;
	Queue<GameObject> coinsQueue = new Queue<GameObject> ();
    Queue<GameObject> diasQueue = new Queue<GameObject>();

    [Space]
	[Header ("Animation settings")]
	[SerializeField] [Range (0.5f, 0.9f)] float minAnimDuration;
	[SerializeField] [Range (0.9f, 2f)] float maxAnimDuration;

	[SerializeField] Ease easeType;
	public float spread;

	Vector3 CointargetPosition;
    Vector3 DiatargetPosition;

    //싱글턴 인스턴스 선언
    public static CoinsManager instance = null;
    void Awake ()
	{
        //싱글턴 인스턴스 할당
        instance = this;
        CointargetPosition = cointarget.position;

		PrepareCoins ();
	}

	void PrepareCoins ()
	{
		GameObject coin;
        GameObject dia;
		for (int i = 0; i < maxCoins; i++) {
			coin = Instantiate (animatedCoinPrefab);
			coin.transform.parent = CoinRoot.transform;
			coin.SetActive (false);
			coinsQueue.Enqueue (coin);

            dia = Instantiate(animatedDiaPrefab);
            dia.transform.parent = DiaRoot.transform;
            dia.SetActive(false);
            diasQueue.Enqueue(dia);
        }
	}

	void CoinAnimate (Vector3 collectedCoinPosition, int amount)
	{
        CointargetPosition = cointarget.position;

        for (int i = 0; i < amount; i++) {

			if (coinsQueue.Count > 0) {
				//extract a coin from the pool
				GameObject coin = coinsQueue.Dequeue ();
				coin.SetActive (true);

                //move coin to the collected coin pos
                coin.transform.position = collectedCoinPosition + new Vector3 (Random.Range (-spread, spread), Random.Range(-spread, spread), 0f);

                //animate coin to target position
                float duration = Random.Range (minAnimDuration, maxAnimDuration);

				coin.transform.DOMove (CointargetPosition, duration)
				.SetEase (easeType)
				.OnComplete (() => {
					//executes whenever coin reach target position
					coin.SetActive (false);
					coinsQueue.Enqueue (coin);
                    BGSoundCtrl.instance.CoinSound_On(); // 코인 사운드
                }); 
			}
		}
	}
    void DiaAnimate(Vector3 collectedCoinPosition, int amount)
    {
        DiatargetPosition = diatarget.position;

        for (int i = 0; i < amount; i++)
        {
            if (diasQueue.Count > 0)
            {
                //extract a coin from the pool
                GameObject dia = diasQueue.Dequeue();
                dia.SetActive(true);

                //move coin to the collected coin pos
                dia.transform.position = collectedCoinPosition + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0f);

                //animate coin to target position
                float duration = Random.Range(minAnimDuration, maxAnimDuration);

                dia.transform.DOMove(DiatargetPosition, duration)
                .SetEase(easeType)
                .OnComplete(() => {
                    //executes whenever coin reach target position
                    dia.SetActive(false);
                    diasQueue.Enqueue(dia);
                    BGSoundCtrl.instance.CoinSound_On(); // 코인 사운드
                });
            }
        }
    }

    public void AddCoins (string type, Vector3 collectedCoinPosition, int amount)
	{
        if (type.Contains("coin")) CoinAnimate(collectedCoinPosition, amount);
        if (type.Contains("dia")) DiaAnimate(collectedCoinPosition, amount);
    }

    // 보상 코인 호출 (타입(coin, dia), 코인개수, 코인값, 시작 위치)
    public void MissionCoinList_call(string type, int count, string value, Transform pos)
    {
        GameObject Poolobj = coinValueList.GetPoolObj_A();
        CoinValue obj = Poolobj.GetComponent<CoinValue>();
        if (obj == null) return; //불러올 오브젝트가 없다면 리턴
        obj.gameObject.SetActive(true);

        spread = 1.0f;
        obj.value = value;
        CoinListGet_All(type, count, pos, obj.gameObject); // 코인 바로 수거
    }

    //코인 리스트 모두 수거  (코인개수, 시작 위치, 코인값을 넘겨줄 오브젝트 및 증가값 Text 표기위치)
    public void CoinListGet_All(string type, int count, Transform pos, GameObject obj)
    {
        AddCoins(type, pos.transform.position, count);
        if (type.Contains("coin"))
        {
            obj.transform.position = goldTextPos.transform.position; //증가값 Text 표기 위치
            obj.GetComponent<CoinValue>().MissionClearCoin(); // 골드 획득 text 표기
        }
        if (type.Contains("dia"))
        {
            obj.transform.position = diaTextPos.transform.position; //증가값 Text 표기 위치
            obj.GetComponent<CoinValue>().MissionClearDia(); // 다이아 획득 text 표기
        }
    }
}
