using System.Collections;
using UnityEngine;

public class DistantObjectPool : WorldSidePieceBase
{
    [SerializeField]
    protected NonCollisionObjectBase[] rubblePrefabs;

    public override void Setup()
    {
        StopRunningCoroutine();
        coroutine = GenerateSequence();
        StartCoroutine(coroutine);
    }

    private IEnumerator GenerateSequence()
    {
        finishedSetup = false;

        InitializeValues();

        for (var h = 0; h < zSteps; ++h)
        {
            for (var i = 0; i < xSteps; ++i)
            {
                for (var j = 0; j < ySteps; ++j)
                {
                    var position = CalculatePosition(i, j, h);
                    var instance = SpawnObject(position);
                    instance.Init();
                    instance.transform.SetParent(transform);
                    pieces.Add(instance);
                }
            }

            yield return null;
        }

        UpdatePieces();

        finishedSetup = true;
    }

    private void InitializeValues()
    {
        xSpacing = 55.0f / xSteps;
        ySpacing = 60.0f / ySteps;
        zSpacing = SideCellLength / zSteps;
        xRange = xSpacing * 0.4f;
        yRange = ySpacing * 0.4f;
        zRange = zSpacing * 0.4f;
    }

    protected override NonCollisionObjectBase GetPrefabToSpawn()
    {
        var randomChoice = Random.Range(0, rubblePrefabs.Length);
        var prefab = rubblePrefabs[randomChoice];
        return prefab;
    }
}