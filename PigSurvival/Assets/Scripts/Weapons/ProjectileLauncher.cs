using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : GenericWeapon
{
    public GameObject projectilePrefab;
    public float degrees = 360f;

    protected virtual IEnumerable LaunchProjectiles()
    {
        var count = this.myData.GetProjectilesCount();
        float twoPi = Mathf.PI * 2f;
        var timeBetweenShots = myData.GetTimeBetweenSpawns();
        var waitTimeBetweenShots = new WaitForSeconds(timeBetweenShots);
        for (int i = 0; i < count; ++i)
        {
            var obj = ObjectPool.Instance.GetObject(projectilePrefab);
            var proj = obj.GetComponent<Projectile>();
            proj.myData = myData;
            //Stuff
            Vector3 direction = Vector3.zero;
            direction.x= Mathf.Cos(Mathf.Lerp(0, degrees, (float)i / count) * twoPi);
            direction.y = Mathf.Sin(Mathf.Lerp(0, degrees, (float)i / count) * twoPi);
            obj.transform.right = direction;
            transform.position = PlayerController.Instance.MyTransform.position;

            if(timeBetweenShots > float.Epsilon)
                yield return waitTimeBetweenShots;
        }
        yield break;
    }
}
