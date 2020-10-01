using UnityEngine;

public class BulletHole : MonoBehaviour
{
    // The length of time that the bullet hole should remain
    private float lifeTime = 5.0f;

    // Additive time of frame completion
    private float timeElapsed = 0.0f;

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        // Cleanup when the time elapsed is equal or greater than the objects lifeTime
        if (timeElapsed >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
