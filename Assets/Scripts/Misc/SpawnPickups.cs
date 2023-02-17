using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPickups : MonoBehaviour
{
    //public Transform spawnPoint;

    //public GameObject[] prefabArray;
    ////int random = Random.Range(0, 3);

    //public Pickup pickupPrefab;
    //public void SpawnRandomPickup()
    //{
    //    Pickup curPickUp = Instantiate(pickupPrefab, spawnPoint.position, spawnPoint.rotation);
    //}

    public Transform spawnPoint;

    public List<GameObject> prefabList = new List<GameObject>();
    public GameObject Prefab1;
    public GameObject Prefab2;
    public GameObject Prefab3;

    void Start()
    {
        prefabList.Add(Prefab1);
        prefabList.Add(Prefab2);
        prefabList.Add(Prefab3);

        int prefabIndex = UnityEngine.Random.Range(0, 3);
        Instantiate(prefabList[prefabIndex], spawnPoint.position, spawnPoint.rotation);
    }
}
