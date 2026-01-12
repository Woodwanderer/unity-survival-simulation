using UnityEngine;

public class AnimateActions : MonoBehaviour // on ProtagonistPrefab; called by renderWorld
{
    public CharacterActions actions;
    public SpriteRenderer foodRaw;
    [SerializeField] GameObject progressBarPrefab;
    GameObject progressBar;
    ActionProgressUI progressUI;

    bool isInitialised = false;

    public void Init(CharacterActions actions)
    {
        this.actions = actions;
        isInitialised = true;
    }

    private void Awake()
    {
        progressBar = Instantiate(progressBarPrefab, transform);
        progressUI = progressBar.GetComponentInChildren<ActionProgressUI>();
    }

    public void SetEatingAnimation(bool active)
    {
        foodRaw.enabled = active;
    }

    private void Update()
    {
        if (!isInitialised)
            return;

        if (actions.currentAction == null)
        {
            progressUI.Hide();
            SetEatingAnimation(false);
            return;
        }
            
        bool isEating = actions.currentAction is EatAction;
        SetEatingAnimation(isEating);

        SetMiniBar();
    }

    void SetMiniBar()
    {
        if(actions.currentAction.IsFinished)
        {
            progressUI.Hide();
            return;
        }
        if (actions.currentAction is Movement m)
        {
            progressUI.Hide();
            return;
        }
        if (actions.currentAction is HarvestAction h)
        {
            progressUI.SetProgress(h.progress);
        }
        if (actions.currentAction is CollectItem c)
        {
            progressUI.SetProgress(c.progress);
        }
        if (actions.currentAction is EatAction e)
        {
            progressUI.SetProgress(e.progress);
        }
        if (actions.currentAction is BuildAction b)
        {
            progressUI.SetProgress(b.progress);
        }
    }

}
