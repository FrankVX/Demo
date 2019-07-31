using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EditorBtn : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (GameSetting.Ins)
                GameSetting.Ins.isEditor = !GameSetting.Ins.isEditor;
        });
    }
}
