using UnityEngine;

/// <summary>
/// Component to spawn cat to fall from the top of the screen.
/// </summary>
public class SpawnController : MonoBehaviour
{
    [SerializeField]
    private float maxSpawnVelocity;

    [SerializeField]
    private Cat[] spawnables;

    [SerializeField]
    private float minSpawnTime = 0.25f;

    [SerializeField]
    private float maxSpawnTime = 0.5f;

    [SerializeField]
    private float minX;

    [SerializeField]
    private float maxX;

    protected virtual void Awake()
    {
        // Initial spawn
        Spawn();
    }
    private void Spawn()
    {
        Cat randomCat = spawnables[Random.Range(0, spawnables.Length)];
        Cat spawnedCat = Instantiate(randomCat);

        // Apply some initial velocity
        var catRigidBody = spawnedCat.GetComponent<Rigidbody2D>();
        if (catRigidBody != null)
        {
            Vector2 randomVelocity = Random.insideUnitCircle * maxSpawnVelocity;

            // Always go down
            if (randomVelocity.y > 0)
            {
                randomVelocity.y = -randomVelocity.y;
            }

            catRigidBody.velocity = randomVelocity;
        }

        var spawnPosition = new Vector2(Random.Range(minX, maxX), transform.position.y);
        spawnedCat.transform.position = spawnPosition;

        // Queue next spawn
        Invoke(nameof(Spawn), Random.Range(minSpawnTime, maxSpawnTime));
    }
}
