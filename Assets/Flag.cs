using UnityEngine;
using System.Collections;

public class Flag : MonoBehaviour {
    public Vector2 destination;
    public float speed = 0.1f;

    void Start () {
        destination = transform.position;
        destination.y -= 8.0f;
    }

    void Update () {
        transform.position = Vector2.Lerp(transform.position, destination, speed * Time.deltaTime);
    }
}
