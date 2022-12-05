using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpObject : MonoBehaviour
{
    public int expValue = 1;
    public float tweenTime = .5f;

    public SpriteRenderer xpSprite;

    private GameObject tweenTarget;
    private Vector3 startPosition;

    private CircleCollider2D coll;

    private void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
    }

    public int CollectExp(Action onComplete, GameObject target)
    {
        tweenTarget = target;
        startPosition = transform.position;

        StartCoroutine(CollectExpCoroutine(onComplete, target));

        return expValue;
    }

    private IEnumerator CollectExpCoroutine(Action onComplete, GameObject target)
    {
        LeanTween.value(gameObject, MoveToTarget, 0, 1, tweenTime).setEase(LeanTweenType.easeInBack);
        coll.enabled = false;
        yield return new WaitForSeconds(tweenTime);

        DisableExp();
        onComplete();
    }

    private void MoveToTarget(float val)
    {
        gameObject.transform.position = Vector3.Lerp(startPosition, tweenTarget.transform.position, val);
    }


    public void EnableExp()
    {
        xpSprite.enabled = true;
        coll.enabled = true;
    }

    public void DisableExp()
    {
        xpSprite.enabled = false;
        tweenTarget = null;

        ObjectPool.Instance.FreeObject(gameObject);
    }
}
