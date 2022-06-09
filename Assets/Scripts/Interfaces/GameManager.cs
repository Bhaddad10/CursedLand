using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : IPersistentSingleton<GameManager>
{
    public const string SHOP_SCENE_NAME = "Shop";

    public Action<string> OnLoadedSceneComplete;

    private string _currentScene;
    private string _previousScene;

    public PlayerState playerState;
    
    //[HideInInspector]]
    public PlayerController playerController;

    public UIManager uiManager;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.playerState.Initialize();
        _currentScene = firstSceneName;
        //playerState.items.Add("Health", new Potion(5, Resources.Load<Sprite>("small Potions")));
        if (_currentScene != SHOP_SCENE_NAME)
            uiManager.potionsTray = GameObject.Find("Potions");
            uiManager.UpdateInventory();
        loadPlayerController();
    }


    void loadPlayerController()
    {
        if (playerController == null)
        {
            Debug.Log("Player Controller will be populated through code.");
            playerController = GameObject.FindObjectOfType<PlayerController>();
            //DontDestroyOnLoad(playerController.gameObject);
            if (playerController == null)
                Debug.Log("Player Controller unavailable");
        }

        if (_currentScene != SHOP_SCENE_NAME)
            playerState.healthBar = GameObject.Find("HealthBar-Image").GetComponent<Image>();
            //playerState
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadScene(string sceneName)
    {
        playerController.stashLastPosition();
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


        loadPlayerController();
        playerState.UpdateHealth();

        GameObject playerCamera = GameObject.Find("PlayerCamera");
        if (_currentScene != SHOP_SCENE_NAME)
        {
            uiManager.potionsTray = GameObject.Find("Potions");
            uiManager.UpdateInventory();
            playerController.goToLastPosition();
            playerCamera?.SetActive(true);
        } else
        {
            playerCamera?.SetActive(false);
        }
        playerController.saveLastPosition();

    }

    internal void ChangeToPreviousScene()
    {
        LoadScene(_previousScene);
        //GameManager.Instance.uiManager.potionsTray = GameObject.Find("Potions").GetComponent<GameObject>(); ;
    }
}
