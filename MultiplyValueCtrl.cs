using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplyValueCtrl : MonoBehaviour
{
    string key = "";

    public Button MultiplyBtn;
    public Text MultiplyBtnText;
    public Text TapdiaText;
    public Text SecdiaText;

    // 탭당 곱하기 값
    public int TapMultiply
    {
        get { return PlayerPrefs.GetInt("TapMultiply_" + key, 10); }
        set { PlayerPrefs.SetInt("TapMultiply_" + key, value); }
    }
    // 초당 곱하기 값
    public int SecMultiply
    {
        get { return PlayerPrefs.GetInt("SecMultiply_" + key, 10); }
        set { PlayerPrefs.SetInt("SecMultiply_" + key, value); }
    }


    public static MultiplyValueCtrl instance;
    void Awake()
    {
        //싱글턴 인스턴스 할당
        instance = this;
        key = DataController.Instance.key;
    }

    void Start()
    {
        if (TapMultiply == 10) Multiply_x10();
        if (TapMultiply == 20) Multiply_x20();
        if (TapMultiply == 50) Multiply_x50();
        if (TapMultiply == 100) Multiply_x100();
    }

    public void OnClickBtnMultiply()
    {
        if (TapMultiply == 10)
        {
            Multiply_x20();
            return;
        }
        if (TapMultiply == 20)
        {
            Multiply_x50();
            return;
        }
        if (TapMultiply == 50)
        {
            Multiply_x100();
            return;
        }
        if (TapMultiply == 100)
        {
            Multiply_x10();
            return;
        }
    }

    void Multiply_x10()
    {
        TapMultiply = 10;
        SecMultiply = 10;
        MultiplyBtnText.text = "x10";
        TapdiaText.text = TapMultiply.ToString();
        SecdiaText.text = SecMultiply.ToString();
        Language_Ctrl.instance.LanguageStart();
    }

    void Multiply_x20()
    {
        TapMultiply = 20;
        SecMultiply = 20;
        MultiplyBtnText.text = "x20";
        TapdiaText.text = TapMultiply.ToString();
        SecdiaText.text = SecMultiply.ToString();
        Language_Ctrl.instance.LanguageStart();
    }

    void Multiply_x50()
    {
        TapMultiply = 50;
        SecMultiply = 50;
        MultiplyBtnText.text = "x50";
        TapdiaText.text = TapMultiply.ToString();
        SecdiaText.text = SecMultiply.ToString();
        Language_Ctrl.instance.LanguageStart();
    }

    void Multiply_x100()
    {
        TapMultiply = 100;
        SecMultiply = 100;
        MultiplyBtnText.text = "x100";
        TapdiaText.text = TapMultiply.ToString();
        SecdiaText.text = SecMultiply.ToString();
        Language_Ctrl.instance.LanguageStart();
    }
}
