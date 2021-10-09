using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Platformer.Core;

using Platformer.Model;
using static Platformer.Core.Simulation;


namespace Platformer.Mechanics
{
    public class PowerBlock : MonoBehaviour
    {

        private Tilemap tilemap;
        private Grid grid;
        public AudioClip brickSound;
        public AudioClip bumpSound;
        public PlayerController player;
        public Sprite newSprite;
        public GameObject power;
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
        //     Debug.Log("Collision");
        //     var cell = grid.WorldToCell(collision.transform.position);
        //     cell.y += 1;
        //                 Debug.Log(collision.GetContact(0).point);
        //         Debug.Log(collision.GetContact(0).normal);
        //         Debug.Log(cell);
        //     if (collision.gameObject.tag == "Player")
        //     {

        //         if (collision.GetContact(0).normal.y == 1.0f)
        //             BreakTile(cell);
        //         else if (collision.GetContact(0).normal.y == 1.0f)
        //             BumpTile(cell);
        //     }
        // }

        // void OnTriggerEnter2D(Collider2D other)
        // {
        //     Debug.Log("Trigger");
        // }

        // void OnCollisionStay2D(Collision2D other)
        // {
        //     Debug.Log("CollisionStay");
        // }

        // void OnCollisionExit2D(Collision2D other)
        // {
        //     Debug.Log("Exit");
        // }

        public void BreakTile(Vector3 block)
        {
            Debug.Log(block);
            var cell = grid.WorldToCell(block);
            Debug.Log(cell);
            // cell.y += 1;
            // Debug.Log(cell);
            var newTile = ScriptableObject.CreateInstance<Tile>();
            newTile.sprite = newSprite;
            newTile.name = "Broken";
            tilemap.SetTile(cell, null);
            other.SetTile(cell, newTile);
            if (brickSound != null)
                player.audioSource.PlayOneShot(brickSound);
            // Debug.Log(tilemap.GetTile(cell).name);
            var pos = new Vector3(cell.x, cell.y, cell.z);
            pos.x += 0.5f;
            pos.y += 2.0f;
            var powerObj = Instantiate(power, pos, Quaternion.identity);
            // powerObj.GetComponent<TokenInstance>().collect(player);
            // StartCoroutine(powerFade(powerObj));
            gameObject.GetComponent<PowerBlock>().enabled = false;
        }

        void BumpTile(Vector3 pos)
        {
            var cell = grid.WorldToCell(pos);
            cell.y += 1;
            var tile = tilemap.GetTile(cell);
            if (bumpSound != null)
                    player.audioSource.PlayOneShot(bumpSound);
        }

        IEnumerator powerFade(GameObject power) {
            yield return new WaitForSecondsRealtime(1.0f);
            Destroy(power);
        }
    }

}
