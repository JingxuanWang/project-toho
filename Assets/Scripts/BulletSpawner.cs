using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletSpawner : MonoBehaviour
{
	[SerializeField]
	private float spawnInterval = 1.0f;

	[SerializeField]
	private int maximumBullets = 64;

	[SerializeField]
	private Rect gameRegion = new Rect(-5f, -5f, 10f, 10f);
	public Rect GameRegion 
	{   
		get 
		{
			return this.gameRegion;
		}
	}

	[SerializeField]
	private GameObject bulletPrefab;

	[SerializeField]
	private float playerTargetingRate = 0.1f;
	
	private List<BulletController> bulletList = new List<BulletController>();
	
	private GameObject player;

	private float lastSpawnAt = 0.0f;


	// Use this for initialization
	void Start()
	{
		this.player = GameObject.Find("player");

		this.adjustGameRegion();

	}
	
	// Update is called once per frame
	void Update()
	{
		if (Time.time - this.lastSpawnAt > this.spawnInterval && 
		    bulletList.Count < this.maximumBullets)
		{
			spawn();
			this.lastSpawnAt = Time.time;
		}


		ResetTargetPoint();
	}

	void spawn()
	{
		// set initial point
		GameObject bullet = Instantiate(this.bulletPrefab) as GameObject;
		BulletController bulletController = ResetBullet(bullet);

		// set parent to BulletSpawner
		bullet.transform.parent = transform;

		// add to list;
		this.bulletList.Add(bulletController);
	}

	void ResetTargetPoint()
	{
		foreach(BulletController bc in bulletList)
		{
			// if outside the border the reset the target point
//			if(!this.gameRegion.IsInside(bc.transform.position))
//			{
//				ResetBullet(bc.gameObject);
//			}
		}
	}

	public BulletController ResetBullet(GameObject bullet)
	{
		bullet.transform.position = 
			new Vector3(
				Random.Range(this.gameRegion.xMin, this.gameRegion.xMax),
				Random.Range(this.gameRegion.yMin, this.gameRegion.yMax),
				0);
		
		// set target point
		BulletController bulletController = bullet.GetComponent<BulletController>();
		if (Random.Range(0.0f, 1.0f) < this.playerTargetingRate)
		{
			bulletController.TargetPoint = this.player.transform.position;
		}
		else
		{
			bulletController.TargetPoint =
				new Vector3(
					Random.Range(this.gameRegion.xMin, this.gameRegion.xMax),
					Random.Range(this.gameRegion.yMin, this.gameRegion.yMax),
					0);
		}
		return bulletController;
	}

	void adjustGameRegion()
	{
		float height = Camera.main.orthographicSize;
		float width = height * Camera.main.aspect;
		this.gameRegion = new Rect(-width, -height, 2 * width, 2 * height);
	}
}
