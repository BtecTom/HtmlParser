using System.IO;
using System.Windows;
using System.Windows.Controls;
using HtmlAgilityPack;
using Microsoft.Win32;

namespace HtmlScraper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ScrapingMode_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                if (GetScrapingModeValue() == "Configuration")
                {
                    Name.IsEnabled = false;
                }
                else
                {
                    Name.IsEnabled = true;
                }
            }
        }

        private string? GetScrapingModeValue() => ScrapingMode.SelectedItem.ToString()?.Split(":")[1].Trim();

        private void SelectOutputDirectory_OnClick(object sender, RoutedEventArgs e)
        {
            var folderDialog = new OpenFolderDialog
            {
                Title = "Select Folder",
            };

            if (folderDialog.ShowDialog() == true)
            {
                OutputDirectory.Text = folderDialog.FolderName;
            }
        }

        private void Name_OnGotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                if (textBox.Text == textBox.ToolTip.ToString())
                {
                    textBox.Text = "";
                }
            }
        }

        private void Name_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                if (textBox.Text == "")
                {
                    textBox.Text = textBox.ToolTip.ToString();
                }
            }
        }

        private void ScrapeButton_OnClick(object sender, RoutedEventArgs e)
        {
            Tuple<bool, string>? result = null;
            switch (GetScrapingModeValue())
            {
                case "Person":
                    result = ScrapeByPage("profile", Name.Text.ToLower().Replace(' ', '-'));
                    break;
                case "Configuration":
                    break;
                default:
                    break;
            }

            if (result is { Item1: true })
            {
                MessageBox.Show($"Scraped page saved to {result.Item2}", "Scraper", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Unable scrape page", "Scraper", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }


        }

        private Tuple<bool, string> ScrapeByPage(string type, string name)
        {
            var url = $"https://www.forbes.com/{type}/{name}";


            HtmlWeb web = new HtmlWeb();

            HtmlDocument htmlDoc = web.Load(url);

            if (htmlDoc == null || htmlDoc.ParsedText == "")
            {
                return new Tuple<bool, string>(false, string.Empty);
            }

            var fileName = Path.Combine(OutputDirectory.Text, $"{type}-{name}.html");

            if (!Directory.Exists(OutputDirectory.Text))
            {
                Directory.CreateDirectory(OutputDirectory.Text);
            }

            htmlDoc.Save(fileName);
            return new Tuple<bool, string>(true, fileName);
        }
    }
}