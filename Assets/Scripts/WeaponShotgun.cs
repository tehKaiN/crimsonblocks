using UnityEngine;

public class WeaponShotgun : Weapon
{
  public WeaponShotgun(GameObject prefabBullet, ScriptProjectiles projectiles):
		base(prefabBullet, projectiles)
	{
		m_angleRange = 30;
		m_shotInterval = 1.0f;
	}

	public override void Shoot(Transform transform)
	{
		for(int i = 0; i < 10; ++i) {
			base.Shoot(transform);
		}
	}
}
