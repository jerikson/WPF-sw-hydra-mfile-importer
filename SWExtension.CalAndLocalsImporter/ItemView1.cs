using System;
using System.ComponentModel;
using System.Reflection;
using RemObjects.Hydra;
using SystemWeaver.ExtensionsAPI;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Linq;

namespace SWExtension.CalAndLocalsImporter
{
    [Plugin, NonVisualPlugin]
    public partial class ItemView1 : NonVisualPlugin, IswItemView
    {
        private IswBroker _broker;

        public ItemView1()
        {
            InitializeComponent();
        }

        public ItemView1(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void Initialize(IswBroker broker)
        {
            _broker = broker;
        }

        public string GetCaption()
        {
            return "Import .M-File";
        }

        public string GetGroup()
        {
            return "Extensions";
        }

        public string GetName()
        {
            return typeof(ItemView1Content).FullName;
        }

        public int GetImageIndex()
        {
            return -1;
        }

        public string GetPluginContentName()
        {
            return typeof(ItemView1Content).FullName;
        }

        public bool SupportsItem(IswItem AItem)
        {
            return AItem.IsSID("I");
        }

        public Guid GetGUID()
        {
            return Guid.Parse("fe8a632c-da4f-456c-8341-57b8bbe51753");
        }

        public string GetConfigXMLExample()
        {
            return string.Empty;
        }

        public void ValidateConfigXML(string AXML)
        {
        }
       
    }
}
