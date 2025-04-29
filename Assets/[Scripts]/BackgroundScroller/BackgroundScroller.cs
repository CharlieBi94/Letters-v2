using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class BackgroundScroller : MonoBehaviour
{
    RawImage img;
    [SerializeField]
    private float xMultiplier , yMultiplier;
    
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<RawImage>();
        
    }

    // Update is called once per frame
    void Update()
    {
        img.uvRect = new Rect(img.uvRect.position + new Vector2(1*xMultiplier, 1*yMultiplier) * Time.deltaTime, img.uvRect.size);
    }
}
