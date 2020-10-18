using UnityEngine;
using UnityEngine.Assertions;

public class TrailManager : MonoBehaviour
{
    [SerializeField]
    protected TrailEffect[] trails;

    private void OnEnable()
    {
        if (!Application.isPlaying)
        {
            trails = GetComponentsInChildren<TrailEffect>();
        }

        DisableAllTrails();
    }

    public void Show(PlayerDisplayType type)
    {
        DisableAllTrails();
        StartTrail(type);
    }

    private void StartTrail(PlayerDisplayType type)
    {
        var index = (int) type;
        var totalTrails = trails.Length;
        Assert.IsTrue((index > -1 || index > totalTrails), "trail: " + type + " out of range" + totalTrails);

        var currentTrail = trails[index];
        currentTrail.transform.localPosition = Vector3.zero;
        currentTrail.Show();
    }

    public void DisableAllTrails()
    {
        foreach (var trail in trails)
        {
            trail.Hide();
        }
    }
}