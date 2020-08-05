using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KunTunLib
{
    public interface IMetadata
    {
        [DefaultValue(0)]
        int Priority { get; }
        string Name { get; }
        string Description { get; }
        string Author { get; }
        string Version { get; }
    }

    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ExportPluginMetadata : ExportAttribute, IMetadata
    {
        public int Priority { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Author { get; private set; }
        public string Version { get; private set; }

        public ExportPluginMetadata() : base(typeof(IMetadata))
        {
        }

        public ExportPluginMetadata(int priority) : this()
        {
            this.Priority = priority;
        }

        public ExportPluginMetadata(int priority, string name) : this(priority)
        {
            this.Name = name;
        }

        public ExportPluginMetadata(int priority, string name, string description) : this(priority, name)
        {
            this.Description = description;
        }

        public ExportPluginMetadata(int priority, string name, string description, string author) : this(priority, name, description)
        {
            this.Author = author;
        }

        public ExportPluginMetadata(int priority, string name, string description, string author, string version) : this(priority, name, description, author)
        {
            this.Version = version;
        }
    }
}
