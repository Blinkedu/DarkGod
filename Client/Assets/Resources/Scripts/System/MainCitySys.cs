using PEProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 主城业务系统
/// </summary>
public class MainCitySys : SystemRoot
{
    public static MainCitySys Instance = null;

    public MainCityWnd mainCityWnd;
    public InfoWnd infoWnd;
    public GuideWnd guideWnd;
    public StrongWnd strongWnd;
    public ChatWnd chatWnd;
    public BuyWnd buyWnd;
    public TaskWnd taskWnd;

    private PlayerController playerCtrl;
    private Transform charCamTrans;
    private AutoGuideCfg curTaskData;
    private Transform[] npcPosTrans;
    private NavMeshAgent nav;


    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
        PECommon.Log("Init MainCitySys...");
    }

    public void EnterMainCity()
    {
        MapCfg mapData = resSvc.GetMapCfg(Constants.MainCityMapID);
        resSvc.AsyncLoadScene(mapData.sceneName, () =>
         {
             PECommon.Log("Enter MainCity...");
             // 加载主角
             LoadPlayer(mapData);

             // 打开主城场景UI
             mainCityWnd.SetWndState();

             GameRoot.Instance.GetComponent<AudioListener>().enabled = false;

             // 播放主城背景音乐
             audioSvc.PlayBGMusic(Constants.BGMainCity);

             GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
             MainCityMap mcm = map.GetComponent<MainCityMap>();
             npcPosTrans = mcm.NpcPosTrans;

             // 设置人物展示相机
             if (charCamTrans != null)
             {
                 charCamTrans.gameObject.SetActive(false);
             }
         });
    }

    #region FubenSys
    public void EnterFuben()
    {
        StopNavTask();
        FubenSys.Instance.EnterFunben();
    }
    #endregion

    private void LoadPlayer(MapCfg mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssassinCityPlayerPrefab, true);
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        // 相机位置初始化
        Camera.main.transform.position = mapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mapData.mainCamRote;

        playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();
        nav = player.GetComponent<NavMeshAgent>();
    }

    public void SetMoveDir(Vector2 dir)
    {
        StopNavTask();
        if (dir == Vector2.zero)
        {
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
        else
        {
            playerCtrl.SetBlend(Constants.BlendMove);
        }

        playerCtrl.Dir = dir;
    }

    #region Task Wnd
    public void OpenTaskRewardWnd()
    {
        StopNavTask();
        taskWnd.SetWndState();
    }

    public void RspTakeTaskReward(GameMsg msg)
    {
        RspTakeTaskReward data = msg.rspTakeTaskReward;
        GameRoot.Instance.SetPlayerDataByRspTakeTaskReward(data);
        if (taskWnd.GetWndState())
        {
            taskWnd.RefreshUI();
        }
        mainCityWnd.RefreshUI();
    }

    public void PshTaskPrgs(GameMsg msg)
    {
        PshTaskPrgs data = msg.pshTaskPrgs;
        GameRoot.Instance.SetPlayerDataByPshTaskPrgs(data);
        if (taskWnd.GetWndState())
        {
            taskWnd.RefreshUI();
        }
    }
    #endregion

    #region Buy Wnd
    public void OpenBuyWnd(int type)
    {
        StopNavTask();
        buyWnd.SetBuyType(type);
        buyWnd.SetWndState();
    }
    public void RspBuy(GameMsg msg)
    {
        RspBuy rspBuyData = msg.rspBuy;
        GameRoot.Instance.SetPlayerDataByBuy(rspBuyData);

        GameRoot.AddTips("购买成功");

        mainCityWnd.RefreshUI();
        buyWnd.SetWndState(false);
        if (msg.pshTaskPrgs != null)
        {
            GameRoot.Instance.SetPlayerDataByPshTaskPrgs(msg.pshTaskPrgs);
            if (taskWnd.GetWndState())
            {
                taskWnd.RefreshUI();
            }
        }
    }

    public void PshPower(GameMsg msg)
    {
        PshPower data = msg.pshPower;
        GameRoot.Instance.SetPlayerDataByPshPower(data);
        if (mainCityWnd.GetWndState())
        {
            mainCityWnd.RefreshUI();
        }
    }

    #endregion

    #region Chat Wnd
    public void OpenChatWnd()
    {
        StopNavTask();
        chatWnd.SetWndState();
    }

    public void PshChat(GameMsg msg)
    {
        chatWnd.AddChatMsg(msg.pshChat.name, msg.pshChat.chat);
    }

    #endregion

    #region Strong Wnd
    public void OpenStrongWnd()
    {
        StopNavTask();
        strongWnd.SetWndState();
    }

    public void RspStrong(GameMsg msg)
    {
        int zhanliPre = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);
        GameRoot.Instance.SetPlayerDataByStrong(msg.rspStrong);
        int zhanliNow = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);
        GameRoot.AddTips(Constants.Color("战力提升: " + (zhanliNow - zhanliPre), TextColor.Blue));
        strongWnd.UpdateUI();
        mainCityWnd.RefreshUI();
    }
    #endregion

    #region Info Wnd
    public void OpenInfoWnd()
    {
        StopNavTask();
        if (charCamTrans == null)
        {
            charCamTrans = GameObject.FindGameObjectWithTag("CharShowCam").transform;
        }
        // 设置相对位置
        charCamTrans.localPosition = playerCtrl.transform.position + playerCtrl.transform.forward * 3.8f + new Vector3(0, 1.2f, 0);
        charCamTrans.localEulerAngles = new Vector3(0, 180 + playerCtrl.transform.localEulerAngles.y, 0);
        charCamTrans.localScale = Vector3.one;
        charCamTrans.gameObject.SetActive(true);
        infoWnd.SetWndState();
    }

    public void CloseInfoWnd()
    {
        if (charCamTrans != null)
        {
            charCamTrans.gameObject.SetActive(false);
            infoWnd.SetWndState(false);
        }
    }

    private float startRoate = 0;
    public void SetStartRoate()
    {
        startRoate = playerCtrl.transform.localEulerAngles.y;
    }

    public void SetPlayerRoate(float roate)
    {
        playerCtrl.transform.localEulerAngles = new Vector3(0, startRoate + roate, 0);
    }
    #endregion

    #region Guide Wnd
    private bool isNavGuide = false;
    public void RunTask(AutoGuideCfg agc)
    {
        if (agc != null)
        {
            curTaskData = agc;
        }

        // 解析任务数据
        nav.enabled = true;
        if (curTaskData.npcID != -1)
        {
            float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[agc.npcID].position);
            if (dis < 0.5f)
            {
                isNavGuide = false;
                nav.isStopped = true;
                playerCtrl.SetBlend(Constants.BlendIdle);
                nav.enabled = false;
                OpenGuideWnd();
            }
            else
            {
                isNavGuide = true;
                nav.enabled = true;
                nav.speed = Constants.PlayerMoveSpeed;
                nav.SetDestination(npcPosTrans[agc.actID].position);
                playerCtrl.SetBlend(Constants.BlendMove);
            }
        }
        else
        {
            OpenGuideWnd();
        }
    }

    private void Update()
    {
        if (isNavGuide)
        {
            IsAriveNavPos();
            playerCtrl.SetCam();
        }
    }

    private void IsAriveNavPos()
    {
        float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[curTaskData.npcID].position);
        if (dis < 0.5f)
        {
            isNavGuide = false;
            nav.isStopped = true;
            playerCtrl.SetBlend(Constants.BlendIdle);
            nav.enabled = false;
            OpenGuideWnd();
        }
    }

    private void StopNavTask()
    {
        if (isNavGuide)
        {
            isNavGuide = false;
            nav.isStopped = true;
            nav.enabled = false;
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
    }

    private void OpenGuideWnd()
    {
        guideWnd.SetWndState();
    }

    public AutoGuideCfg GetCurtTaskData()
    {
        return curTaskData;
    }

    public void RspGuide(GameMsg msg)
    {
        RspGuide data = msg.rspGuide;
        GameRoot.AddTips(Constants.Color("任务奖励 金币+" + curTaskData.coin + " 经验+" + curTaskData.exp, TextColor.Blue));
        switch (curTaskData.actID)
        {
            case 0:
                // 与智者对话
                break;
            case 1:
                // 进入副本
                EnterFuben();
                break;
            case 2:
                // 进入强化界面
                OpenStrongWnd();
                break;
            case 3:
                // 进入体力购买
                OpenBuyWnd(0);
                break;
            case 4:
                // 进入金币锻造
                OpenBuyWnd(1);
                break;
            case 5:
                // 进入世界聊天
                OpenChatWnd();
                break;
        }
        GameRoot.Instance.SetPlayerDataByGuide(data);
        mainCityWnd.RefreshUI();
    }
    #endregion
}
