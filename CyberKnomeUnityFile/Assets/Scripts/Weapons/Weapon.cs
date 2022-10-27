using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	[SerializeField] GameObject interactWeaponObj;
	[SerializeField] Animator AN;
	float lastFireTime;

	[Header("Weapon Trait")]
	public WeaponType weaponType;
	[SerializeField] float cooltime;
	[SerializeField] int damage;
	[Header("Sword")]
	[SerializeField] Vector2 hitboxSize;
	[Header("Gun")]
	[SerializeField] GameObject projectilePrefab;
	[SerializeField] Transform firePos;
	[SerializeField] float speed;
	[SerializeField] float duration;

    public bool Use()
	{
		if (lastFireTime + cooltime < Time.time)
		{
			AN.SetTrigger("Use");
			lastFireTime = Time.time;
			
			switch (weaponType)
			{
				case WeaponType.staff:

					break;

				case WeaponType.gun:
					Instantiate(projectilePrefab, firePos.position, transform.rotation).GetComponent<Projectile>().Setup(speed, damage, duration);
					break;
				case WeaponType.sword:
					StartCoroutine(UseRoutineSword());
					break;
			}

			return true;
		}
		else
		{
			return false;
		}
		
	}

	IEnumerator UseRoutineSword()
	{
		yield return new WaitForSeconds(0.15f);

		Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + transform.right, hitboxSize, transform.rotation.z, 1 << LayerMask.NameToLayer("Enemy"));

		if (colliders.Length > 0)
		{
			foreach (Collider2D obj in colliders)
			{
				if (obj.TryGetComponent(out IDamageable damageable))
				{
					damageable.OnDamage(damage);
				}
			}
		}
	}



	public void Drop()
	{
		Destroy(gameObject);
		Instantiate(interactWeaponObj, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity);
	}
}

public enum WeaponType
{
	staff,
	gun,
	sword,
}
