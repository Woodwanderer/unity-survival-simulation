using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSheetUI : MonoBehaviour
{
    //External Data
    PlayerCommandRouter pCR;
    CharacterBrain brain;
    //Deriveratives
    CharacterSheet stats;

    [SerializeField] Image hungerBar;
    [SerializeField] Image energyBar;
    [SerializeField] Image selectionFrame;
    public TMP_Text protagonistName;

    bool protagonistSelected;
    public void Init(PlayerCommandRouter pCR, CharacterBrain brain) // called by GameState after pCR
    {
        this.pCR = pCR;
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

        if (protagonistSelected)
            pCR.Bind(brain);
        else
            pCR.UnBind();
    }
}
