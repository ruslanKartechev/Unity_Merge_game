using UnityEngine;

namespace Game.Merging
{
    // [CreateAssetMenu(menuName = "SO/" + nameof(HunterSettingsProvider), fileName = nameof(HunterSettingsProvider), order = 0)]
    public abstract class HunterSettingsProvider<T> : ScriptableObject
    {
        [SerializeField] protected T _settings;
        public virtual T GetSettings() => _settings;
    }
}