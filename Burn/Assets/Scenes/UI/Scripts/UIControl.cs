using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    [SerializeField]
    protected UIGroup mainMenu;

    [SerializeField]
    protected UIGroup settings;

    [SerializeField]
    protected GamePlay gamePlay;

    [SerializeField]
    protected UIGroup gameOver;

    [SerializeField]
    protected UIGroup levelComplete;

    [SerializeField]
    protected UIGroup levelLoading;

    [SerializeField]
    protected UIGroup thanks;

    [SerializeField]
    protected Camera uiCamera;

    [SerializeField]
    protected SceneTransition sceneTransition;

    private IEnumerator coroutine;

    private const float HidingTime = 0.3f;

    private Dictionary<UIDisplay, UIGroup> uiGroups;
    private UIGroup currentUIGroup;

    protected void OnEnable()
    {
        CoreConnector.UIControl = this;
    }

    public void Setup()
    {
        uiGroups = new Dictionary<UIDisplay, UIGroup>
        {
            {UIDisplay.LevelLoading, levelLoading},
            {UIDisplay.LevelComplete, levelComplete},
            {UIDisplay.MainMenu, mainMenu},
            {UIDisplay.GamePlay, gamePlay},
            {UIDisplay.GameOver, gameOver},
            {UIDisplay.Settings, settings},
            {UIDisplay.Thanks, thanks}
        };

        // enforce all GameObjects on, I kept manually turning them off to get them out of the way.

        foreach (var keyPair in uiGroups)
        {
            var uiGroup = keyPair.Value;
            uiGroup.gameObject.SetActive(true);
        }

        sceneTransition.gameObject.SetActive(true);
        HideAll();
        EnableUICamera();
    }

    public void PlayEasyGame()
    {
        var gameSetting = CoreConnector.Instance.GetGameSettings();
        gameSetting.easyMode = true;
        LoadGameLevel();
    }

    public void PlayGame()
    {
        var gameSetting = CoreConnector.Instance.GetGameSettings();
        gameSetting.easyMode = false;
        LoadGameLevel();
    }

    void LoadGameLevel()
    {
        CoreConnector.SoundManager.PlaySound(SoundManager.Sounds.ButtonPress);
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = LoadGameLevelSequence();
        StartCoroutine(coroutine);
    }

    private IEnumerator LoadGameLevelSequence()
    {
        HideAll();
        ShowTransition();

        var timer = 0.0f;
        while (timer < HidingTime)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        var gameSetting = CoreConnector.Instance.GetGameSettings();

        CoreConnector.CameraControl.ClearShipMarkers();
        if (gameSetting.easyMode)
        {
            CoreConnector.Levels.ShowLevel(LevelNames.Level_02);
        }
        else
        {
            CoreConnector.Levels.ShowLevel(LevelNames.Level_01);
        }

        CoreConnector.CameraControl.DisplayShipMarkers();
    }

    public void QuitGame()
    {
#if !UNITY_EDITOR
		Application.Quit();
#endif
    }

    public void Display(UIDisplay uiDisplay)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = DisplaySequence(uiDisplay);
        StartCoroutine(coroutine);
    }

    private IEnumerator DisplaySequence(UIDisplay uiDisplay)
    {
        HideAll();

        DecideIfShowTransition(uiDisplay);

        var timer = 0.0f;
        while (timer < HidingTime)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        HideAllInstantly();
        uiGroups.TryGetValue(uiDisplay, out currentUIGroup);
        Assert.IsNotNull(currentUIGroup, "UIGroup not found:" + uiDisplay + " currentUIGroup:" + currentUIGroup);

        currentUIGroup.Show();

        // Level loading does not put away the transition, it is put away by the gameplay
        if (uiDisplay != UIDisplay.LevelLoading)
        {
            // now load the game

            PutAwayTransition();
        }

        if (uiDisplay == UIDisplay.MainMenu)
        {
            CoreGameControl.DisableGameRenderers();
        }
    }

    private void DecideIfShowTransition(UIDisplay uiDisplay)
    {
        // gameplay UI won't load the transition because it is already shown by the Loading screen
        if (uiDisplay == UIDisplay.GamePlay || uiDisplay == UIDisplay.GameOver ||
            uiDisplay == UIDisplay.LevelComplete)
        {
            // not showing transition
            return;
        }

        if (mainMenu.ShowCount < 1)
        {
            // if this is the first time the game has shown the main menu, then don't show the transition
            return;
        }

        ShowTransition();
    }

    private void HideAll()
    {
        foreach (var keyPair in uiGroups)
        {
            var ui = keyPair.Value;
            ui.Hide();
        }
    }

    private void HideAllInstantly()
    {
        foreach (var keyPair in uiGroups)
        {
            var ui = keyPair.Value;
            ui.HideInstantly();
        }
    }

    private void EnableUICamera()
    {
        uiCamera.enabled = true;
    }

    public void DisplayHealth()
    {
        gamePlay.DisplayHealth();
    }

    public void QuitToMainMenu()
    {
        CoreConnector.SoundManager.PlaySound(SoundManager.Sounds.ButtonPress);
        Display(UIDisplay.MainMenu);
    }

    public void ShowThanks()
    {
        Display(UIDisplay.Thanks);
    }

    private void ShowTransition()
    {
        sceneTransition.AnimateTransitionIn();
    }

    private void PutAwayTransition()
    {
        sceneTransition.AnimateTransitionOut();
    }

    public void OpenEmail()
    {
        Application.OpenURL("mailto:terrypaton1@gmail.com?subject=Burn");
    }

    public void OpenSethLink()
    {
        Application.OpenURL("https://twitter.com/SethLaster");
    }
}