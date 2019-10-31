using PEProtocol;
/// <summary>
/// 任务奖励系统
/// </summary>
public class TaskSys
{
    private static TaskSys instance = null;
    public static TaskSys Instance
    {
        get
        {
            if (instance == null)
                instance = new TaskSys();
            return instance;
        }
    }
    private TaskSys() { }

    private CacheSvc cacheSvc = null;
    private CfgSvc cfgSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        cfgSvc = CfgSvc.Instance;
        PECommon.Log("TaskSys Init Done.");
    }

    public void ReqTakeTaskReward(MsgPack pack)
    {
        ReqTakeTaskReward data = pack.msg.reqTakeTaskReward;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspTakeTaskReward
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        TaskRewardCfg trc = cfgSvc.GetTaskRewardCfg(data.rid);
        TaskRewardData trd = CalcTaskRewardData(pd, data.rid);
        if(trd.prgs == trc.count && !trd.taked)
        {
            pd.coin += trc.coin;
            PECommon.CalcExp(pd, trc.exp);
            trd.taked = true;

            // 更新任务进度数据
            CalcTaskArr(pd, trd);

            if (!cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                RspTakeTaskReward rspTakeTaskReward = new RspTakeTaskReward
                {
                    coin = pd.coin,
                    exp = pd.exp,
                    lv = pd.lv,
                    taskArr = pd.taskArr
                };
                msg.rspTakeTaskReward = rspTakeTaskReward;
            }
        }
        else
        {
            msg.err = (int)ErrorCode.ClientDataError;
        }
        pack.session.SendMsg(msg);
    }

    public TaskRewardData CalcTaskRewardData(PlayerData pd,int rid)
    {
        TaskRewardData trd = null;
        for (int i = 0; i < pd.taskArr.Length; i++)
        {
            string[] taskInfo = pd.taskArr[i].Split('|');
            if(int.Parse(taskInfo[0]) == rid)
            {
                trd = new TaskRewardData
                {
                    ID = int.Parse(taskInfo[0]),
                    prgs = int.Parse(taskInfo[1]),
                    taked = taskInfo[2].Equals("1")
                };
                break;
            }
        }
        return trd;
    }

    public void CalcTaskArr(PlayerData pd,TaskRewardData trd)
    {
        string result = trd.ID + "|" + trd.prgs + "|" + (trd.taked ? 1 : 0);
        int index = -1;
        for (int i = 0; i < pd.taskArr.Length; i++)
        {
            string[] taskinfo = pd.taskArr[i].Split('|');
            if(int.Parse(taskinfo[0])  == trd.ID)
            {
                index = i;
                break;
            }
        }
        pd.taskArr[index] = result;
    }

    public void CalcTaskPrgs(PlayerData pd,int tid)
    {
        TaskRewardData trd = CalcTaskRewardData(pd, tid);
        TaskRewardCfg trc = cfgSvc.GetTaskRewardCfg(tid);
        if (trd.prgs < trc.count)
        {
            trd.prgs += 1;
            // 更新任务进度
            CalcTaskArr(pd, trd);

            ServerSession session = cacheSvc.GetOnlineServerSession(pd.id);
            if (session != null)
            {
                session.SendMsg(new GameMsg
                {
                    cmd = (int)CMD.PshTaskPrgs,
                    pshTaskPrgs = new PshTaskPrgs
                    {
                        taskArr = pd.taskArr
                    }
                });
            }
        }
    }

    public PshTaskPrgs GetTaskPrgs(PlayerData pd, int tid)
    {
        TaskRewardData trd = CalcTaskRewardData(pd, tid);
        TaskRewardCfg trc = cfgSvc.GetTaskRewardCfg(tid);
        if (trd.prgs < trc.count)
        {
            trd.prgs += 1;
            // 更新任务进度
            CalcTaskArr(pd, trd);
            return new PshTaskPrgs
            {
                taskArr = pd.taskArr
            };
        }
        else
        {
            return null;
        }
    }
}

