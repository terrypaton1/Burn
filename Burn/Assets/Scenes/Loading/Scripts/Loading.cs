using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private const string UI = "UI";
    private const string CoreGame = "CoreGame";
    private const string Level1 = "Level_01";
    private const string Level2 = "Level_02";
    private int currentLoadingIndex;
    private const int targetFPS = 60;

    private readonly string[] loadSceneQueue = {UI, CoreGame, Level1, Level2};
    private IEnumerator gameLevelCoroutine;

    private void OnEnable()
    {
        // start the process of loading the UI and then core game
        Init();
        CoreConnector.Loading = this;
        CoreConnector.IsLevelsLoaded = true;
    }

    protected void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
    }

    protected void Update()
    {
        if (Application.targetFrameRate != targetFPS)
        {
            Debug.Log("setting frame rate to " + targetFPS);
            Application.targetFrameRate = targetFPS;
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Init()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadNextSceneInQueue();
        currentLoadingIndex++;
    }

    private void StartUpGame()
    {
        StartCoroutine(StartUpGameSequence());
    }

    private IEnumerator StartUpGameSequence()
    {
        CoreConnector.UIControl.Setup();
        CoreConnector.UIControl.Display(UIDisplay.MainMenu);
        yield return new WaitForSeconds(0.5f);
        CoreConnector.WorldSides.Setup();
    }

    private void LoadNextSceneInQueue()
    {
        if (currentLoadingIndex >= loadSceneQueue.Length)
        {
            //Debug.Log("All levels loaded");
            StartUpGame();
            return;
        }

        var nextSceneToLoad = loadSceneQueue[currentLoadingIndex];
        SceneManager.LoadScene(nextSceneToLoad, LoadSceneMode.Additive);
    }

    private void GamePlayLevelLoaded()
    {
        //     CoreConnector.CoreGameControl.ResetGame();
    }

    private enum LoadingType
    {
        None,
        Initial,
        GamePlayLevel
    }
}