using System;
using System.Collections.Generic;
using UnityEngine;

public class AutoCompleteOptions : MonoBehaviour
{
    [SerializeField] private AutoCompleteButton optionButtonPrefab;
    [SerializeField] private RectTransform contentArea;
    [Min(1)]
    [SerializeField] private int maxContentShowButtonSize = 10;

    private RectTransform rectTransform;
    private readonly List<AutoCompleteButton> currentButtons = new List<AutoCompleteButton>();
    public event Action<string> OnOptionSelected;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        OnOptionSelected += x => Hide();
    }

    public void ShowOptions(List<string> options)
    {
        gameObject.SetActive(true);
        foreach (AutoCompleteButton button in currentButtons)
        {
            Destroy(button.gameObject);
        }
        currentButtons.Clear();

        foreach (string option in options)
        {
            AutoCompleteButton button = Instantiate(optionButtonPrefab, contentArea);
            button.Initialize(option, OnOptionSelected);
            currentButtons.Add(button);
        }
        RecalculateSize();
    }

    private void RecalculateSize()
    {
        float ySize = contentArea.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, ySize * Mathf.Min(currentButtons.Count, maxContentShowButtonSize));
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}