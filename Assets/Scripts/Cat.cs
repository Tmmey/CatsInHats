using UnityEngine;


/// <summary>
/// Trash item that falls from the top of the screen and can be swiped over to launch into a bin.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Cat : MonoBehaviour
{
    [SerializeField]
    private CatType catType;

    [SerializeField]
    private TrailRenderer trail;

    private Rigidbody2D cachedRigidbody;
    private int launchedLayerId;

    /// <summary>
    /// Gets whether this cat has been flung before.
    /// </summary>
    public bool HasFlung { get; set; }

    public CatType Type => catType;

    /// <summary>
    /// Cache our components.
    /// </summary>
    protected virtual void Awake()
    {
        cachedRigidbody = GetComponent<Rigidbody2D>();
        launchedLayerId = LayerMask.NameToLayer("FlungTrash");
        if (trail != null)
        {
            trail.emitting = false;
        }
    }

    /// <summary>
    /// Launch this cat at the given bin.
    /// </summary>
    /// <param name="target">The target to launch at, in world space.</param>
    /// <param name="velocity">The speed to launch at, in world units per second.</param>
    public void LaunchAt(Vector2 target, float velocity)
    {
        // Set our layer so we can collide with the bins
        gameObject.layer = launchedLayerId;

        // Make our rigidbody ignore gravity
        cachedRigidbody.gravityScale = 0;

        Vector2 newVelocity = (target - (Vector2)transform.position).normalized * velocity;
        cachedRigidbody.velocity = newVelocity;

        HasFlung = true;
        if (trail != null)
        {
            trail.emitting = true;
        }
    }
}

