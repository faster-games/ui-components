using UnityEngine;
using UnityEngine.UIElements;

namespace FasterGames.UI.Components.Bindings
{
    /// <summary>
    /// A simple demo binding to show how to bind engine values to a <see cref="Bar"/>
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class BarBinding : MonoBehaviour
    {
        public string barName;
        public float value = 10f;
        public float minValue = 0f;
        public float maxValue = 100f;

        private VisualElement m_Root;
        private Bar m_Bar;
        
        private void Awake()
        {
            m_Root = GetComponent<UIDocument>().rootVisualElement;
            m_Bar = m_Root.Q<Bar>(barName);
        }

        private void Update()
        {
            m_Bar.value = value;
            m_Bar.lowValue = minValue;
            m_Bar.highValue = maxValue;
        }
    }
}