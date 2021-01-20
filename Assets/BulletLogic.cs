using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    [SerializeField]
    float speed; // 4 points per second by default

    [SerializeField]
    float minDistance, maxDistance; // 1,4 by default
    float distance;

    float flightTime;
    
    bool landed;

    ObjectPooler objectPooler;

    //for ignoring initial collisions 
    GameObject parrentTurret;
    public void SetParrentTurret(GameObject value)
	{
        parrentTurret = value;
	}

    [SerializeField]
    string prefabTag;

    void Awake()
    {
        distance = Random.Range(minDistance, maxDistance);
        flightTime = distance / speed;

        objectPooler = ObjectPooler.GetInstance();
       
    }

	private void OnEnable()
	{
        landed = false;
        StartCoroutine(FlightCorutine());
	}
	void FixedUpdate()
    {
        if(!landed)
            transform.position += transform.forward * speed * Time.deltaTime;
    }

    IEnumerator FlightCorutine()
    { 
        yield return new WaitForSeconds(flightTime);
        
        landed = true;
        
        if(!TurretLogic.IsMaxTurretRiched())
            SpawnTurret();

        gameObject.SetActive(false);
    }

    void SpawnTurret()
    {
        GameObject spawnedObj = objectPooler.SpawObjectFromPool(prefabTag, transform.position, Quaternion.identity);
        TurretLogic turret = spawnedObj.GetComponent<TurretLogic>();

        if (turret == null)
            Debug.LogWarning("Bullet is trying to spawn a wrong object type");
        else
            turret.CreateTurret();
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Turret" && other.gameObject != parrentTurret)
		{
            other.gameObject.SetActive(false);

            landed = true;
            gameObject.SetActive(false);

        }
	}

}
