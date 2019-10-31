using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 角色信息展示界面
/// </summary>
public class InfoWnd : WindowRoot
{
    #region UI Define
    public RawImage imgChar;

    public Text txtInfo;
    public Text txtExp;
    public Image imgExpPrg;
    public Text txtPower;
    public Image imgPowerPrg;

    public Text txtJob;
    public Text txtFight;
    public Text txtHP;
    public Text txtHurt;
    public Text txtDef;

    public Button btnClose;
    public Button btnDetail;
    public Button btnCloseDetail;
    public Transform transDetail;

    public Text dtxhp;
    public Text dtxad;
    public Text dtxap;
    public Text dtxaddef;
    public Text dtxapdef;
    public Text dtxdodge;
    public Text dtxpierce;
    public Text dtxcritical;
    #endregion

    private Vector2 startPos;

    protected override void InitWnd()
    {
        base.InitWnd();
        RegTouchEvts();
        RefreshUI();
    }

    private void RegTouchEvts()
    {
        OnClickDown(imgChar.gameObject, e =>
        {
            startPos = e.position;
            MainCitySys.Instance.SetStartRoate();
        });

        OnDrag(imgChar.gameObject, e =>
        {
            float roate = -(e.position.x - startPos.x) * 0.4f;
            MainCitySys.Instance.SetPlayerRoate(roate);
        });
    }


    private void RefreshUI()
    {
        var playerData = GameRoot.Instance.PlayerData;
        SetText(txtInfo, playerData.name + " LV." + playerData.lv);
        SetText(txtExp, playerData.exp + "/" + PECommon.GetExpUpValByLv(playerData.lv));
        imgExpPrg.fillAmount = (float)playerData.exp / PECommon.GetExpUpValByLv(playerData.lv);
        SetText(txtPower, playerData.power + "/" + PECommon.GetPowerLimit(playerData.lv));
        imgPowerPrg.fillAmount = (float)playerData.power / PECommon.GetPowerLimit(playerData.lv);

        SetText(txtJob, "暗夜刺客");
        SetText(txtFight, PECommon.GetFightByProps(playerData));
        SetText(txtHP, playerData.hp);
        SetText(txtHurt, playerData.ad + playerData.ap);
        SetText(txtDef, playerData.addef + playerData.apdef);

        // Detall TODO
        SetText(dtxhp, playerData.hp);
        SetText(dtxad, playerData.ad);
        SetText(dtxap, playerData.ap);
        SetText(dtxaddef, playerData.addef);
        SetText(dtxapdef, playerData.apdef);
        SetText(dtxdodge, playerData.dodge + "%");
        SetText(dtxpierce, playerData.pierce + "%");
        SetText(dtxcritical, playerData.critical + "%");
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.Instance.CloseInfoWnd();
    }

    public void ClickDetailBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetActive(transDetail);
    }

    public void ClickCloseDetailBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetActive(transDetail, false);
    }
}