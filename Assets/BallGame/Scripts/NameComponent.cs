using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cinemachine;
using TMPro;

public class NameComponent : NetworkBehaviour
{
    public TextMeshPro textMesh;

    [SyncVar] string player_name;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        var name = PlayerPrefs.GetString("player_name", "");
        CmdSetName(name.Trim());
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        textMesh.text = player_name;
    }

    [Command]
    void CmdSetName(string name)
    {
        player_name = name;
        RpcSetName(name);
    }

    [ClientRpc]
    void RpcSetName(string name)
    {
        textMesh.text = name;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
