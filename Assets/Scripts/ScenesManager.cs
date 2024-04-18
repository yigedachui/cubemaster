using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    public Button gameStart;

    private void Awake()
    {
        gameStart.onClick.AddListener(GameStart);    
    }

    public void GameStart() {
        SceneManager.LoadScene(1);
    }

}
