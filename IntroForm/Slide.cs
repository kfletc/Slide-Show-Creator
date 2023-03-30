﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private SlideImage image;

        public SlideImage Image
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

        private TransitionType transition;

        public TransitionType Transition
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
        
        public Slide(SlideImage image, int slideDuration=3000, TransitionType transitionType=TransitionType.None, int transitionDuration=1000)
        {
            this.image = image;
            this.slideDuration = slideDuration;
            this.transition = transitionType;
            this.transitionDuration = transitionDuration;
        }
    }
}
