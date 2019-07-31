using DigitalRubyShared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    public static GameUIController Ins { get; private set; }
    NetManager manager => NetManager.Ins;
    NetDiscovery discovery => NetDiscovery.ins;

    public GameObject server;
    public Text ip_lab, winer_lab;

    public VariableJoystick joystick;

    private void Awake()
    {
        Ins = this;
        var strHostName = System.Net.Dns.GetHostName();


        var ipEntry = System.Net.Dns.GetHostEntry(strHostName);


        var addr = ipEntry.AddressList;
        if (addr.Length > 0)
            ip_lab.text = addr[addr.Length - 1].MapToIPv4().ToString();

        CreateDoubleTapGesture();
    }

    private void RemoveAsteroids(float screenX, float screenY, float radius)
    {
        if (NetManager.Ins.IsHost && GameSetting.Ins.isEditor)
        {
            Vector3 pos = new Vector3(screenX, screenY, 0.0f);
            pos = Camera.main.ScreenToWorldPoint(pos);

            RaycastHit2D[] hits = Physics2D.CircleCastAll(pos, radius, Vector2.zero);
            foreach (RaycastHit2D h in hits)
            {
                if (h.transform.GetComponent<LineObj>())
                    NetworkServer.Destroy(h.transform.gameObject);
            }
        }
    }
    private void DoubleTapGestureCallback(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            RemoveAsteroids(gesture.FocusX, gesture.FocusY, 1);
        }
    }

    private void CreateDoubleTapGesture()
    {
        var doubleTapGesture = new TapGestureRecognizer();
        doubleTapGesture.NumberOfTapsRequired = 2;
        doubleTapGesture.StateUpdated += DoubleTapGestureCallback;
        //doubleTapGesture.RequireGestureRecognizerToFail = tripleTapGesture;
        FingersScript.Instance.AddGesture(doubleTapGesture);
    }

    public void ShowWiner(string winner)
    {
        StartCoroutine(ShowText(winner));
    }

    IEnumerator ShowText(string winner)
    {
        winer_lab.text = $"{winner} 牛逼!!";
        winer_lab.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        winer_lab.transform.parent.gameObject.SetActive(false);
    }

    public void LobyScene()
    {
        if (manager.IsHost)
            manager.StopHost();
        else
            manager.StopClient();
    }

    public void StartGame()
    {
        if (GameSetting.Ins && NetManager.Ins.IsHost)
        {
            GameSetting.Ins.CmdStartGame();
        }
    }

    private void Update()
    {
        if (GameSetting.Ins && GameSetting.Ins.isEditor != joystick.gameObject.activeInHierarchy)
            joystick.gameObject.SetActive(GameSetting.Ins.isEditor);
        //if (NetManager.Ins && NetManager.Ins.isNetworkActive)
        //{
        //    ip_lab.text = NetManager.Ins.client.serverIp;
        //}
    }
}
