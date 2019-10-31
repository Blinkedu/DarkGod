using System;
using System.Threading;
/// <summary>
/// 服务器入口
/// </summary>
class ServerStart
{
    static void Main(string[] args)
    {
        ServerRoot.Instance.Init();
        while (true)
        {
            ServerRoot.Instance.Update();
            Thread.Sleep(20);
        }
    }
}
