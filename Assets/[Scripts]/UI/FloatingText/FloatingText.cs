using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI tmp;
    [SerializeField]
    AnimationCurve movementCurve;
    [SerializeField]
    AnimationCurve fadeCurve;
    [SerializeField]
    RectTransform textRect;
    [SerializeField]
    float yOffset;
    RectTransform rect;
    Coroutine animationCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        tmp.gameObject.SetActive(false);
        rect = GetComponent<RectTransform>();
    }
    public void PlayText(string text, Color textColor, Vector3 position)
    {
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        tmp.text = text;
        tmp.color = textColor;
        animationCoroutine = StartCoroutine(FloatAtPos(position));
    }

    private IEnumerator FloatAtPos(Vector3 initialPos)
    {
        rect.localPosition = initialPos;
        textRect.localPosition = Vector3.zero;
        tmp.gameObject.SetActive(true);
        Color color = tmp.color;
        float elpasedTime = 0f;
        while (elpasedTime <= 1f)
        {
            float deltaY = movementCurve.Evaluate(elpasedTime) * 10;
            float alpha = fadeCurve.Evaluate(elpasedTime);
            //Debug.Log($"Movement delta: {deltaY} | Alpha: {alpha}");
            textRect.localPosition = new Vector3(0, deltaY + yOffset, 0);
            tmp.color = new Color(color.r, color.g, color.b, alpha);
            elpasedTime += Time.deltaTime;
            yield return null;
        }
        tmp.gameObject.SetActive(false);
    }


}
