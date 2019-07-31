using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.Networking;
using Cinemachine;
using TMPro;

public class BallController : NetworkBehaviour
{
    public static BallController Self;

    public TextMeshPro textMesh;
    public float velocity = 1, max_distance = 10;
    Rigidbody2D _rigidbody;
    [SyncVar]
    public string player_name = "";


    bool canMove;
    float curForce;
    VariableJoystick joystick;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        joystick = GameUIController.Ins.joystick;
        var camera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        camera.LookAt = transform;
        camera.Follow = transform;
        GetComponent<SpriteRenderer>().color = Color.red;
        Self = this;
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

    public void Reset()
    {
        if (StartPoint.trans && isLocalPlayer)
        {
            transform.position = StartPoint.trans.position;
            _rigidbody.velocity = Vector2.zero;
        }
    }


    int num;
    private void Update()
    {
        textMesh.transform.position = transform.position + Vector3.up;
        textMesh.transform.rotation = Quaternion.identity;
        if (GameSetting.Ins && GameSetting.Ins.isEditor)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.velocity = Vector2.zero;
            if (isLocalPlayer)
            {
                var x = Input.GetAxis("Horizontal");
                var y = Input.GetAxis("Vertical");
                transform.position += new Vector3(x, y, 0) * Time.deltaTime * 10;

                transform.position += new Vector3(joystick.Direction.x, joystick.Direction.y, 0) * Time.deltaTime * 10;
            }
            return;
        }
        _rigidbody.isKinematic = false;
        if (!isLocalPlayer) return;
        if (transform.position.y < -10)
        {
            Reset();
        }
    }

    public void Move(Vector3 worldPos)
    {
        if (!isLocalPlayer) return;

        if (_rigidbody.velocity.magnitude < 0.1f)
        {
            curForce = velocity;
            num = 3;
            canMove = true;
        }
        else if (canMove && _rigidbody.velocity.y <= 0)
        {
            curForce *= 0.66f;
            num--;
        }
        else return;

        if (num <= 0) return;
        var dir = transform.position - worldPos;
        dir.z = 0;
        var legth = Mathf.Min(dir.magnitude, max_distance);
        dir = dir.normalized * legth;
        _rigidbody.velocity = dir * curForce * 0.1f;

    }

    private void OnCollisionEnter(Collision collision)
    {
        canMove = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        canMove = false;
    }
}