using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 副本选择界面
/// </summary>
public class FubenWnd : WindowRoot
{
    public Button[] fbBtnArr;
    public Transform pointerTrans;
    private PlayerData pd;

    protected override void InitWnd()
    {
        base.InitWnd();
        pd = GameRoot.Instance.PlayerData;

        RefreshUI();
    }

    public void RefreshUI()
    {
        int fbid = pd.fuben;
        for (int i = 0; i < fbBtnArr.Length; i++)
        {
            if (i < fbid % 10000)
            {
                SetActive(fbBtnArr[i], true);
                if (i == fbid % 10000 - 1)
                {
                    pointerTrans.SetParent(fbBtnArr[i].transform);
                    pointerTrans.localPosition = new Vector3(25f, 100f, 0);
                }
            }
            else
            {
                SetActive(fbBtnArr[i], false);
            }
        }
    }

    public void ClickTaskBtn(int fbid)
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        // 检查体力是否足够
        int power = resSvc.GetMapCfg(fbid).power;
        if (power > pd.power)
        {
            GameRoot.AddTips("体力值不足");
        }
        else
        {
            // 发送网络消息
            netSvc.SendMsg(new GameMsg
            {
                cmd = (int)CMD.ReqFBFight,
                reqFBFight = new ReqFBFight
                {
                    fbid = fbid
                }
            });
        }
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }
}
