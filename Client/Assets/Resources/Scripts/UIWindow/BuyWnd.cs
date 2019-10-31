using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 购买交易窗口
/// </summary>
public class BuyWnd : WindowRoot
{
    public Text txtInfo;
    public Button btnSure;

    private int buyType; // 0:体力，1:金币

    public void SetBuyType(int type)
    {
        this.buyType = type;
    }

    protected override void InitWnd()
    {
        base.InitWnd();
        btnSure.interactable = true;
        RefreshUI();
    }

    private void RefreshUI()
    {
        switch (buyType)
        {
            case 0:
                // 体力
                txtInfo.text = "是否花费"+Constants.Color("10钻石",TextColor.Red)+"购买"+ Constants.Color("100体力", TextColor.Green)+"?";
                break;
            case 1:
                // 金币
                txtInfo.text = "是否花费" + Constants.Color("10钻石", TextColor.Red) + "购买" + Constants.Color("1000金币", TextColor.Green) + "?";
                break;
        }
    }

    public void ClickSureBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        // 发送网络消息 
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.ReqBuy,
            reqBuy = new ReqBuy
            {
                type = buyType,
                cost = 10
            }
        };
        netSvc.SendMsg(msg);
        btnSure.interactable = false;
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }
}
