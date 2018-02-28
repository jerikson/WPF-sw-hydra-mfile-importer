using System;
using System.Windows;
using RemObjects.Hydra;
using RemObjects.Hydra.CrossPlatform;
using SystemWeaver.ExtensionsAPI;
using System.Windows.Forms;
using System.IO;
using System.Linq;


namespace SWExtension.CalAndLocalsImporter
{
    /// <summary>
    /// Interaction logic for ItemView1Content.xaml
    /// </summary>

    [Plugin, VisualPlugin, NeedsManagedWrapper(typeof(WPFPluginContentWrapper))]
    public partial class ItemView1Content : RemObjects.Hydra.WPF.VisualPlugin, IswItemViewContent
    {
        private IswItemViewHost _host;
        private SWEventManager _eventManager;
        private IswDialogs _dialogs;
        private IswItem _item;
        private IswItem _newImplementationAtom;
        private IswItem _newCalibrationParameter;
        private IswItem _newLocalImplementationSignal;


        private bool _includeParameter = false;
        private string _parameterName;
        private string _datatype;
        private string _includedParameterType;
        private int _itemCount;


        public ItemView1Content()
        {
            InitializeComponent();
            _eventManager = new SWEventManager();
        }

        public void SetHost(IswItemViewHost host)
        {
            _host = host;
            _host.SetEvent(_eventManager);
            _dialogs = _host.GetDialogs();
        }



        private void btn_BrowseFile_Click_1(object sender, RoutedEventArgs e)
        {
            BrowseFile();
        }

        public void BrowseFile()
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "M File (*.m)|*.m";
            fd.InitialDirectory = "C:\\";
            fd.Title = "Browse .M File";
            _itemCount = 0;

            lbl_LibraryName.Content = _item.HomeLibrary.Name.ToString();
            

