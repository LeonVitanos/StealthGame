using UnityEngine;

namespace Stealth.Objects
{
    /// <summary>
    /// Manages player input and moves player.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// The movement speed of the player.
        /// </summary>
        [SerializeField]
        private float moveSpeed = 10f;

        private Rigidbody2D body;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.gravityScale = 0;
        }

        private void FixedUpdate()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            body.MovePosition(body.position + new Vector2(horizontalInput, verticalInput) * moveSpeed * Time.deltaTime);
        }
    }
}
