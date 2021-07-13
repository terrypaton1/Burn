using System.Collections;
using UnityEngine;

public class RespawnableObject : MonoBehaviour
{
    [SerializeField]
    protected Collider colliderRef;

    [SerializeField]
    protected ObjectVariation[] objectVariations;

    public bool hasBeenCollidedWith;
    protected bool eventTriggered;
    protected IEnumerator coroutine;
    protected int currentShowingIndex;

    public virtual void Reset()
    {
        DisableVisuals();
        hasBeenCollidedWith = false;
        EnableColliders();
        RandomizeCurrentShowingIndex();
    }

    private void RandomizeCurrentShowingIndex()
    {
        currentShowingIndex = Random.Range(0, objectVariations.Length);
    }

    private void OnDrawGizmos()
    {
        DrawGizmos();
    }

    public virtual void UpdateLoop()
    {
    }

    public virtual void SetCollidedWith()
    {
        hasBeenCollidedWith = true;
        DisableColliders();
    }

    protected virtual void DrawGizmos()
    {
    }

    public void TestEvents(Vector3 playerPosition)
    {
        if (HasEventBeenTriggered())
        {
            return;
        }

        var basePosition = transform.position;

        if (playerPosition.z > basePosition.z)
        {
            TriggerEvent();
        }
    }

    protected virtual void TriggerEvent()
    {
        eventTriggered = true;
    }

    private bool HasEventBeenTriggered()
    {
        return eventTriggered;
    }

    public void EnableColliders()
    {
        if (colliderRef != null)
        {
            colliderRef.enabled = true;
        }
    }

    public void DisableColliders()
    {
        if (colliderRef != null)
        {
            colliderRef.enabled = false;
        }
    }

    public void EnableVisuals()
    {
        for (var i = 0; i < objectVariations.Length; ++i)
        {
            var obj = objectVariations[i];
            if (i == currentShowingIndex)
            {
                obj.Show();
            }
            else
            {
                obj.Hide();
            }
        }
    }

    public virtual void DisableVisuals()
    {
        foreach (var objectVariation in objectVariations)
        {
            objectVariation.Hide();
        }
    }

    public void SetToStatic()
    {
        gameObject.isStatic = true;
    }

    public void SetToNotStatic()
    {
        gameObject.isStatic = false;
    }

    protected void StopRunningCoroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
}