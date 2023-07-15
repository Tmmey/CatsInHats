using UnityEngine;

/// <summary>
/// A bin that can catch the trash.
/// </summary>
public class Bin : MonoBehaviour
{
    [SerializeField]
    private CatType catType;
    [SerializeField]
    private int correctScore = 1;
    [SerializeField]
    private int incorrectScore = -2;

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
            cachedScoreController.AddScore(cat.Type == catType ? correctScore : incorrectScore);
            Destroy(cat.gameObject);
        }
    }
}
