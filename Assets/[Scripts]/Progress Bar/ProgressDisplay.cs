using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressDisplay : MonoBehaviour
{
    [SerializeField]
    private float value;
    [SerializeField]
    RectTransform progressBar;

    Queue<float> progressQueue = new();
    private float startingValue;
    private float targetValue;
    bool animate = false;
    float t = 0;
    // Start is called before the first frame update
    void Start()
    {
        value = Mathf.Clamp01(value);
        progressBar.localScale = new Vector3(value, progressBar.localScale.y, progressBar.localScale.z);
    }

    private void Update()
    {
        if (animate)
        {
            value = Mathf.Lerp(startingValue, targetValue, t);
            progressBar.localScale = new Vector3( value,
                progressBar.localScale.y,
                progressBar.localScale.z);
            t += 1.2f * Time.deltaTime;
            if (t > 1.0f)
            {
                t = 0.0f;
                
                value = targetValue;
                progressBar.localScale = new Vector3(value,
                progressBar.localScale.y,
                progressBar.localScale.z);
                if (progressQueue.Count > 0)
                {
                    startingValue = value;
                    targetValue = progressQueue.Dequeue();
                }
                else
                {
                    animate = false;
                }
                return;
            }
        }
    }

    public void SetValue(float amount)
    {
        if(animate == true)
        {
            progressQueue.Enqueue(amount);
        }
        else
        {
            targetValue = Mathf.Clamp01(amount);
            startingValue = value;
            animate = true;
        }
        
    }

    public float GetValue()
    {
        return value;
    }
}
