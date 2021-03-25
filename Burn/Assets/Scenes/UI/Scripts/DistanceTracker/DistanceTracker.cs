using System.Collections.Generic;
using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    [SerializeField]
    protected Transform progressMarker;

    [SerializeField]
    protected Renderer[] renderers;

    [SerializeField]
    protected ShipMarker shipMarkerPrefab;

    [SerializeField]
    protected Transform markerStart;

    [SerializeField]
    protected Transform markerEnd;

    private List<ShipMarker> shipMarkerPool;
    private List<float> shipMarkers;

    private readonly Vector3 basePosition = new Vector3(0.07f, -3.9f, -0.2f);

    protected void OnEnable()
    {
        // todo change this to a setup function and call it implicity instead of using OnEnable
        shipMarkerPool = new List<ShipMarker>();
        shipMarkers = new List<float>();
    }

    public void DisplayProgress()
    {
        var percent = GetPercentOfLevelComplete();
        progressMarker.localPosition = basePosition + Vector3.up * (6.0f * percent);
    }

    private float GetPercentOfLevelComplete()
    {
        var targetPosition = CoreConnector.Levels.GetLevelEndLocation();
        var playerPosition = CoreConnector.Player.GetCurrentPosition();
        var percent = playerPosition.z / targetPosition.z;
        return percent;
    }

    public void DisableRenderers()
    {
        foreach (var ren in renderers)
        {
            ren.enabled = false;
        }
    }

    public void EnableRenderers()
    {
        foreach (var ren in renderers)
        {
            ren.enabled = true;
        }
    }

    public void ClearShipMarkers()
    {
        shipMarkers.Clear();
        if (shipMarkerPool == null)
        {
            return;
        }

        foreach (var shipMarkerObj in shipMarkerPool)
        {
            shipMarkerObj.Hide();
        }
    }

    public void DisplayShipMarkers()
    {
        var shipMarkerCount = shipMarkers.Count;
        while (shipMarkerPool.Count < shipMarkerCount)
        {
            var newMarker = Instantiate(shipMarkerPrefab, transform);
            shipMarkerPool.Add(newMarker);
        }

        foreach (var shipMarker in shipMarkerPool)
        {
            shipMarker.Hide();
        }

        for (int i = 0; i < shipMarkerCount; ++i)
        {
            var markerPercent = shipMarkers[i];
            var shipMarker = shipMarkerPool[i];

            var pos = CalculateShipMarkerPosition(markerPercent);
            shipMarker.transform.localPosition = pos;
            shipMarker.Show();
        }
    }

    private Vector3 CalculateShipMarkerPosition(float percent)
    {
        var pos = markerStart.localPosition;
        var MarkerBarVector = markerEnd.localPosition - pos;
        pos += MarkerBarVector * percent;
        return pos;
    }

    public void AddShipMarker(float markerAtPercent)
    {
        shipMarkers.Add(markerAtPercent);
    }
}