using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.IO;
using SolCom.iOS;
using SolCom.Clases;

[assembly: Dependency(typeof(SaveIOS))]
namespace SolCom.iOS
{
    class SaveIOS : ISave
	{
		public async Task SaveTextAsync(string filename, string contentType, MemoryStream s)
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			string filePath = Path.Combine(path, filename);
			try
			{
				FileStream fileStream = File.Open(filePath, FileMode.Create);
				s.Position = 0;
				s.CopyTo(fileStream);
				fileStream.Flush();
				fileStream.Close();
			}
			catch (Exception e)
			{

			}


		}

		public string sRuta()
		{
			string root = null;
			root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			return root;
		}
	}
}