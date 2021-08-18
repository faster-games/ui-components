using UnityEngine;
using UnityEngine.UIElements;

namespace FasterGames.UI.Components
{
    /// <summary>
    /// An image holder
    /// </summary>
    /// <remarks>
    /// To set the image, use uss: <u:Image style="background-image: url(&apos;/Assets/logo.png&apos;);"/>
    /// </remarks>
    public class Image : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<Image, UxmlTraits> {}
        
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private UxmlIntAttributeDescription m_WidthPx = new UxmlIntAttributeDescription {name = "width", defaultValue = 10};
            private UxmlIntAttributeDescription m_HeightPx = new UxmlIntAttributeDescription {name = "height", defaultValue = 10};
            
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var width = m_WidthPx.GetValueFromBag(bag, cc);
                var height = m_HeightPx.GetValueFromBag(bag, cc);

                if (width == m_WidthPx.defaultValue && height != m_HeightPx.defaultValue)
                {
                    width = height;
                }
                
                if (height == m_HeightPx.defaultValue && width != m_WidthPx.defaultValue)
                {
                    height = width;
                }
                
                ((Image) ve).style.width =
                    new StyleLength(new Length(width, LengthUnit.Pixel));

                ((Image) ve).style.height =
                    new StyleLength(new Length(height, LengthUnit.Pixel));
            }
        }

        public Image()
        {
            style.unityBackgroundScaleMode = new StyleEnum<ScaleMode>(ScaleMode.ScaleAndCrop);
        }
    }
}