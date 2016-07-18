using SealSignBSSClientTest.BiometrisSignatureService;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SealSignBSSClientTest.Samples
{
    /// <summary>
    /// Interaction logic for SignatureProviderSample.xaml
    /// </summary>
    public partial class SignatureProviderSample : SampleBase
    {
        public SignatureProviderSample()
        {
            InitializeComponent();
        }

        protected override void SampleBase_Loaded(object sender, RoutedEventArgs e)
        {
            base.SampleBase_Loaded(sender, e);
            if (_signaturePanel != null)
                RootGrid.Children.Add((UIElement)_signaturePanel);
        }

        protected override void SampleBase_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_signaturePanel != null && RootGrid.Children.Contains((UIElement)_signaturePanel))
            {
                RootGrid.Children.Remove((UIElement)_signaturePanel);
            }
            base.SampleBase_Unloaded(sender, e);
        }

        private async void OnFirmarClick(object sender, RoutedEventArgs e)
        {
            botonFirmar.IsEnabled = false;
            botonBorrar.IsEnabled = false;
            _signaturePanel.Stop();
            
            BiometricSignatureServiceBasicClient biometricSignatureClient = new BiometricSignatureServiceBasicClient("WSHttpBinding_IBiometricSignatureServiceBasic");

            var biometricResponse = await biometricSignatureClient.BeginSignatureProviderAsync(
                string.Empty,
                string.Empty,
                "demo://documentoDocumento",
                string.Empty,
                null);

            var biometricState = _signaturePanel.GetSignature(biometricResponse.instance, biometricResponse.biometricState);

            var signedBytes = await biometricSignatureClient.EndSignatureProviderAsync(biometricResponse.instance, biometricState, "demo://documentoDocumento", string.Empty, true);

            using (FileStream documentStream = new FileStream(@"d:\test\sample.signed.pdf", FileMode.Create))
            {
                await documentStream.WriteAsync(signedBytes, 0, signedBytes.Length);
                await documentStream.FlushAsync();
                documentStream.Close();
            }

            biometricSignatureClient.Close();

            _signaturePanel.Start();
            botonFirmar.IsEnabled = true;
            botonBorrar.IsEnabled = true;
        }
    }
}
