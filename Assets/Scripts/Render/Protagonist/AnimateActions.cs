using UnityEngine;
public class AnimateActions : MonoBehaviour // on ProtagonistPrefab; called by renderWorld
{
    public CharacterBrain brain;

    public SpriteRenderer foodRaw;
    public SpriteRenderer axe;
    public SpriteRenderer hammer;

    [SerializeField] GameObject progressBarPrefab;
    GameObject progressBar;
    ActionProgressUI progressUI;

    bool isInitialised = false;

    IAction previous = null;

    public void Init(CharacterBrain brain)
    {
        this.brain = brain;
        isInitialised = true;
    }
    private void Awake()
    {
        progressBar = Instantiate(progressBarPrefab, transform);
        progressUI = progressBar.GetComponentInChildren<ActionProgressUI>();
    }
    private void Update()
    {
        if (!isInitialised)
            return;

        if (brain.actionRunner.currentAction == null || brain.actionRunner.currentAction.IsFinished)
        {
            progressUI.Hide();
            SetAnimationFor(previous, false);
            previous = null;
            return;
        }

        SetUIFor(brain.actionRunner.currentAction);
    }
    void SetUIFor(IAction current)
    {
        if (previous != current)
        {
            if (previous != null)
                SetAnimationFor(previous, false);

            SetAnimationFor(current, true);

            previous = current;
        }

        switch (current)
        {
            case Movement:
                progressUI.Hide();
                break;
            case CollectItem c:
                progressUI.SetProgress(c.progress);
                break;
            case HarvestAction h:
                progressUI.SetProgress(h.progress);
                break;
            case EatAction e:
                progressUI.SetProgress(e.progress);
                break;
            case BuildAction b:                
                progressUI.SetProgress(b.progress);
                break;
        }
    }
    void SetAnimationFor(IAction curent, bool active)
    {
        switch(curent)
        {
            case HarvestAction h:
                axe.enabled = active;
                break;
            case EatAction e:
                foodRaw.enabled = active;
                break;
            case BuildAction b:
                hammer.enabled = active;
                break;
        }
    }
}
