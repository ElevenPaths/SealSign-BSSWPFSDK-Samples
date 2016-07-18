using SealSignBSSClientLibrary;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SealSignBSSClientTest.Samples
{
    public abstract class SampleBase : UserControl
    {
        protected static ISealSignBSSPanel _signaturePanel;

        static SampleBase()
        {
            _signaturePanel = SealSignBSSPanelFactory.GetSealSignBSSPanel();
        }

        public SampleBase()
        {
            this.Loaded += SampleBase_Loaded;
            this.Unloaded += SampleBase_Unloaded;
        }

        protected virtual void SampleBase_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_signaturePanel != null)
            {
                _signaturePanel.Stop();
                //_signaturePanel.Disconnect();               
            }
        }

        protected virtual void SampleBase_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //_signaturePanel = SealSignBSSPanelFactory.GetSealSignBSSPanel();
            if (_signaturePanel != null)
                _signaturePanel.Start();
        }

        protected void OnBorrarClick(object sender, RoutedEventArgs e)
        {
            if (_signaturePanel != null)
                _signaturePanel.CleanSignature();
        }
    }
}
