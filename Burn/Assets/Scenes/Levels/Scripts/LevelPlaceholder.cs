using UnityEngine;

[ExecuteInEditMode]
public class LevelPlaceholder : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        OnDrawGizmosFunction();
    }
#endif

    private void OnDrawGizmosFunction()
    {
        var p1 = transform.position;
        var p2 = p1 + Vector3.forward * GameSettings.CellLength;
        Debug.DrawLine(p1, p2, Color.green);

        var sideOffset = Vector3.left * GameSettings.CellWidth;
        Debug.DrawLine(p1 - sideOffset, p2 - sideOffset, Color.yellow);
    }
}