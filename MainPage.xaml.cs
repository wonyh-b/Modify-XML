using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Modify_XML
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void btn_read(object sender, RoutedEventArgs e)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.GetFileAsync("file_group.xml");
            string fileContent = await FileIO.ReadTextAsync(file);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(fileContent);
            XmlNodeList tags = doc.GetElementsByTagName("person");
            result.Children.Clear();
            status.Visibility = Visibility.Collapsed;

            if (tags.Count > 0)
            {
                for (int i = 0; i < tags.Count; i++)
                {
                    string resId = "null";
                    string resName = "null";
                    if (tags.ElementAt(i).Attributes.GetNamedItem("id") != null)
                        resId = tags.ElementAt(i).Attributes.GetNamedItem("id").InnerText;
                    if (tags.ElementAt(i).Attributes.GetNamedItem("name") != null)
                        resName = tags.ElementAt(i).Attributes.GetNamedItem("name").InnerText;

                    ListViewItem item = new ListViewItem();
                    StackPanel item_container = new StackPanel();
                    TextBlock result_id = new TextBlock();
                    TextBlock result_name = new TextBlock();

                    result_id.Text = "ID: " + resId;
                    result_name.Text = "Name: " + resName;

                    item_container.Children.Add(result_id);
                    item_container.Children.Add(result_name);
                    item.Content = item_container;
                    result.Children.Add(item);
                }
            }
        }
        private async void btn_save(object sender, RoutedEventArgs e)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.GetFileAsync("file_group.xml");
            string fileContent = await FileIO.ReadTextAsync(file);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(fileContent);
            XmlNodeList tags = doc.GetElementsByTagName("person");
            result.Children.Clear();

            if (tags.Count > 0)
            {
                int index = 0;
                for (int i = 0; i < tags.Count; i++)
                {
                    if(tags.ElementAt(i).Attributes.GetNamedItem("id").InnerText == "12345678901234"/*this is a unique id, based on xml*/ && tags.ElementAt(i).Attributes.GetNamedItem("id") != null)
                    {
                        index = i;
                        break;
                    }
                }

                XmlAttribute attr_name = doc.CreateAttribute("name");
                attr_name.Value = txtBox_name.Text;

                tags.ElementAt(index).Attributes.SetNamedItem(attr_name);
                await doc.SaveToFileAsync(file);
            }
            status.Text = "Saved! Your name is: " + txtBox_name.Text;
            status.Visibility = Visibility.Visible;
        }
    }
}
