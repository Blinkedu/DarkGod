﻿using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 登录系统
/// </summary>
public class LoginSys : SystemRoot
{
    public static LoginSys Instance = null;

    public LoginWnd loginWnd;
    public CreateWnd createWnd;

    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
        PECommon.Log("Init LoginSys...");
    }

    // 进入登录场景
    public void EnterLogin()
    {
        // 异步加载登录场景
        // 并显示加载的进度条
        resSvc.AsyncLoadScene(Constants.SceneLogin, () =>
         {
             // 加载完成后再打开注册登录界面
             loginWnd.SetWndState();
             audioSvc.PlayBGMusic(Constants.BGLogin);
         });
    }

    public void RspLogin(GameMsg msg)
    {
        GameRoot.AddTips("登录成功");
        GameRoot.Instance.SetPlayerData(msg.rspLogin);

        if (msg.rspLogin.playerData.name == "")
        {
            // 打开角色创建界面
            createWnd.SetWndState();
        }
        else
        {
            // 进入主城
            MainCitySys.Instance.EnterMainCity();
        }
        // 关闭登录界面
        loginWnd.SetWndState(false);
    }

    public void RspRename(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerName(msg.rspRename.name);

        // 跳转场景进入主城
        MainCitySys.Instance.EnterMainCity();

        // 打开主城界面

        // 关闭创建界面
        createWnd.SetWndState(false);

    }
}