using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpVacuum : MonoBehaviour
{
    public PlayerController expTarget;

    public CircleCollider2D expCollectionBox;

    private Queue<int> expValues;

    private void Awake()
    {
        expValues = new Queue<int>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == gameObject.tag)
        {
            ExpObject exp = collision.gameObject.GetComponent<ExpObject>();
            expValues.Enqueue(exp.CollectExp(AddExpToPlayer, gameObject));
        }
    }

    public void AddExpToPlayer()
    {
        expTarget.AddExp(expValues.Dequeue());
    }
}
