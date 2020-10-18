using System.Collections;
using UnityEngine;

public class WorldSideSegment : MonoBehaviour
{
    [SerializeField]
    protected WorldSidePieceBase[] sidePieces;

    [SerializeField]
    protected WorldSidePieceBase[] distantPieces;

    public bool finishedSetup;
    private IEnumerator coroutine;

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
        finishedSetup = false;

        foreach (var sidePiece in sidePieces)
        {
            sidePiece.Setup();
            while (!sidePiece.finishedSetup)
            {
                yield return null;
            }
        }

        foreach (var distantPiece in distantPieces)
        {
            distantPiece.Setup();
            while (!distantPiece.finishedSetup)
            {
                yield return null;
            }
        }

        finishedSetup = true;
    }

    public void Randomize()
    {
        RandomizePieces(sidePieces);
        RandomizePieces(distantPieces);
    }

    private void RandomizePieces(WorldSidePieceBase[] pieces)
    {
        foreach (var piece in pieces)
        {
            piece.Randomize();
        }
    }

    public void DisableRenderers()
    {
        foreach (var sidePiece in sidePieces)
        {
            sidePiece.DisableRenderers();
        }

        foreach (var piece in distantPieces)
        {
            piece.DisableRenderers();
        }
    }

    public void EnableRenderers()
    {
        foreach (var sidePiece in sidePieces)
        {
            sidePiece.EnableRenderers();
        }

        foreach (var piece in distantPieces)
        {
            piece.EnableRenderers();
        }
    }
}