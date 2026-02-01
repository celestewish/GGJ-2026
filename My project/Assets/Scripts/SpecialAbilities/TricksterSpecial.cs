using UnityEngine;

public class TricksterSpecial : CharacterParentClass
{
    [Header("Clone Prefabs")]
    [SerializeField] private GameObject purpleClonePrefab;
    [SerializeField] private GameObject greenClonePrefab;
    [SerializeField] private GameObject goldClonePrefab;

    // Offset so it doesn’t spawn exactly on top of the player
    [SerializeField] private Vector2 spawnOffset = new Vector2(0.5f, 0f);

    [Header("Audio")]
    public AudioClip tricksterSound;

    protected override void Awake()
    {
        base.Awake();
        attackClip = tricksterSound;
    }

    protected override void PerformSpecial()
    {
        base.PerformSpecial();
        // Finds prefab and spawns it if possible
        GameObject prefabToSpawn = GetRandomClonePrefab();
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("No clone prefab assigned on " + name);
            return;
        }

        Vector3 spawnPos = transform.position + (Vector3)spawnOffset;
        Quaternion spawnRot = Quaternion.identity;

        Instantiate(prefabToSpawn, spawnPos, spawnRot);
    }

    private GameObject GetRandomClonePrefab()
    {
        // Choose between 0,1,2
        int index = Random.Range(0, 3);

        switch (index)
        {
            case 0: return purpleClonePrefab;
            case 1: return greenClonePrefab;
            case 2: return goldClonePrefab;
        }
        return null;
    }
}
