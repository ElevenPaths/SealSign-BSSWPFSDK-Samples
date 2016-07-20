using SealSignBSSClientLibrary;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System;
using System.Threading.Tasks;

namespace SealSignBSSClientTest.Samples
{
    public abstract class SampleBase : UserControl, ISealSignBSSEventListener
    {
        protected static ISealSignBSSPanel _signaturePanel;
        protected static string _backgroundPath = string.Empty;

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
                _signaturePanel.RemoveEventListener(this);
                _signaturePanel.Stop();
                //_signaturePanel.Disconnect();               
            }
        }

        protected virtual void SampleBase_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //_signaturePanel = SealSignBSSPanelFactory.GetSealSignBSSPanel();
            if (_signaturePanel != null)
            {
                var deviceId = _signaturePanel.GetDeviceID();
                if (!string.IsNullOrEmpty(deviceId) && string.IsNullOrEmpty(_backgroundPath))
                {
                    if (deviceId.StartsWith("STU-530", System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        _backgroundPath = ConfigurationManager.AppSettings["backgroundPathSTU-530"];
                        _signaturePanel.SetButtonArea("OK", 619, 24, 778, 74);
                        _signaturePanel.SetButtonArea("Cancel", 619, 86, 778, 136);
                    }
                    else if (deviceId.StartsWith("STU-520", System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        _backgroundPath = ConfigurationManager.AppSettings["backgroundPathSTU-520"];
                        _signaturePanel.SetButtonArea("OK", 619, 24, 778, 74);
                        _signaturePanel.SetButtonArea("Cancel", 619, 86, 778, 136);
                    }
                    else if (deviceId.StartsWith("STU-500", System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        _backgroundPath = ConfigurationManager.AppSettings["backgroundPathSTU-500"];
                        _signaturePanel.SetButtonArea("OK", 495, 24, 622, 74);
                        _signaturePanel.SetButtonArea("Cancel", 495, 86, 622, 136);
                    }
                    else if (deviceId.StartsWith("STU-430", System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        _backgroundPath = ConfigurationManager.AppSettings["backgroundPathSTU-430"];
                        _signaturePanel.SetButtonArea("OK", 247, 10, 312, 21);
                        _signaturePanel.SetButtonArea("Cancel", 247, 35, 312, 55);
                    }

                    System.Drawing.Image backgroundImage = System.Drawing.Image.FromFile(_backgroundPath);

                    _signaturePanel.SetBackgroundImage(backgroundImage);                    
                }
                _signaturePanel.AddEventListener(this);
                _signaturePanel.Start();
            }

        }

        protected void OnBorrarClick(object sender, RoutedEventArgs e)
        {
            if (_signaturePanel != null)
                _signaturePanel.CleanSignature();
        }

        internal void Disconnect()
        {
            if (_signaturePanel != null)
                _signaturePanel.Disconnect();
        }

        public void SignatureStarted() { }

        public void SignatureCleared() { }

        public abstract Task ButtonClick(SealSignBSSPanelButtonEvent e);        
    }
}
