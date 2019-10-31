using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI界面基类
/// </summary>
public class WindowRoot : MonoBehaviour
{
    protected ResSvc resSvc = null;
    protected AudioSvc audioSvc = null;
    protected NetSvc netSvc = null;
    protected TimerSvc timerSvc = null;

    public void SetWndState(bool isActive = true)
    {
        if (gameObject.activeSelf != isActive)
        {
           SetActive(gameObject,isActive);
        }
        if (isActive)
        {
            InitWnd();
        }
        else
        {
            ClearWnd();
        }
    }

    public bool GetWndState()
    {
        return gameObject.activeSelf;
    }

    protected virtual void InitWnd()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        netSvc = NetSvc.Instance;
        timerSvc = TimerSvc.Instance;
    }

    protected virtual void ClearWnd()
    {
        resSvc = null;
        audioSvc = null;
        netSvc = null;
    }

    #region Tool Functions
    protected void SetActive(GameObject go,bool isActive = true)
    {
        go.SetActive(isActive);
    }

    protected void SetActive(Component component,bool isActive = true)
    {
        component.gameObject.SetActive(isActive);
    }

    protected void SetText(Text txt,object context = null)
    {
        if (txt.text == null)
            txt.text = "";
        else
            txt.text = context.ToString();
    }

    protected void SetText(Transform trans,object context = null)
    {
        Text txt = trans.GetComponent<Text>();
        if (txt == null)
            throw new System.Exception("Text not found!");
        SetText(txt, context);
    }

    protected T GetOrAddComponect<T>(GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        if (t == null)
        {
            t = go.AddComponent<T>();
        }
        return t;
    }

    protected void SetSprite(Image img,string path)
    {
        Sprite sprite = resSvc.LoadSprite(path, true);
        img.sprite = sprite;
    }

    protected Transform GetTrans(Transform trans,string name)
    {
        if (trans != null)
        {
            return trans.Find(name);
        }
        else
        {
            return transform.Find(name);
        }
    } 
    #endregion

    #region Click Events
    protected void OnClick(GameObject go, Action<object> cb,object agrs)
    {
        PEListener listener = GetOrAddComponect<PEListener>(go);
        listener.onClick = cb;
        listener.agrs = agrs;
    }

    protected void OnClickDown(GameObject go,Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponect<PEListener>(go);
        listener.onClickDown = cb;
    }

    protected void OnClickUp(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponect<PEListener>(go);
        listener.onClickUp = cb;
    }

    protected void OnDrag(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponect<PEListener>(go);
        listener.onDrag = cb;
    }
    #endregion
}
