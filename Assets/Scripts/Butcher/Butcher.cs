using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butcher : MonoBehaviour
{

    [SerializeField] private float mAcceleration;
    [SerializeField] private float mMaxSpeed;
    [SerializeField] private GameObject mTarget;
    [SerializeField] private float mMaxTargetDistance;
    [SerializeField] private float mMinTargetDistance;
    [SerializeField] [Range(0, 1.0f)] private float mSlowFactor;

    private Animator mAnimator;
    private Vector3 mDirection = Vector3.zero;
    private Rigidbody2D mRb2D;
    private bool mSlowed = false;
    private GameObject mCleaver;
    private Vector3 mSpawn;
    private bool mPigHunt = false;

    private static readonly int ATTACK = Animator.StringToHash("Attack");
    private static readonly int WALK = Animator.StringToHash("Walk");

    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        mRb2D = GetComponent<Rigidbody2D>();
        mCleaver = transform.Find("Cleaver").gameObject;
        mSpawn = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        Vector3 lTranslationSecond = mDirection * mAcceleration;
        mRb2D.AddForce(Time.fixedDeltaTime * lTranslationSecond);
        float lCurrentMaxSpeed = (mSlowed ? mMaxSpeed * mSlowFactor : mMaxSpeed);
        if (Mathf.Abs(mRb2D.velocity.x) > lCurrentMaxSpeed)
        {
            mRb2D.velocity = new Vector2(mRb2D.velocity.x * lCurrentMaxSpeed, mRb2D.velocity.y);
        }
        //Look at the target or the spawn
        if (mDirection.magnitude > Mathf.Epsilon)
            transform.localScale = new Vector3(((mPigHunt ? mTarget.transform.position.x : mSpawn.x)- transform.position.x < 0.0f) ? 1.0f : -1.0f, transform.localScale.y, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        bool lVisible = false;
        mTarget = GameObject.FindGameObjectWithTag("Player");

        if (mTarget)
        {
            //Search a target
            Transform lTargetTransform = mTarget.transform;
            float lDistanceFromTarget = Vector3.Distance(transform.position, lTargetTransform.position);

            // Is it visible ?
            int layerMask = 1 << 8 | 1 << 16 | 1 << 20; //Only collide on player, ground and door

            Vector3 lViewDirection = Vector3.Normalize(lTargetTransform.position - transform.position);
            Vector3 lUppedVector = transform.position + (Vector3.up * 0.15f);
            RaycastHit2D lHit = Physics2D.Raycast(lUppedVector, transform.TransformDirection(lViewDirection), Mathf.Infinity, layerMask);
            if (lHit.collider != null)
            {
                if (lHit.transform.gameObject.CompareTag("Player"))
                {
                    Debug.DrawRay(lUppedVector, transform.TransformDirection(lViewDirection) * lHit.distance, Color.red);
                    lVisible = true;
                }
                else
                {
                    Debug.DrawRay(lUppedVector, transform.TransformDirection(lViewDirection) * lHit.distance, Color.yellow);
                }
            }

            // Is it close enought ?
            if (lVisible && lDistanceFromTarget < mMaxTargetDistance)
            {
                mPigHunt = true;
                if (lDistanceFromTarget > mMinTargetDistance)
                {
                    mDirection = lTargetTransform.position.x > transform.position.x ? Vector3.right : Vector3.left;
                    mAnimator.SetBool(WALK, true);
                }
                else
                {
                    mDirection = lTargetTransform.position.x > transform.position.x ? Vector3.right : Vector3.left;
                    mAnimator.SetBool(WALK, false);
                }
            }
            else
            {
                mPigHunt = false;
                if (Mathf.Abs(transform.position.x - mSpawn.x) > 0.1)
                {
                    mDirection = mSpawn.x > transform.position.x ? Vector3.right : Vector3.left;
                    mAnimator.SetBool(WALK, true);
                }
                else
                {
                    mDirection = Vector3.zero;
                    mAnimator.SetBool(WALK, false);
                }
            }

            //If very close, try to hit the pig
            if (lDistanceFromTarget <= mMinTargetDistance + Mathf.Epsilon)
            {
                mAnimator.SetBool(ATTACK, true);
            }
            else
            {
                mAnimator.SetBool(ATTACK, false);
            }
        }
        else
        {
            mAnimator.SetBool(ATTACK, false);
            mPigHunt = false;
            if (Mathf.Abs(transform.position.x-mSpawn.x) > 0.1)
            {
                mDirection = mSpawn.x > transform.position.x ? Vector3.right : Vector3.left;
                mAnimator.SetBool(WALK, true);
            }
            else
            {
                mDirection = Vector3.zero;
                mAnimator.SetBool(WALK, false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pig"))
        {
            mSlowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pig"))
        {
            mSlowed = false;
        }
    }
}
