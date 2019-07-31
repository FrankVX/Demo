using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetDiscovery : NetworkDiscovery
{
    public static NetDiscovery ins { get; private set; }
    public event Action<string, string> onReceive;
    void Awake()
    {
        ins = this;
    }

    
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);
        onReceive?.Invoke(fromAddress, data);
    }
}
