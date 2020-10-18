using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMarker : MonoBehaviour
{
    [SerializeField]
    private Renderer rendererRef;

    public void Hide()
    {
        rendererRef.enabled = false;
    }

    public void Show()
    {
        rendererRef.enabled = true;
    }
}