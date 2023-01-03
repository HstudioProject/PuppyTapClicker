using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickSlider : MonoBehaviour
{
    string key = "";

    public Slider slider;

    public int max = 5;
    public int current
    {
        get { return PlayerPrefs.GetInt("slidercurrent_" + key + gameObject.name, 1); }
        set { PlayerPrefs.SetInt("slidercurrent_" + key + gameObject.name, value); }
    }

    void Awake() => key = DataController.Instance.key;

    void Start()
    {
        slider.value = (float)current / (float)max;
    }

    public void ClickSliderButton()
    {
        if (current < max -1)
        {
            current++;
        }
        else
        {
            //¿©±â¼­ ½Å±Ô È¹µæ
            current = 0;
            if (AniCollectionCtrl.instance.randomList.Count > 0) AniCollectionCtrl.instance.AniCollectPanel.SetActive(true);
        }
        slider.value = (float)current / (float)max;
    }
}
