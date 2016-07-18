using SealSignBSSClientTest.BiometricAuthenticationService;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SealSignBSSClientTest.Samples
{
    /// <summary>
    /// Interaction logic for SignatureAuthSample.xaml
    /// </summary>
    public partial class SignatureAuthSample : SampleBase
    {
        public SignatureAuthSample()
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

        private async void OnEnrolarClick(object sender, RoutedEventArgs e)
        {
            botonEnrolar.IsEnabled = false;
            botonVerificar.IsEnabled = false;
            botonBorrar.IsEnabled = false;

            _signaturePanel.Stop();

            BiometricAuthenticationServiceBasicClient service = new BiometricAuthenticationServiceBasicClient("WSHttpBinding_IBiometricAuthenticationServiceBasic");

            byte[] biometricFinalState = _signaturePanel.GetSignature(Guid.Empty, null);

            await service.EnrollAsync(BiometricAuthenticationType.Signature,
                biometricFinalState);

            service.Close();

            _signaturePanel.Start();

            botonEnrolar.IsEnabled = true;
            botonVerificar.IsEnabled = true;
            botonBorrar.IsEnabled = true;
        }

        private void OnVerificarClick(object sender, RoutedEventArgs e)
        {
            botonEnrolar.IsEnabled = false;
            botonVerificar.IsEnabled = false;
            botonBorrar.IsEnabled = false;

            double score = 0.0;

            _signaturePanel.Stop();

            BiometricAuthenticationServiceBasicClient service = new BiometricAuthenticationServiceBasicClient();

            byte[] biometricFinalState = _signaturePanel.GetSignature(Guid.Empty, null);

            bool verificationResult = service.Verify(BiometricAuthenticationType.Signature,
                biometricFinalState, out score);

            service.Close();

            _signaturePanel.Start();

            botonEnrolar.IsEnabled = true;
            botonVerificar.IsEnabled = true;
            botonBorrar.IsEnabled = true;
        }
    }
}
