using UnityEngine;
using UnityEngine.UIElements;

namespace FasterGames.UI.Components
{
    /// <summary>
    /// A circle element, with an optional label
    /// </summary>
    public class Circle : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<Circle, UxmlTraits> {}

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private UxmlIntAttributeDescription m_Radius = new UxmlIntAttributeDescription
            {
                name = "radius", defaultValue = 2
            };
            
            private UxmlStringAttributeDescription m_Label = new UxmlStringAttributeDescription
            {
                name = "label", defaultValue = ""
            };

            private UxmlIntAttributeDescription m_Size = new UxmlIntAttributeDescription
            {
                name = "size", defaultValue = 12
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var radius = m_Radius.GetValueFromBag(bag, cc);
                var label = m_Label.GetValueFromBag(bag, cc);
                var size = m_Size.GetValueFromBag(bag, cc);

                Circle v = (Circle) ve;

                if (!string.IsNullOrWhiteSpace(label))
                {
                    v.labelEl.text = label;
                    v.labelEl.style.fontSize = size;
                    v.labelEl.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                }
                
                v.style.borderTopLeftRadius = radius;
                v.style.borderTopRightRadius = radius;
                v.style.borderBottomLeftRadius = radius;
                v.style.borderBottomRightRadius = radius;
                v.style.width = radius * 2;
                v.style.height = radius * 2;
                v.style.minWidth = radius * 2;
                v.style.minHeight = radius * 2;
            }
        }

        private readonly Label labelEl;
        
        public Circle()
        {
            AddToClassList("components-circle__container");
            
            labelEl = new Label();
            labelEl.AddToClassList("components-circle__label");
            labelEl.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            labelEl.style.alignItems = new StyleEnum<Align>(Align.Center);
            labelEl.style.justifyContent = new StyleEnum<Justify>(Justify.Center);
            labelEl.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter);
            labelEl.style.flexShrink = 0;
            labelEl.style.flexGrow = 1;
            Add(labelEl);

            style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            style.flexGrow = 0;
            style.flexShrink = 1;
            style.overflow = new StyleEnum<Overflow>(Overflow.Hidden);
        }
    }
}