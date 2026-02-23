using UnityEngine;

public class SelectionPulse : MonoBehaviour
{
    [SerializeField] float pulseSpeed = 2.5f;
    [SerializeField] float pulseAmount = 0.1f;

    Vector3 baseScale;
    private void Start()
    {
        baseScale = transform.localScale;
    }
    private void Update()
    {
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = baseScale * pulse;
    }


}
