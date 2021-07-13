using System.Collections;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField]
    protected Transform container;

    [SerializeField]
    protected Vector3 offScreenLocalPosition = new Vector3(-1500.0f, 0.0f, 0.0f);

    private IEnumerator coroutine;
    private bool displayed;
    private const float TimeToMoveOff = 0.3f;
    private const float TimeToMoveOn = 0.3f;

    protected void OnEnable()
    {
        OnEnableFunction();
    }

    protected virtual void OnEnableFunction()
    {
        HideInstantly();
    }

    public virtual void Show(float delay = 0.0f)
    {
        if (displayed)
        {
            return;
        }

        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        displayed = true;
        StopRunningCoroutine();
        coroutine = ShowSequence(delay);
        StartCoroutine(coroutine);
    }

    public virtual void Hide(float delay = 0.0f)
    {
        if (!displayed)
        {
            return;
        }

        displayed = false;
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        StopRunningCoroutine();
        coroutine = HideSequence(delay);
        StartCoroutine(coroutine);
    }

    private IEnumerator HideSequence(float delay)
    {
        yield return new WaitForSeconds(delay);
        var timer = 0.0f;

        while (timer < TimeToMoveOff)
        {
            var percent = timer / TimeToMoveOff;
            AnimateButtonOff(percent);
            yield return null;
            timer += Time.deltaTime;
        }

        container.transform.localPosition = offScreenLocalPosition;
    }

    private IEnumerator ShowSequence(float delay)
    {
        yield return new WaitForSeconds(delay);
        var time = 0.0f;
        while (time < TimeToMoveOn)
        {
            var percent = time / TimeToMoveOn;
            AnimateButtonOn(percent);
            yield return null;
            time += Time.deltaTime;
        }

        container.transform.localPosition = Vector3.zero;
    }

    private void AnimateButtonOff(float percent)
    {
        var gameSettings = CoreConnector.Instance.GetGameSettings();
        var sampledPercent = gameSettings.uiTweenOutCurve.Evaluate(percent);
        var position = Vector3.LerpUnclamped(Vector3.zero, offScreenLocalPosition, sampledPercent);
        container.transform.localPosition = position;
    }

    private void AnimateButtonOn(float percent)
    {
        var gameSettings = CoreConnector.Instance.GetGameSettings();
        var sampledPercent = gameSettings.uiTweenInCurve.Evaluate(percent);
        var position = Vector3.LerpUnclamped(offScreenLocalPosition, Vector3.zero, sampledPercent);
        container.transform.localPosition = position;
    }

    public virtual void HideInstantly()
    {
        StopRunningCoroutine();
        displayed = false;
        container.localPosition = offScreenLocalPosition;
    }

    private void StopRunningCoroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
}