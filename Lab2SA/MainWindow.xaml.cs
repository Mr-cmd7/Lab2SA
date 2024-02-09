using HtmlAgilityPack;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2SA
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetBook(uri.Text);

        }

        void GetBook(string uri)
        {
            WebClient wc = new WebClient();

            wc.DownloadStringCompleted += (s, eArgs) =>
            {
                string htmlContent = eArgs.Result;
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlContent);

                string textContent = ExtractText(htmlDocument.DocumentNode);

                Dispatcher?.Invoke(() => text.Text = textContent);
            };
            wc.DownloadStringAsync(new Uri(uri));
        }

        string ExtractText(HtmlNode htmlNode)
        {
            if (htmlNode == null)
                return "";

            if (htmlNode.NodeType == HtmlNodeType.Text)
                return htmlNode.InnerText.Trim();

            if (htmlNode.NodeType == HtmlNodeType.Element && htmlNode.Name == "script")
                return "";

            var sb = new StringBuilder();
            foreach (var childNode in htmlNode.ChildNodes)
            {
                sb.AppendLine(ExtractText(childNode));
            }

            return sb.ToString().Trim();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string input = text.Text;
            if (!string.IsNullOrEmpty(input))
            {
                string result = input.First().ToString() + input.Last();
                text.Text = result;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string input = text.Text;
            if (!string.IsNullOrEmpty(input))
            {
                StringBuilder evenIndices = new StringBuilder();
                StringBuilder oddIndices = new StringBuilder();
                for (int i = 0; i < input.Length; i++)
                {
                    if (i % 2 == 0)
                        evenIndices.Append(input[i]);
                    else
                        oddIndices.Append(input[i]);
                }

                text.Clear();

                text.AppendText("Строка с символами на четных индексах:\n");
                text.AppendText(evenIndices.ToString() + "\n");
                text.AppendText("Строка с символами на нечетных индексах:\n");
                text.AppendText(oddIndices.ToString() + "\n");
            }
        }

    }
}