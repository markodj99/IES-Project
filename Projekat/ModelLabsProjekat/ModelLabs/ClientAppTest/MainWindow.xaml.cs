using ClientAppTest.Stuff;
using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;

namespace ClientAppTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region FIELDS

        readonly TestGdaApp testGdaApp = new TestGdaApp();

        #endregion

        #region CONSTRUCTOR

        public MainWindow()
        {
            InitializeComponent();

            ComboBox1.ItemsSource = new List<string> { "GetValues", "GetExtentValues", "GetRelatedVlaues" };
            LabelNum6.Content = "Selected propertis";
        }

        #endregion

        #region CB1

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string value = (string)ComboBox1.SelectedItem;
            Enums.Methods method = Enums.GetMethodEnum(value);

            switch (method)
            {
                case Enums.Methods.GetValues:
                    CaseGetValues1();
                    break;
                case Enums.Methods.GetExtentValues:
                    CaseGetExtendedValues1();
                    break;
                case Enums.Methods.GetRelatedValues:
                    GetRelatedValues1();
                    break;
                case Enums.Methods.Unknown:
                    break;
            }
        }

        private void CaseGetValues1()
        {
            LabelNum2.Content = "Choose GID:";
            LabelNum3.Content = "";
            LabelNum4.Content = "";
            LabelNum5.Content = "Choose propertis:";

            ComboBox3.ItemsSource = "";
            ComboBox4.ItemsSource = "";
            ListBoxProp.ItemsSource = "";
            TextBoxProps.Text = "";

            Dictionary<string, string> gidName = testGdaApp.GetGidValues();

            List<string> gidStrings = new List<string>();

            foreach (var item in gidName)
            {
                gidStrings.Add(item.Key + "-" + item.Value);
            }

            ComboBox2.ItemsSource = gidStrings;
        }

        private void CaseGetExtendedValues1()
        {
            LabelNum2.Content = "Choose entity type:";
            LabelNum3.Content = "";
            LabelNum4.Content = "";
            LabelNum5.Content = "Choose propertis:";

            ComboBox3.ItemsSource = "";
            ComboBox4.ItemsSource = "";
            ListBoxProp.ItemsSource = "";
            TextBoxProps.Text = "";

            List<string> concreteClasses = new List<string>
            {
                "CONNECTIVITYNODECONTAINER",
                "TOPOLOGICALNODE",
                "CONNECTIVITYNODE",
                "BASEVOLTAGE",
                "SWITCH",
                "TERMINAL"
            };

            ComboBox2.ItemsSource = concreteClasses;
        }

        private void GetRelatedValues1()
        {
            LabelNum2.Content = "Choose GID:";
            LabelNum3.Content = "Choose PropertyId:";
            LabelNum4.Content = "Choose Type:";
            LabelNum5.Content = "Choose propertis:";

            ComboBox3.ItemsSource = "";
            ComboBox4.ItemsSource = "";
            ListBoxProp.ItemsSource = "";
            TextBoxProps.Text = "";

            var gidName = testGdaApp.GetGidValues();
            var gidStrings = gidName.Select(item => item.Key + "-" + item.Value).ToList();
            ComboBox2.ItemsSource = gidStrings;
        }

        #endregion

        #region CB2

        private void ComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string valueMethod = (string)ComboBox1.SelectedItem;
            Enums.Methods method = Enums.GetMethodEnum(valueMethod);

            switch (method)
            {
                case Enums.Methods.GetValues:
                    CaseGetValues2();
                    break;
                case Enums.Methods.GetExtentValues:
                    CaseGetExtendedValues2();
                    break;
                case Enums.Methods.GetRelatedValues:
                    GetRelatedValues2();
                    break;
                case Enums.Methods.Unknown:
                    break;
            }
        }

        private void CaseGetValues2()
        {
            string gidValue = (string)ComboBox2.SelectedItem;

            if (gidValue == null) return;

            string gid = gidValue.Split('-')[0];
            string name = gidValue.Split('-')[1];

            ResourceDescription rd = testGdaApp.GetValues(long.Parse(gid));

            var listProp = rd.Properties.Select(item => item.Id.ToString()).ToList();

            ListBoxProp.ItemsSource = listProp;
            TextBoxProps.Text = "";
        }

        private void CaseGetExtendedValues2()
        {
            string modelCodeType = (string)ComboBox2.SelectedItem;
            Enum.TryParse(modelCodeType, out ModelCode model);
            var listProp = testGdaApp.GetModelCodesForEntity(model);

            ListBoxProp.ItemsSource = listProp;
            TextBoxProps.Text = "";
        }

        private void GetRelatedValues2()
        {
            string gidValue = (string)ComboBox2.SelectedItem;
            if (gidValue == null) return;

            string gid = gidValue.Split('-')[0];

            var refList = testGdaApp.GetReferencesForGid(long.Parse(gid));
            var listProp = refList.ToList();

            ComboBox3.ItemsSource = listProp;

            TextBoxProps.Text = "";
        }

        #endregion

        #region LIST

        private void ListBoxProp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string valueMethod = (string)ComboBox1.SelectedItem;
            Enums.Methods method = Enums.GetMethodEnum(valueMethod);

            switch (method)
            {
                case Enums.Methods.GetValues:
                case Enums.Methods.GetExtentValues:
                case Enums.Methods.GetRelatedValues:
                {
                    string propValue = (string)ListBoxProp.SelectedItem;
                    bool exist = false;

                    if (TextBoxProps.Text == "")
                    {
                        TextBoxProps.Text = propValue;
                    }
                    else
                    {
                        if (TextBoxProps.Text.Split('\n').Any(item => item == propValue)) exist = true;

                        if (exist) return;
                        string str = TextBoxProps.Text;
                        str += "\n" + propValue;
                        TextBoxProps.Text = str;
                    }

                    break;
                }
                case Enums.Methods.Unknown:
                    break;
            }
        }

        #endregion

        #region CB3

        private void ComboBox3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string valueMethod = (string)ComboBox1.SelectedItem;
            Enums.Methods method = Enums.GetMethodEnum(valueMethod);

            List<string> classes = new List<string>();

            if (!Enums.Methods.GetRelatedValues.Equals(method)) return;

            string refValue = (string)ComboBox3.SelectedItem;
            string gidValue = (string)ComboBox2.SelectedItem;
            ResourceDescription rdRef = new ResourceDescription();

            if (refValue == "" || gidValue == "") return;

            ResourceDescription rd = testGdaApp.GetValues(long.Parse(gidValue.Split('-')[0]));

            foreach (var item in rd.Properties.Where(item => item.Id.ToString() == refValue))
            {
                if (item.Type == PropertyType.Reference)
                {
                    rdRef = testGdaApp.GetValues(item.AsReference());
                    foreach (var refItem in rdRef.Properties.Where(refItem => !classes.Contains(refItem.Id.ToString().Split('_')[0])))
                    {
                        classes.Add(refItem.Id.ToString().Split('_')[0]);
                    }
                }

                if (item.Type != PropertyType.ReferenceVector || item.AsReferences().Count <= 0) continue;
                {
                    foreach (var refItem in item.AsReferences())
                    {
                        rdRef = testGdaApp.GetValues(refItem);
                        foreach (var refItem1 in rdRef.Properties.Where(refItem1 => !classes.Contains(refItem1.Id.ToString().Split('_')[0])))
                        {
                            classes.Add(refItem1.Id.ToString().Split('_')[0]);
                        }
                    }
                }
            }

            classes.Add("NONE");

            ComboBox4.ItemsSource = classes;
        }

        #endregion

        #region CB4

        private void ComboBox4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string valueMethod = (string)ComboBox1.SelectedItem;
            Enums.Methods method = Enums.GetMethodEnum(valueMethod);

            if (Enums.Methods.GetRelatedValues.Equals(method))
            {
                var modelCodes = new List<string>();

                string type = (string)ComboBox4.SelectedItem;


                if (type == "NONE")
                {
                    modelCodes.Add("IDOBJ_GID");
                    modelCodes.Add("IDOBJ_ALIASNAME");
                    modelCodes.Add("IDOBJ_MRID");
                    modelCodes.Add("IDOBJ_NAME");
                    ListBoxProp.ItemsSource = modelCodes;
                }
                else
                {
                    Enum.TryParse(type, out ModelCode model);
                    modelCodes = testGdaApp.GetModelCodesForEntity(model);
                    ListBoxProp.ItemsSource = modelCodes;
                }
            }
        }

        #endregion

        #region MyRegion

        private void Button_Click_Execute(object sender, RoutedEventArgs e)
        {
            string valueMethod = (string)ComboBox1.SelectedItem;
            Enums.Methods method = Enums.GetMethodEnum(valueMethod);

            switch (method)
            {
                case Enums.Methods.GetValues:
                    ExecuteGetValues();
                    break;
                case Enums.Methods.GetExtentValues:
                    ExecuteGetExtentValues();
                    break;
                case Enums.Methods.GetRelatedValues:
                    ExecuteGetRelatedValues();
                    break;
                case Enums.Methods.Unknown:
                    break;
            }
        }

        private void ExecuteGetValues()
        {
            string gidValue = (string)ComboBox2.SelectedItem;
            if (gidValue == null) return;

            string gid = gidValue.Split('-')[0];
            string name = gidValue.Split('-')[1];

            string richText = "---------------------------------------------------------" + DateTime.Now + "-------------------------------------------------------\n";
            richText += "\tMethod: GetValues\n";
            richText += "\tClass: " + name.Split('_')[0] + " " + name.Split('_')[1] + "\n";
            richText += "\tGID: " + gid + "\n";

            var listProp = TextBoxProps.Text.Split('\n').ToList();
            if (listProp.Count <= 1 && listProp[0] == "") return;

            ResourceDescription rd = testGdaApp.GetValues(long.Parse(gid));

            foreach (var itemProp in listProp)
            {
                foreach (var prop in rd.Properties.Where(prop => itemProp == prop.Id.ToString()))
                {
                    switch (prop.Type)
                    {
                        case PropertyType.Reference:
                            richText += "\t\t" + itemProp + " : " + prop.AsReference() + "\n";
                            break;
                        case PropertyType.ReferenceVector:
                            string str = prop.AsReferences().Aggregate("", (current, item) => current + (item + ", "));
                            richText += "\t\t" + itemProp + " : " + str + "\n";
                            break;
                        default:
                            richText += "\t\t" + itemProp + " : " + prop + "\n";
                            break;
                    }

                }
            }

            TextRange textRange = new TextRange(RichTextBoxValues.Document.ContentStart, RichTextBoxValues.Document.ContentEnd);
            string text = textRange.Text;
            richText += "------------------------------------------------------------------------------------------------------------------------------------------\n\n";
            richText += text;
            RichTextBoxValues.Document.Blocks.Clear();
            RichTextBoxValues.Document.Blocks.Add(new Paragraph(new Run(richText)));
        }

        private void ExecuteGetExtentValues()
        {
            var models = new List<ModelCode>();
            string modelCodeType = (string)ComboBox2.SelectedItem;

            Enum.TryParse(modelCodeType, out ModelCode model);
            if (modelCodeType == null) return;

            string richText = "---------------------------------------------------------" + DateTime.Now + "-------------------------------------------------------\n";
            richText += "\tMethod: GetExtentValues\n";
            richText += "\tModelCode: " + modelCodeType + "\n";

            var listProp = TextBoxProps.Text.Split('\n').ToList();
            if (listProp.Count <= 1 && listProp[0] == "") return;

            foreach (var item in listProp)
            {
                Enum.TryParse(item, out ModelCode modelProp);
                models.Add(modelProp);
            }

            var ids = testGdaApp.GetExtentValues(model, models);
            foreach (var rd in ids.Select(id => testGdaApp.GetValues(id, models)))
            {
                richText += "\n";
                foreach (var itemProp in listProp)
                {
                    foreach (var prop in rd.Properties.Where(prop => itemProp == prop.Id.ToString()))
                    {
                        switch (prop.Type)
                        {
                            case PropertyType.Reference:
                                richText += "\t\t" + itemProp + " : " + prop.AsReference() + "\n";
                                break;
                            case PropertyType.ReferenceVector:
                                string str = prop.AsReferences().Aggregate("", (current, item) => current + (item + ", "));
                                richText += "\t\t" + itemProp + " : " + str + "\n";
                                break;
                            default:
                                richText += "\t\t" + itemProp + " : " + prop + "\n";
                                break;
                        }

                        break;
                    }
                }
                richText += "\n";
            }

            TextRange textRange = new TextRange(RichTextBoxValues.Document.ContentStart, RichTextBoxValues.Document.ContentEnd);
            string text = textRange.Text;
            richText += "------------------------------------------------------------------------------------------------------------------------------------------\n\n";
            richText += text;
            RichTextBoxValues.Document.Blocks.Clear();
            RichTextBoxValues.Document.Blocks.Add(new Paragraph(new Run(richText)));
        }

        private void ExecuteGetRelatedValues()
        {
            string gidValue = ((string)ComboBox2.SelectedItem).Split('-')[0];
            long gid = long.Parse(gidValue);
            var abstractclasses = new List<ModelCode>() { ModelCode.IDENTIFIEDOBJECT, ModelCode.POWERSYSTEMRESOURCE, ModelCode.CONDUCTINGEQUIPMENT};

            string propertyID = (string)ComboBox3.SelectedItem;
            Enum.TryParse(propertyID, out ModelCode modelPropID);

            string type = (string)ComboBox4.SelectedItem;
            Enum.TryParse(type, out ModelCode modelType);

            var models = new List<ModelCode>();
            var listProp = TextBoxProps.Text.Split('\n').ToList();
            var ids = new List<long>();
            if (listProp.Count <= 1 && listProp[0] == "") return;

            foreach (var item in listProp)
            {
                Enum.TryParse(item, out ModelCode modelProp);
                models.Add(modelProp);
            }

            var association = new Association
            {
                PropertyId = modelPropID,
                Type = abstractclasses.Contains(modelType) ? (ModelCode) 0 : modelType
            };
            ids = testGdaApp.GetRelatedValues(gid, association, models);

            string richText = "-------------------------------------------------------" + DateTime.Now + "-------------------------------------------------------\n";
            richText += "\tMethod: GetRelatedVlaues\n";
            richText += "\tProperyID: " + propertyID + "\n";
            richText += "\tType: " + type + "\n";

            foreach (var item in listProp)
            {
                Enum.TryParse(item, out ModelCode modelProp);
                models.Add(modelProp);
            }

            foreach (var rd in ids.Select(id => testGdaApp.GetValues(id, models)))
            {
                richText += "\n";

                foreach (var itemProp in listProp)
                {
                    foreach (var prop in rd.Properties.Where(prop => itemProp == prop.Id.ToString()))
                    {
                        switch (prop.Type)
                        {
                            case PropertyType.Reference:
                                richText += "\t\t" + itemProp + " : " + prop.AsReference() + "\n";
                                break;
                            case PropertyType.ReferenceVector:
                                string str = prop.AsReferences().Aggregate("", (current, item) => current + (item + ", "));
                                richText += "\t\t" + itemProp + " : " + str + "\n";
                                break;
                            default:
                                richText += "\t\t" + itemProp + " : " + prop + "\n";
                                break;
                        }
                    }
                }
                richText += "\n";
            }

            TextRange textRange = new TextRange(RichTextBoxValues.Document.ContentStart, RichTextBoxValues.Document.ContentEnd);
            string text = textRange.Text;
            richText += "------------------------------------------------------------------------------------------------------------------------------------------\n\n";
            richText += text;
            RichTextBoxValues.Document.Blocks.Clear();
            RichTextBoxValues.Document.Blocks.Add(new Paragraph(new Run(richText)));
        }

        #endregion

        #region Restart

        private void Button_Click_Restart(object sender, RoutedEventArgs e) => TextBoxProps.Text = string.Empty;

        #endregion

        #region SELECT

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var propList = (List<string>)ListBoxProp.ItemsSource;
            TextBoxProps.Text = "";

            foreach (var item in propList)
            {
                if (TextBoxProps.Text == "") TextBoxProps.Text = item;
                else TextBoxProps.Text += "\n" + item;
            }
        }

        #endregion
    }
}