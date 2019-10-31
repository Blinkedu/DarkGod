using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UI事件监听插件
/// </summary>
public class PEListener : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler,IPointerClickHandler
{
    public Action<PointerEventData> onClickDown; 
    public Action<PointerEventData> onClickUp; 
    public Action<PointerEventData> onDrag;
    public Action<object> onClick;

    public object agrs;

    public void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null)
        {
            onDrag(eventData);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onClickDown != null)
        {
            onClickDown(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onClickUp != null)
        {
            onClickUp(eventData);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(onClick!=null)
        {
            onClick(agrs);
        }
    }
}
