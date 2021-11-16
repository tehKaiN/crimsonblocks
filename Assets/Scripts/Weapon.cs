using UnityEngine;

/// Implements a default weapon.
public class Weapon
{
	private GameObject m_prefabBullet;
	private ScriptProjectiles m_projectiles;
	protected float m_angleRange = 5.0f;
	protected float m_shotInterval = .3f;
	public float ShotInterval {
		get {
			return m_shotInterval;
		}
		protected set {
			m_shotInterval = value;
		}
	}

	public Weapon(GameObject prefabBullet, ScriptProjectiles projectiles)
	{
		m_prefabBullet = prefabBullet;
		m_projectiles = projectiles;
	}

	public virtual void Shoot(Transform transform)
	{
		m_projectiles.add(m_prefabBullet, transform, m_angleRange);
	}
}
