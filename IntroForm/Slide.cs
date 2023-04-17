using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IntroForm
{
    public class Slide
    {
        public enum TransitionType
        {
            None,
            WipeLeft,
            WipeRight,
            WipeUp,
            WipeDown,
            CrossFade
        }

        private SlideImage? image;

        public SlideImage? Image
        {
            get { return image; }
            set { image = value; }
        }

        private int slideDuration;

        public int SlideDuration
        {
            get { return slideDuration; }
            set { slideDuration = value; }
        }

        private TransitionType? transition;

        public TransitionType? Transition
        {
            get { return transition; }
            set { transition = value; }
        }

        private int transitionDuration;

        public int TransitionDuration
        {
            get { return transitionDuration; }
            set { transitionDuration = value; }
        }

        public Slide(SlideImage image)
        {
            Image = image;
            SlideDuration = 3000;
            Transition = TransitionType.None;
            TransitionDuration = 1000;
        }

        [JsonConstructor]
        public Slide() { }

        public Slide(SlideImage image, int slideDuration, TransitionType transition, int transitionDuration)
        {
            Image = image;
            SlideDuration = slideDuration;
            Transition = transition;
            TransitionDuration = transitionDuration;
        }
    }
}
