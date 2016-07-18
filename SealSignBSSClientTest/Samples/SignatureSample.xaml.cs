using SealSignBSSClientLibrary;
using SealSignBSSClientTest.BiometrisSignatureService;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Windows;
using System.Windows.Media.Imaging;
using System;
using System.Threading.Tasks;

namespace SealSignBSSClientTest.Samples
{
    public partial class SignatureSample : SampleBase, ISealSignBSSEventListener
    {
        public SignatureSample() : base()
        {
            InitializeComponent();
        }

        protected override void SampleBase_Loaded(object sender, RoutedEventArgs e)
        {
            base.SampleBase_Loaded(sender, e);
            if (_signaturePanel != null)
            {                
                RootGrid.Children.Add((UIElement)_signaturePanel);
            }

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

            byte[] documentBytes = null;

            var assembly = Assembly.GetExecutingAssembly();

            using (Stream documentStream = assembly.GetManifestResourceStream("SealSignBSSClientTest.Assets.sample.pdf"))
            {
                documentBytes = new byte[documentStream.Length];
                await documentStream.ReadAsync(documentBytes, 0, (int)documentStream.Length);
                documentStream.Close();
            }

            BiometricSignatureServiceBasicClient biometricSignatureClient = new BiometricSignatureServiceBasicClient("WSHttpBinding_IBiometricSignatureServiceBasic");

            var biometricResponse = await biometricSignatureClient.BeginSignatureAsync(
                SignatureProfile.PDF,
                BiometricSignatureType.Default,
                string.Empty,
                string.Empty,
                BiometricSignatureFlags.Default,
                null,
                SignatureFlags.Default,
                null,
                null,
                documentBytes);

            var biometricState = _signaturePanel.GetSignature(biometricResponse.instance, biometricResponse.biometricState);

            var signedBytes = await biometricSignatureClient.EndSignatureAsync(biometricResponse.instance, biometricState);

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

        private void OnFondoClick(object sender, RoutedEventArgs e)
        {
            _signaturePanel.Stop();
            System.Drawing.Image image = System.Drawing.Image.FromFile(@"C:\Users\fernando.delahermosa\Pictures\poil00.jpg");
            _signaturePanel.SetBackgroundImage(image);
            _signaturePanel.Start();
        }

        private void OnBotonesClick(object sender, RoutedEventArgs e)
        {
            _signaturePanel.ClearButtonArea();

            _signaturePanel.SetButtonArea("Firmar", 0, 0, 100, 50);
            _signaturePanel.SetButtonArea("Borrar", 0, 60, 100, 50);

            _signaturePanel.AddEventListener(this);
        }

        public void SignatureStarted()
        {
            
        }

        public void SignatureCleared()
        {
            
        }

        public Task ButtonClick(SealSignBSSPanelButtonEvent e)
        {
            return Task.FromResult<int>(0);
        }
    }
}
