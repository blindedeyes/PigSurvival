using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Animator playerAnimator;
    public SpriteRenderer sprRenderer;

    public GameObject DeathScreen;
    public GameObject WinnersText;

    public LevelScript levelData;

    public float InvincibleTimer = .25f;

    [Range(1, 10)]
    public int level = 0;
    public int totalExp = 0;

    public Slider healthSlider; 

    private int[] levelUpCaps = {0, 5, 10, 15, 20, 30, 40, 60, 80, 100 };

    private Timer invTimer = new Timer();
    private bool isInvincible = false;

    EntityStats stats;

    [HideInInspector]
    public Vector3 moveDir = Vector3.zero;

    //Weapon Timer struct

    private readonly int ANIM_WALKBOOL = Animator.StringToHash("Walking");
    private readonly int ANIM_DIETRIGGER = Animator.StringToHash("Die");
    private readonly int ANIM_RESETTRIGGER = Animator.StringToHash("Reset");

    [System.Serializable]
    public struct Weapon
    {
        private Timer timer;

        public int WeaponLevel
        {
            get{ return stats.currentLevel; }
            set {
                if (stats.currentLevel != value)
                {
                    stats.currentLevel = value;
                    //timer.Value = stats.GetSpawnTime();
                }
                timer.Time = stats.GetSpawnTime();
            }
        }

        public WeaponData stats;

        public bool Tick(float deltaTime)
        {
            bool res = (timer.Tick(deltaTime));
            if (res) timer.Value = 0;//Technically, this is wrong, if it would go off twice in one tick, it would miss one.
            return res;
        }
    }

    public Weapon[] weapons;

    private void Awake()
    {
        instance = this;
        MyTransform = transform;
        myRigid = GetComponent<Rigidbody2D>();
        Time.timeScale = 1.0f; //safety, in case we came from a previous play.
    }

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<EntityStats>();
        stats.Init();
        stats.RegisterOnDeath(OnPlayerDied);
        stats.RegisterOnDamage(SetHealth);
        invTimer.Time = InvincibleTimer;

        SetHealth(stats.currentHealth, stats.Health);

        //Setup the weapons in the object pool
        for (int i = 0; i < weapons.Length; i++)
        {
            ObjectPool.Instance.CreatePool(weapons[i].stats.prefab);
            weapons[i].WeaponLevel = level;
        }

        UIManager.instance.SetExp(totalExp, levelUpCaps[level]);
    }

    private void OnPlayerDied(EntityStats e)
    {
        //Player died, show the death screen.
        DeathScreen.SetActive(true);

        Debug.Log(levelData.runTime);
        if(levelData.runTime >= 180)
        {
            WinnersText.SetActive(true);
        }

        Time.timeScale = 0f;

        for (int i = 0; i < weapons.Length; i++)
        {
            ObjectPool.Instance.CreatePool(weapons[i].stats.prefab);
            weapons[i].WeaponLevel = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.G))
        {
            stats.currentHealth = 99999f;
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.T))
        {
            stats.currentHealth = stats.Health;
        }

        MovePlayer();

        if(isInvincible)
            isInvincible = !invTimer.Tick(Time.deltaTime);

        HandleWeapons();
    }

    void MovePlayer()
    {
        float speed = stats.Speed;
        Vector3 move = Vector3.zero;
        //moveDir = Vector3.zero;
        bool isWalking = false;
        if (Input.GetKey(KeyCode.W))
        {
            move.y += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            move.x -= 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move.y -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move.x += 1;
        }

        if (move.sqrMagnitude > 0)
        {
            move.Normalize();
            moveDir = move;

            isWalking = true;

            //Should rotate the player the right way
            //MyTransform.right = moveDir;
            sprRenderer.flipX = (moveDir.x > 0);

            moveDir *= speed*Time.deltaTime;

            //MyTransform.position = MyTransform.position + moveDir;
            myRigid.MovePosition(MyTransform.position + moveDir);
        }

        if (playerAnimator)
        {
            playerAnimator.SetBool(ANIM_WALKBOOL, isWalking);
        }
    }

    public void HandleWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].Tick(Time.deltaTime))
            {
                SpawnWeapon(weapons[i]);
            }
        }
    }

    private void SpawnWeapon(Weapon weapon)
    {
        var obj = ObjectPool.Instance.GetObject(weapon.stats.prefab);
        var wep = obj.GetComponent<GenericWeapon>();
        wep.myData = weapon.stats;
        wep.Spawn();
    }

    public void AddExp(int xpValue)
    {
        if (level >= levelUpCaps.Length) return;

        totalExp += xpValue;

        if (totalExp >= levelUpCaps[level])
        {
            while(totalExp >= levelUpCaps[level])
            {
                // subtract current level exp
                totalExp -= levelUpCaps[level];
                // level up
                level++;
                LevelUp();
            }
        }

        //Update weapons with level.
        for (int i = 0; i < weapons.Length; ++i)
        {
            weapons[i].WeaponLevel = level;
        }

        UIManager.instance.SetExp(totalExp, levelUpCaps[level]);
    }

    private void LevelUp()
    {
        //Play some fx
        // display info text
        

    }

    private void SetHealth(EntityStats e, float damage)
    {
        if (healthSlider)
        {
            healthSlider.maxValue = e.Health;
            healthSlider.value = e.currentHealth;
        }

    }

    private void SetHealth(float healthVal, float maxHealth)
    {
        if (healthSlider)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = healthVal;
        }

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isInvincible) return;

        //Collided with an enemy?
        var enemy = collision.gameObject;
        var estats = enemy.transform.parent.GetComponent<EntityStats>();
        if (estats.IsActive) { stats.TakeDamage(estats.TouchDamage); }
        isInvincible = true;
        invTimer.Value = 0;
    }
}
