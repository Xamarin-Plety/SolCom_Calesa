using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using SolCom.Clases;
using SolCom.Droid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveAndroid))]
namespace SolCom.Droid
{
    public class SaveAndroid : ISave
    {

        public async Task SaveTextAsync(string fileName, String contentType, MemoryStream s)
        {
            string root = null;
            //if (Android.OS.Environment.IsExternalStorageEmulated)
            //{
            //    root = Android.OS.Environment.ExternalStorageDirectory.ToString();
            //}
            //else

            //root = Android.OS.Environment.ExternalStorageDirectory.ToString();
            //root = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
            root = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;


            Java.IO.File myDir = new Java.IO.File(root + "/SolCom");
            myDir.Mkdirs();

            Java.IO.File file = new Java.IO.File(myDir, fileName);



            if (file.Exists()) file.Delete();

            try
            {

                FileOutputStream outs = new FileOutputStream(file);
                outs.Write(s.ToArray());

                outs.Flush();
                outs.Close();

            }
            catch (Exception e)
            {
                string sError = e.Message;

            }

        }

        public string sRuta()
        {
            string root = null;
            //root = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
            root = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;

            root += "/SolCom";

            return root;
        }
    }
}