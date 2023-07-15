using UnityEngine;

/// <summary>
/// Simple object to catch cat that falls off the screen.
/// </summary>
public class Gutter : MonoBehaviour
{
    [SerializeField]
    private int gutterScore = -1;

    /// <summary>
    /// Found score controller in scene.
    /// </summary>
    private ScoringController cachedScoreController;

    private void Start()
    {
        cachedScoreController = ScoringController.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (cachedScoreController == null)
        {
            return;
        }

        // Try find cat on there
        var cat = other.GetComponent<Cat>();

        if (cat != null)
        {
            cachedScoreController.AddScore(gutterScore);
            Destroy(cat.gameObject);
        }
    }
}
