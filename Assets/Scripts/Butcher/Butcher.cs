using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butcher : MonoBehaviour
{

    [SerializeField] private float mAcceleration;
    [SerializeField] private float mMaxSpeed;
    [SerializeField] private GameObject mSpawn;
    [SerializeField] private GameObject mTarget;
    [SerializeField] private float mMaxTargetDistance;

    private Vector3 mDirection = Vector3.zero;
    private Rigidbody2D mRb2D;

    // Start is called before the first frame update
    void Start()
    {
        mRb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector3 lTranslationSecond = mDirection * mAcceleration;
        mRb2D.AddForce(Time.fixedDeltaTime * lTranslationSecond);
        if (Mathf.Abs(mRb2D.velocity.x) > mMaxSpeed)
            mRb2D.velocity = new Vector2(mRb2D.velocity.normalized.x * mMaxSpeed, mRb2D.velocity.y);

    }

    // Update is called once per frame
    void Update()
    {
        //Search a target
        Transform lTargetTransform = mTarget.transform;
        bool lVisible = false;

        // Is it visible ?
        int layerMask = 1 << 1; // TODO : Use the correct layer mask !! Currently, it is just hitting itself....
        layerMask = ~layerMask;

        Vector3 lViewDirection = Vector3.Normalize(lTargetTransform.position- transform.position);
        // Does the ray intersect any objects excluding the butcher layer
        RaycastHit2D lHit = Physics2D.Raycast(transform.position, transform.TransformDirection(lViewDirection), Mathf.Infinity, layerMask);
        if (lHit.collider != null)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(lViewDirection) * lHit.distance, Color.yellow);
            lVisible = true;
        }

        // Is it close enought ?
        if (lVisible && Vector3.Distance(transform.position, lTargetTransform.position) < mMaxTargetDistance)
        {
            mDirection = lTargetTransform.position.x > transform.position.x ? Vector3.right : Vector3.left;
        } 
        else
        {
            mDirection = Vector3.zero;
        }
    }
}
