using System.Collections;
using UnityEngine;

public class WorldSides : MonoBehaviour
{
    [SerializeField]
    protected Transform sideCollisionHolder;

    [SerializeField]
    protected WorldSideSegment sidePrefab;

    [SerializeField]
    protected WorldSideSegment[] segments;

    [SerializeField]
    protected Transform holder;

    private bool finishedSetup;
    private IEnumerator coroutine;

    private const int totalSegments = 4;
    private int lastLoopStep;

    protected void OnEnable()
    {
        CoreConnector.WorldSides = this;
    }

    protected void OnDisable()
    {
        CoreConnector.WorldSides = null;
    }

    public bool SetupFinished()
    {
        return finishedSetup;
    }

    public void Setup()
    {
        StopRunningCoroutine();
        coroutine = GenerateSequence();
        StartCoroutine(coroutine);
    }

    private void StopRunningCoroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator GenerateSequence()
    {
        // This could spawns 'worldSideSegments', which are classes that generate 
        // more sub sections that eventually generate meteors
        finishedSetup = false;
        segments = new WorldSideSegment[totalSegments];
        var spawnedAmount = 0;

        while (spawnedAmount < totalSegments)
        {
            var obj = SpawnObject();
            obj.transform.parent = holder;
            segments[spawnedAmount] = obj;
            obj.Setup();

            // wait for the segment to finish setting itself up
            while (!obj.finishedSetup)
            {
                yield return null;
            }

            spawnedAmount++;
        }

        finishedSetup = true;
        DisableRenderers();
    }

    private WorldSideSegment SpawnObject()
    {
        var obj = Instantiate(sidePrefab);
        return obj;
    }

    public void SetPosition(Vector3 newPosition, bool forceUpdate = false)
    {
        newPosition.x = 0.0f;
        sideCollisionHolder.position = newPosition;

        var steps = Mathf.FloorToInt(newPosition.z / WorldSidePieceBase.SideCellLength);
        var loopStep = (steps - 1) % (totalSegments - 1);

        if (loopStep == lastLoopStep && !forceUpdate)
        {
            return;
        }

        lastLoopStep = loopStep;
        var num = steps - 1;
        for (var i = 0; i < totalSegments; ++i)
        {
            EvaluatePiece(num + i);
        }
    }

    private void EvaluatePiece(int steps)
    {
        var zFactor = steps * WorldSidePieceBase.SideCellLength;
        var value1 = steps % totalSegments;
        if (value1 < 0)
        {
            value1 += totalSegments;
        }

        var obj = segments[value1];
        obj.transform.localPosition = new Vector3(0, 0, zFactor);
    }

    private void RandomizeDisplay()
    {
        for (var i = 0; i < totalSegments; ++i)
        {
            var obj = segments[i];
            obj.Randomize();
        }
    }

    public void MoveForLevelComplete()
    {
        var pos = holder.localPosition;
        pos.z -= 1.0f;
        holder.localPosition = pos;
    }

    public void Reset()
    {
        EnableRenderers();
        SetPosition(Vector3.zero, true);
        holder.localPosition = Vector3.zero;
        RandomizeDisplay();
    }

    public void DisableRenderers()
    {
        if (!finishedSetup)
        {
            return;
        }

        foreach (var segment in segments)
        {
            segment.DisableRenderers();
        }
    }

    public void EnableRenderers()
    {
        if (!finishedSetup)
        {
            return;
        }

        foreach (var segment in segments)
        {
            segment.EnableRenderers();
        }
    }
}