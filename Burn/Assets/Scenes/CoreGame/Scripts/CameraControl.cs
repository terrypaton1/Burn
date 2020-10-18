using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    protected Camera cameraRef;

    [SerializeField]
    protected Camera orthoCameraRef;

    [Space(10), SerializeField]
    protected Transform cameraHolder;

    [SerializeField]
    protected Transform distantStarHolder;

    [SerializeField]
    protected Transform angleTransform;

    [SerializeField]
    protected Transform cameraHeightTransform;

    [Space(10), SerializeField]
    public float currentDistance;

    [Range(-10.0f, 25.0f)]
    [SerializeField]
    protected float zOffset = 10.0f;

    [SerializeField]
    protected DistanceTracker distanceTracker;

    [SerializeField]
    protected Sun sun;

    [SerializeField]
    protected ParticleSystem starField;

    [SerializeField]
    public float targetAngle = 25.0f;

    [SerializeField]
    public float xAngle = -10.0f;

    [SerializeField]
    private Light[] gameLights;

    private Vector3 cameraAngle = Vector3.zero;

    private const float CenterX = 6.0f;
    private Vector3 currentPosition;
    private Vector3 velocity;
    private float timePassed;
    private Vector3 cameraPivotAngle;
    private float currentCameraPivotX;

    private readonly Vector3 cameraHeightLevelCompletePosition = new Vector3(0, 5.0f, -14.0f);

    public Vector3 GetCameraPosition()
    {
        return currentPosition;
    }

    protected void OnEnable()
    {
        CoreConnector.CameraControl = this;
        DisableVisuals();
    }

    public void ResetPosition()
    {
        xAngle = -10.0f;
        currentDistance = 12.0f;
        currentPosition = new Vector3(6, 0, currentDistance);
        DisplayCameraPosition();
        starField.Play();
        MoveCameraHeightTo(new Vector3(0, 30, 0));
        distantStarHolder.localPosition = Vector3.zero;
        angleTransform.localEulerAngles = Vector3.zero;
    }

    public void ManageCameraPosition()
    {
        var target = CoreConnector.Player.GetCameraFocusPosition();
        var xDiff = target.x - CenterX;
        target.x = CalculatePlayerX(xDiff);

        target.z += zOffset;

        target.y = 0;
        currentPosition = target;
    }

    public void ApplyAngleToCameraBasedOnSpeed()
    {
        var speedPercent = CoreConnector.Player.CurrentThrust / 50.0f;
        var percentAngle = speedPercent * targetAngle;
        var diff = percentAngle - xAngle;

        xAngle += diff * 0.01f;

        cameraAngle.x = xAngle;
        angleTransform.localEulerAngles = cameraAngle;
        angleTransform.localPosition = new Vector3(0, 0, speedPercent * 3);
    }

    public void AngleCameraForLevelComplete()
    {
        var target = -60;
        var diff = target - xAngle;
        xAngle += diff * 0.02f;

        cameraAngle.x = Mathf.Lerp(cameraAngle.x, xAngle, 0.1f);
        angleTransform.localEulerAngles = cameraAngle;
        angleTransform.localPosition = new Vector3(0, 0, 0);

        LerpCameraHeightTransformTo(cameraHeightLevelCompletePosition);
        zOffset = 30.0f;
    }

    private void MoveCameraHeightTo(Vector3 targetPosition)
    {
        cameraHeightTransform.localPosition = targetPosition;
    }

    private void LerpCameraHeightTransformTo(Vector3 targetPosition)
    {
        var cameraPosition = cameraHeightTransform.localPosition;
        var newP = Vector3.Lerp(cameraPosition, targetPosition, 0.02f);
        cameraHeightTransform.localPosition = newP;
    }

    private float CalculatePlayerX(float xDiff)
    {
        return CenterX + xDiff * 0.3f;
    }

    public void DisplayCameraPosition()
    {
        cameraHolder.position = currentPosition;
        distanceTracker.DisplayProgress();
    }

    private void DisableVisuals()
    {
        cameraRef.enabled = false;
        starField.Stop();

        foreach (var gameLight in gameLights)
        {
            gameLight.enabled = false;
        }
    }

    public void EnableVisuals()
    {
        cameraRef.enabled = true;
        starField.Play();
        foreach (var gameLight in gameLights)
        {
            gameLight.enabled = true;
        }
    }

    public void PunchBackSun()
    {
        sun.PunchSunBack();
    }

    public Vector3 ConvertTouchPositionToWorld(Vector3 touchPosition)
    {
        var p = new Vector3(touchPosition.x, touchPosition.y, orthoCameraRef.nearClipPlane);
        return orthoCameraRef.ScreenToWorldPoint(p);
    }

    public void MoveDistantStarsForGameComplete()
    {
        var pos = distantStarHolder.localPosition;
        pos.z -= 0.5f;
        distantStarHolder.localPosition = pos;
    }

    public void DisableRenderers()
    {
        distantStarHolder.gameObject.SetActive(false);
        distanceTracker.DisableRenderers();
        sun.DisableRenderers();
        starField.Stop();
    }

    public void EnableRenderers()
    {
        distantStarHolder.gameObject.SetActive(true);
        distanceTracker.EnableRenderers();
        sun.EnableRenderers();
        starField.Play();
    }

    public void ClearShipMarkers()
    {
        distanceTracker.ClearShipMarkers();
    }

    public void DisplayShipMarkers()
    {
        distanceTracker.DisplayShipMarkers();
    }

    public void AddShipMarker(float percent)
    {
        distanceTracker.AddShipMarker(percent);
    }

    public void LateUpdateLoop()
    {
        ManageCameraPosition();
        ApplyAngleToCameraBasedOnSpeed();
        DisplayCameraPosition();
    }

    public void LevelCompleteLoop()
    {
        
        ManageCameraPosition();
        DisplayCameraPosition();
        MoveDistantStarsForGameComplete();
    }

}