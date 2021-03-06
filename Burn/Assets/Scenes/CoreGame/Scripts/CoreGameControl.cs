﻿using UnityEngine;

public class CoreGameControl : MonoBehaviour
{
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

    public static void ManageControls()
    {
        // don't test the controls until 1 seconds into the game
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
        return CoreConnector.Player.TimePassed > 1.0f;
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
                break;
            case GameState.SpaceWarp:
                SpaceWarpGameLoop.LateUpdateMainLoop();
                // move the player towards the finish line
                break;
        }
    }

    public void PlayerKilled()
    {
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
        CoreConnector.CameraControl.EnableGameCamera();
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

        CoreConnector.SoundManager.StopSound(SoundManager.Sounds.StartLevel);
        CoreConnector.SoundManager.StopSound(SoundManager.Sounds.LevelComplete);
        CoreConnector.SoundManager.StopSound(SoundManager.Sounds.CollectLife);
    }

    public void ChangeGameState(GameState state)
    {
        currentGameState = state;
    }

    public static void DisableGameRenderers()
    {
        CoreConnector.Levels.HideAllLevels();
        CoreConnector.CameraControl.DisableRenderers();
        CoreConnector.WorldSides.DisableRenderers();
    }

    private static void EnableGameRenderers()
    {
        CoreConnector.Levels.EnableRenderers();
        CoreConnector.CameraControl.EnableRenderers();
        CoreConnector.WorldSides.EnableRenderers();
    }
}