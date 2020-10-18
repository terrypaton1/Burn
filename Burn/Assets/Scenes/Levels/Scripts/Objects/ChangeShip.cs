using UnityEngine;

[ExecuteInEditMode]
public class ChangeShip : GameEventObject
{
    [SerializeField]
    protected PlayerDisplayType shipType;

    public override void SetCollidedWith()
    {
        base.SetCollidedWith();
        colliderRef.enabled = false;
    }

#if UNITY_EDITOR
    protected override void DrawGizmos()
    {
        var basePosition = transform.position;
        var p1 = basePosition;
        var p2 = basePosition + Vector3.right * GameSettings.CellWidth;
        Debug.DrawLine(p1, p2, Color.green);
    }
#endif

    protected override void TriggerEvent()
    {
        base.TriggerEvent();

        // start the solar flare sequence
        CoreConnector.Player.ChangeShip(shipType);
    }

}
