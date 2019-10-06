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
    [SerializeField] private float mMinTargetDistance;

    private Animator mAnimator;
    private Vector3 mDirection = Vector3.zero;
    private Rigidbody2D mRb2D;
    private bool mSlowed = false;
    private GameObject mCleaver;

    private static readonly int ATTACK = Animator.StringToHash("Attack");
    private static readonly int WALK = Animator.StringToHash("Walk");

    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        mRb2D = GetComponent<Rigidbody2D>();
        mCleaver = transform.Find("Cleaver").gameObject;
    }

    private void FixedUpdate()
    {
        Vector3 lTranslationSecond = mDirection * mAcceleration;
        mRb2D.AddForce(Time.fixedDeltaTime * lTranslationSecond);
        float lCurrentMaxSpeed = mMaxSpeed;// (mSlowed ? mMaxSpeed * 0.5f : mMaxSpeed);
        if (Mathf.Abs(mRb2D.velocity.x) > lCurrentMaxSpeed)
        {
            mRb2D.velocity = new Vector2(mRb2D.velocity.x * lCurrentMaxSpeed, mRb2D.velocity.y);
        }
        transform.localScale = new Vector3(mRb2D.velocity.x < 0.0f ? 1.0f : -1.0f, transform.localScale.y, transform.localScale.z);
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
        if (lVisible && lDistanceFromTarget < mMaxTargetDistance && lDistanceFromTarget > mMinTargetDistance)
        {
            mDirection = lTargetTransform.position.x > transform.position.x ? Vector3.right : Vector3.left;
            mAnimator.SetBool(WALK, true);
        }
        else
        {
            mDirection = Vector3.zero;
            mAnimator.SetBool(WALK, false);
        }

        //If very close, try to hit the pig
        if (lDistanceFromTarget <= mMinTargetDistance + Mathf.Epsilon)
        {
            mAnimator.SetBool(ATTACK, true);
            //Trigger attack animation -> active the cleaver child object
        }
        else
        {
            mAnimator.SetBool(ATTACK, false);
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
