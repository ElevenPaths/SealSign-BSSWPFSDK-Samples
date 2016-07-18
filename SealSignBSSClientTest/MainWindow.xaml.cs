using SealSignBSSClientTest.Samples;
using System.Windows;

namespace SealSignBSSClientTest
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

        private void OnSignatureSampleClick(object sender, RoutedEventArgs e)
        {
            SampleContent.Content = new SignatureSample();
        }

        private void OnSignatureOfflineSampleClick(object sender, RoutedEventArgs e)
        {
            SampleContent.Content = new SignatureOfflineSample();
        }

        private void OnSignatureWithProviderSampleClick(object sender, RoutedEventArgs e)
        {
            SampleContent.Content = new SignatureProviderSample();
        }

        private void OnSignatureAuthSampleClick(object sender, RoutedEventArgs e)
        {
            SampleContent.Content = new SignatureAuthSample();
        }
    }
}
