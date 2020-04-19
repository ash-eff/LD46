using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    TransistionLoader loader;

    private void Awake()
    {
        loader = FindObjectOfType<TransistionLoader>();
    }

    public void StartGame()
    {
        loader.LoadNextScene(1);
    }
}
