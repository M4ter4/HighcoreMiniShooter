using TMPro;
using UnityEngine;

namespace UI.HUD
{
    public interface IHudView
    {
        public void SetHealth(int current, int max);
        public void SetWeaponName(string name);
        public void SetAmmo(int current, int magazineSize);
        public void SetReloading(bool isReloading);
        public void SetKillCount(int count);
        public void SetVisible(bool visible);
    }

    public class HudView : MonoBehaviour, IHudView
    {
        [SerializeField] private GameObject root;
        
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text weaponNameText;
        [SerializeField] private TMP_Text ammoText;
        [SerializeField] private GameObject reloadingIndicator;
        [SerializeField] private TMP_Text killCountText;

        public void SetHealth(int current, int max) => healthText.text = $"HP: {current}/{max}";
        public void SetWeaponName(string name) => weaponNameText.text = $"Current weapon: {name}";
        public void SetAmmo(int current, int magazineSize) => ammoText.text = $"Ammo: {current}/{magazineSize}";
        public void SetReloading(bool isReloading) => reloadingIndicator.SetActive(isReloading);
        public void SetKillCount(int count) => killCountText.text = $"Kill count: {count.ToString()}";
        public void SetVisible(bool visible) => root.SetActive(visible);
    }
}