using UnityEngine;

public class AnimateActions : MonoBehaviour // on ProtagonistPrefab; called by renderWorld
{
    public SpriteRenderer foodRaw;
    [SerializeField] GameObject progressBarPrefab;

    private void Awake()
    {
        progressBarPrefab = Instantiate(progressBarPrefab, transform);
    }

    public void SetEatingAnimation(bool active)
    {
        foodRaw.enabled = active;
    }

    void ActionProgressView()
    {
        
    }




}
