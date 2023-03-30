using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroForm
{
    public class SlideShow
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public SlideShow(string inName)
        {
            this.name = inName;
        }
    }
}
