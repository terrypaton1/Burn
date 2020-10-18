using System.Collections;
using UnityEngine;

public class CoreGameControl : MonoBehaviour
{

    private IEnumerator coroutine;
    public GameState currentGameState;

    public bool IsGameRunning
    {
        get;
        private set;
    }

    protected void OnEnable()
    {
        CoreConnector.CoreGameControl = this;
    }

    protected void Update()
    {
        switch (currentGameState)
        {
            case GameState.StartUp:
                break;
            case GameState.MainLoop:
                MainLateUpdateGameLoop.UpdateMainLoop();
                break;
            case GameState.GameOver:
                break;
            case GameState.GameComplete:
                break;
            case GameState.Stopped:
                break;
        }
    }

    public void SetWorldSidePositionToMatchPlayer()
    {
        var playerPosition = CoreConnector.Player.GetCurrentPosition();
        CoreConnector.WorldSides.SetPosition(playerPosition);
    }

    public void ManageControls()
    {
        // don't test the controls until 1 seconds into the game
// use the game time passed
        if (HasInitialGameTimePassed())
        {
            CoreConnector.GameInput.ProcessMainTouchControls();
        }
        else
        {
            CoreConnector.GameInput.PositionShipWithDisplayOffset();
        }
    }

    private static bool HasInitialGameTimePassed()
    {
        if (CoreConnector.Player.TimePassed > 1.0f)
        {
            return true;
        }

        return false;
    }

    protected void LateUpdate()
    {
        switch (currentGameState)
        {
            case GameState.StartUp:
                break;
            case GameState.MainLoop:
                MainLateUpdateGameLoop.LateUpdateMainLoop();
                break;
            case GameState.GameOver:
                break;
            case GameState.GameComplete:
                LevelCompleteGameLoop.LateUpdateMainLoop();
                // move the player towards the finish line
                // then do seqeunce
                break;
            case GameState.SpaceWarp:
                SpaceWarpGameLoop.LateUpdateMainLoop();
                // move the player towards the finish line
                // then do seqeunce
                break;
        }
    }

    public void PlayerKilled()
    {
        //Debug.Log("Player has been killed");
        IsGameRunning = false;
        ChangeGameState(GameState.GameOver);
        CoreConnector.UIControl.Display(UIDisplay.GameOver);
    }

    public void ResetGame()
    {
        ChangeGameState(GameState.StartUp);
        CoreConnector.UIControl.Display(UIDisplay.GamePlay);
        CoreConnector.CameraControl.ResetPosition();
        CoreConnector.Player.Reset();
        CoreConnector.GameInput.Reset();
        CoreConnector.WorldSides.Reset();
        SetWorldSidePositionToMatchPlayer();
        CoreConnector.Levels.ResetForNewGame();
        CoreConnector.CameraControl.EnableVisuals();
        IsGameRunning = true;
        ChangeGameState(GameState.MainLoop);
        EnableGameRenderers();
        CoreConnector.SoundManager.PlaySound(SoundManager.Sounds.StartLevel);
    }

    public void LevelCompleted()
    {
        IsGameRunning = false;

        ChangeGameState(GameState.GameComplete);

        var endCell = CoreConnector.Levels.GetEndCell();
        endCell.StartLevelCompleteEffects();
    }

    public void StopGame()
    {
        IsGameRunning = false;
        currentGameState = GameState.Stopped;
    }

    public void DisableGameRenderers()
    {
        CoreConnector.Levels.HideAllLevels();
        CoreConnector.CameraControl.DisableRenderers();
        CoreConnector.WorldSides.DisableRenderers();
    }

    private void EnableGameRenderers()
    {
        CoreConnector.Levels.EnableRenderers();
        CoreConnector.CameraControl.EnableRenderers();
        CoreConnector.WorldSides.EnableRenderers();
    }

    public void ChangeGameState(GameState state)
    {
        currentGameState = state;
    }
}