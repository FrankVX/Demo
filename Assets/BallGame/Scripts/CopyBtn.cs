using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyBtn : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (GameSetting.CurEditorBlock)
            {
                Instantiate(GameSetting.CurEditorBlock).transform.position += Vector3.up * 3;
                return;
            }
        });
    }

}
