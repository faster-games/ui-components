using UnityEngine.UIElements;

namespace FasterGames.UI.Components
{
    /// <summary>
    /// A horizontal wrapper
    /// </summary>
    public class HBox : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<HBox, UxmlTraits> {}

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private UxmlBoolAttributeDescription m_Shrink = new UxmlBoolAttributeDescription
                {name = "shrink", defaultValue = false};

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var shrink = m_Shrink.GetValueFromBag(bag, cc);
                
                HBox v = (HBox) ve;
                v.style.flexGrow = shrink ? 0 : 1;
                v.style.flexShrink = shrink ? 1 : 0;
            }
        }
        
        public HBox()
        {
            style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
        }
    }
}