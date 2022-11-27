namespace Enemy
{
    using UnityEngine;
    using Enemy.Movement;
    using System.Collections;
    using System.Collections.Generic;
    using Unity.Collections;
    using Unity.Jobs;
    using UnityEngine.Jobs;


    public class EnemyManager : MonoBehaviour
    {
        public LevelScript levelData;

        // Start is called before the first frame update
        void Start()
        {
            levelData.Init(this.transform);
        }

        // Update is called once per frame
        void Update()
        {
            //Stuff
            Debug.Log("Update");
            levelData.Tick(Time.deltaTime);
        }
    }
}