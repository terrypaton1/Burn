using UnityEngine;

[ExecuteInEditMode]
public class GameEventObject : RespawnableObject
{
    // todo note for this object, 'collidedWith' may be invalid.
    // But then, when a solar flare hits the player we want to know that it has.

    public override void Reset()
    {
        base.Reset();
        eventTriggered = false;
    }

    protected override void DrawGizmos()
    {
        var basePosition = transform.position;

        var p1 = basePosition;
        var p2 = basePosition + Vector3.right * GameSettings.CellWidth;
        Debug.DrawLine(p1, p2, Color.yellow);
    }
}