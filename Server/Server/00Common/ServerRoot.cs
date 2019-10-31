/// <summary>
/// 服务器初始化
/// </summary>
public class ServerRoot
{
    private static ServerRoot instance = null;
    public static ServerRoot Instance
    {
        get
        {
            if (instance == null)
                instance = new ServerRoot();
            return instance;
        }
    }

    private ServerRoot() { }

    public void Init()
    {
        // 数据库
        DBMgr.Instance.Init();

        // 服务层
        NetSvc.Instance.Init();
        CacheSvc.Instance.Init();
        CfgSvc.Instance.Init();
        TimerSvc.Instance.Init();

        // 业务系统
        LoginSys.Instance.Init();
        GuideSys.Instance.Init();
        StrongSys.Instance.Init();
        ChatSys.Instance.Init();
        BuySys.Instance.Init();
        PowerSys.Instance.Init();
        TaskSys.Instance.Init();
        FubenSys.Instance.Init();
    }

    public void Update()
    {
        NetSvc.Instance.Update();
        TimerSvc.Instance.Update();
    }

    private int sessionID = 0;
    public int GetSessionID()
    {
        if (sessionID == int.MaxValue)
        {
            sessionID = 0;
        }
        return ++sessionID;
    }
}
