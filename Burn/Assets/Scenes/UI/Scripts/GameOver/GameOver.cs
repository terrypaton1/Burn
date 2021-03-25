using UnityEngine;

public class GameOver : UIGroup
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
        if (!showing)
        {
            return;
        }

        playButton.Hide();
        quitButton.Hide();
    }

    public override void HideInstantly()
    {
        base.HideInstantly();

        playButton.HideInstantly();
        quitButton.HideInstantly();
    }
}