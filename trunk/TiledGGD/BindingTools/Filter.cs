using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TiledGGD.BindingTools
{
    class Filter : IFilter
    {

        #region Constructors
        /// <summary>
        /// Create an empty filter. For use in FilterSet only.
        /// </summary>
        protected Filter() { }
        /// <summary>
        /// Create a new Filter from an XmlNode
        /// </summary>
        /// <param name="filterNode">The XmlNode to create a Filter from</param>
        public Filter(XmlNode filterNode)
        {
            throw new Exception("The Constructtor Filter(XmlNode) has not yet been implemented");
        }
        #endregion

        #region Method: Passes(string)
        /// <summary>
        /// Checks if a file passes this filter
        /// </summary>
        /// <param name="filename">The filename (or path) of the file to check</param>
        /// <returns><code>true</code> iff the file passes this filter</returns>
        public bool Passes(string filename)
        {
            return false;
        }
        #endregion
    }
    public enum FilterType
    {
        /// <summary>
        /// A Filter that filters on the Magic Header of a file
        /// </summary>
        MAGIC,
        /// <summary>
        /// A Filter thar filters on the extension of a file
        /// </summary>
        FILEEXT
    }
}
