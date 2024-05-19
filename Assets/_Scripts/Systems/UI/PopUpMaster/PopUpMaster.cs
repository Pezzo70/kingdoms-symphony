using System;
using System.Collections;
using Assets.SimpleLocalization.Scripts;
using TMPro;
using UnityEngine;

namespace Kingdom.UI
{
    public class PopUpMaster : PersistentSingleton<PopUpMaster>
    {
        public GameObject popUpCanvas;
        public RectTransform popUp;
        public GameObject opacityPanel;
        public LocalizedTextMeshProUGUI popUpDescription;
        public GameObject closeButton;
        public GameObject yesButton;
        public GameObject noButton;
        public Animator animator;
        private Action callback = delegate { };

        private void OnEnable() => EventManager.ShowPopUp += ShowPopUpHandler;

        private void OnDisable() => EventManager.ShowPopUp = ShowPopUpHandler;

        private void ShowPopUpHandler(
            (
                string localizedKey,
                bool isError,
                bool needConfirmation,
                Action confirmationCallback,
                bool timed,
                float seconds,
                bool closable,
                Vector3 targetPosition,
                bool shouldDisplayOpacityPanel
            ) options
        )
        {
            if (popUpCanvas.activeInHierarchy)
                return;

            Camera mainCamera = Camera.main;

            TextMeshProUGUI tmp = popUpDescription.GetComponent<TextMeshProUGUI>();
            popUpDescription.LocalizationKey = options.localizedKey;

            if (options.shouldDisplayOpacityPanel)
                opacityPanel.SetActive(true);
            else
                opacityPanel.SetActive(false);

            if (options.isError)
                tmp.color = Color.red;
            else
                tmp.color = Color.white;

            if (options.needConfirmation)
            {
                callback = options.confirmationCallback;
                yesButton.SetActive(true);
                noButton.SetActive(true);
            }
            else
            {
                yesButton.SetActive(false);
                noButton.SetActive(false);
            }

            if (options.timed)
                StartCoroutine(WaitAndClose(options.seconds));

            if (options.closable)
                closeButton.SetActive(true);
            else
                closeButton.SetActive(false);

            Vector3 desiredPosition = mainCamera.WorldToScreenPoint(options.targetPosition);
            float xOffset = popUp.sizeDelta.x / 2;
            float yOffset = popUp.sizeDelta.y / 2;
            popUp.anchoredPosition = new Vector3(
                desiredPosition.x - xOffset,
                desiredPosition.y - yOffset,
                desiredPosition.z
            );

            popUpCanvas.SetActive(true);

            animator.SetBool("FadeIn", true);
        }

        public void InvokeCallback()
        {
            callback.Invoke();
            callback = delegate { };
            StartCoroutine(WaitAndClose(0f));
        }

        public void CleanCallbackOrClose()
        {
            callback = delegate { };
            StartCoroutine(WaitAndClose(0f));
        }

        private IEnumerator WaitAndClose(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            animator.SetBool("FadeIn", false);
            animator.SetBool("FadeOut", true);
            yield return new WaitForSeconds(1f);
            popUpCanvas.SetActive(false);
            animator.SetBool("FadeOut", false);
        }
    }
}
