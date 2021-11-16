using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStuff : MonoBehaviour
{
	private Vector2 m_Input;
	private Rigidbody m_Rb;
	private bool m_isShooting;
	private float m_fNextShotTime;
	private int m_Hp;
	private bool m_isAlive;
	private ScriptProjectiles m_Projectiles;
	private Weapon m_Weapon;

	[SerializeField] CrossHair Crosshair;
	[SerializeField] GameObject BulletPrefab;

	public int Hp {
		get {return m_Hp;}
	}
	public bool isAlive {
		get {return m_isAlive;}
	}

	void OnMovement(InputValue Movement)
	{
		m_Input = Movement.Get<Vector2>();
	}

	void OnFire(InputValue Fire)
	{
		m_isShooting = (Fire.Get<float>() > .0f);
	}

	// Start is called before the first frame update
	void Start()
	{
		m_Rb = GetComponent<Rigidbody>();
		m_Projectiles = GameObject.Find("Projectiles").GetComponent<ScriptProjectiles>();
		reset();
	}

	public void reset()
	{
		m_Weapon = new WeaponShotgun(BulletPrefab, m_Projectiles);
		m_isShooting = false;
		m_Hp = 100;
		m_isAlive = true;
		transform.position = new Vector3(
			Random.Range(-1.0f, 1.0f), 1.5f, Random.Range(-1.0f, 1.0f)
		);
		m_Projectiles.reset();
	}

	void fixedAlive()
	{
		// Movement
		Vector3 Pos = m_Rb.position;
		Pos.x += m_Input.x / 10;
		Pos.z += m_Input.y / 10;
		m_Rb.MovePosition(Pos);
		m_Rb.MoveRotation(Quaternion.Euler(new Vector3(
			0, Quaternion.LookRotation(Crosshair.transform.position - transform.position).eulerAngles.y, 0
		)));

		// Shooting
		if(m_isShooting && Time.time >= m_fNextShotTime) {
			m_Weapon.Shoot(transform);
			m_fNextShotTime = Time.time + m_Weapon.ShotInterval;
			Crosshair.pulseSize();
		}
	}

	void FixedUpdate()
	{
		if(m_isAlive) {
			fixedAlive();
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void damage(int Dmg)
	{
		if(m_isAlive) {
			m_Hp = Mathf.Clamp(m_Hp - Dmg, 0, 100);
			if(m_Hp <= 0) {
				m_isAlive = false;
			}
		}
	}
}
