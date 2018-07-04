using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public class PowerUpManager : MonoBehaviour
    {
        public bool Enabled { get; set; }

        public List<GameObject> spawningObjects = new List<GameObject>();
        public List<GameObject> spawningPoints = new List<GameObject>();

        public float spawnTime = 10f;

        private System.DateTime lastSpawn;
        private List<GameObject> occupiedPoints = new List<GameObject>();

        private GameObject GetRandomObjectFrom(List<GameObject> objects)
        {
            if (objects.Count > 0)
            {
                System.Random rnd = new System.Random();
                int i = rnd.Next(0, objects.Count);
                return objects[i];
            }
            return null;
        }

        private bool CanSpawn()
        {
            if (spawningObjects.Count > 0 && spawningPoints.Count > 0)
            {
                return (System.DateTime.Now - lastSpawn).Seconds > spawnTime;
            }
            return false;
        }


        private void Update()
        {
            if (Enabled && CanSpawn())
            {
                GameObject point = GetRandomObjectFrom(spawningPoints.FindAll(s => !occupiedPoints.Contains(s)));

                if (point != null)
                {
                    GameObject prefab = GetRandomObjectFrom(spawningObjects);

                    // Create an instance of the shell and store a reference to it's rigidbody.
                    GameObject powerup = Instantiate(prefab, point.transform.position, point.transform.rotation) as GameObject;
                    powerup.GetComponent<AbstractPowerUp>().PowerUpManager = this;

                    Debug.Log("Spawned a fucking cool power-up at " + point.transform.position);

                    lastSpawn = System.DateTime.Now;
                    occupiedPoints.Add(point);
                }
                else
                {
                    Debug.Log("Cannot spawn power-up. All Spawning-points are occupied!");
                }
            }
        }

        public void CleanSpawningPoint(GameObject powerUp)
        {
            GameObject point = occupiedPoints.Find(p => p.transform.position == powerUp.transform.position);
            if (point != null)
            {
                occupiedPoints.Remove(point);
                Debug.Log("Spawning Point " + point.transform.position + " is free again!");
            }
        }
    }
}