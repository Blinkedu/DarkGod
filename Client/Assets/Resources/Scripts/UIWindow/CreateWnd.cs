using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 创建角色界面
/// </summary>
public class CreateWnd : WindowRoot
{
    public InputField iptName;

    protected override void InitWnd()
    {
        base.InitWnd();

        // 显示一个随机名字
        iptName.text = resSvc.GetRDNameData(false);
    }

    public void ClickRandBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        iptName.text = resSvc.GetRDNameData(false);
    }

    public void ClickEnterBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        if(!string.IsNullOrEmpty(iptName.text))
        {
            // 发送名字数据到服务器，登录主城
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqRename,
                reqRename = new ReqRename
                {
                    name = iptName.text
                }
            };
            netSvc.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("当前名字不符合规范");
        }
    }
}
