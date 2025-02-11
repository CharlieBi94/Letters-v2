using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressDisplay : MonoBehaviour
{
    [SerializeField]
    private float value;
    [SerializeField]
    RectTransform progressBar;

    private float startingValue;
    private float targetValue;
    bool animate = false;
    float t = 0;
    // Start is called before the first frame update
    void Start()
    {
        value = Mathf.Clamp01(value);
        progressBar.localScale = new Vector3(value, progressBar.localScale.y, progressBar.localScale.z);
        SetValue(0);
    }

    private void Update()
    {
        if (animate)
        {
            value = Mathf.Lerp(startingValue, targetValue, t);
            progressBar.localScale = new Vector3( value,
                progressBar.localScale.y,
                progressBar.localScale.z);
            t += 0.7f * Time.deltaTime;
            if (t > 1.0f)
            {
                t = 0.0f;
                animate = false;
                return;
            }
        }
    }

    public void SetValue(float amount)
    {
        targetValue = Mathf.Clamp01(amount);
        startingValue = value;
        animate = true;
    }
}
