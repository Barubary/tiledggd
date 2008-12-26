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

        #endregion

        #region Constructor
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
        }
        #endregion

        #region Method: GetTarget
        public unsafe FileProcessor GetTarget()
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
                    MethodInfo mi = bd.GetType().GetMethod(this.Target, new Type[] { typeof(String) });
                    return FileProcessor.CreateDelegate(bd.GetType(), mi) as FileProcessor;

                    //break;
                case TargetType.LUA:
                    MainWindow.showError("Binding.GetTarget has not yet been implemented for Lua plugins");
                    return this.loadNothing;
                default:
                    MainWindow.showError("Invalid binding: "+this.Name+" has as invalid TargetType: "+this.TargetType.ToString());
                    return this.loadNothing;
            }
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
