﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : Character
{
    [SerializeField]
    private float       moveDir = -1.0f;
    [SerializeField]
    private float       moveSpeed = 100.0f;
    [SerializeField]    
    private Transform   wallCheckObject;

    protected override bool knockbackOnHit => false;

    // Update is called once per frame
    protected override void Update()
    {
        if (!isDead)
        {
            bool hasGround = IsGround();

            Collider2D wallCollider = Physics2D.OverlapCircle(wallCheckObject.position, groundCheckRadius, groundCheckLayer);

            bool hasWall = (wallCollider != null);

            if ((!hasGround) || (hasWall))
            {
                moveDir = -moveDir;
            }

            Vector2 currentVelocity = rb.velocity;

            currentVelocity.x = moveDir * moveSpeed;

            if (canMove)
            {
                rb.velocity = currentVelocity;
            }

            TurnTo(currentVelocity.x);
        }

        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Ant collided with " + collision.name);

        int layerId = collision.gameObject.layer;
        int checkLayer = dealDamageLayers.value & (1 << layerId);

        if (checkLayer == 0)
        {
            Character character = collision.GetComponentInParent<Character>();
            if (character != null)
            {
                if (character.IsHostile(faction))
                {
                    Vector2 hitDirection = character.transform.position - transform.position;

                    character.DealDamage(1, hitDirection);
                }
            }
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        if (wallCheckObject != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(wallCheckObject.position, groundCheckRadius);
        }
    }
}
