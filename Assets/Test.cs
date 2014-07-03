using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
	private float speed = 0.1f;

	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Input.GetAxis("Horizontal") > 0)
		{
				transform.position += new Vector3(speed, 0, 0);
		}
		else if (Input.GetAxis("Horizontal") < 0)
		{
				transform.position -= new Vector3(speed, 0, 0);
		}

		if (Input.GetAxis("Vertical") > 0)
			{
				transform.position += new Vector3(0, speed, 0);
			}
		else if (Input.GetAxis("Vertical") < 0)
			{
				transform.position -= new Vector3(0, speed, 0);
			}
	}
}
