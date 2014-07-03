using UnityEngine;
using System.Collections;

public class PlayerViewModel : MonoBehaviour
{
	private StageManager stageManager;

	private Animator animator;

	private GameObject borderBack;
	private GameObject borderFront;

	[SerializeField]
	private float maxSpeed = 0.1f;

	private Vector3 prevPosition;

	private bool isSlowMode = false;

	// Use this for initialization
	void Start()
	{
		GameObject stageManagerObject = GameObject.Find("StageManager");
		this.stageManager = stageManagerObject.GetComponent<StageManager>();
		if (this.stageManager == null)
		{
			Debug.LogError("Can not find stage manager");
		}

		this.borderBack = transform.Find("BorderBack").gameObject;
		this.borderFront = transform.Find("BorderFront").gameObject;

		this.borderBack.renderer.enabled = false;
		this.borderFront.renderer.enabled = false;

		this.animator = GetComponent<Animator>() as Animator;

		this.prevPosition = transform.position;
	}

	// Update is called once per frame
	void Update()
	{

		keyboardInput();

		// rotate borders if enabled
		if (this.borderBack.renderer.enabled == true &&
		    this.borderFront.renderer.enabled == true)
		{
			this.borderBack.transform.Rotate(new Vector3(0, 0, 1f));
			this.borderFront.transform.Rotate(new Vector3(0, 0, -1f));
		}
	
		this.animator.SetFloat("Horizontal", transform.position.x - this.prevPosition.x);
		this.prevPosition = transform.position;
	}

	void keyboardInput()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		this.animator.SetFloat("Horizontal", horizontal);
	
		if (Input.GetKeyDown(KeyCode.LeftShift) && !this.isSlowMode)
		{
			slowModeOn();
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			slowModeOff();
		}

		if (horizontal > 0f)
		{
			horizontal = 1f;
		}
		else if (horizontal < 0f)
		{
			horizontal = -1f;
		}

		if (vertical > 0f)
		{
			vertical = 1f;
		}
		else if (vertical < 0f)
		{
			vertical = -1f;
		}

		this.safeMove(new Vector3(horizontal * this.maxSpeed, vertical * this.maxSpeed, 0));
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		this.stageManager.ResetBullet(other.gameObject);
		Destroy(this.gameObject);

		this.stageManager.PlayerHit();
	}

	void slowModeOn()
	{
		Debug.Log("slow mode on");
		this.isSlowMode = true;
		this.maxSpeed = 0.04f;
		this.borderBack.renderer.enabled = true;
		this.borderFront.renderer.enabled = true;
	}

	void slowModeOff()
	{
		Debug.Log("slow mode off");
		this.isSlowMode = false;
		this.maxSpeed = 0.1f;
		this.borderBack.renderer.enabled = false;
		this.borderFront.renderer.enabled = false;
	}

	void safeMove(Vector3 offset)
	{
		transform.position = transform.position + offset;
	}
}
