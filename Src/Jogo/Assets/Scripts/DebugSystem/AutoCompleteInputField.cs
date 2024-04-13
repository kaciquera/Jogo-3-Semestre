using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.DebugSystem
{
    public class AutoCompleteInputField : TMP_InputField
    {
        [SerializeField] private AutoCompleteOptions dropdown;
        [SerializeField] private Button sendButton;

        private List<string> autoCompleteOptions;

        protected override void Start()
        {
            base.Start();
            dropdown.OnOptionSelected += OnOptionSelected;
        }

        private void Update()
        {
            if (!isFocused) return;
            if (Input.GetKeyDown(KeyCode.Return))
            {
                sendButton.onClick?.Invoke();
            }
        }

        public void SetAutoCompleteOptions(List<string> options)
        {
            autoCompleteOptions = options;
        }

        public void OnValueChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                dropdown.Hide();
            }
            else
            {
                List<string> options = GetValidOptions(value);
                if (options.Count == 0)
                {
                    dropdown.Hide();
                }
                else
                {
                    dropdown.ShowOptions(options);
                }
            }
        }

        private List<string> GetValidOptions(string value)
        {
            List<string> options = autoCompleteOptions
                .Where(x => x.StartsWith(value, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x)
                .ToList();

            return options;
        }

        private void OnOptionSelected(string text)
        {
            SetTextWithoutNotify(text);
            ActivateInputField();
            caretPosition = text.Length;
        }
    }
}