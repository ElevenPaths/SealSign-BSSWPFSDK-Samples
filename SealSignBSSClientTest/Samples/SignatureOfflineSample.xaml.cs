using SealSignBSSClientTest.BiometrisSignatureService;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace SealSignBSSClientTest.Samples
{
    /// <summary>
    /// Interaction logic for SignatureOfflineSample.xaml
    /// </summary>
    public partial class SignatureOfflineSample : SampleBase
    {
        public SignatureOfflineSample()
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
            if (_signaturePanel != null && RootGrid.Children.Contains((UIElement) _signaturePanel))
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

            byte[] documentBytes = null;

            var assembly = Assembly.GetExecutingAssembly();

            using (Stream documentStream = assembly.GetManifestResourceStream("SealSignBSSClientTest.Assets.sample.pdf"))
            {
                documentBytes = new byte[documentStream.Length];
                await documentStream.ReadAsync(documentBytes, 0, (int)documentStream.Length);
                documentStream.Close();
            }

            Guid instance = Guid.Empty;

            var biometricFinalState = _signaturePanel.GetOfflineSignature(documentBytes, "test", out instance);

            BiometricSignatureServiceBasicClient biometricSignatureClient = new BiometricSignatureServiceBasicClient("WSHttpBinding_IBiometricSignatureServiceBasic");

            OfflineBiometricSignature[] offlineSignatures = new OfflineBiometricSignature[1];
            offlineSignatures[0] = new OfflineBiometricSignature();
            offlineSignatures[0].id = "";
            offlineSignatures[0].account = "";
            offlineSignatures[0].biometricOptions = BiometricSignatureFlags.Default;
            offlineSignatures[0].biometricParameters = null;
            offlineSignatures[0].options = SignatureFlags.Default;
            offlineSignatures[0].parameters = null;
            offlineSignatures[0].instance = instance;
            offlineSignatures[0].offlineBiometricState = biometricFinalState;

            var signedBytes = await biometricSignatureClient.SyncOfflineSignaturesAsync(SignatureProfile.PDF, offlineSignatures, null, documentBytes);

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
