using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;

public class EnemyManager : MonoBehaviour
{
    public LevelScript levelData;

    TransformAccessArray transArray;

    private List<EntityStats> stats;
    private JobHandle handle;

    NativeArray<bool> isActive;
    NativeArray<float> speeds;

    int activeObjects = 0;
    bool CanSpawn;

    // Start is called before the first frame update
    void Start()
    {
        levelData.Init(PlayerController.Instance.MyTransform);
        levelData.RegisterEntitySpawnedEvent(EntitySpawned);

        //Create a transform access array capped at 2048 transforms.
        //Removing transforms from this is expensive, so we are just
        //going to get a bunch prepped and use as needed.
        transArray = new TransformAccessArray(2048);
        stats = new List<EntityStats>(2048);
    }

    private void OnDestroy()
    {
        handle.Complete();
        if (isActive.IsCreated) isActive.Dispose();
        if (speeds.IsCreated) speeds.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        CanSpawn = (activeObjects < 2048);
        levelData.Tick(Time.deltaTime, ref CanSpawn);

        HandleJobSystem();

        //Kicks off al jobs.
        JobHandle.ScheduleBatchedJobs();
    }

    private void LateUpdate()
    {
        handle.Complete();
        isActive.Dispose();
        speeds.Dispose();
    }

    private void HandleJobSystem()
    {
        int cnt = stats.Count;
        isActive = new NativeArray<bool>(cnt, Allocator.TempJob);
        speeds = new NativeArray<float>(cnt, Allocator.TempJob);
        //populate arrays.
        for(int i = 0; i < cnt; i++)
        {
            var stat = stats[i];
            isActive[i] = stat.IsActive;
            speeds[i] = stat.Speed;
        }

        GenericStraightLinePathJob pathJob = new GenericStraightLinePathJob()
        {
            deltaTime = Time.deltaTime,
            isActive = isActive,
            velocity = speeds,
            worldSpaceTarget = PlayerController.Instance.MyTransform.position
        };

        handle = pathJob.Schedule(transArray);
        
    }

    void EntitySpawned(GameObject o)
    {
        var stat = o.GetComponent<EntityStats>();
        stat.Init();
        activeObjects++;
        CanSpawn = (activeObjects <= 2048);
        if (stat.TransformArrayIndex == -1)
        {
            stat.RegisterOnDeath(EntityDied);
            stat.TransformArrayIndex = transArray.length;
            if (transArray.length >= 2048)
            {
                Debug.LogError("ERROR MORE THAN 2K ENTITIES IN TRANSFORM ARRAY");
                return;
            }
            transArray.Add(stat.transform);
            stats.Add(stat);
        }
    }

    void EntityDied(EntityStats e)
    {
        activeObjects--;
        CanSpawn = (activeObjects <= 2048);

    }
}
