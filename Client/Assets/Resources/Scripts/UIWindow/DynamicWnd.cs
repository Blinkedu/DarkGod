using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 动态UI元素界面
/// </summary>
public class DynamicWnd : WindowRoot
{
    public Animation tipsAni;
    public Text txtTips;
    public Transform hpItemRoot;

    public Animation selfDodgeAni;

    private bool isTipShow = false;
    private Queue<string> tipsQue = new Queue<string>();
    private Dictionary<string, ItemEntityHP> itemDic = new Dictionary<string, ItemEntityHP>();

    protected override void InitWnd()
    {
        base.InitWnd();

        SetActive(txtTips, false);
    }

    #region Tips相关
    public void AddTips(string tips)
    {
        lock (tipsQue)
        {
            tipsQue.Enqueue(tips);
        }
    }

    private void Update()
    {
        if (tipsQue.Count > 0 && !isTipShow)
        {
            lock (tipsQue)
            {
                string tips = tipsQue.Dequeue();
                isTipShow = true;
                SetTips(tips);
            }
        }
    }

    public void SetTips(string tips)
    {
        SetActive(txtTips);
        SetText(txtTips, tips);
        AnimationClip clip = tipsAni.GetClip("TipsShow");
        tipsAni.Play();

        // 延时关闭激活状态
        StartCoroutine(AniPlayDone(clip.length, () =>
         {
             SetActive(txtTips, false);
             isTipShow = false;
         }));
    }

    private IEnumerator AniPlayDone(float sec, Action cb)
    {
        yield return new WaitForSeconds(sec);
        if (cb != null)
            cb();
    }
    #endregion

    public void AddHpItemInfo(string mName,Transform trans, int hp)
    {
        ItemEntityHP item = null;
        if(itemDic.TryGetValue(mName,out item))
        {
            return;
        }
        else
        {
            GameObject go = resSvc.LoadPrefab(PathDefine.HPItemPrefab, true);
            go.transform.SetParent(hpItemRoot);
            go.transform.localPosition = new Vector3(-1000, 0, 0);
            ItemEntityHP ieh = go.GetComponent<ItemEntityHP>();
            ieh.InitItemInfo(trans, hp);
            itemDic.Add(mName, ieh);
        }
    }

    public void RmvHpItemInfo(string mName)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(mName, out item))
        {
            Destroy(item.gameObject);
            itemDic.Remove(mName);
        }
    }

    public void RmvAllHpItemInfo()
    {
        foreach (var item in itemDic)
        {
            Destroy(item.Value.gameObject);
        }
        itemDic.Clear();
    }

    public void SetDodge(string key)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetDodge();
        }
    }

    public void SetCritical(string key,int critical)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetCritical(critical);
        }
    }

    public void SetHurt(string key, int hurt)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetHurt(hurt);
        }
    }

    public void SetHpVal(string key, int oldVal,int newVal)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetHpVal(oldVal, newVal);
        }
    }

    public void SetSelfDodge()
    {
        selfDodgeAni.Stop();
        selfDodgeAni.Play();
    }
}
