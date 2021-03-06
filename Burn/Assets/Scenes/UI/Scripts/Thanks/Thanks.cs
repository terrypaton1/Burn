﻿using UnityEngine;

public class Thanks : UIGroup
{
    [SerializeField]
    protected MenuButton quitButton;

    [SerializeField]
    protected MenuButton sethButton;

    [SerializeField]
    protected ParticleSystem starField;

    [SerializeField]
    protected Camera thanksCamera;

    public override void Show()
    {
        base.Show();

        CoreConnector.SoundManager.PlaySound(SoundManager.Sounds.Thanks);
        quitButton.Show();
        sethButton.Show();
        starField.Play();
    }

    public override void Hide()
    {
        if (!showing)
        {
            return;
        }

        quitButton.Hide();
        sethButton.Hide();
        CoreConnector.SoundManager.StopSound(SoundManager.Sounds.Thanks);
    }

    public override void HideInstantly()
    {
        base.HideInstantly();
        quitButton.HideInstantly();
        sethButton.HideInstantly();
    }

    protected override void SetRendererState(bool state)
    {
        base.SetRendererState(state);
        thanksCamera.enabled = state;
    }
}