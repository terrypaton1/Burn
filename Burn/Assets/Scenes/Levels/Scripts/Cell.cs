using UnityEngine;

//[ExecuteInEditMode]
public class Cell : MonoBehaviour
{
    [SerializeField]
    protected RespawnableObject[] respawnableObjects;

    private bool displayEnabled;

#if UNITY_EDITOR

    private void OnEnable()
    {
        OnEnableFunction();
    }

    private void OnDrawGizmos()
    {
        OnDrawGizmosFunction();
    }
#endif
    private void Update()
    {
        if (!displayEnabled)
        {
            return;
        }

        foreach (var respawnableObject in respawnableObjects)
        {
            respawnableObject.UpdateLoop();
        }
    }

    public virtual void Reset()
    {
        foreach (var spawn in respawnableObjects)
        {
            if (spawn != null)
            {
                spawn.Reset();
            }
        }
    }

    public void TestEvents()
    {
        var playerPosition = CoreConnector.Player.GetCurrentPosition();

        foreach (var obj in respawnableObjects)
        {
            obj.TestEvents(playerPosition);
        }
    }

    public void Position(Vector3 position)
    {
        transform.position = position;
    }

    private void OnEnableFunction()
    {
        if (!Application.isPlaying)
        {
            // this will possibly cause the scene to be dirtied everytime you open it.
            respawnableObjects = gameObject.GetComponentsInChildren<RespawnableObject>();
        }
    }

    private void OnDrawGizmosFunction()
    {
        if (Application.isPlaying)
        {
            return;
        }

        var p1 = transform.position;
        var p2 = p1 + Vector3.forward * GameSettings.CellLength;
        Debug.DrawLine(p1, p2, Color.green);

        var sideOffset = Vector3.left * GameSettings.CellWidth;
        Debug.DrawLine(p1 - sideOffset, p2 - sideOffset, Color.yellow);
    }

    public void Show()
    {
        foreach (var obj in respawnableObjects)
        {
            obj.EnableVisuals();
            obj.EnableColliders();
        }

        displayEnabled = true;
    }

    public void Hide()
    {
        DisableRenderers();
        DisableAllColliders();
    }

    public void SetObjectsToStatic()
    {
        foreach (var obj in respawnableObjects)
        {
            obj.SetToStatic();
        }
    }

    public void SetObjectsToNoTStatic()
    {
        foreach (var obj in respawnableObjects)
        {
            obj.SetToNotStatic();
        }
    }

    public void EnableColliders()
    {
        foreach (var obj in respawnableObjects)
        {
            obj.EnableColliders();
        }
    }

    public void DisableAllColliders()
    {
        foreach (var obj in respawnableObjects)
        {
            obj.DisableColliders();
        }
    }

    public virtual void DisableRenderers()
    {
        foreach (var respawnableObject in respawnableObjects)
        {
            respawnableObject.DisableVisuals();
        }
        displayEnabled = false;
    }

    public virtual void EnableRenderers()
    {
        foreach (var respawnableObject in respawnableObjects)
        {
            respawnableObject.EnableVisuals();
        }
    }
}