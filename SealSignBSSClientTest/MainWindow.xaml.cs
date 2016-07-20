using SealSignBSSClientTest.Samples;
using System.Windows;
using System.ComponentModel;

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

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (SampleContent.Content != null)
            {
                ((SampleBase)SampleContent.Content).Disconnect();
            }
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
