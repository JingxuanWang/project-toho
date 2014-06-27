using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
	[SerializeField]
	private float minSpeed = 0.01f;

	[SerializeField]
	private float maxSpeed = 0.1f;

	[SerializeField]
	private float timeToMove = 1.0f;

	private float lifetime;
	private float speed;
	private Vector3 targetPoint;
	private Vector3 direction;


	[SerializeField]
	private Sprite[] sprites;

	public Vector3 TargetPoint 
	{
		get
		{
			return this.targetPoint;
		}
		set
		{
			this.targetPoint = value;
			this.direction = (targetPoint - transform.position).normalized;
			this.speed = Random.Range(this.minSpeed, this.maxSpeed);
			this.lifetime = 0.0f;
			this.transform.up = this.direction;
		}
	}
	
	// Use this for initialization
	void Start()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		sr.sprite = this.sprites[Random.Range(0, this.sprites.GetLength(0) - 1)];

		this.lifetime = 0.0f;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (this.lifetime < this.timeToMove)
		{
			this.lifetime += Time.deltaTime;
		}
		else
		{
			transform.position += this.direction * this.speed;
		}
	}
}
