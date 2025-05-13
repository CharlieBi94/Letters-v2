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
    RectTransform rect;
    Coroutine animationCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        tmp.gameObject.SetActive(false);
        rect = GetComponent<RectTransform>();
        
    }
    private void Update()
    {
        // For testing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayText("words", Color.green, Vector3.zero);
        }
    }
    public void PlayText(string text, Color textColor, Vector3 position)
    {
        tmp.text = text;
        tmp.color = textColor;
        animationCoroutine = StartCoroutine(FloatAtPos(position));
    }

    private IEnumerator FloatAtPos(Vector3 initialPos)
    {
        tmp.gameObject.transform.position = initialPos;
        tmp.gameObject.SetActive(true);
        Color color = tmp.color;
        float elpasedTime = 0f;
        while (elpasedTime <= 1f)
        {
            float deltaY = movementCurve.Evaluate(elpasedTime) * 10;
            float alpha = fadeCurve.Evaluate(elpasedTime);
            Debug.Log($"Movement delta: {deltaY} | Alpha: {alpha}");
            rect.localPosition = new Vector3(initialPos.x, initialPos.y + deltaY, initialPos.z);
            tmp.color = new Color(color.r, color.g, color.b, alpha);
            elpasedTime += Time.deltaTime;
            yield return null;
        }
        tmp.gameObject.SetActive(false);
    }


}
