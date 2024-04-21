using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(ToggleGroup))]
public class LevelSelectionToggleGroup : MonoBehaviour
{
    public LevelSelectionToggle SelectedLevel
    {
        get
        {
            var activeToggles = toggleGroup.ActiveToggles();
            foreach (var toggle in activeToggles)
            {
                return toggle.GetComponent<LevelSelectionToggle>();
            }
            return null;
        }
    }
    private ToggleGroup toggleGroup;
    void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();
    }
}
