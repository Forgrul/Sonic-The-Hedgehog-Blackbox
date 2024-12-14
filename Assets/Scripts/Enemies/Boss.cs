using UnityEngine;
using System.Collections;

using Giometric.UniSonic.Objects;

namespace Giometric.UniSonic.Enemies
{
    [RequireComponent(typeof(ObjectTriggerBase))]
    public class Boss : MonoBehaviour
    {
        private enum FacingDirection
        {
            Left = -1,
            Right = 1
        }

        [SerializeField]
        [Tooltip("How many hits it takes for the player to destroy this enemy.")]
        private int health = 10;

        [SerializeField]
        private FacingDirection facingDirection = FacingDirection.Right;

        [SerializeField]
        private float moveSpeed = 60f;
        
        [SerializeField]
        private float moveTime = 2f;

        [SerializeField]
        private float turnAroundWaitTime = 1f;

        [SerializeField]
        private LayerMask collisionMask;

        [SerializeField]
        private SpriteRenderer sprite;

        [SerializeField]
        private ObjectTriggerBase hitbox;

        private float waitTimer = 0f;
        private float moveTimer;

        private void Awake()
        {
            if (hitbox != null)
            {
                hitbox.PlayerEnteredTrigger.AddListener(OnPlayerEnteredTrigger);
            }
            moveTimer = moveTime;
        }

        private void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;
            Vector3 newPos = transform.position;

            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(collisionMask);

            if (waitTimer > 0f)
            {
                waitTimer -= deltaTime;
                if (waitTimer <= 0f)
                {
                    // Turn around after the wait timer is over
                    facingDirection = facingDirection == FacingDirection.Right ? FacingDirection.Left : FacingDirection.Right;
                    moveTimer = moveTime;
                }
            }

            if (moveTimer > 0f)
            {
                // If moving, do horizontal movement first, then grounded check to see if where we're going we would still be grounded
                // If it isn't, cancel the horizontal movement and change direction
                newPos.x += (float)facingDirection * moveSpeed * deltaTime;
                transform.position = newPos;
                moveTimer -= deltaTime;
                if(moveTimer <= 0f)
                {
                    waitTimer = turnAroundWaitTime;
                }
            }

            if (sprite != null)
            {
                sprite.flipX = facingDirection != FacingDirection.Left;
            }
        }

        public void OnPlayerEnteredTrigger(ObjectTriggerBase trigger, Movement player)
        {
            if (player.IsBall)
            {
                if (!player.Grounded)
                {
                    Vector2 newVelocity = player.Velocity;
                    // TODO: Move the rebound code to player movement script, or base enemy?
                    if (player.transform.position.y < transform.position.y || player.Velocity.y > 0f)
                    {
                        newVelocity.y -= 60f * Mathf.Sign(newVelocity.y);
                    }
                    else
                    {
                        newVelocity.y = -newVelocity.y;
                    }
                    player.Velocity = -player.Velocity;
                }
                TakeDamage(1);
            }
            else if (!player.IsInvulnerable)
            {
                player.SetHitState(transform.position);
            }
        }

        private void TakeDamage(int amount)
        {
            health -= amount;
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}