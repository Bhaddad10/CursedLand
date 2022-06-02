using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : IPersistentSingleton<GameManager>
{

    public Action<string> OnLoadedSceneComplete;

    private string _currentScene;
    private string _previousScene;

    public PlayerState playerState;
    public UIManager uiManager;


    // Start is called before the first frame update
    void Start()
    {
        //playerState.items.Add("Health", new Potion(5, Resources.Load<Sprite>("small Potions")));
        uiManager.UpdateInventory();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadScene(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        if(asyncOperation == null)
        {
            Debug.LogError("Erro ao carregar a scene " + sceneName);
            return;
        }
        _previousScene = _currentScene ?? firstSceneName;
        //_previousScene = _currentScene ?? gameObject.scene.name;
        _currentScene = sceneName;
        asyncOperation.completed += OnSceneLoaded;
    }

    private void OnSceneLoaded(AsyncOperation asyncOperation)
    {
        OnLoadedSceneComplete?.Invoke(_currentScene);


        //GameObject Hud = 
        if (_currentScene == "Main")
        {
            uiManager.potionsTray = GameObject.Find("Potions");
            uiManager.UpdateInventory();
        }
    }

    internal void ChangeToPreviousScene()
    {
        LoadScene(_previousScene);
        //GameManager.Instance.uiManager.potionsTray = GameObject.Find("Potions").GetComponent<GameObject>(); ;
    }
}
