using Foundation;
using SolCom.iOS;
using SolCom.UserControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PDFView), typeof(PDFViewRenderer))]
namespace SolCom.iOS
{
    public class PDFViewRenderer : ViewRenderer<PDFView, UIWebView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<PDFView> e)
        {
            base.OnElementChanged(e);

            if (NativeView != null && e.NewElement != null)
            {
                var pdfControl = NativeView as UIWebView;

                if (pdfControl == null)
                    return;

                pdfControl.ScalesPageToFit = true;
            }
        }
    }
}