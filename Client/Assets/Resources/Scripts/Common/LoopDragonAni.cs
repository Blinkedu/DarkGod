using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 飞龙循环动画
/// </summary>
public class LoopDragonAni : MonoBehaviour
{
    private Animation ani;

    private void Awake()
    {
        ani = GetComponent<Animation>();
    }

    private void Start()
    {
        if (ani != null)
        {
            InvokeRepeating("PlayDragonAni", 0, 20);
        }
    }

    private void PlayDragonAni()
    {
        if (ani != null)
        {
            ani.Play();
        }
    }
}
