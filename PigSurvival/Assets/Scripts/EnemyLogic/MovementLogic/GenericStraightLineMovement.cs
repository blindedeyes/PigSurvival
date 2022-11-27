using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

public struct GenericStraightLinePathJob : IJobParallelForTransform
{

    // Jobs declare all data that will be accessed in the job
    // By declaring it as read only, multiple jobs are allowed to access the data in parallel
    [ReadOnly]
    public NativeArray<float> velocity;
    [ReadOnly]
    public NativeArray<bool> isActive;

    // Delta time must be copied to the job since jobs generally don't have a concept of a frame.
    // The main thread waits for the job same frame or next frame, but the job should do work deterministically
    // independent on when the job happens to run on the worker threads.        
    public float deltaTime;

    //Target to walk to in world space.
    public Vector3 worldSpaceTarget;

    //Enemy movement code. Executes once per enemy.
    public void Execute(int Index, TransformAccess transform)
    {

        if (isActive[Index])
        {
            var pos = transform.position;

            var direction = worldSpaceTarget - pos;
            direction.Normalize();

            var speed = velocity[Index] * deltaTime;
            direction *= speed;

            transform.position = transform.position + direction;
        }
    }
}
