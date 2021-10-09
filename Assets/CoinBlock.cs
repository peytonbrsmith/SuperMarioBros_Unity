using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using static Platformer.Core.Simulation;

public class CoinBlock : MonoBehaviour
{

    private Tilemap tilemap;
    private Grid grid;
    public AudioClip brickSound;
    public AudioClip bumpSound;
    public PlayerController player;
    public Sprite newSprite;
    public GameObject coin;
    public Tilemap other;

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
    //     var cell = grid.WorldToCell(collision.transform.position);
    //     cell.y += 1;
    //     if (collision.gameObject.tag == "Player")
    //     {
    //         // Debug.Log(collision.GetContact(0).point);
    //         // Debug.Log(collision.GetContact(0).normal);
    //         // Debug.Log(cell);
    //         if (collision.GetContact(0).normal.y == 1.0f && tilemap.GetTile(cell).name != "Broken")
    //             BreakTile(cell);
    //         else if (collision.GetContact(0).normal.y == 1.0f)
    //             BumpTile(cell);
    //     }
    // }

    public void BreakTile(Vector3 position)
    {
        var newTile = ScriptableObject.CreateInstance<Tile>();
        newTile.sprite = newSprite;
        var cell = grid.WorldToCell(position);
        // cell.y += 1;
        tilemap.SetTile(cell, null);
        other.SetTile(cell, newTile);
        if (brickSound != null)
            player.audioSource.PlayOneShot(brickSound);
        // Debug.Log(tilemap.GetTile(cell).name);
        var pos = new Vector3(cell.x, cell.y, cell.z);
        pos.x += 0.5f;
        pos.y += 2.0f;
        var coinObj = Instantiate(coin, pos, Quaternion.identity);
        coinObj.GetComponent<TokenInstance>().collect(player);
        StartCoroutine(coinFade(coinObj));
    }

    void BumpTile(Vector3 pos)
    {
        var cell = grid.WorldToCell(pos);
        cell.y += 1;
        var tile = tilemap.GetTile(cell);
        if (bumpSound != null)
                player.audioSource.PlayOneShot(bumpSound);
    }

    IEnumerator coinFade(GameObject coin) {
        yield return new WaitForSecondsRealtime(1.0f);
        Destroy(coin);
    }
}
