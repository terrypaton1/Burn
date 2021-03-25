using UnityEngine;

[ExecuteInEditMode]
public class FinalStop : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        var p1 = transform.position;
        var p2 = p1 + Vector3.right * GameSettings.CellLength;
        Debug.DrawLine(p1, p2, Color.red);
    }
}