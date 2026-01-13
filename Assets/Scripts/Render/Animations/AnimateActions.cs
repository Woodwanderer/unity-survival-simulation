using UnityEngine;

public class AnimateActions : MonoBehaviour // on ProtagonistPrefab; called by renderWorld
{
    public CharacterActions actions;
    public SpriteRenderer foodRaw;
    public SpriteRenderer axe;
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
    void SetHarvestAnimation(bool active)
    {
        axe.enabled = active;
    }

    private void Update()
    {
        if (!isInitialised)
            return;
        if (actions.currentAction == null) 
        {
            SetEatingAnimation(false);
            SetHarvestAnimation(false);
            progressUI.Hide();
            return;
        }
        SetAnimation();

        SetMiniBar();
    }

    void SetAnimation()
    {

        bool isHarvesting = actions.currentAction is HarvestAction;
        if (isHarvesting)
        {
            SetHarvestAnimation(isHarvesting);
            return;
        }
        
        bool isEating = actions.currentAction is EatAction;
        if (isEating)
        {
            SetEatingAnimation(isEating);
            return;
        }
    }
 

    void SetMiniBar()
    {
        if(actions.currentAction.IsFinished || actions.currentAction == null)
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
            return;
        }
        if (actions.currentAction is CollectItem c)
        {
            progressUI.SetProgress(c.progress);
            return;
        }
        if (actions.currentAction is EatAction e)
        {
            progressUI.SetProgress(e.progress);
            return;
        }
        if (actions.currentAction is BuildAction b)
        {
            progressUI.SetProgress(b.progress);
            return;
        }
    }

}
