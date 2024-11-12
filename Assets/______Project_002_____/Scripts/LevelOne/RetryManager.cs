using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project002
{
    public class RetryManager : MonoBehaviour
    {
        public GameObject retryScreenUI;
        public Image retryScreenBackground;

        private float time;
        private float fadeTime = 1f;

        public Button retryButton;
        public Button exitButton;

        private void Start()
        {
            retryButton.onClick.AddListener(() =>
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex)
            );
            exitButton.onClick.AddListener(() =>
                //SceneManager.LoadScene(0)
                SceneManager.LoadScene("MapChoosingScene")
            );
        }

        private void Update()
        {
            if (PlayerHealth.Instance.currentHealth <= 0f)
            {
                StartCoroutine(DeathScreen());
            }
        }

        private IEnumerator DeathScreen()
        {
            LevelOneTutorialCanvas.Instance.isOpen = true;
            retryScreenUI.SetActive(true);
            Color alpha = retryScreenBackground.color;
            while(alpha.a < 1f)
            {
                time += Time.deltaTime / fadeTime;
                alpha.a = Mathf.Lerp(0, 1, time);
                retryScreenBackground.color = alpha;
                yield return null;
            }

            yield return null;
        }

        public void PreeExitBTN()
        {
            SceneManager.LoadScene("MapChoosingScene");
        }

    }
}
