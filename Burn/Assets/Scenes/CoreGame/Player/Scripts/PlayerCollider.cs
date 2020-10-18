﻿using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField]
    protected Player player;
    [SerializeField]
    protected Collider collider;
    
    private int collisionCounter;
    private const int collisionInterval = 1;
    private bool collisionIsActive;

    public void DisableCollider()
    {
        collider.enabled = false;
    }
    public void EnableCollider()
    {
        collider.enabled = true;
    }
    public void Reset()
    {
        collisionIsActive = false;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collisionIsActive)
        {
            return;
        }

        foreach (var contact in collision.contacts)
        {
            // allow for only 1 collision

            if (contact.otherCollider == null)
            {
                continue;
            }
            var obj = GetRespawnableObjectFromParent(contact);
            SetObjectCollidedWith(obj);
            player.Collision(contact, collisionIsActive);
            collisionIsActive = true;
            break;
        }

        collisionCounter = collisionInterval;
    }

    private static void SetObjectCollidedWith(RespawnableObject obj)
    {
        if (obj == null)
        {
            return;
        }
        if (!obj.hasBeenCollidedWith)
        {
            obj.SetCollidedWith();
        }
    }

    private static RespawnableObject GetRespawnableObjectFromParent(ContactPoint contact)
    {
        return contact.otherCollider.transform.parent.GetComponent<RespawnableObject>();
    }

    protected void OnTriggerEnter(Collider colliderRef)
    {
        var obj = colliderRef.transform.parent.GetComponent<RespawnableObject>();
        if (obj == null)
        {
            return;
        }

        if (obj.hasBeenCollidedWith)
        {
            return;
        }

        obj.SetCollidedWith();
        player.Trigger(colliderRef);
        collisionIsActive = true;
    }

    public void UpdateLoop()
    {
        if (collisionCounter >= 0)
        {
            collisionCounter--;
        }
        else
        {
            collisionIsActive = false;
        }
    }
}