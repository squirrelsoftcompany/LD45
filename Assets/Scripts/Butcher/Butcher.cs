using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butcher : MonoBehaviour
{

    [SerializeField] private float mAcceleration;
    [SerializeField] private float mMaxSpeed;
    //[SerializeField] private GameObject mSpawn;
    [SerializeField] private GameObject mTarget;
    [SerializeField] private float mMaxTargetDistance;

    private Vector3 mDirection = Vector3.zero;
    private Rigidbody2D mRb2D;
    private bool mSlowed = false;
    private GameObject mCleaver;

    // Start is called before the first frame update
    void Start()
    {
        mRb2D = GetComponent<Rigidbody2D>();
        mCleaver = transform.Find("Cleaver").gameObject;
    }

    private void FixedUpdate()
    {
        Vector3 lTranslationSecond = mDirection * mAcceleration;
        mRb2D.AddForce(Time.fixedDeltaTime * lTranslationSecond);
        float lCurrentMaxSpeed = (mSlowed ? mMaxSpeed * 0.5f : mMaxSpeed);
        if (Mathf.Abs(mRb2D.velocity.x) > lCurrentMaxSpeed)
        {
            mRb2D.velocity = new Vector2(mRb2D.velocity.x * lCurrentMaxSpeed, mRb2D.velocity.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Search a target
        Transform lTargetTransform = mTarget.transform;
        float lDistanceFromTarget = Vector3.Distance(transform.position, lTargetTransform.position);
        bool lVisible = false;

        // Is it visible ?
        int layerMask = 1 << 1; // TODO : Use the correct layer mask !! Currently, it is just hitting itself....
        layerMask = ~layerMask;

        Vector3 lViewDirection = Vector3.Normalize(lTargetTransform.position - transform.position);
        // Does the ray intersect any objects excluding the butcher layer
        RaycastHit2D lHit = Physics2D.Raycast(transform.position, transform.TransformDirection(lViewDirection), Mathf.Infinity, layerMask);
        if (lHit.collider != null)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(lViewDirection) * lHit.distance, Color.yellow);
            lVisible = true;
        }

        // Is it close enought ?
        float lMinDistance = 1.4f;
        if (lVisible && lDistanceFromTarget < mMaxTargetDistance && lDistanceFromTarget > lMinDistance)
        {
            mDirection = lTargetTransform.position.x > transform.position.x ? Vector3.right : Vector3.left;
        }
        else
        {
            mDirection = Vector3.zero;
        }

        //If very close, try to hit the pig
        if (lDistanceFromTarget <= lMinDistance + Mathf.Epsilon)
        {
            //Trigger attack animation -> active the cleaver child object
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 12) // 12 is body layer
        {
            mSlowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 12)// 12 is body layer
        {
            mSlowed = false;
        }
    }
}
