using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    Spawn[] SpawnPoints;
    void Awake()
    {
        Instance = this;
        SpawnPoints = GetComponentsInChildren<Spawn>();
    }
    public Transform GetSpawnPoints()
    {
        return SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform;
    }

}
