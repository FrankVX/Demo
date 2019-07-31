using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StartPoint : NetworkBehaviour
{
    public static Transform trans;
    private void Awake()
    {
        trans = this.transform;
    }
}
