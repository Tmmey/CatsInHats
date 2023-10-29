using System.Collections.Generic;
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
    [SerializeField]
    private List<AudioClip> meows = new List<AudioClip>();
    private AudioSource AudioSource;

    /// <summary>
    /// Found score controller in scene.
    /// </summary>
    private ScoringController cachedScoreController;

    private void Start()
    {
        cachedScoreController = ScoringController.Instance;
        AudioSource = GetComponent<AudioSource>();
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

            PlayRandomAudioSource();
        }
    }

    private void PlayRandomAudioSource()
    {
        var rnd = Random.Range(0, meows.Count);
        var meow = meows[rnd];
        AudioSource.PlayOneShot(meow);
    }
}
