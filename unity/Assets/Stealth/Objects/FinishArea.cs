using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stealth.Objects
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class FinishArea : MonoBehaviour
    {
        public event Action PlayerEnteredGoal;
        public event Action PlayerExitedGoal;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerEnteredGoal?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerExitedGoal?.Invoke();
            }
        }
    }
}