using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] SpriteRenderer SR;
    [SerializeField] ParticleSystem impactPS, timeoutPS;

    [SerializeField] bool isPlayers;

    bool isDestroyed;

    float speed = 5;
    int damage = 10;
    float maxDistance = 7;
    float traveledDistance = 0;

    public void Setup(float _speed, int _damage, float _distance)
    {
        speed = _speed;
        damage = _damage;
        maxDistance = _distance;
    }

    void FixedUpdate()
    {
        if (isDestroyed || maxDistance == 0) return;

        if (traveledDistance > maxDistance)
		{
            timeoutPS.Play();
            SR.enabled = false;
            isDestroyed = true;
            Destroy(gameObject, 1);
        }

        transform.Translate(Vector2.right * speed * Time.deltaTime);

        traveledDistance += speed * Time.deltaTime;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (isDestroyed) return;

        if (isPlayers && (collision.CompareTag("Player") || collision.CompareTag("Item"))) return;

		if (collision.TryGetComponent(out IDamageable damageable))
		{
            damageable.OnDamage(damage);

        }

        impactPS.Play();
        SR.enabled = false;
        isDestroyed = true;
        Destroy(gameObject, 1);
    }
}
