using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace PngToIco
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

		// private void btnConverter_Click(object sender, EventArgs e)
		// {
			// OpenFileDialog openFileDialog = new OpenFileDialog();

			// openFileDialog.Filter = "PNG Image|*.png";

			// if (openFileDialog.ShowDialog() == DialogResult.OK)
			// {
				// string pngPath = openFileDialog.FileName;

				// string icoPath = Path.Combine(
					// Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
					// "logo.ico"
				// );

				// ConvertTo32x32Ico(pngPath, icoPath);

				// MessageBox.Show("Ícone criado em:\n" + icoPath);
			// }
		// }
		
		private void btnConverter_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();

			openFileDialog.Filter = "PNG Image|*.png";

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string pngPath = openFileDialog.FileName;

				SaveFileDialog saveFileDialog = new SaveFileDialog();

				saveFileDialog.Filter = "Icon File|*.ico";
				saveFileDialog.Title = "Salvar ícone";

				// Nome padrão baseado no PNG
				saveFileDialog.FileName = Path.GetFileNameWithoutExtension(pngPath) + ".ico";

				if (saveFileDialog.ShowDialog() == DialogResult.OK)
				{
					string icoPath = saveFileDialog.FileName;

					ConvertTo32x32Ico(pngPath, icoPath);

					MessageBox.Show("Ícone criado em:\n" + icoPath);
				}
			}
		}

        public static void ConvertTo32x32Ico(string pngPath, string icoPath)
        {
            using (Bitmap original = new Bitmap(pngPath))
            using (Bitmap resized = new Bitmap(32, 32))
            {
                using (Graphics g = Graphics.FromImage(resized))
                {
                    g.Clear(Color.Transparent);

                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    g.DrawImage(original, 0, 0, 32, 32);
                }

                using (MemoryStream pngStream = new MemoryStream())
                {
                    resized.Save(pngStream, ImageFormat.Png);
                    byte[] pngBytes = pngStream.ToArray();

                    using (BinaryWriter writer = new BinaryWriter(File.Open(icoPath, FileMode.Create)))
                    {
                        // ICONDIR
                        writer.Write((short)0);
                        writer.Write((short)1);
                        writer.Write((short)1);

                        // ICONDIRENTRY
                        writer.Write((byte)32);
                        writer.Write((byte)32);
                        writer.Write((byte)0);
                        writer.Write((byte)0);
                        writer.Write((short)1);
                        writer.Write((short)32);
                        writer.Write(pngBytes.Length);
                        writer.Write(22);

                        // PNG
                        writer.Write(pngBytes);
                    }
                }
            }
        }
    }
}