using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;

namespace TiledGGD.BindingTools
{
    class Binding
    {
        #region Fields & Properties

        #region Name
        /// <summary>
        /// The name of this Binding
        /// </summary>
        private string name;
        /// <summary>
        /// The name of this Binding
        /// </summary>
        public string Name { get { return this.name; } private set { this.name = value; } }
        #endregion

        #region BindingType
        /// <summary>
        /// To what this Binding binds
        /// </summary>
        private BindingType bindingType;
        /// <summary>
        /// To what this Binding binds
        /// </summary>
        public BindingType BindingType { get { return this.bindingType; } private set { this.bindingType = value; } }
        #endregion

        #region Target
        /// <summary>
        /// The target of this Binding (either a method name or Lua file name)
        /// </summary>
        private string target;
        /// <summary>
        /// The target of this Binding (either a method name or Lua file name)
        /// </summary>
        public string Target { get { return this.target; } private set { this.target = value; } }
        #endregion

        #region TargetType
        /// <summary>
        /// The type of this Binding's target
        /// </summary>
        private TargetType targetType;
        /// <summary>
        /// The type of this Binding's target
        /// </summary>
        public TargetType TargetType { get { return this.targetType; } private set { this.targetType = value; } }
        #endregion

        #region FilterSet
        /// <summary>
        /// The FilterSet of this Binding
        /// </summary>
        private FilterSet filterSet;
        /// <summary>
        /// The FilterSet of this Binding
        /// </summary>
        public FilterSet FilterSet { get { return this.filterSet; } private set { this.filterSet = value; } }
        #endregion

        #region IsValid
        /// <summary>
        /// If this Binding is valid (ie: had been initialzed with a valid XmlNode)
        /// </summary>
        private bool isValid = false;
        /// <summary>
        /// If this Binding is valid (ie: had been initialzed with a valid XmlNode)
        /// </summary>
        public bool IsValid { get { return this.isValid; } private set { this.isValid = value; } }
        #endregion

        #region Enabled
        /// <summary>
        /// If this Binding is enabled. If not, this Binding binds nothing
        /// </summary>
        private bool enabled = true;
        /// <summary>
        /// If this Binding is enabled. If not, this Binding binds nothing
        /// </summary>
        public bool Enabled
        {
            get { return this.enabled; }
            private set { this.enabled = value; }
        }
        #endregion

        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new Binding
        /// </summary>
        /// <param name="xmlnode">The XmlNode this Binding is defined in</param>
        public Binding(XmlNode xmlnode)
        {
            string btype = xmlnode.Attributes["type"].Value;
            string nm = xmlnode.SelectSingleNode("Name").InnerText;
            XmlNode tnode = xmlnode.SelectSingleNode("Target");
            string ttype = tnode.Attributes["type"].Value;
            string tval = tnode.InnerText;

            this.Enabled = xmlnode.Attributes["enabled"].Value == "1";
            
            this.Name = nm;
            switch (btype.ToUpper())
            {
                case "GRAPHICS": this.BindingType = BindingType.GRAPHICS; break;
                case "PALETTE": this.BindingType = BindingType.PALETTE; break;
                default: MainWindow.showError("Invalid Binding: " + nm + " has an invalid value for BindingType: " + btype); return;
            }
            this.Target = tval;
            switch (ttype.ToUpper())
            {
                case "METHOD": this.TargetType = TargetType.METHOD; break;
                case "LUA": this.TargetType = TargetType.LUA; break;
                default: MainWindow.showError("Invalid Binding: " + nm + " has an invalid value for TargetType: " + ttype); return;
            }
            XmlNode fsetnode = xmlnode.SelectSingleNode("FilterSet");
            if (fsetnode == null)
            {
                MainWindow.showError("Invalid Binding: " + nm + " does not have a FilterSet.");
                return;
            }
            this.filterSet = new FilterSet(fsetnode);

            this.IsValid = true;
        }
        /// <summary>
        /// Create a new Binding
        /// </summary>
        /// <param name="nm">The name of this Binding</param>
        /// <param name="ttype">The type of target this Binding has</param>
        /// <param name="btype">To what this Binding binds</param>
        /// <param name="fset">The Filterset defining this Binding</param>
        /// <param name="target">To where this Binding binds</param>
        public Binding(string nm, TargetType ttype, BindingType btype, FilterSet fset, string target)
        {
            this.Name = nm;
            this.Target = target;
            this.TargetType = ttype;
            this.BindingType = btype;
            this.filterSet = fset;
            this.IsValid = true;
        }
        #endregion

        #region Method: GetTarget
        /// <summary>
        /// Get the target of this Binding
        /// </summary>
        /// <returns>The handle to a method that can process a file. This will be a zero-method (one that loads nothing) if something went wrong.</returns>
        public FileProcessor GetTarget()
        {
            switch (this.TargetType)
            {
                case TargetType.METHOD:
                    BrowseableData bd;
                    switch (this.BindingType)
                    {
                        case BindingType.GRAPHICS:
                            bd = MainWindow.GraphData; break;
                        case BindingType.PALETTE:
                            bd = MainWindow.PalData; break;
                        default: MainWindow.showError("Invalid binding: "+this.Name+" has an invalid BindingType value: "+this.bindingType.ToString()); return this.loadNothing;
                    }
                    MethodInfo mi;
                    try { mi = bd.GetType().GetMethod(this.Target, new Type[] { typeof(String) }); }
                    catch { MainWindow.showError("Invalid Binding: no such method " + this.Target); return this.loadNothing; }
                    try
                    {
                        return FileProcessor.CreateDelegate(typeof(FileProcessor), bd, mi, true) as FileProcessor;
                    }
                    catch(Exception e)
                    {
                        MainWindow.showError("Something went wrong in Binding.GetTarget; " + e.Message);
                        return this.loadNothing;
                    }
                case TargetType.LUA:

                    LuaTool tool = new LuaTool(this, this.Target);
                    return new FileProcessor(tool.loadFile);

                default:
                    MainWindow.showError("Invalid binding: "+this.Name+" has as invalid TargetType: "+this.TargetType.ToString());
                    return this.loadNothing;
            }
        }
        #endregion

        #region Method: Binds
        /// <summary>
        /// If this Binding binds a file
        /// </summary>
        /// <param name="filename">The file to check</param>
        /// <param name="type">Where the file is to be loaded</param>
        /// <returns><code>true</code> iff this binds to the correct place the FilterSet defining this Binding passes the file</returns>
        public bool Binds(string filename, BindingType type)
        {
            return this.Enabled 
                    && type == this.BindingType 
                    && this.filterSet.Passes(filename);
        }
        #endregion

        public void loadNothing(String filename)
        {
            // dummy method; do nothing. is used for when errors occur during getTarget()
        }

    }
    public enum BindingType
    {
        /// <summary>
        /// Binds to a Graphics
        /// </summary>
        GRAPHICS,
        /// <summary>
        /// Binds to Palette
        /// </summary>
        PALETTE
    }
    public enum TargetType
    {
        /// <summary>
        /// The target is a method
        /// </summary>
        METHOD,
        /// <summary>
        /// The target is a Lua plug-in
        /// </summary>
        LUA
    }
    /// <summary>
    /// Processes a file into data to be shown
    /// </summary>
    /// <param name="fileName">The path to the file</param>
    public delegate void FileProcessor(string fileName);
}
