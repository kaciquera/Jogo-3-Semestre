using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.DebugSystem
{
    public class HandLog : MonoBehaviour
    {
        [Header("Commands")]
        [SerializeField] private LogCommand[] commands;

        [Header("Settings")]
        [Min(1)]
        [SerializeField] private int debugLimit = 10;

        [Header("Colors")]
        [SerializeField] protected Color manualColor = Color.white;
        [SerializeField] protected Color callbackColor = Color.white;
        [SerializeField] protected Color logColor = Color.white;
        [SerializeField] protected Color warningColor = Color.yellow;
        [SerializeField] protected Color errorColor = Color.red;

        [Header("References")]
        [SerializeField] private TextMeshProUGUI debugTextPrefab;
        [SerializeField] private AutoCompleteInputField inputField;
        [SerializeField] private Transform content;

        private readonly Queue<GameObject> debugTexts = new Queue<GameObject>();

        private void Awake()
        {
            List<string> options = new List<string>();

            foreach (LogCommand command in commands)
            {
                options.Add(command.Text);
            }

            inputField.SetAutoCompleteOptions(options);
        }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        public void OnCommandButtonClick()
        {
            if (string.IsNullOrEmpty(inputField.text)) return;
            AddCommand(inputField.text);
            inputField.text = string.Empty;
        }

        public void AddCommand(string message)
        {
            AddText(message, manualColor);

            foreach (LogCommand command in commands)
            {
                if (command.ValidateWithCallback(message) && command.HasCallbackText)
                {
                    AddText(command.CallbackText, callbackColor);
                }
            }
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            Color color = GetLogColor(type);

            switch (type)
            {
                case LogType.Error:
                case LogType.Exception:
                case LogType.Assert:
                    logString = $"Error: {logString}";
                    break;
                case LogType.Warning:
                    logString = $"Warning: {logString}";
                    break;
                case LogType.Log:
                    logString = $"Debug: {logString}";
                    break;
            }

            AddText($"{logString}", color);
        }

        private void AddText(string message, Color color)
        {
            if (debugTexts.Count > debugLimit)
            {
                GameObject oldText = debugTexts.Dequeue();
                oldText.SetActive(false);
            }

            TextMeshProUGUI newText = Instantiate(debugTextPrefab, content);
            newText.text = $"[{DateTime.Now:HH:mm:ss}] {message}";
            newText.color = color;
            debugTexts.Enqueue(newText.gameObject);
        }

        private Color GetLogColor(LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                case LogType.Exception:
                case LogType.Assert:
                    return errorColor;
                case LogType.Warning:
                    return warningColor;
                case LogType.Log:
                    return logColor;
            }
            return Color.white;
        }
    }
}