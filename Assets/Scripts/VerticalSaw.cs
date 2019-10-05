using System.Collections;
using System.Collections.Generic;
using GameEventSystem;
using UnityEngine;

public class VerticalSaw : MonoBehaviour
{
    public GameEvent onEnter;

    [SerializeField] private AnimationCurve mCurve;
    [SerializeField] private int mSize;
    [SerializeField] private float mDuration;

    private Vector3 mInitialPosition;
    // Start is called before the first frame update

    void Start()
    {
        mInitialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(mInitialPosition.x, mInitialPosition.y + mCurve.Evaluate(Time.time / mDuration) *mSize, mInitialPosition.z);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            onEnter.Raise();
        }
    }
}
