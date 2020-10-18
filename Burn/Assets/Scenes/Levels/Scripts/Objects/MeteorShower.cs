using System.Collections;
using UnityEngine;

public class MeteorShower : GameEventObject
{
    [SerializeField]
    protected Meteorite[] meteorites;

    [SerializeField]
    protected Vector3 meteorLaunchForce;

    [SerializeField]
    protected bool leftSide;

    private float asteroidSpacing = 5.0f;

    public override void Reset()
    {
        base.Reset();

        hasBeenCollidedWith = false;
        StopRunningCoroutine();

        ResetMeteoriteLocations();
    }

    protected override void TriggerEvent()
    {
        base.TriggerEvent();

        StopRunningCoroutine();
        coroutine = MeteorShowerSequence();
        StartCoroutine(coroutine);
    }

    private IEnumerator MeteorShowerSequence()
    {
        hasBeenCollidedWith = true;

        ResetMeteoriteLocations();

        yield return null;

        CoreConnector.SoundManager.PlaySound(SoundManager.Sounds.Alert);
        foreach (var meteorite in meteorites)
        {
            meteorite.Launch(meteorLaunchForce);
        }
    }

    private void ResetMeteoriteLocations()
    {
        var position = transform.position;
        if (leftSide)
        {
            position.x -= GameSettings.CellWidth - 2.0f;
        }
        else
        {
            position.x += GameSettings.CellWidth + 2.0f;
        }

        position.z += GameSettings.CellLength * 0.7f;
        var total = meteorites.Length;
        var count = 0;
        while (count < total)
        {
            var x = Mathf.RoundToInt(count / 4.0f);
            var z = count - (x * 4);

            var newPosition = CalculateMeteorPosition(position, x, z);
            /*
            var newPosition = position;

            if (leftSide)
            {
                x = -x;
            }

            newPosition += new Vector3(x * asteroidSpacing, 0.0f, z * asteroidSpacing);
            var randomRange = asteroidSpacing * 0.25f;
            newPosition.x += Random.Range(-randomRange, randomRange);
            newPosition.z += Random.Range(-randomRange, randomRange);
            */
            var meteorite = meteorites[count];
            meteorite.SetPosition(newPosition);
            meteorite.Reset();
            count++;
        }
    }

    private Vector3 CalculateMeteorPosition(Vector3 basePosition, int x, int z)
    {
        var newPosition = basePosition;

        if (leftSide)
        {
            x = -x;
        }

        newPosition += new Vector3(x * asteroidSpacing, 0.0f, z * asteroidSpacing);
        var randomRange = asteroidSpacing * 0.25f;
        newPosition.x += Random.Range(-randomRange, randomRange);
        newPosition.z += Random.Range(-randomRange, randomRange);
        return newPosition;
    }

#if UNITY_EDITOR
    protected override void DrawGizmos()
    {
        var basePosition = transform.position;

        var p1 = basePosition;

        var p2 = basePosition;
        if (leftSide)
        {
            p2 += Vector3.left * GameSettings.CellWidth;
        }
        else
        {
            p2 += Vector3.right * GameSettings.CellWidth;
        }

        Debug.DrawLine(p1, p2, Color.cyan);

        if (meteorites == null)
        {
            return;
        }

        foreach (var meteorite in meteorites)
        {
            var metLoc = meteorite.transform.position;

            UnityEditor.Handles.color = Color.grey;
            UnityEditor.Handles.DrawLine(p1, metLoc);
            var normalizedForce = meteorLaunchForce.normalized;
            UnityEditor.Handles.color = Color.magenta;
            var dir = Quaternion.LookRotation(normalizedForce);
            UnityEditor.Handles.ArrowHandleCap(0, metLoc, dir, asteroidSpacing, EventType.Repaint);
        }
    }
#endif
}