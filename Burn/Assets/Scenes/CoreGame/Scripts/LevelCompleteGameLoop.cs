public class LevelCompleteGameLoop
{
    public static void LateUpdateMainLoop()
    {
        Player.MovePlayerToFinalPosition();

        CoreConnector.Levels.UpdateLoop();

        CoreConnector.CameraControl.LevelCompleteLoop();

        CoreConnector.Player.ManageMovement();
        var pos = CoreConnector.GameInput.GetShipPosition();
        CoreConnector.Player.MoveToTargetPosition(pos);

        CoreConnector.GameInput.PositionShipWithDisplayOffset();
        CoreConnector.Player.ManageAndDisplayPlayerRotation();
        CoreConnector.WorldSides.MoveForLevelComplete();

        if (!CoreConnector.Player.HasPlayerReachedFinalStop())
        {
            return;
        }

        PlayerHasReachedFinishLine();
    }

    private static void PlayerHasReachedFinishLine()
    {
        CoreConnector.Player.StopTrails();
        CoreConnector.UIControl.Display(UIDisplay.LevelComplete);
        CoreConnector.CoreGameControl.ChangeGameState(GameState.SpaceWarp);
    }
}