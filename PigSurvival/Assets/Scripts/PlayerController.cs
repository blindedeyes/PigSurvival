using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Timer
{
    public float Time;
    public float Value;

    public bool Tick(float deltaTime)
    {
        Value += deltaTime;
        return (Value >= Time);
    }
}

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;
    public static PlayerController Instance
    {
        get { return instance; }
    }

    public Transform MyTransform { get; private set; }
    public Rigidbody2D myRigid { get; private set; }
    EntityStats stats;

    //Weapon Timer struct


    [System.Serializable]
    public struct Weapon
    {
        private Timer timer;

        public int WeaponLevel
        {
            get{ return stats.currentLevel; }
            set { 
                stats.currentLevel = value; 
                timer.Time = stats.GetSpawnTime();
            }
        }

        public WeaponData stats;

        public bool Tick(float deltaTime)
        {
            bool res = (timer.Tick(deltaTime));
            if (res) timer.Value -= timer.Time;
            return res;
        }

    }

    public Weapon[] weapons;

    private void Awake()
    {
        instance = this;
        MyTransform = transform;
        myRigid = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<EntityStats>();

        //Setup the weapons in the object pool
        for (int i = 0; i < weapons.Length; i++)
        {
            ObjectPool.Instance.CreatePool(weapons[i].stats.prefab);
        }
        //First weapon level is 1 by default.
        weapons[0].WeaponLevel = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
        
        HandleWeapons();
    }

    void MovePlayer()
    {
        float speed = stats.Speed;
        Vector3 moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDir.y += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDir.x -= 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDir.y -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir.x += 1;
        }

        if (moveDir.sqrMagnitude > 0)
        {
            moveDir.Normalize();

            //Should rotate the player the right way
            MyTransform.right = moveDir;

            moveDir *= speed*Time.deltaTime;

            //MyTransform.position = MyTransform.position + moveDir;
            myRigid.MovePosition(MyTransform.position + moveDir);
        }
    }

    public void HandleWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].Tick(Time.deltaTime))
            {
                StartCoroutine(SpawnWeapon(weapons[i]));
            }
        }
    }

    private IEnumerator SpawnWeapon(Weapon weapon)
    {
        //Spawn weapon Attack object
        for (int spawncnt = 0; spawncnt < weapon.stats.GetProjectilesCount(); ++spawncnt)
        {
            var obj = ObjectPool.Instance.GetObject(weapon.stats.prefab);
            var wep = obj.GetComponent<GenericWeapon>();
            wep.myData = weapon.stats;
            wep.Spawn();
            yield return new WaitForSeconds(weapon.stats.GetTimeBetweenSpawns());
        }
    }
}
