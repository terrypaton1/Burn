public class SpaceWarpGameLoop 
{
    public static void LateUpdateMainLoop()
    {
        var lastPos = CoreConnector.Player.GetCurrentPosition();
        // this is where the tunnel effect will go, constantly animating
// rotate the camera down
        CoreConnector.CameraControl.AngleCameraForLevelComplete();
        // now move the player around 
        CoreConnector.CoreGameControl.ManageControls();

        
        var pos = CoreConnector.GameInput.GetShipPosition();
        CoreConnector.Player.MoveToTargetPosition(pos);
        CoreConnector.GameInput.PositionShipWithDisplayOffset();
        
        // add rotation to the players model
        CoreConnector.GameInput.differenceInMovement = lastPos - pos;

        CoreConnector.Player.ManageAndDisplayPlayerRotation();
    }
}