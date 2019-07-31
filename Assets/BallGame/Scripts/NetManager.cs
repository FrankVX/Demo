using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetManager : NetworkManager
{
    public static NetManager Ins => NetworkManager.singleton as NetManager;


    public bool IsHost { get; set; }
    public bool IsServer { get; set; }

    public override void OnStartServer()
    {
        base.OnStartServer();
        IsServer = true;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        IsServer = false;
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        IsHost = true;
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
        IsHost = false;
    }
}
