using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using static Platformer.Core.Simulation;

public class Bricks : MonoBehaviour
{

    private Tilemap tilemap;
    private Grid grid;
    public AudioClip brickSound;
    public PlayerController player;
    public AudioClip bumpSound;

    // Start is called before the first frame update
    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        grid = tilemap.layoutGrid;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.GetContact(0).normal.y == 1.0f)
    //     {
    //         if (collision.gameObject.tag == "Player")
    //         {
    //             if (player.health.currentHP > 1)
    //                 BreakTile(collision.transform.position);
    //             else
    //                 BumpTile(collision.transform.position);
    //         }
    //     }
    // }

    public void BreakTile(Vector3 pos)
    {
        var cell = grid.WorldToCell(pos);
        // cell.y += 2;
        // Debug.Log("Breaking tile at " + cell);
        tilemap.SetTile(cell, null);
        if (brickSound != null)
            player.audioSource.PlayOneShot(brickSound);
    }

    public void BumpTile()
    {
        if (bumpSound != null)
                player.audioSource.PlayOneShot(bumpSound);
    }
}
