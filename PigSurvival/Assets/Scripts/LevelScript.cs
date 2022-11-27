using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelScript", order = 1)]
public class LevelScript : ScriptableObject
{
    private float runTime;
    private GameObject[] spawnPool;
    private float spawnRate;
    private int lastUsedIndex;
    private Transform player;

    private float timeSinceSpawn;

    private Camera cam;

    public enum SpawnType
    {
        Random,
        PoolSwap,
        OffsetOnPlayer
    }

    [System.Serializable]
    public class EntitySpawnData
    {
        public SpawnType spawnType;
        public GameObject[] enemyPrefabs;
        public float time;
        public float spawnRate;
    }

    public class EntitySpawnDataComparer : IComparer<EntitySpawnData>
    {
        public int Compare(EntitySpawnData a, EntitySpawnData b)
        {
            if (a.time > b.time)
                return 1;
            else if (a.time < b.time)
                return -1;

            return 0;
        }
    }

    [SerializeField]
    private List<EntitySpawnData> SpawnData;

    public void Init(Transform player)
    {
        runTime = 0;
        lastUsedIndex = -1;

        //Sort the spawn data by time.
        var spawnData = SpawnData;

        SpawnData.Sort(new EntitySpawnDataComparer());
        cam = Camera.main;
    }

    public void Tick(float DeltaTime)
    {
        float newTime = runTime + DeltaTime;

        //Check to see if we are within the bounds of the play state.
        if (lastUsedIndex + 1 < SpawnData.Count)
        {
            while (SpawnData[lastUsedIndex + 1].time < newTime)
            {
                //Handle next entry.
                lastUsedIndex++;
                var data = SpawnData[lastUsedIndex];
                switch (data.spawnType)
                {
                    case SpawnType.Random:
                        HandleRandom(data);
                        break;
                    case SpawnType.PoolSwap:
                        HandlePoolSwap(data);
                        break;
                    case SpawnType.OffsetOnPlayer:
                        SpawnOffset(data);
                        break;
                    default:
                        break;
                }
            }
        }
        runTime = newTime;
        SpawnFromPool(DeltaTime);
    }

    public void HandlePoolSwap(EntitySpawnData data)
    {
        spawnPool = data.enemyPrefabs;
        spawnRate = data.spawnRate;
    }
    public void HandleRandom(EntitySpawnData data)
    {

    }
    public void SpawnOffset(EntitySpawnData data)
    {

    }

    public void SpawnFromPool(float DeltaTime)
    {
        timeSinceSpawn += DeltaTime;
        if(timeSinceSpawn >= 1f / spawnRate)
        {
            Debug.LogWarning("Spawning From Pool");
            timeSinceSpawn = 0;
            var indx = Random.Range(0, spawnPool.Length);
            var obj = ObjectPool.Instance.GetObject(spawnPool[indx]);
            //Position off screen.
            Vector3 pos = cam.transform.position;
            pos.y = 0f; //force y 0
            //TODO: Offset from camera off screen?
            obj.transform.position = pos;
        }
    }
}
