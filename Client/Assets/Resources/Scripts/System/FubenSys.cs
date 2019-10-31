using System;
using System.Collections;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;

/// <summary>
/// 副本业务系统
/// </summary>
public class FubenSys : SystemRoot
{
    public static FubenSys Instance = null;

    public FubenWnd fubenWnd;

    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
        PECommon.Log("Init FubenSys...");
    }

    public void EnterFunben()
    {
        SetFunbenWndState();
    }

    #region Fuben Wnd
    public void SetFunbenWndState(bool isActive = true)
    {
        fubenWnd.SetWndState(isActive);
    }
    #endregion
    
    public void RspFBFight(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerDataByFBStart(msg.rspFBFight);
        MainCitySys.Instance.mainCityWnd.SetWndState(false);
        SetFunbenWndState(false);
        BattleSys.Instance.StartBattle(msg.rspFBFight.fbid);
    }
}