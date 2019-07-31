using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text;

public class LobyUIController : MonoBehaviour
{
    public InputField room_input, ip_input;
    public Button join_btn, creat_btn, serch_btn;
    public Transform grid;
    public GameObject prefab;

    NetManager manager => NetManager.Ins;
    NetDiscovery discovery => NetDiscovery.ins;

    void Start()
    {
        ip_input.text = PlayerPrefs.GetString("room_IP", "");
        room_input.text = PlayerPrefs.GetString("room_name", "");
        discovery.onReceive += Discovery_onReceive;

        if (discovery.running)
        {
            discovery.StopBroadcast();
        }

        if (discovery.Initialize())
        {
            discovery.StartAsClient();
        }

        creat_btn.onClick.AddListener(() =>
            {
                if (room_input.text == "") return;
                manager.networkAddress = "localhost";
                manager.StartHost();
                discovery.useNetworkManager = false;
                discovery.broadcastData = room_input.text;
                PlayerPrefs.SetString("room_name", room_input.text);
                if (discovery.running)
                    discovery.StopBroadcast();
                discovery.Initialize();
                discovery.StartAsServer();
            });

        serch_btn.onClick.AddListener(() =>
        {
            RefreshList();
            if (discovery.running && discovery.isClient)
            {
                return;
            }
            if (discovery.running)
                discovery.StopBroadcast();
            discovery.Initialize();
            discovery.StartAsClient();
        });

        join_btn.onClick.AddListener(() =>
        {
            if (ip_input.text == "") return;
            if (discovery.running)
                discovery.StopBroadcast();
            manager.networkAddress = ip_input.text;
            PlayerPrefs.SetString("room_IP", ip_input.text);
            manager.StartClient();
        });
    }

    void OnDestroy()
    {
        discovery.onReceive -= Discovery_onReceive;
    }

    HashSet<string> set = new HashSet<string>();
    private void Discovery_onReceive(string addr, string data)
    {
        if (!set.Contains(addr))
        {
            print($"Discovery_onReceive  add--->{addr}, data:{data}");
            set.Add(addr);
            var obj = Instantiate(prefab, grid);
            obj.SetActive(true);
            obj.GetComponentInChildren<Text>().text = data;
            obj.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                if (discovery.running)
                    discovery.StopBroadcast();
                manager.networkAddress = addr;
                manager.StartClient();
            });
        }
    }

    public void RefreshList()
    {
        set.Clear();
        for (int i = 0; i < grid.childCount; i++)
        {
            Destroy(grid.GetChild(i).gameObject);
        }
    }



}
