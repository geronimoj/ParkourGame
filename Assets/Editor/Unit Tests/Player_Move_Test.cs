using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Player_Move_Test
    {
        [UnityTest]
        public IEnumerator Player_Move_Ground()
        {


            GameObject wall = new GameObject
            {
                name = "Wall"
            };
            BoxCollider bc = wall.AddComponent<BoxCollider>();
            bc.size = new Vector3(20, 20, 1);
            wall.transform.position = new Vector3(0, 0, 10.5f);


            GameObject player = CreatePlayer();
            player.transform.position = new Vector3(0, 1, 0);
            PlayerController pc = player.GetComponent<PlayerController>();
            Debug.Assert(player.GetComponent<PlayerStateMachine>() != null);
            //Wait a short while for unity to finish moving everything otherwise the players caspualcast all seems to hit it despite it being out of range
            yield return new WaitForSeconds(1);

            float timer = 0;
            //Wait for 2.5 seconds
            while (timer < 3f)
            {
                timer += Time.deltaTime;
                pc.Move(Vector3.forward * 5 * Time.deltaTime);
                yield return null;
            }

            Debug.Assert(player.transform.position == new Vector3(0, 1, 9.5f - pc.colInfo.CollisionOffset));
        }

        public GameObject CreatePlayer()
        {
            GameObject player = new GameObject
            {
                name = "Player"
            };
            PlayerController pc = player.AddComponent<PlayerController>();
            pc.colInfo.Radius = 0.5f;
            pc.colInfo.LowerHeight = 1f;
            pc.colInfo.UpperHeight = 0.5f;
            pc.colInfo.CollisionOffset = 0.01f;
            pc.colInfo.SetOrigin(player.transform);
            return player;
        }
    }
}
