using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StarUIController : MonoBehaviour
{
    public InputField name_input;
    public Button name_btn;


    private void Start()
    {
        name_input.text = PlayerPrefs.GetString("player_name", "");
        name_btn.onClick.AddListener(() =>
        {
            if (name_input.text == "") return;
            PlayerPrefs.SetString("player_name", name_input.text);
            SceneManager.LoadScene(NetManager.Ins.offlineScene);
        });
    }
}
