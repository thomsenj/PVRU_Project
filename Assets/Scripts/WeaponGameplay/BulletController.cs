﻿using Fusion;
using UnityEngine;

public class BulletController : NetworkBehaviour
{
    public float speed = 20f;
    public float maxDistance = 100f;
    public Vector3 gravity = new Vector3(0, -9.81f, 0);
    public float dragCoefficient = 0.47f;
    public float airDensity = 1.225f;
    public float crossSectionalArea = 0.01f;
    public int damage = 10;

    private Vector3 velocity;
    private float distanceTraveled = 0f;

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
            float deltaTime = Time.deltaTime;

            Vector3 dragForce = -0.5f * dragCoefficient * airDensity * crossSectionalArea * velocity.sqrMagnitude * velocity.normalized;
            velocity += (gravity + dragForce) * deltaTime;

            Vector3 displacement = velocity * deltaTime;
            gameObject.transform.position += displacement;

            distanceTraveled += displacement.magnitude;

            if (distanceTraveled >= maxDistance)
            {
                ResetBullet();
            }
    }

    public void Shoot(Vector3 startPosition, Vector3 shootDirection)
    {
        velocity = shootDirection.normalized * speed;
        gameObject.transform.position = startPosition;
        distanceTraveled = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        ResetBullet();

        if (other.GetComponent<EnemyAI>() != null)
        {
            other.GetComponent<EnemyAI>().TakeDamage(damage);
        }
    }

    private void ResetBullet()
    {
        Runner.Despawn(gameObject.GetComponent<NetworkObject>());
    }
}