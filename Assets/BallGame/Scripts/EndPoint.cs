using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EndPoint : NetworkBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnEnter(collision.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnEnter(collision.transform);
    }

    private void OnEnter(Transform transform)
    {
        if (GameSetting.Ins && NetManager.Ins.IsHost && GameSetting.Ins.isGameing)
        {
            var name = transform.GetComponent<BallController>().player_name;
            GameSetting.Ins.GameOver(name);
        }

    }
}
