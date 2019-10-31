using PEProtocol;
using System.Collections.Generic;
/// <summary>
/// 缓存层
/// </summary>
public class CacheSvc
{
    private static CacheSvc instance = null;
    public static CacheSvc Instance
    {
        get
        {
            if (instance == null)
                instance = new CacheSvc();
            return instance;
        }
    }
    private CacheSvc() { }

    private DBMgr dbMgr = null;
    private Dictionary<string, ServerSession> onLineAcctDic = new Dictionary<string, ServerSession>();
    private Dictionary<ServerSession, PlayerData> onLineSessionDic = new Dictionary<ServerSession, PlayerData>();

    public void Init()
    {
        dbMgr = DBMgr.Instance;
        PECommon.Log("CacheSvc Init Done.");
    }

    // 账号是否在线
    public bool IsAcctOnLine(string acct)
    {
        return onLineAcctDic.ContainsKey(acct);
    }

    // 通过账号密码获取玩家数据，密码错误返回null,账号不存在则默认创建新账号
    public PlayerData GetPlayerData(string acct, string pass)
    {
        return dbMgr.QueryPlayerData(acct, pass);
    }

    // 账号上线，缓存数据
    public void AcctOnline(string acct, ServerSession session, PlayerData playerData)
    {
        onLineAcctDic.Add(acct, session);
        onLineSessionDic.Add(session, playerData);
    }

    // 名字是否存在
    public bool IsNameExist(string name)
    {
        return dbMgr.QueryNameData(name);
    }

    // 获取所有在线的客户端
    public List<ServerSession> GetOnlineServerSession()
    {
        List<ServerSession> lst = new List<ServerSession>();
        foreach (var item in onLineSessionDic)
        {
            lst.Add(item.Key);
        }
        return lst;
    }

    // 根据Session获取玩家数据
    public PlayerData GetPlayerDataBySession(ServerSession session)
    {
        if (onLineSessionDic.TryGetValue(session, out PlayerData playerData))
        {
            return playerData;
        }
        else
        {
            return null;
        }
    }

    // 获取当前所有在线玩家的缓存
    public Dictionary<ServerSession,PlayerData> GetOnlineCache()
    {
        return onLineSessionDic;
    }
    
    // 根据Id号获取玩家连接
    public ServerSession GetOnlineServerSession(int id)
    {
        ServerSession session = null;
        foreach (var item in onLineSessionDic)
        {
            if(item.Value.id == id)
            {
                session = item.Key;
                break;
            }
        }
        return session;
    }

    // 更新玩家数据
    public bool UpdatePlayerData(int id, PlayerData playerData)
    {
        return dbMgr.UpdatePlayerData(id, playerData);
    }

    // 账号下线
    public void AcctOffline(ServerSession session)
    {
        foreach (var item in onLineAcctDic)
        {
            if(item.Value == session)
            {
                onLineAcctDic.Remove(item.Key);
                break;
            }
        }

        bool succ = onLineSessionDic.Remove(session);
        PECommon.Log("Offline Result: SessionID:"+session.sessionID +" "+ succ);
    }
}

