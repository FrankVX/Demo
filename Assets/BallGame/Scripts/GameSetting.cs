using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameSetting : NetworkBehaviour
{
    public static GameSetting Ins;

    private static GameObject curEditorBlock;

    [SyncVar] public bool isEditor = true;
    [SyncVar] public bool isGameing = false;

    public override void OnStartClient()
    {
        base.OnStartClient();
        Ins = this;
        GameUIController.Ins.server.SetActive(isServer);
    }

    [Command]
    public void CmdStartGame()
    {
        isEditor = false;
        isGameing = true;
        RpcOnStartGame();
    }

    [ClientRpc]
    void RpcOnStartGame()
    {
        BallController.Self?.Reset();
    }

    public void GameOver(string winner)
    {
        isEditor = true;
        isGameing = false;
        RpcOnGameOver(winner);
    }

    [ClientRpc]
    void RpcOnGameOver(string winner)
    {
        GameUIController.Ins?.ShowWiner(winner);
    }




    public static bool IsDrawing { get; set; } = false;
    public static GameObject CurEditorBlock
    {
        get => curEditorBlock;
        set
        {
            if (curEditorBlock)
            {
                curEditorBlock.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Default"); ;
                value.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Selecting");
            }
            curEditorBlock = value;
        }
    }


    public static Vector3 CurMouseWorldPos(Vector3 screenPos)
    {
        var pos = Camera.main.ScreenToWorldPoint(screenPos);
        pos.z = 0;
        return pos;
    }
}
