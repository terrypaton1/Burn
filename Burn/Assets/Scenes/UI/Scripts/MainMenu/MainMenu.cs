using UnityEngine;

public class MainMenu : UIGroup
{
    [SerializeField]
    protected MenuButton playButton;

    [SerializeField]
    protected MenuButton playButtonWait;

    [SerializeField]
    protected MenuButton quitButton;

    [SerializeField]
    protected MenuButton thanksButton;

    [SerializeField]
    protected MenuButton emailButton;

    [SerializeField]
    protected Transform[] resetRotations;

    [SerializeField]
    protected ParticleSystem starField;

    [SerializeField]
    protected Transform cameraPivot;

    private Vector3 cameraPivotAngle = Vector3.zero;
    private float timePassed;
    private const float FullRotation = 360.0f;

    private void Update()
    {
        if (!showing)
        {
            return;
        }

        if (!CoreConnector.WorldSides.SetupFinished())
        {
            playButton.HideInstantly();
            playButtonWait.Show();
        }
        else
        {
            playButton.Show(0.2f);
            playButtonWait.Hide();
        }

        timePassed += Time.deltaTime * 10.0f;
        timePassed %= FullRotation;
        // use the amount of time passed as a radian, that osoclates like a pendulum
        var radians = timePassed * Mathf.Deg2Rad;
        var gameSettings = CoreConnector.Instance.GetGameSettings();
        var rotation = gameSettings.deviceYTiltValue * Mathf.Sin(radians);

        cameraPivotAngle.y = rotation;
        cameraPivot.localEulerAngles = cameraPivotAngle;
    }

    public override void Show()
    {
        base.Show();

        playButton.HideInstantly();
        playButtonWait.HideInstantly();

        quitButton.Show();
        thanksButton.Show(0.1f);
        emailButton.Show(0.2f);

        foreach (var obj in resetRotations)
        {
            obj.localEulerAngles = Vector3.zero;
        }

        starField.Play();
        CoreConnector.SoundManager.PlaySound(SoundManager.Sounds.GameMusic);
    }

    public override void Hide()
    {
        if (!showing)
        {
            return;
        }

        showing = false;

        playButton.Hide();
        playButtonWait.Hide();
        quitButton.Hide();
        thanksButton.Hide();
        emailButton.Hide();
        // Particles
        starField.Stop();

        CoreConnector.SoundManager.StopSound(SoundManager.Sounds.GameMusic);
    }

    public override void HideInstantly()
    {
        base.HideInstantly();

        playButton.HideInstantly();
        playButtonWait.HideInstantly();
        quitButton.HideInstantly();
        thanksButton.HideInstantly();
        emailButton.HideInstantly();
        // Particles
        starField.Stop();
        starField.Clear();
    }

    public override UIDisplay GetUIDisplay()
    {
        return UIDisplay.MainMenu;
    }
}