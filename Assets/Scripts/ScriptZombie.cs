using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptZombie : MonoBehaviour
{
	private Rigidbody m_Rb;
	private bool m_isAlive;
	private bool m_isAboutToBeDestroyed;
	private float m_fHp;
	private float? m_fTimeToRotAnimEh;
	private float m_fTimeLastBite;
	private bool m_isAnimRotStarted = false;
	private PlayerStuff m_TouchingPlayer = null;
	[SerializeField] GameObject HomingTarget = null;
	[SerializeField] ParticleSystem PrefabBlood;
	// Start is called before the first frame update

	public void Initialize(GameObject Homing)
	{
		HomingTarget = Homing;
	}

	void Start()
	{
		m_fTimeLastBite = 0;
		m_Rb = GetComponent<Rigidbody>();
		m_fHp = 100;
		m_isAlive = true;
	}

	void fixedAlive()
	{
		if(HomingTarget != null && HomingTarget.GetComponent<PlayerStuff>().isAlive) {
			// Go to target
			var TargetAngle = Mathf.Rad2Deg * Mathf.Atan2(
				HomingTarget.transform.position.x - transform.position.x,
				HomingTarget.transform.position.z - transform.position.z
			) - 90.0f;
			if(TargetAngle < 0) {
				TargetAngle += 360.0f;
			}

			// Change angle a bit to match the TargetAngle
			var CurrentAngle = transform.rotation.eulerAngles.y;
			var DeltaAngle = Mathf.Clamp(
				Mathf.DeltaAngle(CurrentAngle, TargetAngle), -3.0f, 3.0f
			);
			var NewRotation = Quaternion.Euler(.0f, CurrentAngle + DeltaAngle, .0f);
			m_Rb.MoveRotation(NewRotation);

			// Move forward
			var NewPos = transform.position + NewRotation * new Vector3(.05f, .0f, .0f);
			m_Rb.MovePosition(NewPos);

			var Now = Time.time;
			if(m_TouchingPlayer != null && m_fTimeLastBite < Now - 1.0f) {
				// Bite
				m_fTimeLastBite = Now;
				m_TouchingPlayer.damage(Mathf.RoundToInt(Random.Range(5, 10)));
			}
		}
	}

	void fixedDead()
	{
		if(m_Rb.velocity == Vector3.zero) {
			if(!m_fTimeToRotAnimEh.HasValue) {
				// Body is lying on ground - start rot countdown
				m_fTimeToRotAnimEh = Time.time + 10;
			}
			else {
				if(Time.time < m_fTimeToRotAnimEh) {
					if(!m_isAnimRotStarted) {
						GetComponent<BoxCollider>().enabled = false;
						m_Rb.isKinematic = true;
						m_isAnimRotStarted = true;
					}
					m_Rb.MovePosition(transform.position - new Vector3(.0f, .1f, .0f));
				}
			}
		}
		else if(!m_isAnimRotStarted) {
			m_fTimeToRotAnimEh = null;
		}

		if(transform.position.y < -1.0f) {
			Destroy(gameObject);
		}
	}

	void FixedUpdate()
	{
		if(m_isAlive) {
			fixedAlive();
		}
		else {
			fixedDead();
		}
	}

	public void damage(Quaternion Angle)
	{
		if(m_isAlive) {
			m_fHp -= 50;
			if(m_fHp <= 0) {
				m_Rb.isKinematic = false;
				m_isAlive = false;
				var Level = GameObject.Find("LevelState").GetComponent<ScriptLevel>();
				Level.scoreKill(10);
				SpawnBlood();
			}
		}
		if(!m_isAlive) {
			m_Rb.AddForce(Angle * new Vector3(0.0f, 100.0f, 1000.0f));
		}
	}

	void OnCollisionEnter(Collision Col)
	{
		var Collider = Col.collider;
		if(m_isAlive) {
			var Player = Collider.GetComponent<PlayerStuff>();
			if(Player != null) {
				m_TouchingPlayer = Player;
			}
		}
		else {
			if(Collider.tag != "ground") {
				// If colliding with multiple objects at once, emit blood only once
				// to prevent slowdowns.
				if(!m_isAboutToBeDestroyed) {
					SpawnBlood();
					Destroy(gameObject);
					m_isAboutToBeDestroyed = true;
				}
			}
		}
	}

	void OnCollisionExit(Collision Col)
	{
		var Player = Col.collider.GetComponent<PlayerStuff>();
		if(Player != null && m_TouchingPlayer == Player) {
			m_TouchingPlayer = null;
		}
	}

	void SpawnBlood()
	{
		var Blood = Instantiate(
			PrefabBlood, transform.position, PrefabBlood.transform.rotation
		);
		Blood.Play();
	}

}
