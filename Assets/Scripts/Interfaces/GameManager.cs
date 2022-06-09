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


    // Available on HUD
    public PlayerState playerState;   
    [HideInInspector]
    public PlayerController playerController;
    [HideInInspector]
    public PotionUIManager potionUiManager;


    // Start is called before the first frame update
    void Start()
    {
        playerState.Initialize();
        _currentScene = firstSceneName;

        if (_currentScene != SHOP_SCENE_NAME)
            potionUiManager.updatePotionTray();
            
        loadPlayerController();
    }


    void loadPlayerController()
    {
        if (playerController == null)
        {
            Debug.Log("Player Controller will be populated through code.");
            playerController = FindObjectOfType<PlayerController>();

            // Had some problems
            //DontDestroyOnLoad(playerController.gameObject);

            if (playerController == null)
                Debug.Log("Player Controller unavailable");
        }

        if (_currentScene != SHOP_SCENE_NAME)
            playerState.healthBar = GameObject.Find("HealthBar-Image").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {}

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
        _currentScene = sceneName;
        asyncOperation.completed += OnSceneLoaded;
    }

    private void OnSceneLoaded(AsyncOperation asyncOperation)
    {
        OnLoadedSceneComplete?.Invoke(_currentScene);


        loadPlayerController();
        playerState.UpdateHealth();

        GameObject playerCamera = GameObject.Find("PlayerCamera");

        if (_currentScene == SHOP_SCENE_NAME) {
            playerCamera?.SetActive(false);
            playerController.saveLastPosition();
            return;
        }
        
        playerCamera?.SetActive(true);
        potionUiManager.updatePotionTray();

        playerController.goToLastPosition();
        playerController.saveLastPosition();

    }

    internal void ChangeToPreviousScene()
    {
        LoadScene(_previousScene);
    }
}
