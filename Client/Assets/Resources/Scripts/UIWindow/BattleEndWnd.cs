using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 战斗结算界面
/// </summary>
public class BattleEndWnd : WindowRoot 
{
    #region UI Define
    public Transform rewardTrans;
    public Button btnClose;
    public Button btnExit;
    public Button btnSure;
    public Text txtTime;
    public Text txtRestHP;
    public Text txtReward;
    public Animation ani;

    #endregion

    private FBEndType endType = FBEndType.None;

    protected override void InitWnd()
    {
        base.InitWnd();
        RefreshUI();
    }

    private void RefreshUI()
    {
        switch (endType)
        {
            case FBEndType.Pause:
                SetActive(rewardTrans, false);
                SetActive(btnExit.gameObject);
                SetActive(btnClose.gameObject);
                break;
            case FBEndType.Win:
                SetActive(rewardTrans, false);
                SetActive(btnExit.gameObject,false);
                SetActive(btnClose.gameObject,false);
                MapCfg cfg = resSvc.GetMapCfg(fbid);
                int min = costtime / 60;
                int sec = costtime % 60;
                int coin = cfg.coin;
                int exp = cfg.exp;
                int crystal = cfg.crystal;
                SetText(txtTime, string.Format("通关时间: {0}:{1}", min, sec));
                SetText(txtRestHP, string.Format("剩余血量: {0}", resthp));
                SetText(txtReward, string.Format("关卡奖励: {0} {1} {2}", Constants.Color("金币" + coin, TextColor.Green), 
                    Constants.Color("经验" + exp, TextColor.Yellow) ,
                    Constants.Color("水晶" + crystal, TextColor.Blue)));
                timerSvc.AddTimeTask(tid =>
                {
                    SetActive(rewardTrans);
                    ani.Play();
                    timerSvc.AddTimeTask(tid1 =>
                    {
                        audioSvc.PlayUIAudio(Constants.FBItemEnter);
                        timerSvc.AddTimeTask(tid2 =>
                        {
                            audioSvc.PlayUIAudio(Constants.FBItemEnter);
                            timerSvc.AddTimeTask(tid3 =>
                            {
                                audioSvc.PlayUIAudio(Constants.FBItemEnter);
                                timerSvc.AddTimeTask(tid4 =>
                                {
                                    audioSvc.PlayUIAudio(Constants.FBLogoEnter);
                                }, 300);
                            }, 270);
                        }, 270);
                    },325);
                },1000);
                break;
            case FBEndType.Lose:
                SetActive(rewardTrans, false);
                SetActive(btnExit.gameObject);
                SetActive(btnClose.gameObject,false);
                audioSvc.PlayUIAudio(Constants.FBLose);
                break;
        }
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        BattleSys.Instance.battleMgr.isPauseGame = false;
        SetWndState(false);
    }

    public void ClickExitBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        // 进入主城，销毁当前战斗
        MainCitySys.Instance.EnterMainCity();
        BattleSys.Instance.DestroyBattle();
    }

    public void ClickSureBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        // 进入主城，销毁当前战斗
        MainCitySys.Instance.EnterMainCity();
        BattleSys.Instance.DestroyBattle();
        // 打开副本界面
        FubenSys.Instance.EnterFunben();
    }

    public void SetWndType(FBEndType endType)
    {
        this.endType = endType;
    }

    private int fbid;
    private int costtime;
    private int resthp;
    public void SetBattleEndData(int fbid,int costtime,int resthp)
    {
        this.fbid = fbid;
        this.costtime = costtime;
        this.resthp = resthp;
    }
}

public enum FBEndType
{
    None,
    Pause,
    Win,
    Lose
}
