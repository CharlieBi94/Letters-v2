using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorController : MonoBehaviour
{
    [SerializeField]
    private GameObject indicatorPrefab;
    [SerializeField]
    CarouselController controller;
    private List<Indicator> indicators = new();

    public void CreateIndicators(int amount, int currentIndex)
    {
        if (indicators.Count > 0) return;
        for(int i = 0; i < amount; i++)
        {
            var obj = GameObject.Instantiate(indicatorPrefab, transform);
            Indicator indicator = obj.GetComponent<Indicator>();
            indicators.Add(indicator);
            if(i == currentIndex)
            {
                indicator.SetSelctedState(true);
            }
        }
        controller.IndexChanged += OnIndexChanged;
    }

    private void OnIndexChanged(int newIndex)
    {
        for(int i = 0; i < indicators.Count; i++)
        {
            if(i == newIndex)
            {
                indicators[i].SetSelctedState(true);
            }
            else
            {
                indicators[i].SetSelctedState(false);
            }
        }
    }
}
