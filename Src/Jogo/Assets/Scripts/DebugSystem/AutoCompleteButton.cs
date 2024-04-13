using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoCompleteButton : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI textMeshPro;

    public string Text { get; private set; }
    public Button Button => button;

    private void Awake()
    {
        button = GetComponent<Button>();
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Initialize(string text, Action<string> onSelected)
    {
        textMeshPro.text = text;
        button.onClick.AddListener(() => onSelected?.Invoke(text));
    }
}
