using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;

public class pipe : MonoBehaviour
{
    public Transform end;
    public AudioClip sound;
    public PlayerController player;
    public GameObject currentCam;
    public GameObject newCam;
    public bool needsCrouch;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !needsCrouch)
        {
            if (player.audioSource && sound)
                    player.audioSource.PlayOneShot(sound);
            other.transform.position = end.position;
            currentCam.SetActive(false);
            newCam.SetActive(true);
        }
        // Debug.Log("Collision");
        else if (other.gameObject.tag == "Player" && player.crouch)
        {
            if (player.audioSource && sound)
                    player.audioSource.PlayOneShot(sound);
            other.transform.position = end.position;
            currentCam.SetActive(false);
            newCam.SetActive(true);
        }
    }
}
