﻿public static class MainLateUpdateGameLoop
{
    public static void UpdateMainLoop()
    {
        CoreGameControl.ManageControls();
        CoreConnector.CoreGameControl.SetWorldSidePositionToMatchPlayer();

        CoreConnector.Player.ManageGamePlay();
        CoreConnector.Levels.UpdateLoop();
    }

    public static void LateUpdateMainLoop()
    {
        var pos = CoreConnector.GameInput.GetShipPosition();
        CoreConnector.Player.MoveToTargetPosition(pos);

        CoreConnector.CameraControl.LateUpdateLoop();
    }
}