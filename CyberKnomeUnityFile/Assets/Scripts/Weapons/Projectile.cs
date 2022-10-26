using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] SpriteRenderer SR;
    [SerializeField] ParticleSystem PS;

    [SerializeField] bool isPlayers;

    bool isDestroyed;
    float initializedTime;
    float speed = 5;
    int damage = 10;
    float duration = 4;

    public void Setup(float _speed, int _damage, float _duration)
    {
        initializedTime = Time.time;
        speed = _speed;
        damage = _damage;
        duration = _duration;
    }

    void FixedUpdate()
    {
        if (isDestroyed || initializedTime == 0) return;

        if (initializedTime + duration < Time.time)
		{
            Destroy(gameObject);
		}

        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (isDestroyed) return;

        if (isPlayers && (collision.CompareTag("Player") || collision.CompareTag("Item"))) return;

		if (collision.TryGetComponent(out IDamageable damageable))
		{
            damageable.OnDamage(damage);

        }

        PS.Play();
        SR.enabled = false;
        isDestroyed = true;
        Destroy(gameObject, 1);
    }
}
