using System;
using System.Collections.Generic;
using System.Text;

namespace TiledGGD.BindingTools
{
    interface IFilter
    {
        bool Passes(String filename);
    }
}
