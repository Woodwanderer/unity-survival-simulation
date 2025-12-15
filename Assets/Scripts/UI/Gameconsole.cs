using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameConsole : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI consoleText;
    public TMP_InputField consoleInput;
    public ScrollRect scrollRect;

    private void Start()
    {
        Log("Welcome to the wild world of your imagination.");
        EventBus.OnLogMessage += Log;


        consoleInput.onSubmit.AddListener(HandleCommand); //.onSubmit -> odpala się po nacisnieciu enter
    }

    public void Log(string message) //wpisuje string w konsoli
    {
        consoleText.text += "\n" + message;
        StartCoroutine(ScrollToBottomNextFrame());
    }

    private void HandleCommand(string cmd) //sczytuje komende z input np. HandleCommand("zbieram")
    {
        cmd = cmd.ToLower().Trim(); //czyści wpisany string

        if (cmd == "") return;

        Log("> " + cmd);

        switch (cmd)
        {
            case "zbieram":
            case "zbierz":
            case "collect":
                Log("Zbierasz trochę dzikiego drewna...");
                break;

            case "szukam":
            case "eksploruj":
                Log("Rozglądasz się po okolicy...");
                break;

            case "odpoczywam":
            case "rest":
                Log("Oddychasz głęboko. Energia wraca...");
                break;

            case "patrzę":
            case "look":
                Log("Widzisz ślady zwierząt i szumiącą trawę.");
                break;

            default:
                Log("Nie znam komendy: " + cmd);
                break;
        }

        // Czyści pole
        consoleInput.text = "";
        consoleInput.ActivateInputField(); //focus wraca do pola pisania
    }

    // AUTOSCROLL
    private IEnumerator ScrollToBottomNextFrame()
    {
        yield return null; // czekamy jeden frame, yield pauza, zwraca wtedy wartosc null
        // Wymusza aktualizację layoutu UI
        Canvas.ForceUpdateCanvases();
        // Ustawia scroll na sam dół
        scrollRect.verticalNormalizedPosition = 0f;
    }


}
