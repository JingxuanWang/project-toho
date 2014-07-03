using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
	private bool gameOver = false;

	private float startTime = 0f;
	private float elapsedTime = 0f;

	[SerializeField]
	private int hp = 1;

	[SerializeField]
	private float spawnInterval = 0.2f;

	[SerializeField]
	private int maximumBullets = 128;

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
	private GameObject playerPrefab;

	[SerializeField]
	private float playerTargetingRate = 0.1f;
	
	private List<BulletController> bulletList = new List<BulletController>();
	
	private GameObject player;

	private float lastSpawnAt = 0.0f;


	// Use this for initialization
	void Start()
	{
		this.adjustGameRegion();

		this.Restart();
	}

	void OnGUI()
	{
		GUILayout.Label("Time: " + this.elapsedTime);
		if (gameOver)
		{
			GUILayout.Label("Game Over");
			if (GUILayout.Button("Try Again?"))
			{
				this.Restart();
			}
		}
		else
		{
			this.elapsedTime = Time.realtimeSinceStartup - this.startTime;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Time.time - this.lastSpawnAt > this.spawnInterval &&
		    bulletList.Count < this.maximumBullets)
		{
			SpawnBullet();
			this.lastSpawnAt = Time.time;
		}


		ResetTargetPoint();
	}

	private void Restart()
	{
		foreach (BulletController bullet in this.bulletList)
		{
			Destroy(bullet.gameObject);
		}

		this.bulletList.Clear();


		this.SpawnPlayer();

		this.startTime = Time.realtimeSinceStartup;
		this.elapsedTime = 0f;
		this.gameOver = false;
	}

	private void SpawnBullet()
	{
		// set initial point
		GameObject bullet = Instantiate(this.bulletPrefab) as GameObject;
		BulletController bulletController = ResetBullet(bullet);

		// set parent to BulletSpawner
		bullet.transform.parent = transform;

		// add to list;
		this.bulletList.Add(bulletController);
	}

	private void SpawnPlayer()
	{
		this.player = Instantiate(this.playerPrefab) as GameObject;
	}

	public void PlayerHit()
	{
		this.hp--;

		if (this.hp <= 0)
		{
			this.gameOver = true;
		}
	}

	void ResetTargetPoint()
	{
		foreach (BulletController bc in bulletList)
		{
			// if outside the border the reset the target point
			if(!this.gameRegion.Contains(bc.transform.position))
			{
				ResetBullet(bc.gameObject);
			}
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
		if (Random.Range(0.0f, 1.0f) < this.playerTargetingRate && this.player != null)
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
