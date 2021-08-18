using System;
using FasterGames.UI.Components.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace FasterGames.UI.Components
{
    /// <summary>
    /// A game clock with an optional size
    /// </summary>
    public class GameClock : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<GameClock, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private UxmlIntAttributeDescription m_Size = new UxmlIntAttributeDescription
                {name = "size", defaultValue = 12};

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var size = m_Size.GetValueFromBag(bag, cc);

                GameClock v = (GameClock) ve;
                v.style.fontSize = new StyleLength(new Length(size, LengthUnit.Pixel));
            }
        }

        private readonly Label labelEl;

        private float lastResetTime = 0f;
        
        public GameClock()
        {
            AddToClassList("components-gameclock__container");
            style.alignItems = new StyleEnum<Align>(Align.Center);
            style.justifyContent = new StyleEnum<Justify>(Justify.Center);
            style.flexShrink = 1;
            style.flexGrow = 0;
            
            labelEl = new Label();
            labelEl.text = "00:00";
            labelEl.AddToClassList("components-gameclock__label");
            
            Add(labelEl);
            
            // the ctor isn't always called at an appropriate time, but the geometry event will be
            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        private void OnGeometryChanged(GeometryChangedEvent ev)
        {
            UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            
            // setup our invoke work
            if (Application.isPlaying)
            {
                Reset();
                this.Invoke(TimeSpan.FromSeconds(1), Update);
            }
        }

        public void Update()
        {
            var at = Application.isPlaying ? Time.timeSinceLevelLoad : 0;
            labelEl.text = TimeSpan.FromSeconds(at - lastResetTime).ToString("mm':'ss");
        }
        
        public void Reset()
        {
            lastResetTime = Application.isPlaying ? Time.timeSinceLevelLoad : 0;
        }
    }
}