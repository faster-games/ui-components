using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FasterGames.UI.Components
{
    /// <summary>
    /// A horizontal progress bar, ported from <see cref="AbstractProgressBar"/>
    /// </summary>
    public class Bar : BindableElement, INotifyValueChanged<float>
    {
        public new class UxmlFactory : UxmlFactory<Bar, UxmlTraits> {}
        
        /// <summary>
        /// USS Class Name used to style the <see cref="Bar"/>.
        /// </summary>
        public static readonly string ussClassName = "components-bar";
        /// <summary>
        /// USS Class Name used to style the container of the <see cref="Bar"/>.
        /// </summary>
        public static readonly string containerUssClassName = ussClassName + "__container";
        /// <summary>
        /// USS Class Name used to style the progress bar of the <see cref="Bar"/>.
        /// </summary>
        public static readonly string progressUssClassName = ussClassName + "__progress";
        /// <summary>
        /// USS Class Name used to style the background of the <see cref="Bar"/>.
        /// </summary>
        public static readonly string backgroundUssClassName = ussClassName + "__background";

        /// <undoc/>
        public new class UxmlTraits : BindableElement.UxmlTraits
        {
            UxmlFloatAttributeDescription m_LowValue = new UxmlFloatAttributeDescription { name = "low-value", defaultValue = 0 };
            UxmlFloatAttributeDescription m_Value = new UxmlFloatAttributeDescription { name = "value", defaultValue = 0 };
            UxmlFloatAttributeDescription m_HighValue = new UxmlFloatAttributeDescription { name = "high-value", defaultValue = 100 };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var bar = ve as Bar;
                bar.lowValue = m_LowValue.GetValueFromBag(bag, cc);
                bar.value = m_Value.GetValueFromBag(bag, cc);
                bar.highValue = m_HighValue.GetValueFromBag(bag, cc);
            }
        }

        readonly VisualElement m_Background;
        readonly VisualElement m_Progress;

        public float lowValue { get; set; }

        public float highValue { get; set; } = 100f;

        /// <undoc/>
        public Bar()
        {
            AddToClassList(ussClassName);
            style.flexGrow = 1;
            style.flexShrink = 0;

            var container = new VisualElement() { name = ussClassName };
            container.style.flexGrow = 1;
            container.style.flexShrink = 0;

            m_Background = new VisualElement();
            m_Background.AddToClassList(backgroundUssClassName);
            m_Background.style.flexGrow = 1;
            m_Background.style.flexShrink = 0;
            container.Add(m_Background);

            m_Progress = new VisualElement();
            m_Progress.AddToClassList(progressUssClassName);
            m_Progress.style.flexGrow = 1;
            m_Progress.style.flexShrink = 0;
            m_Background.Add(m_Progress);

            container.AddToClassList(containerUssClassName);
            hierarchy.Add(container);

            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        void OnGeometryChanged(GeometryChangedEvent e)
        {
            SetProgress(value);
        }

        float m_Value;

        /// <summary>
        /// Sets the progress value. If the value has changed, dispatches an <see cref="ChangeEvent{T}"/> of type float.
        /// </summary>
        public virtual float value
        {
            get { return m_Value; }
            set
            {
                if (!EqualityComparer<float>.Default.Equals(m_Value, value))
                {
                    if (panel != null)
                    {
                        using (ChangeEvent<float> evt = ChangeEvent<float>.GetPooled(m_Value, value))
                        {
                            evt.target = this;
                            SetValueWithoutNotify(value);
                            SendEvent(evt);
                        }
                    }
                    else
                    {
                        SetValueWithoutNotify(value);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the progress value.
        /// </summary>
        /// <param name="newValue"></param>
        public void SetValueWithoutNotify(float newValue)
        {
            m_Value = newValue;
            SetProgress(value);
        }

        void SetProgress(float p)
        {
            float right;
            if (p < lowValue)
            {
                right = lowValue;
            }
            else if (p > highValue)
            {
                right = highValue;
            }
            else
            {
                right = p;
            }

            right = CalculateProgressWidth(right);
            if (right >= 0)
            {
                m_Progress.style.right = right;
            }
        }

        const float k_MinVisibleProgress = 0.0f;

        float CalculateProgressWidth(float width)
        {
            if (m_Background == null || m_Progress == null)
            {
                return 0f;
            }

            if (float.IsNaN(m_Background.layout.width))
            {
                return 0f;
            }

            var maxWidth = m_Background.layout.width;
            return maxWidth - Mathf.Max((maxWidth) * width / highValue, k_MinVisibleProgress);
        }
    }
}
