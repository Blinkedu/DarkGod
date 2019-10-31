using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图管理器
/// </summary>
public class MapMgr : MonoBehaviour {
    private int waveIndex = 1;  // 默认生成第一批怪物
    private BattleMgr battleMgr;
    public TriggerData[] triggerArr;

    public void Init(BattleMgr battleMgr)
    {
        this.battleMgr = battleMgr;

        // 实例化第一批怪物
        battleMgr.LoadMonsterByWaveID(waveIndex);

        PECommon.Log("Init MapMgr Done.");
    }

    public void TriggerMonsterBorn(TriggerData trigger, int waveIndex)
    {
        if (battleMgr != null)
        {
            BoxCollider co = trigger.gameObject.GetComponent<BoxCollider>();
            co.isTrigger = false;
            battleMgr.LoadMonsterByWaveID(waveIndex);
            battleMgr.ActiveCurrentBatchMonsters();
            battleMgr.triggerCheck = true;
        }
    }

    public bool SetNextTriggerOn()
    {
        waveIndex += 1;
        for (int i = 0; i < triggerArr.Length; i++)
        {
            if(triggerArr[i].triggerWave == waveIndex)
            {
                BoxCollider co = triggerArr[i].GetComponent<BoxCollider>();
                co.isTrigger = true;
                return true;
            }
        }
        return false;
    }
}
