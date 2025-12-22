using UnityEngine;
using UnityEngine.UI;

public class CharacterSheetUI : MonoBehaviour
{
    public Image conditionBar;
    public CharacterSheet dataState;

    public void Init(CharacterSheet stats)
    {
       dataState = stats;
    }

    private void Awake()
    {
        //gameObject.SetActive(false);        

    }
    private void Update()
    {
        conditionBar.fillAmount = dataState.Hunger;
    }


}
