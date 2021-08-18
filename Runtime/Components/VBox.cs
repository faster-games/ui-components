using UnityEngine.UIElements;

namespace FasterGames.UI.Components
{
    /// <summary>
    /// A vertical wrapper
    /// </summary>
    public class VBox : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<VBox, UxmlTraits> {}

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private UxmlBoolAttributeDescription m_Shrink = new UxmlBoolAttributeDescription
                {name = "shrink", defaultValue = false};

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var shrink = m_Shrink.GetValueFromBag(bag, cc);
                
                VBox v = (VBox) ve;
                v.style.flexGrow = shrink ? 0 : 1;
                v.style.flexShrink = shrink ? 1 : 0;
            }
        }

        public VBox()
        {
            style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
        }
    }
}