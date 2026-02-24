using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSheetUI : MonoBehaviour
{
    //External Data
    CharacterBrain brain;
    //Deriveratives
    CharacterSheet stats;

    [SerializeField] Image hungerBar;
    [SerializeField] Image energyBar;
    [SerializeField] Image selectionFrame;
    public TMP_Text protagonistName;

    bool protagonistSelected;
    public void Init(CharacterBrain brain)
    {
        this.brain = brain;

        stats = brain.stats;
        protagonistName.text = stats.name;

        protagonistSelected = brain.IsPlayerControlled;
        selectionFrame.enabled = protagonistSelected;
    }
    private void Update()
    {
        hungerBar.fillAmount = stats.Hunger;
        energyBar.fillAmount = stats.energy;
    }
    public void OnProtagonistClick()
    {
        protagonistSelected = !protagonistSelected;
        selectionFrame.enabled = protagonistSelected;

        brain.SwitchPlayerControl(protagonistSelected);

    }


}
