using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSidePieceBase : MonoBehaviour
{
    [SerializeField]
    protected Transform holder;

    [Range(1, 10)]
    [SerializeField]
    protected int ySteps = 2;

    [Range(1, 10)]
    [SerializeField]
    protected int xSteps = 3;

    [Range(1, 50)]
    [SerializeField]
    protected int zSteps = 3;

    public bool finishedSetup;
    protected IEnumerator coroutine;
    protected readonly List<NonCollisionObjectBase> pieces = new List<NonCollisionObjectBase>();

    protected float xRange = 2.0f;
    protected float xSpacing = 1.0f;

    protected float yRange = 2.0f;
    protected float ySpacing = 1.0f;

    protected float zRange = 2.0f;
    protected float zSpacing = 1.0f;

    public static readonly float SideCellLength = 50.0f;

    private void OnDrawGizmos()
    {
        if (Application.isPlaying || !GameSettings.DrawWallSidesDebug)
        {
            return;
        }

        DebugPieceLocations();
    }

    public virtual void Setup()
    {
        StopRunningCoroutine();
        coroutine = GenerateSequence();
        StartCoroutine(coroutine);
    }

    protected void StopRunningCoroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator GenerateSequence()
    {
        finishedSetup = false;
        SetupValues();

        for (var h = 0; h < zSteps; ++h)
        {
            for (var i = 0; i < xSteps; ++i)
            {
                for (var j = 0; j < ySteps; ++j)
                {
                    var position = CalculatePosition(i, j, h);
                    var instance = SpawnObject(position);
                    instance.Init();
                    instance.transform.SetParent(holder);
                    pieces.Add(instance);
                }
            }

            yield return null;
        }

        UpdatePieces();
        // intentional delay for Update pieces.
        yield return null;
        finishedSetup = true;
    }

    private void SetupValues()
    {
        xSpacing = 8.0f / xSteps;
        ySpacing = 20.0f / ySteps;
        zSpacing = SideCellLength / zSteps;
        xRange = xSpacing * 0.4f;
        yRange = ySpacing * 0.4f;
        zRange = zSpacing * 0.4f;
    }

    protected virtual NonCollisionObjectBase GetPrefabToSpawn()
    {
        return null;
    }

    protected NonCollisionObjectBase SpawnObject(Vector3 position)
    {
        var prefab = GetPrefabToSpawn();
        var instance = Instantiate(prefab, position, Quaternion.identity);
        return instance;
    }

    protected Vector3 CalculatePosition(int x, int y, int z)
    {
        var position = transform.position;
        position.x -= ((xSteps - 1) * xSpacing) * 0.5f;
        position.y -= ((ySteps + 1) * ySpacing) * 0.5f;

        position.x += x * xSpacing;
        position.y += y * ySpacing;
        position.z += z * zSpacing;

        position += Random.insideUnitSphere * 1.0f;

        position.x += Random.Range(-xRange, xRange);
        position.y += Random.Range(-yRange, yRange);
        position.z += Random.Range(-zRange, zRange);

        return position;
    }

    protected void UpdatePieces()
    {
        var count = 0;
        for (var h = 0; h < zSteps; ++h)
        {
            for (var i = 0; i < xSteps; ++i)
            {
                for (var j = 0; j < ySteps; ++j)
                {
                    var position = CalculatePosition(i, j, h);
                    var piece = pieces[count];
                    piece.transform.position = position;

                    piece.Init();
                    count++;
                }
            }
        }
    }

    private void DebugPieceLocations()
    {
        xSpacing = 8.0f / xSteps;
        ySpacing = 20.0f / ySteps;
        zSpacing = SideCellLength / zSteps;
        xRange = xSpacing * 0.4f;
        yRange = ySpacing * 0.4f;
        zRange = zSpacing * 0.4f;

        for (var h = 0; h < zSteps; ++h)
        {
            for (var i = 0; i < xSteps; ++i)
            {
                for (var j = 0; j < ySteps; ++j)
                {
                    var position = CalculateDebugPosition(i, j, h);

                    var temp1 = position;
                    temp1 += Vector3.right * xRange * 0.5f;

                    Debug.DrawRay(temp1, Vector3.left * xRange, Color.magenta);

                    var temp2 = position;
                    temp2 += Vector3.down * yRange * 0.5f;
                    Debug.DrawRay(temp2, Vector3.up * yRange, Color.yellow);

                    var temp3 = position;
                    temp3 += Vector3.back * zRange * 0.5f;
                    Debug.DrawRay(temp3, Vector3.forward * zRange, Color.green);
                }
            }
        }
    }

    private Vector3 CalculateDebugPosition(int x, int y, int z)
    {
        var position = transform.position;
        position.x -= ((xSteps - 1) * xSpacing) * 0.5f;
        position.y -= ((ySteps + 1) * ySpacing) * 0.5f;
        position.x += x * xSpacing;
        position.y += y * ySpacing;
        position.z += z * zSpacing;

        return position;
    }

    public virtual void Randomize()
    {
        UpdatePieces();
    }

    public void DisableRenderers()
    {
        foreach (var piece in pieces)
        {
            piece.DisableRenderers();
        }
    }

    public void EnableRenderers()
    {
        foreach (var piece in pieces)
        {
            piece.EnableRenderers();
        }
    }
}