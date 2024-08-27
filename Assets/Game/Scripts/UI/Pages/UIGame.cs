using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Watermelon.LevelSystem;
using Watermelon.SquadShooter;

namespace Watermelon
{
    public class UIGame : UIPage
    {
        [SerializeField] Joystick joystick;
        [SerializeField] RectTransform floatingTextHolder;

        [Space]
        [SerializeField] TextMeshProUGUI areaText;

        [Space]
        [SerializeField] Transform roomsHolder;
        [SerializeField] GameObject roomIndicatorUIPrefab;

        [Space]
        [SerializeField] Image fadeImage;
        [SerializeField] TextMeshProUGUI coinsText;

        [Space]
        [SerializeField] BlackFadeBehavior blackFade;
        public BlackFadeBehavior BlackFade => blackFade;

        [Header("Pause Panel")]
        [SerializeField] Button pauseButton;

        [Space]
        [SerializeField] GameObject pausePanelObject;
        [SerializeField] CanvasGroup pausePanelCanvasGroup;
        [SerializeField] Button pauseResumeButton;
        [SerializeField] Button pauseExitButton;

        public Joystick Joystick => joystick;
        public RectTransform FloatingTextHolder => floatingTextHolder;

        private List<UIRoomIndicator> roomIndicators = new List<UIRoomIndicator>();
        private PoolGeneric<UIRoomIndicator> roomIndicatorsPool;

        private void Awake()
        {
            roomIndicatorsPool = new PoolGeneric<UIRoomIndicator>(new PoolSettings(roomIndicatorUIPrefab.name, roomIndicatorUIPrefab, 3, true, roomsHolder));

            pauseButton.onClick.AddListener(OnPauseButtonClicked);
            pauseExitButton.onClick.AddListener(OnPauseExitButtonClicked);
            pauseResumeButton.onClick.AddListener(OnPauseResumeButtonClicked);
        }

        public void FadeAnimation(float time, float startAlpha, float targetAlpha, Ease.Type easing, SimpleCallback callback, bool disableOnComplete = false)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = fadeImage.color.SetAlpha(startAlpha);
            fadeImage.DOFade(targetAlpha, time).SetEasing(easing).OnComplete(delegate
            {
                callback?.Invoke();

                if (disableOnComplete)
                    fadeImage.gameObject.SetActive(false);
            });
        }

        public override void Initialise()
        {
            joystick.Initialise(UIController.MainCanvas);
        }

        public override void PlayHideAnimation()
        {
            OverlayUI.HideOverlay();

            UIController.OnPageClosed(this);
        }

        public override void PlayShowAnimation()
        {
            OverlayUI.HideOverlay();

            UIController.OnPageOpened(this);

            blackFade.SlideReveal();

            UIMainMenu.DotsBackground.gameObject.SetActive(false);
        }

        public void InitRoomsUI(RoomData[] rooms)
        {
            roomIndicatorsPool.ReturnToPoolEverything();
            roomIndicators.Clear();

            for (int i = 0; i < rooms.Length; i++)
            {
                roomIndicators.Add(roomIndicatorsPool.GetPooledComponent());
                roomIndicators[i].Init();

                if (i == 0)
                    roomIndicators[i].SetAsReached();
            }

            areaText.text = LevelController.GetCurrentAreaText();
        }

        public void UpdateReachedRoomUI(int roomReachedIndex)
        {
            roomIndicators[roomReachedIndex % roomIndicators.Count].SetAsReached();
        }

        public void UpdateCoinsText(int newAmount)
        {
            coinsText.text = CurrenciesHelper.Format(newAmount);
        }

        #region Pause
        private void OnPauseResumeButtonClicked()
        {
            Time.timeScale = 1.0f;

            pausePanelCanvasGroup.alpha = 0.0f;
            pausePanelCanvasGroup.DOFade(0.0f, 0.3f, unscaledTime: true).OnComplete(() =>
            {
                pausePanelObject.SetActive(false);
            });
        }

        private void OnPauseExitButtonClicked()
        {
            Time.timeScale = 1.0f;

            UIController.HidePage<UIGame>();

            ItemDropBehaviour[] dropItems = FindObjectsByType<ItemDropBehaviour>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
            for (int i = 0; i < dropItems.Length; i++)
            {
                dropItems[i].ItemDisable();
            }

            BlackFade.SlideHide(false, onComplete: () =>
            {
                pausePanelObject.SetActive(false);

                CustomMusicController.ToggleMusic(AudioController.Music.menuMusic, 0.3f, 0.3f);

                LevelController.UnloadLevel();
                CameraController.EnableCamera(CameraType.Menu);

                UIController.ShowPage<UIMainMenu>();

                LevelController.LoadCurrentLevel();

                BlackFade.SlideReveal();
            });
        }

        private void OnPauseButtonClicked()
        {
            Time.timeScale = 0.0f;

            pausePanelObject.SetActive(true);
            pausePanelCanvasGroup.alpha = 0.0f;
            pausePanelCanvasGroup.DOFade(1.0f, 0.3f, unscaledTime: true);

        }
        #endregion
    }
}