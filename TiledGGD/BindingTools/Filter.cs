using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

namespace TiledGGD.BindingTools
{
    class Filter : IFilter
    {
        #region Fields & Properties

        #region Value(s)
        /// <summary>
        /// The value of this filter, if this filters for strings
        /// </summary>
        private string value = "";
        /// <summary>
        /// The value of this filter, if this filters for strings
        /// </summary>
        public string Value { get { return this.value; } private set { this.value = value; } }
        #endregion

        #region FilterType
        /// <summary>
        /// The type of this filter
        /// </summary>
        private FilterType filterType;
        /// <summary>
        /// The type of this filter
        /// </summary>
        public FilterType FilterType { get { return this.filterType; } private set { this.filterType = value; } }
        #endregion

        #region IsValid
        /// <summary>
        /// If this Filter is valid (ie: had been initialzed with a valid XmlNode)
        /// </summary>
        private bool isValid = false;
        /// <summary>
        /// If this Filter is valid (ie: had been initialzed with a valid XmlNode)
        /// </summary>
        public bool IsValid { get { return this.isValid; } private set { this.isValid = value; } }
        #endregion

        #endregion

        #region Constructors
        /// <summary>
        /// Create a new Filter from an XmlNode
        /// </summary>
        /// <param name="filterNode">The XmlNode to create a Filter from</param>
        public Filter(XmlNode filterNode)
        {
            if (filterNode == null || !filterNode.Name.Equals("Filter"))
            {
                MainWindow.showError("Unable to make a Filter out of an empty or non-Filter XmlNode");
                return;
            }
            string typeStr = filterNode.Attributes["type"].Value;
            string valueStr = filterNode.InnerText;

            switch (typeStr.ToUpper())
            {
                case "MAGIC":
                    this.FilterType = FilterType.MAGIC; break;
                case "FILENAME":
                    this.FilterType = FilterType.FILENAME; break;
                default:
                    MainWindow.showError("Invalid Filter: invalid FilterType " + typeStr); return;
            }
            this.Value = valueStr.Trim();
            if (this.Value.Length == 0)
                MainWindow.showError("Possible improper filter: empty value. This filter will pass anything");

            this.IsValid = true;
        }
        /// <summary>
        /// Create a new Filter
        /// </summary>
        /// <param name="type">The type of this Filter</param>
        /// <param name="value">The value of this Filter</param>
        internal Filter(FilterType type, string value)
        {
            this.FilterType = type;
            this.Value = value;
            this.IsValid = true;
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
            if (!IsValid)
                return false;
            if (this.Value.Length == 0)
                return true;
            switch (this.FilterType)
            {
                case FilterType.FILENAME:
                    Regex re = new Regex(this.Value);
                    Match m = re.Match(filename.Replace('\\', '/'));
                    return m.Success;
                case FilterType.MAGIC:
                    BinaryReader br = new BinaryReader(new FileStream(filename, FileMode.Open));
                    foreach (char c in this.Value)
                        if ((char)br.ReadByte() != c)
                        {
                            br.Close();
                            return false;
                        }
                    br.Close();
                    return true;
                default: throw new Exception("Invalid FilterType @ Filter.Passes(string): " + this.FilterType.ToString());
            }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // nothing to do here; as all fields are primitive types
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
        /// A Filter thar filters on the name/path of a file
        /// </summary>
        FILENAME
    }
}
