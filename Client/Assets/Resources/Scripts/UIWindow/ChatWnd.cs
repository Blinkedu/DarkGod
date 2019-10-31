using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 聊天界面
/// </summary>
public class ChatWnd : WindowRoot
{
    public InputField iptChat;
    public Text txtChat;
    public Image imgWorld;
    public Image imgGuild;
    public Image imgFriend;

    private int chatType;
    private List<string> chatLst = new List<string>();

    protected override void InitWnd()
    {
        base.InitWnd();
        chatType = 0;
        RefreshUI();
    }

    public void AddChatMsg(string name, string chat)
    {
        chatLst.Add(Constants.Color(name + "：", TextColor.Blue) + chat);
        if (chatLst.Count > 12)
        {
            chatLst.RemoveAt(0);
        }
        if (GetWndState())
            RefreshUI();
    }

    private void RefreshUI()
    {
        // 世界聊天
        if (chatType == 0)
        {
            string chatMsg = "";
            for (int i = 0; i < chatLst.Count; i++)
            {
                chatMsg += chatLst[i] + "\n";
            }
            SetText(txtChat, chatMsg);
            SetSprite(imgWorld, PathDefine.ChatBtnType1);
            SetSprite(imgGuild, PathDefine.ChatBtnType2);
            SetSprite(imgFriend, PathDefine.ChatBtnType2);
        }
        // 公会聊天
        else if (chatType == 1)
        {
            SetText(txtChat, "尚未加入公会");
            SetSprite(imgWorld, PathDefine.ChatBtnType2);
            SetSprite(imgGuild, PathDefine.ChatBtnType1);
            SetSprite(imgFriend, PathDefine.ChatBtnType2);
        }
        // 好友聊天
        else if (chatType == 2)
        {
            SetText(txtChat, "暂无好友信息");
            SetSprite(imgWorld, PathDefine.ChatBtnType2);
            SetSprite(imgGuild, PathDefine.ChatBtnType2);
            SetSprite(imgFriend, PathDefine.ChatBtnType1);
        }
    }

    private bool canSend = true;
    public void ClickSendBtn()
    {
        if (!canSend)
        {
            GameRoot.AddTips("聊天消息每隔5秒才能发送一条");
            return;
        }
        if (!string.IsNullOrEmpty(iptChat.text) && iptChat.text.Trim(' ') != string.Empty)
        {
            if (iptChat.text.Length > 12)
            {
                GameRoot.AddTips("输入的消息不能超过12个字");
            }
            else
            {
                // 发送网络消息到服务器
                GameMsg msg = new GameMsg
                {
                    cmd = (int)CMD.SndChat,
                    sndChat = new SndChat
                    {
                        chat = iptChat.text
                    }
                };
                iptChat.text = "";
                netSvc.SendMsg(msg);
                canSend = false;

                timerSvc.AddTimeTask(id =>
                {
                    canSend = true;
                }, 5, PETimeUnit.Second);
            }
        }
        else
        {
            GameRoot.AddTips("尚未输入聊天信息");
        }
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;
        SetWndState(false);
    }

    public void ClickWorldBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;
        RefreshUI();
    }

    public void ClickGuildBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 1;
        RefreshUI();
    }

    public void ClickFriendBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 2;
        RefreshUI();
    }
}
