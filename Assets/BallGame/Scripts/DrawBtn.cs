using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawBtn : MonoBehaviour
{
    void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameSetting.IsDrawing = !GameSetting.IsDrawing;
        });
    }
}
