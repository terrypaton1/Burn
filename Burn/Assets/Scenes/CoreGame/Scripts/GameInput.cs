using UnityEngine;

public class GameInput : MonoBehaviour
{
    [SerializeField]
    protected GameSettings gameSettings;

    private Touch theTouch;
    private float xMovementFactor;
    private float xOffset;
    private float xDifference;
    private int controlsDisabledCounter;
    private bool controlsDisabledFromCollision;

    public Vector3 differenceInMovement;
    private Vector3 currentPosition = new Vector3(6, 0, 0);
    private Vector3 touchPosition;
    private Vector3 diff;
    private Vector3 displayOffset;
    private Vector3 shipOffset;
    private Vector3 firstTouch;
    private Vector3 shipPosition;
    private Vector3 lastPosition;

    private void OnEnable()
    {
        CoreConnector.GameInput = this;
    }

    private void OnDisable()
    {
        CoreConnector.GameInput = null;
    }

    public void Reset()
    {
        xMovementFactor = 6.0f;
        currentPosition = new Vector3(xMovementFactor, 0, 0);
        displayOffset = Vector3.zero;
    }

    public void ProcessMainTouchControls()
    {
        if (CanPlayerMove())
        {
            ProcessTouches();

#if UNITY_EDITOR
            ProcessMouseControl();
#endif
        }

        PositionShipWithDisplayOffset();
        ApplyPlayerSidewaysMovement();
    }

    public Vector3 GetShipPosition()
    {
        return shipPosition;
    }

    private void ProcessTouches()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);

            touchPosition = theTouch.position;
            switch (theTouch.phase)
            {
                case TouchPhase.Began:
                    MouseDownPressed(theTouch.position);
                    break;
                case TouchPhase.Moved:
                    MouseHeldDown();
                    break;
            }
        }
    }

    private void ProcessMouseControl()
    {
        touchPosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            MouseDownPressed(touchPosition);
        }

        if (Input.GetMouseButton(0))
        {
            MouseHeldDown();
        }

        var mousePosition = Input.mousePosition;
        xOffset = mousePosition.x / Screen.width;
        xMovementFactor = -8.0f + xOffset * gameSettings.xMovementRange;
    }

    public void PositionShipWithDisplayOffset()
    {
        lastPosition = shipPosition;
        var camPosition = CoreConnector.CameraControl.GetCameraPosition();
        shipPosition = camPosition;

        displayOffset.x = Mathf.Clamp(displayOffset.x, -6.0f, 6.0f);
        displayOffset.z = Mathf.Clamp(displayOffset.z, -15.0f, 15.0f);

        shipPosition += displayOffset;
        shipPosition.y = 0.0f;

        differenceInMovement = lastPosition - shipPosition;
    }

    private void MouseDownPressed(Vector3 touchP)
    {
        firstTouch = touchP;
        CalculateShipOffset();
    }

    private void CalculateShipOffset()
    {
        var camPosition = CoreConnector.CameraControl.GetCameraPosition();
        var playerPosition = CoreConnector.Player.GetCurrentPosition();
        shipOffset = playerPosition - camPosition;
    }

    private bool CanPlayerMove()
    {
        if (controlsDisabledFromCollision)
        {
            if (controlsDisabledCounter > 0)
            {
                controlsDisabledCounter--;
                return false;
            }

            controlsDisabledFromCollision = false;
        }

        return true;
    }

    private void ApplyPlayerSidewaysMovement()
    {
        xMovementFactor = ClampXFactor(xMovementFactor);
        xDifference = currentPosition.x - xMovementFactor;

        var maxDelta = CalculateMaxDelta(xDifference);

        currentPosition.x = MoveTowards(currentPosition.x, xMovementFactor, maxDelta);
    }

    private float CalculateMaxDelta(float xDiff)
    {
        var absXDifference = Mathf.Abs(xDiff);
        var gameSetting = CoreConnector.Instance.GetGameSettings();
        var maxDelta = gameSetting.playerMaxMovementDelta;
        // if the difference is position is greater than 1 then make the max amount the position will change, 80% of that distance
        if (absXDifference > 1.0f)
        {
            maxDelta = absXDifference * 0.8f;
        }

        return maxDelta;
    }

    private static float MoveTowards(float current, float target, float maxDelta)
    {
        if (Mathf.Abs(target - current) <= maxDelta)
        {
            return target;
        }

        return current + Mathf.Sign(target - current) * maxDelta;
    }

    public void PushBackPlayer(float value)
    {
        DisableControlsAfterCollision();
        controlsDisabledCounter = 3;
        xMovementFactor += value;
        xMovementFactor = ClampXFactor(xMovementFactor);
    }

    private void DisableControlsAfterCollision()
    {
        controlsDisabledFromCollision = true;
        controlsDisabledCounter = 3;
    }

    private float ClampXFactor(float _xFactor)
    {
        return Mathf.Clamp(_xFactor, -2.0f, 13.0f);
    }

    private void MouseHeldDown()
    {
        var currentTouchPos = GetCurrentTouchPosition();
        var firstTouchPos = GetFirstTouchPosition();

        diff = currentTouchPos - firstTouchPos;
        displayOffset = diff + shipOffset;
    }

    private Vector3 GetCurrentTouchPosition()
    {
        var convertedTouchPositionToWorld = CoreConnector.CameraControl.ConvertTouchPositionToWorld(touchPosition);
        return convertedTouchPositionToWorld;
    }

    private Vector3 GetFirstTouchPosition()
    {
        var convertedTouchPositionToWorld = CoreConnector.CameraControl.ConvertTouchPositionToWorld(firstTouch);
        return convertedTouchPositionToWorld;
    }

    public void SetCurrentPositionX(float newX)
    {
        currentPosition.x = newX;
    }
}