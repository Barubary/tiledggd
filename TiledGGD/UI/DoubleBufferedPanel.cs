using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace TiledGGD
{
    class DoubleBufferedPanel:Panel
    {
        public DoubleBufferedPanel()
            : base()
        {
            this.DoubleBuffered = true;
        }
    }
}
