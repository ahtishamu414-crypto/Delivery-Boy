using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [Header("Segments")]
    public GameObject[] segmentPrefabs;     // only segment 2 and 3
    public int segmentsAhead = 4;
    public float segmentLength = 50f;

    private Transform player;
    private float spawnZ;
    private List<GameObject> activeSegments = new List<GameObject>();
    private int lastSegmentIndex = -1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Start spawning AFTER the starting segment
        spawnZ = segmentLength;

        for (int i = 0; i < segmentsAhead; i++)
            SpawnSegment();
    }

    void Update()
    {
        if (player.position.z + (segmentsAhead * segmentLength) > spawnZ)
            SpawnSegment();

        if (activeSegments.Count > 0)
        {
            // Only despawn randomly spawned segments
            // Starting segment is not in activeSegments list
            if (activeSegments[0].transform.position.z < player.position.z - segmentLength)
                DespawnSegment();
        }
    }

    void SpawnSegment()
    {
        int index;
        do {
            index = Random.Range(0, segmentPrefabs.Length);
        } while (index == lastSegmentIndex && segmentPrefabs.Length > 1);

        lastSegmentIndex = index;

        GameObject seg = Instantiate(
            segmentPrefabs[index],
            new Vector3(0, 0, spawnZ),
            Quaternion.identity
        );

        activeSegments.Add(seg);
        spawnZ += segmentLength;
    }

    void DespawnSegment()
    {
        Destroy(activeSegments[0]);
        activeSegments.RemoveAt(0);
    }
}