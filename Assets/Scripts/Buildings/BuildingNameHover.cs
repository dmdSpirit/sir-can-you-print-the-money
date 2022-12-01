#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using TMPro;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public class BuildingNameHover : InitializableBehaviour
    {
        private Building? _building;

        [SerializeField]
        private RectTransform _panel = null!;

        [SerializeField]
        private Vector3 _offset;

        [SerializeField]
        private TMP_Text _text = null!;

        public bool IsShowing { get; private set; }

        public void ShowName(Building building)
        {
            if (!IsShowing)
            {
                _panel.gameObject.SetActive(true);
                IsShowing = true;
            }

            _panel.position = Input.mousePosition + _offset;
            if (_building == null || _building != building)
            {
                _text.text = building.Title;
                _building = building;
            }
        }

        public void HidePanel()
        {
            _panel.gameObject.SetActive(false);
            _building = null;
            IsShowing = false;
        }
    }
}