            if (fd.ShowDialog() == DialogResult.OK)
            {
                txtBox_FilePath.Text = fd.FileName;
                txtBoxM_FileContent.Text = "Parameters found:" + Environment.NewLine + ("-----------------------------") + Environment.NewLine;
                try
                {         
                    using (StreamReader reader = new StreamReader(txtBox_FilePath.Text))
                    {
                        string entry;
                        // [Calibration Parameters] & [Local Implementation Signals]
                        string[] validParameters = new string[] { "ie", "ke", "ka", "kn", "kt", "ne", "na", "nt", "ve", "va" };
                        string lastLine = File.ReadLines(fd.FileName).Last();
                        if (lastLine == "") { lastLine = "BREAK OUT OF LOOP SINCE LAST LINE WAS EMPTY"; }

                        _includeParameter = false;
                        _parameterName = "";
                        _datatype = "";
                        _includedParameterType = "";

                        _newImplementationAtom = _item.HomeLibrary.CreateItem("5ATM", "import_result_to_be_deleted");

                        while ((entry = reader.ReadLine()) != null)
                        {
                            // Search for ex: [DPSCie_b_ResetStatePropulsion_ob = mpt.Parameter;] and remove everything after "="
                            if (entry.Contains("mpt.Parameter") || entry.Contains("mpt.Signal"))
                            {

                                if (entry.Substring(4, 2) == (validParameters[0]))
                                {
                                    _includeParameter = true;
                                    _parameterName = entry.Remove(entry.IndexOf('=')).Trim();
                                    _includedParameterType = "5CAL"; // Calibration'
                                }
                                else if (entry.Substring(4, 2) == (validParameters[1]))
                                {
                                    _includeParameter = true;
                                    _parameterName = entry.Remove(entry.IndexOf('=')).Trim();
                                    _includedParameterType = "5CAL"; // Calibration
                                }
                                else if (entry.Substring(4, 2) == (validParameters[2]))
                                {
                                    _includeParameter = true;
                                    _parameterName = entry.Remove(entry.IndexOf('=')).Trim();
                                    _includedParameterType = "5CAL"; // Calibration
                                }
                                else if (entry.Substring(4, 2) == (validParameters[3]))
                                {
                                    _includeParameter = true;
                                    _parameterName = entry.Remove(entry.IndexOf('=')).Trim();
                                    _includedParameterType = "5CAL"; // Calibration
                                }
                                else if (entry.Substring(4, 2) == (validParameters[4]))
                                {
                                    _includeParameter = true;
                                    _parameterName = entry.Remove(entry.IndexOf('=')).Trim();
                                    _includedParameterType = "5CAL"; // Calibration
                                }
                                else if (entry.Substring(4, 2) == (validParameters[5]))
                                {
                                    _includeParameter = true;
                                    _parameterName = entry.Remove(entry.IndexOf('=')).Trim();
                                    _includedParameterType = "8CBDS"; // Signal
                                }
                                else if (entry.Substring(4, 2) == (validParameters[6]))
                                {
                                    _includeParameter = true;
                                    _parameterName = entry.Remove(entry.IndexOf('=')).Trim();
                                    _includedParameterType = "8CBDS"; // Signal
                                }
                                else if (entry.Substring(4, 2) == (validParameters[7]))
                                {
                                    _includeParameter = true;
                                    _parameterName = entry.Remove(entry.IndexOf('=')).Trim();
                                    _includedParameterType = "8CBDS"; // Signal
                                }
                                else if (entry.Substring(4, 2) == (validParameters[8]))
                                {
                                    _includeParameter = true;
                                    _parameterName = entry.Remove(entry.IndexOf('=')).Trim();
                                    _includedParameterType = "8CBDS"; // Signal
                                }
                                else if (entry.Substring(4, 2) == (validParameters[9]))
                                {
                                    _includeParameter = true;
                                    _parameterName = entry.Remove(entry.IndexOf('=')).Trim();
                                    _includedParameterType = "8CBDS"; // Signal
                                }
                                else { _includeParameter = false; }
                            }

                            if (_includeParameter)
                            {
                                // Extract Datatype
                                if (entry.Contains(_parameterName + ".DataType"))
                                {
                                    string[] equal_split = entry.Split('=');
                                    if (equal_split.Count() > 1)
                                    {
                                        string[] equal_rightside_split = equal_split[1].Split('\'');
                                        if (equal_rightside_split.Count() > 2)
                                        {
                                            _datatype = equal_rightside_split[1];
                                        }
                                    }
                                }
                            }

                            // Create new Locals & Calibration parameters
                            if ((entry == "") && _includeParameter || lastLine == entry)
                            {
                                if (_includedParameterType == "5CAL")    // Calibration 
                                {
                                    _newCalibrationParameter = _newImplementationAtom.HomeLibrary.CreateItem("5CAL", _parameterName);
                                
                                    txtBoxM_FileContent.Text += "Calibration: " + _parameterName + Environment.NewLine + "    " + _datatype + Environment.NewLine;
                                    _newCalibrationParameter.SetAttributeWithSID("4CCT", _datatype);
                                    _newImplementationAtom.AddPart("5CAM", _newCalibrationParameter, null);
                                    _itemCount ++;

                                }
                                else if (_includedParameterType == "8CBDS")    // Signal 
                                {
                                    _newLocalImplementationSignal = _newImplementationAtom.HomeLibrary.CreateItem("8CBDS", _parameterName);
                                    
                                    txtBoxM_FileContent.Text += "Signal: " + _parameterName + Environment.NewLine + "    " + _datatype + Environment.NewLine;
                                    _newLocalImplementationSignal.SetAttributeWithSID("4CCT", _datatype);
                                    _newImplementationAtom.AddPart("6IML", _newLocalImplementationSignal, null);
                                    _itemCount++;
                                }

                                _includeParameter = false;
                            }
                        }
                        lbl_ItemCount.Content = _itemCount + " Parameters imported";
                    }
                }
                catch (Exception ex) { System.Windows.MessageBox.Show(ex.Message); }

            }

        }

        public void SetCurrentItem(IswItem item)
        {
            _item = item;
        }
    }
}
