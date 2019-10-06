using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OutBoundsDestroy : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        Destroy(other.gameObject);
    }
}