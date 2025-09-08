using UnityEngine;

public class SpriteSwapper : MonoBehaviour
{
    public Animator animator;

    [Header("Animator")]
    public AnimatorOverrideController defaultAnimator;
    public AnimatorOverrideController alternateAnimator;
    public bool usingAlternate;
    private BoxCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SwapAnimator();

        }

    }

    public void SwapAnimator()
    {

        if(usingAlternate)
            animator.runtimeAnimatorController = alternateAnimator;
        else
            animator.runtimeAnimatorController = defaultAnimator;
    }
}
