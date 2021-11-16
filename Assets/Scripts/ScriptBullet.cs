using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptBullet : MonoBehaviour
{
	private float m_TimeStart;
	private Quaternion m_InitialAngle;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if(Time.time - m_TimeStart > 2) {
			Destroy(gameObject);
			return;
		}
	}

	void OnTriggerEnter(Collider Col)
	{
		if(Col.tag != "bullet") {
			Destroy(gameObject);
			var Enemy = Col.GetComponent<ScriptZombie>();
			if(Enemy != null) {
				Enemy.damage(m_InitialAngle);
			}
		}
	}

	public void initialize(float fForce)
	{
		var Rb = GetComponent<Rigidbody>();
		m_InitialAngle = transform.rotation;
		Rb.AddForce(transform.rotation * new Vector3(0.0f, 0.0f, fForce));
		m_TimeStart = Time.time;
	}
}
