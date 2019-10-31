using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 强化升级界面
/// </summary>
public class StrongWnd : WindowRoot
{
    #region UI Define
    public Image imgCurtPos;
    public Text txtStarLv;
    public Transform starTransGrp;
    public Text propHp1;
    public Text propHurt1;
    public Text propDef1;
    public Text propHp2;
    public Text propHurt2;
    public Text propDef2;
    public Image propArr1;
    public Image propArr2;
    public Image propArr3;

    public Text txtNeedLv;
    public Text txtCostCoin;
    public Text txtCostCrystal;

    public Transform costTransRoot;
    public Text txtCoin;
    #endregion

    #region Data Area
    public Transform posBtnTrans;
    private Image[] imgs = new Image[6];
    private int curtIndex;
    private PlayerData pd;
    private StrongCfg nextsd;
    #endregion

    protected override void InitWnd()
    {
        base.InitWnd();
        pd = GameRoot.Instance.PlayerData;
        RegClickEvts();

        ClickPosItem(0);
    }

    private void RegClickEvts()
    {
        for (int i = 0; i < posBtnTrans.childCount; i++)
        {
            Image img = posBtnTrans.GetChild(i).GetComponent<Image>();
            OnClick(img.gameObject, agrs =>
            {
                ClickPosItem((int)agrs);
                audioSvc.PlayUIAudio(Constants.UIClickBtn);
            }, i);
            imgs[i] = img;
        }
    }

    private void ClickPosItem(int index)
    {
        PECommon.Log("Click Item: " + index);
        curtIndex = index;
        for (int i = 0; i < imgs.Length; i++)
        {
            Transform trans = imgs[i].transform;
            if (i == curtIndex)
            {
                // 箭头显示
                SetSprite(imgs[i], PathDefine.ItemArrorBG);
                trans.localPosition = new Vector3(10, trans.localPosition.y, 0);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 95);
            }
            else
            {
                SetSprite(imgs[i], PathDefine.ItemPlatBG);
                trans.localPosition = new Vector3(0, trans.localPosition.y, 0);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(220, 85);
            }
        }
        RefreshItem();
    }

    private void RefreshItem()
    {
        // 金币
        SetText(txtCoin, pd.coin);
        switch (curtIndex)
        {
            case 0:
                SetSprite(imgCurtPos, PathDefine.ItemToukui);
                break;
            case 1:
                SetSprite(imgCurtPos, PathDefine.ItemBody);
                break;
            case 2:
                SetSprite(imgCurtPos, PathDefine.ItemYaobu);
                break;
            case 3:
                SetSprite(imgCurtPos, PathDefine.ItemHand);
                break;
            case 4:
                SetSprite(imgCurtPos, PathDefine.ItemLeg);
                break;
            case 5:
                SetSprite(imgCurtPos, PathDefine.ItemFoot);
                break;
        }
        SetText(txtStarLv, pd.strongArr[curtIndex]+"星级");
        int curtStarLv = pd.strongArr[curtIndex];
        for (int i = 0; i < starTransGrp.childCount; i++)
        {
            Image img = starTransGrp.GetChild(i).GetComponent<Image>();
            if (i < curtStarLv)
                SetSprite(img, PathDefine.SpStar2);
            else
                SetSprite(img, PathDefine.SpStar1);
        }

        int nextStarLv = curtStarLv + 1;
        int sumAddHp = resSvc.GetPropAddValPreLv(curtIndex, nextStarLv, 1);
        int sumAddHurt = resSvc.GetPropAddValPreLv(curtIndex, nextStarLv, 2);
        int sumAddDef = resSvc.GetPropAddValPreLv(curtIndex, nextStarLv, 3);
        SetText(propHp1, "生命 +" + sumAddHp);
        SetText(propHurt1, "伤害 +" + sumAddHurt);
        SetText(propDef1, "防御 +" + sumAddDef);

        nextsd = resSvc.GetStrongCfg(curtIndex, nextStarLv);
        if(nextsd != null)
        {
            SetActive(propHp2, true);
            SetActive(propHurt2, true);
            SetActive(propDef2, true);

            SetActive(costTransRoot, true);
            SetActive(propArr1, true);
            SetActive(propArr2, true);
            SetActive(propArr3, true);

            SetText(propHp2, "强化后 +" + nextsd.addhp);
            SetText(propHurt2, "+" + nextsd.addhp);
            SetText(propDef2, "+" + nextsd.adddef);

            SetText(txtNeedLv,"需要等级："+nextsd.minlv);
            SetText(txtCostCoin, "需要消耗：      "+nextsd.coin);
            SetText(txtCostCrystal, nextsd.crystal > pd.crystal ?
                Constants.Color(nextsd.crystal + "/" + pd.crystal,TextColor.Red) :
                nextsd.crystal + "/" + pd.crystal);
        }
        else
        {
            SetActive(propHp2, false);
            SetActive(propHurt2, false);
            SetActive(propDef2, false);

            SetActive(costTransRoot, false);
            SetActive(propArr1, false);
            SetActive(propArr2, false);
            SetActive(propArr3, false);
        }
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }

    public void ClickStrongBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        if (pd.strongArr[curtIndex] < 10)
        {
            if (pd.lv < nextsd.minlv)
            {
                GameRoot.AddTips("角色等级不够");
                return;
            }
            if (pd.coin < nextsd.coin)
            {
                GameRoot.AddTips("金币数量不够");
                return;
            }
            if (pd.crystal < nextsd.crystal)
            {
                GameRoot.AddTips("水晶数量不够");
                return;
            }
            netSvc.SendMsg(new GameMsg
            {
                cmd = (int)CMD.ReqStrong,
                reqStrong = new ReqStrong
                {
                    pos = curtIndex
                }
            });
        }
        else
        {
            GameRoot.AddTips("星级已经升满");
        }
    }

    public void UpdateUI()
    {
        audioSvc.PlayUIAudio(Constants.FBItemEnter);
        ClickPosItem(curtIndex);
    }
}
