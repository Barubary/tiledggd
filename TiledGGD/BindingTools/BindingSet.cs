using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TiledGGD.BindingTools
{
    class BindingSet : IEnumerable<Binding>
    {
        private List<Binding> bindings;

        #region Constructors
        /// <summary>
        /// Create a new BindingSet using the default xml-location (Plugins/Bindings.xml)
        /// </summary>
        public BindingSet() : this(MainWindow.getPath() + "Plugins/Bindings.xml") { }

        /// <summary>
        /// Create a new BindingSet
        /// </summary>
        /// <param name="xmlfilename">The path to the Xml file defining this BindingSet</param>
        public BindingSet(string xmlfilename)
            : this(toXmlDoc(xmlfilename)) { }

        /// <summary>
        /// Loads an xml-document
        /// </summary>
        /// <param name="filename">The path to the document</param>
        /// <returns>An XmlDocument loaded with the data in <code>filename</code></returns>
        private static XmlDocument toXmlDoc(string filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            return doc;
        }

        /// <summary>
        /// Create a new BindingSet
        /// </summary>
        /// <param name="xmlDoc">The document the BindingSet is defined in</param>
        public BindingSet(XmlDocument xmlDoc)
            : this(xmlDoc.SelectSingleNode("Bindings")) { }

        /// <summary>
        /// Create a new BindingSet
        /// </summary>
        /// <param name="bindingsNode">The XmlNode the BindingSet is defined in</param>
        public BindingSet(XmlNode bindingsNode)
        {
            this.bindings = new List<Binding>();
            foreach (XmlNode bnode in bindingsNode.SelectNodes("Binding"))
                this.bindings.Add(new Binding(bnode));
        }
        #endregion

        #region Bindings-accessor
        /// <summary>
        /// Get a Binding of this BindingSet
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Binding this[int index]
        {
            get { return this.bindings[index]; }
        }
        #endregion

        #region Binding-Count
        /// <summary>
        /// The amount of Bindings in this BindingSet
        /// </summary>
        public int Count { get { return this.bindings.Count; } }
        #endregion

        #region Method: TryToBind
        /// <summary>
        /// Tries to bind a file
        /// </summary>
        /// <param name="filename">The name/path of the file</param>
        /// <param name="type">Where the file is to be loaded</param>
        /// <returns><code>true</code> iff a Binding in this set has bound the file</returns>
        public bool TryToBind(string filename, BindingType type)
        {
            foreach(Binding b in this)
                if (b.Binds(filename, type)){
                    b.GetTarget().Invoke(filename);
                    return true;
                }
            return false;
        }
        #endregion

        #region IEnumerable<Binding> Members

        public IEnumerator<Binding> GetEnumerator()
        {
            return this.bindings.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.bindings.GetEnumerator();
        }

        #endregion
    }
}
