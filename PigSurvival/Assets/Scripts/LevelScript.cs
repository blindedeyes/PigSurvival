using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
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
    public delegate void EntitySpawnedEvent(GameObject o);
    private EntitySpawnedEvent entitySpawnedEvent;

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

        public float SpawnOffset;
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
    }

    public void Tick(float DeltaTime)
    {
        float newTime = runTime + DeltaTime;

        while ((lastUsedIndex + 1 < SpawnData.Count) && SpawnData[lastUsedIndex + 1].time < newTime)
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
        int count = data.enemyPrefabs.Length;
        float twoPi = Mathf.PI * 2f;
        for (int i =0; i < count; i++)
        {
            Vector3 offset = Vector3.zero;
            offset.x = Mathf.Cos((float)i / count * twoPi);
            offset.y = Mathf.Sin((float)i / count * twoPi);
            offset *= data.SpawnOffset;
            offset += PlayerController.Instance.MyTransform.position;

            var obj = ObjectPool.Instance.GetObject(data.enemyPrefabs[i]);
            //Position off screen.
            Vector3 pos = offset;
            //TODO: Offset from camera off screen?
            obj.transform.position = pos;

            entitySpawnedEvent?.Invoke(obj);
        }
    }

    public void SpawnFromPool(float DeltaTime)
    {
        timeSinceSpawn += DeltaTime;

        if ((spawnRate > 0) && timeSinceSpawn >= 1f / spawnRate && (spawnPool != null && spawnPool.Length >0))
        {
            Debug.LogWarning("Spawning From Pool");
            timeSinceSpawn = 0;

            var indx = UnityEngine.Random.Range(0, spawnPool.Length);
            var obj = ObjectPool.Instance.GetObject(spawnPool[indx]);

            float spawnOffset = UnityEngine.Random.Range(0f, 360f);
            float twoPi = Mathf.PI * 2f;

            Vector3 offset = Vector3.zero;
            offset.x = Mathf.Cos(spawnOffset *twoPi);
            offset.y = Mathf.Sin(spawnOffset *twoPi);
            offset *= 60f;//Always spawn 60 off, its off camera.
            offset += PlayerController.Instance.MyTransform.position;

            obj.transform.position = offset;

            entitySpawnedEvent?.Invoke(obj);
        }
    }

    public void RegisterEntitySpawnedEvent(EntitySpawnedEvent e)
    {
        entitySpawnedEvent -= e;
        entitySpawnedEvent += e;
    }

    public void UnregisterEntitySpawnedEvent(EntitySpawnedEvent e)
    {
        entitySpawnedEvent -= e;
    }
}
