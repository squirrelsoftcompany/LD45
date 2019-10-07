using System.Collections;
using System.Collections.Generic;
using GameEventSystem;
using UnityEngine;

public class Cleaver : MonoBehaviour
{
    public GameEvent onEnter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 20)
        {
            onEnter.Raise();
        }
    }
}
