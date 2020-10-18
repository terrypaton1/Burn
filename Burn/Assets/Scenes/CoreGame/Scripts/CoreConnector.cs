using UnityEngine;
using UnityEngine.Assertions;

public class CoreConnector : MonoBehaviour
{
    [SerializeField]
    protected GameSettings gameSettings;

    public ref GameSettings GetGameSettings()
    {
        return ref gameSettings;
    }

    private static CoreConnector instance;

    private static Loading loading;

    private static UIControl uiControl;

    private static CoreGameControl coreGameControl;

    private static GameInput gameInput;

    private static CameraControl cameraControl;

    private static Player player;

    private static WorldSides worldSides;

    private static SoundManager soundManager;

    private static CellManager cellManager;

    private static ParticleManager particleManager;
    private static Levels levels;

    public static CoreConnector Instance
    {
        get { return instance; }
    }

    public static Levels Levels
    {
        get
        {
            Assert.IsNotNull(levels, "Error: levels reference is null");
            return levels;
        }
        set => levels = value;
    }

    public static Loading Loading
    {
        get
        {
            Assert.IsNotNull(loading, "Error: Loading reference is null");
            return loading;
        }
        set => loading = value;
    }

    public static UIControl UIControl
    {
        get
        {
            Assert.IsNotNull(uiControl, "Error: uiControl reference is null");
            return uiControl;
        }
        set => uiControl = value;
    }

    public static CoreGameControl CoreGameControl
    {
        get
        {
            Assert.IsNotNull(coreGameControl, "Error: coreGameControl reference is null");
            return coreGameControl;
        }
        set => coreGameControl = value;
    }

    public static GameInput GameInput
    {
        get
        {
            Assert.IsNotNull(gameInput, "Error: gameInput reference is null");
            return gameInput;
        }
        set => gameInput = value;
    }

    public static CameraControl CameraControl
    {
        get
        {
            Assert.IsNotNull(cameraControl, "Error: cameraControl reference is null");
            return cameraControl;
        }
        set => cameraControl = value;
    }

    public static Player Player
    {
        get
        {
            Assert.IsNotNull(player, "Error: player reference is null");

            return player;
        }
        set => player = value;
    }

    public static WorldSides WorldSides
    {
        get
        {
            Assert.IsNotNull(worldSides, "Error: worldSides reference is null");
            return worldSides;
        }
        set => worldSides = value;
    }

    public static SoundManager SoundManager
    {
        get
        {
            Assert.IsNotNull(soundManager, "Error: soundManager reference is null");
            return soundManager;
        }
        set => soundManager = value;
    }

    public static ParticleManager ParticleManager
    {
        get
        {
            Assert.IsNotNull(particleManager, "Error: particleManager reference is null");
            return particleManager;
        }
        set => particleManager = value;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            //Debug.Log("Core Connector already found");
            return;
        }

        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnDisable()
    {
        instance = null;
    }

    public static bool IsLevelsLoaded
    {
        get { return levelsIsLoaded; }
        set => levelsIsLoaded = value;
    }

    private static bool levelsIsLoaded;
}