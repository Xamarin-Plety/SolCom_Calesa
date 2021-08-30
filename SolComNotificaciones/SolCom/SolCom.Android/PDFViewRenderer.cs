using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SolCom.Droid;
using SolCom.UserControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PDFView), typeof(PDFViewRenderer))]
namespace SolCom.Droid
{
    public class PDFViewRenderer : WebViewRenderer
    {
        public PDFViewRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            /*
            if (e.NewElement != null)
            {
                var pdfView = Element as PDFView;
                Control.Settings.AllowUniversalAccessFromFileURLs = true;
                if (pdfView.IsPDF)
                {
                    Control.LoadUrl("https://drive.google.com/viewerng/viewer?embedded=true&url=" + pdfView.Uri);
                }
                else
                {
                    Control.LoadUrl(pdfView.Uri);
                }
            }
            */
        }

        public void ProcSaveFile(string sArchivo,byte[] bArchivo)
        {
            string path = (string)Android.OS.Environment.DirectoryDownloads;

            // Get the final file name
            string fileName = Path.Combine(path, sArchivo);

            // Save the QR Code as a byte[]
            File.WriteAllBytes(path, bArchivo);
        }
    }
}