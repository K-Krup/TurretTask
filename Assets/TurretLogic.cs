using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLogic : MonoBehaviour
{
    [SerializeField]
    int chargesCount; // 12 by default
    int currentChargesCount;

    [SerializeField]
    float minTurnAngle, maxTurnAngle; //15, 45 by default

    [SerializeField]
    float reloadTime; // 0.5 sec by defalt

    [SerializeField]
    float coolDownTime; // 6 sec by default

    [SerializeField]
    GameObject bulletPrefab;

    ObjectPooler objectPooler;

    [SerializeField]
    string prefabTag;

    MeshRenderer meshRenderer;

    readonly Color RED = new Color(255, 0,0);
    readonly Color WHITE = new Color(255, 255, 255);

    //lets skip waiting stage (cooldown)
    [SerializeField]
    bool isInitial;
  
   
    static HashSet<TurretLogic> visibleTurrets;
    static readonly int maxTurrets = 100;
    public static int GetTurretCout(){ return visibleTurrets.Count;}
    public static bool IsMaxTurretRiched()
	{
        return visibleTurrets.Count >= maxTurrets;
	}



    private void Awake()
    {
        currentChargesCount = chargesCount;
        meshRenderer = GetComponent<MeshRenderer>();
        //turretCouter = 0;
        visibleTurrets = new HashSet<TurretLogic>();
    }

	void Start()
    {
        objectPooler = ObjectPooler.GetInstance();

        if (isInitial)
        {
            Recolor(RED);
            StartCoroutine("ShootingCorutine");
        }
    }

	void RotateAndShoot()
	{
        float rotation = Random.Range(minTurnAngle, maxTurnAngle);
        transform.rotation *= Quaternion.Euler(0, rotation, 0);
 
        GameObject obj = objectPooler.SpawObjectFromPool(prefabTag, transform.position, transform.rotation);
        BulletLogic bullet = obj.GetComponent<BulletLogic>();

        //prevents bullet from destroying the turret it was shot from
        if (bullet == null)
            Debug.LogWarning("Turret is trying to spawn a wrong object type");
        else
            bullet.SetParrentTurret(this.gameObject);

        currentChargesCount--;
    }
    
    IEnumerator ShootingCorutine()
	{
        RotateAndShoot();

        yield return new WaitForSeconds(reloadTime);

        if (currentChargesCount > 0)
			StartCoroutine("ShootingCorutine");
        else
            Recolor(WHITE);
	}

    //is called when bullet creates a turret. 
    public void CreateTurret()
	{
        isInitial = false;
        currentChargesCount = chargesCount;

        StartCoroutine("CoolDownCoroutine");
    }

    IEnumerator CoolDownCoroutine()
	{
        Recolor(WHITE);
        yield return new WaitForSeconds(coolDownTime);
        
        Recolor(RED);
        StartCoroutine("ShootingCorutine");
    }

	private void OnEnable()
	{
        visibleTurrets.Add(this);
       
        if (IsMaxTurretRiched())
            TurretLogic.ActivateVisibleTurrets();
	}

	private void OnDisable()
	{
        //StopAllCoroutines();
        visibleTurrets.Remove(this);
    }

	void Recolor(Color color)
	{
        meshRenderer.material.color = color;
    }

    public void ActivateThisTurret()
	{
        StopAllCoroutines();

        currentChargesCount = chargesCount;

        Recolor(RED);
        StartCoroutine("ShootingCorutine");
    }

    static void ActivateVisibleTurrets()
	{
        foreach(TurretLogic turret in visibleTurrets)
		{
            turret.ActivateThisTurret();
		}
	}
}
