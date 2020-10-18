using UnityEngine;

public class LevelComplete : UIGroup
{
    [SerializeField]
    protected MenuButton playButton;

    [SerializeField]
    protected MenuButton quitButton;

    public override void Show()
    {
        base.Show();

        playButton.Show();
        quitButton.Show();
    }

    public override void Hide()
    {
        base.Hide();

        playButton.Hide();
        quitButton.Hide();
    }

    public override void HideInstantly()
    {
        base.HideInstantly();

        playButton.HideInstantly();
        quitButton.HideInstantly();
    }

    public override UIDisplay GetUIDisplay()
    {
        return UIDisplay.LevelComplete;
    }
}