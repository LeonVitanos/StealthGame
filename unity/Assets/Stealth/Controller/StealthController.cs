using System;
using System.Collections;
using General.Menu;
using Stealth.Objects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util.Geometry.Polygon;

namespace Stealth.Controller
{
    /// <summary>
    /// Manages the overall flow of a single level of the Stealth game.
    /// </summary>
    public class StealthController : MonoBehaviour
    {   
        [SerializeField]
        private Text timeLabel;

        [SerializeField]
        private RectTransform gameOverLabel;

        [SerializeField]
        private float levelResetDelay = 1;
        
        [SerializeField]
        private ButtonContainer advanceButton;

        [SerializeField]
        private PlayerController playerController;

        [SerializeField]
        private string nextSceneName = "stealthVictory";

        private FinishArea finishArea;

        // store starting time of level
        private float puzzleStartTime;


        /// <summary>
        /// Initializes the level and starts gameplay.
        /// </summary>
        private void InitializeLevel()
        {
            advanceButton.Disable();
            gameOverLabel.gameObject.SetActive(false);

            CameraManager.UpdateVisionCameras();
            CameraManager.EnableAllCameras();

            playerController.enabled = true;
        }

        private void Awake()
        {
            finishArea = FindObjectOfType<FinishArea>();
        }

        void Start()
        {
            puzzleStartTime = Time.time;
            InitializeLevel();
        }

        private void OnEnable()
        {
            finishArea.PlayerEnteredGoal += OnPlayerEnteredGoal;
            finishArea.PlayerExitedGoal += OnPlayerExitedGoal;
        }

        private void OnDisable()
        {
            finishArea.PlayerEnteredGoal -= OnPlayerEnteredGoal;
            finishArea.PlayerExitedGoal -= OnPlayerExitedGoal;
        }

        private void OnPlayerEnteredGoal()
        {
            advanceButton.Enable();
        }

        private void OnPlayerExitedGoal()
        {
            advanceButton.Disable();
        }

        /// <summary>
        /// Checks whether the player is visible by a camera.
        /// </summary>
        private void Update()
        {
            UpdateTimeText();

            if (CameraManager.IsPointVisible(playerController.transform.position))
            {
                StartCoroutine(FailLevel());
            }
        }

        /// <summary>
        /// Advances to the next level.
        /// </summary>
        /// <remarks>
        /// This method should be registered with a button in the editor.
        /// </remarks>
        public void AdvanceLevel()
        {
            SceneManager.LoadScene(nextSceneName);
        }

        /// <summary>
        /// Resets the level. Sets m_deactivatedCameras to 0 to avoid softlocking, 
        /// re-enables all cameras, and clears the list of cameras.
        /// Then, it reloads the current scene.
        /// </summary>
        private IEnumerator FailLevel()
        {
            playerController.enabled = false;
            gameOverLabel.gameObject.SetActive(true);

            yield return new WaitForSeconds(levelResetDelay);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        /// <summary>
        /// Update the text field with the time taken for the current level
        /// </summary>
        private void UpdateTimeText()
        {
            timeLabel.text = $"Time: {(int)Mathf.Floor(Time.time - puzzleStartTime)}s";
        }
    }
}