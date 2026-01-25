using UnityEngine;
using UnityEngine.UI;

public class CharacterSheetUI : MonoBehaviour
{
    [SerializeField] Image hungerBar;
    [SerializeField] Image energyBar;
    public CharacterSheet dataState;

    public void Init(CharacterSheet stats)
    {
       dataState = stats;
    }
    private void Update()
    {
        hungerBar.fillAmount = dataState.Hunger;
        energyBar.fillAmount = dataState.energy;
    }


}
