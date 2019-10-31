using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 血条物体
/// </summary>
public class ItemEntityHP : MonoBehaviour
{
    #region UI Define
    public Image imgHPGray;
    public Image imgHPRed;

    public Animation criticalAni;
    public Text txtCritical;

    public Animation dodgeAni;
    public Text txtDodge;

    public Animation hpAni;
    public Text txtHp;
    #endregion

    private RectTransform rect;
    private Transform rootTrans;
    private int hpVal;
    private float scaleRate = 1.0f * Constants.ScreenStandardHeight / Screen.height;


    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetCritical(969);
            SetHurt(336);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            SetDodge();
        }
        */

        Vector3 scenePos = Camera.main.WorldToScreenPoint(rootTrans.position);
        rect.anchoredPosition = scenePos * scaleRate;

        UpdateMixBlend();
        imgHPGray.fillAmount = currentPrg;
    }

    private void UpdateMixBlend()
    {
        if(Mathf.Abs(currentPrg - targetPrg) < Constants.AccelerHPSpeed *  Time.deltaTime)
        {
            currentPrg = targetPrg;
        }
        else if(currentPrg > targetPrg)
        {
            currentPrg -= Constants.AccelerHPSpeed * Time.deltaTime;
        }
        else
        {
            currentPrg += Constants.AccelerHPSpeed * Time.deltaTime;
        }
    }


    public void InitItemInfo(Transform trans, int hp)
    {
        rect = transform.GetComponent<RectTransform>();
        rootTrans = trans;
        hpVal = hp;
        imgHPGray.fillAmount = 1;
        imgHPRed.fillAmount = 1;
    }

    public void SetCritical(int critical)
    {
        criticalAni.Stop();
        txtCritical.text = "暴击 " + critical;
        criticalAni.Play();
    }

    public void SetDodge()
    {
        dodgeAni.Stop();
        txtDodge.text = "闪避 ";
        dodgeAni.Play();
    }

    public void SetHurt(int hurt)
    {
        hpAni.Stop();
        txtHp.text = "-" + hurt;
        hpAni.Play();
    }

    private float currentPrg;
    private float targetPrg;
    public void SetHpVal(int oldVal,int newVal)
    {
        currentPrg = (float)oldVal / hpVal;
        targetPrg = (float)newVal / hpVal;
        imgHPRed.fillAmount = targetPrg;
    }
}
