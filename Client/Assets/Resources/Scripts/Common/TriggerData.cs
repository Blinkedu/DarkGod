using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图触发数据
/// </summary>
public class TriggerData : MonoBehaviour
{
    public int triggerWave;
    public MapMgr mapMgr;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (mapMgr != null)
            {
                mapMgr.TriggerMonsterBorn(this,triggerWave);
            }
        }
    }
}
