using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    protected Animator transitionAnimation;

    public void AnimateTransitionIn()
    {
        transitionAnimation.Play("In", 0, 0.0f);
    }

    public void AnimateTransitionOut()
    {
        transitionAnimation.Play("Out", 0, 0.0f);
    }
}