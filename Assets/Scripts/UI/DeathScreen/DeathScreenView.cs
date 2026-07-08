using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DeathScreen
{
    public interface IDeathScreenView
    {
        event Action OnRestartClicked;
        public void SetVisible(bool visible);
    }

    public class DeathScreenView : MonoBehaviour, IDeathScreenView
    {
        [SerializeField] private GameObject root;
        
        [SerializeField] private Button restartButton;

        public event Action OnRestartClicked;

        private void Awake()
        {
            restartButton.onClick.AddListener(() => OnRestartClicked?.Invoke());
            gameObject.SetActive(false);
        }

        public void SetVisible(bool visible) => root.SetActive(visible);
    }
}