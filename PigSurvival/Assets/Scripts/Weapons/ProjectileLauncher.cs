using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : GenericWeapon
{
    public GameObject projectilePrefab;
    public float degrees = 360f;


    public override void Spawn()
    {
        Restore();
        StartCoroutine(LaunchProjectiles());
    }

    protected virtual IEnumerator LaunchProjectiles()
    {
        var count = this.myData.GetProjectilesCount();
        float toRads = Mathf.PI / 180f;
        var timeBetweenShots = myData.GetTimeBetweenSpawns();
        var waitTimeBetweenShots = new WaitForSeconds(timeBetweenShots);
        for (int i = 0; i < count; ++i)
        {
            var obj = ObjectPool.Instance.GetObject(projectilePrefab);
            var proj = obj.GetComponent<Projectile>();
            proj.myData = myData;
            //Stuff
            Vector3 direction = Vector3.zero;
            direction.x= Mathf.Cos(Mathf.Lerp(0, degrees, (float)i / count) * toRads);
            direction.y = Mathf.Sin(Mathf.Lerp(0, degrees, (float)i / count) * toRads);
            obj.transform.right = direction;
            obj.transform.position = PlayerController.Instance.MyTransform.position;

            if(timeBetweenShots > float.Epsilon)
                yield return waitTimeBetweenShots;
        }
        yield break;
    }
}
