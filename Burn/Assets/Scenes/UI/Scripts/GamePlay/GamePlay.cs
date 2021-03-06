﻿using UnityEngine;
using UnityEngine.UI;

public class GamePlay : UIGroup
{
    [SerializeField]
    protected MenuButton quitToMenuButton;

    [SerializeField]
    protected HealthDisplay healthDisplay;

    [SerializeField]
    protected Text infoText;

    public void DisplayHealth()
    {
        healthDisplay.DisplayHealth();
    }

    public override void Hide()
    {
        if (!showing)
        {
            return;
        }

        showing = false;
        // Game play is forced stopped when hiding the gameplay scene
        CoreConnector.CoreGameControl.StopGame();
        quitToMenuButton.Hide();
        healthDisplay.Hide();
    }

    public override void Show()
    {
        base.Show();

        quitToMenuButton.Show();

        ManageInfoText();
        healthDisplay.Show();
    }

    private void ManageInfoText()
    {
        infoText.enabled = false;
        var gameSettings = CoreConnector.Instance.GetGameSettings();
        if (gameSettings.GodMode)
        {
            DisplayInfoText("God Mode");
        }

        if (gameSettings.easyMode)
        {
            DisplayInfoText("Easy");
        }
    }

    private void DisplayInfoText(string infoTextString)
    {
        infoText.enabled = true;
        infoText.text = infoTextString;
    }

    public override void HideInstantly()
    {
        base.HideInstantly();

        quitToMenuButton.HideInstantly();
        healthDisplay.HideInstantly();
    }
}