// Script para redução de pixels e redução de Imagem(pixel art) - Form1.cs
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PngParaIcoConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 2048;
            numericUpDown1.Value = 64;
        }

        private void btnConverter_Click( object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Imagens|*.png;*.jpg;*.jpeg";

            if (openFileDialog.ShowDialog()
                == DialogResult.OK)
            {
                string imagePath = openFileDialog.FileName;

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg";
                saveFileDialog.Title = "Salvar imagem pixelada";
                saveFileDialog.FileName = Path.GetFileNameWithoutExtension(imagePath) + "_pixelado";

                if (saveFileDialog.ShowDialog()
                    == DialogResult.OK)
                {
                    string outputPath = saveFileDialog.FileName;
                    int targetSize = (int)numericUpDown1.Value;
                    PixelateImage( imagePath, outputPath, targetSize);

                    FileInfo originalFile = new FileInfo(imagePath);
                    FileInfo newFile = new FileInfo(outputPath);

                    MessageBox.Show(
                        "Imagem salva!\n\n" +
                        $"Original: {originalFile.Length / 1024} KB\n" +
                        $"Nova: {newFile.Length / 1024} KB"
                    );
                }
            }
        }

        public static void PixelateImage(string inputPath, string outputPath, int targetSize)
        {
            using (Bitmap original = new Bitmap(inputPath))
            {
                int originalWidth = original.Width;
                int originalHeight = original.Height;
                float aspectRatio = (float)originalWidth / originalHeight;

                int reducedWidth;
                int reducedHeight;

                // mantém proporção
                if (originalWidth >= originalHeight)
                {
                    reducedWidth = targetSize;
                    reducedHeight = Math.Max(1, (int)(targetSize / aspectRatio));
                }
                else
                {
                    reducedHeight = targetSize;
                    reducedWidth = Math.Max( 1, (int)(targetSize * aspectRatio));
                }

                using (Bitmap small = new Bitmap( reducedWidth, reducedHeight))

                using (Bitmap finalImage = new Bitmap( originalWidth, originalHeight))
                {
                    // REDUZ
                    using (Graphics g = Graphics.FromImage(small))
                    {
                        g.InterpolationMode = InterpolationMode.NearestNeighbor;
                        g.SmoothingMode = SmoothingMode.None;
                        g.PixelOffsetMode = PixelOffsetMode.Half;
                        g.CompositingQuality = CompositingQuality.HighSpeed;

                        g.DrawImage(
                            original,
                            0,
                            0,
                            reducedWidth,
                            reducedHeight);
                    }

                    // AMPLIA
                    using (Graphics g = Graphics.FromImage(finalImage))
                    {
                        g.InterpolationMode = InterpolationMode.NearestNeighbor;
                        g.SmoothingMode = SmoothingMode.None;
                        g.PixelOffsetMode = PixelOffsetMode.Half;
                        g.CompositingQuality = CompositingQuality.HighSpeed;
                        g.DrawImage( small, 0, 0, originalWidth, originalHeight);
                    }

                    // detecta extensão
                    string extension = Path.GetExtension(outputPath) .ToLower();

                    // PNG
                    if (extension == ".png")
                    {
                        finalImage.Save( outputPath, ImageFormat.Png);
                    }

                    // JPG
                    else if ( extension == ".jpg" || extension == ".jpeg")
                    {
                        ImageCodecInfo jpgEncoder = ImageCodecInfo.GetImageEncoders().First(codec =>
                                codec.FormatID == ImageFormat.Jpeg.Guid);

                        EncoderParameters encoderParameters = new EncoderParameters(1);

                        // qualidade JPG
                        encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 40L);
                        finalImage.Save(outputPath, jpgEncoder, encoderParameters);
                    }
                }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}



//// Script para redução de pixels (pixel art) - Form1.cs
//using System;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Drawing.Imaging;
//using System.IO;
//using System.Windows.Forms;

//namespace PngParaIcoConverter
//{
//    public partial class Form1 : Form
//    {
//        public Form1()
//        {
//            InitializeComponent();

//            // valores recomendados
//            numericUpDown1.Minimum = 1;
//            numericUpDown1.Maximum = 2048;
//            numericUpDown1.Value = 64;
//        }

//        private void btnConverter_Click(
//            object sender,
//            EventArgs e)
//        {
//            OpenFileDialog openFileDialog =
//                new OpenFileDialog();

//            openFileDialog.Filter =
//                "Imagens|*.png;*.jpg;*.jpeg";

//            if (openFileDialog.ShowDialog()
//                == DialogResult.OK)
//            {
//                string imagePath =
//                    openFileDialog.FileName;

//                SaveFileDialog saveFileDialog =
//                    new SaveFileDialog();

//                saveFileDialog.Filter =
//                    "PNG Image|*.png";

//                saveFileDialog.Title =
//                    "Salvar imagem pixelada";

//                saveFileDialog.FileName =
//                    Path.GetFileNameWithoutExtension(imagePath)
//                    + "_pixelado.png";

//                if (saveFileDialog.ShowDialog()
//                    == DialogResult.OK)
//                {
//                    string outputPath =
//                        saveFileDialog.FileName;

//                    // tamanho escolhido
//                    int targetSize =
//                        (int)numericUpDown1.Value;

//                    PixelateImage(
//                        imagePath,
//                        outputPath,
//                        targetSize);

//                    MessageBox.Show(
//                        "Imagem salva com sucesso!"
//                    );
//                }
//            }
//        }

//        public static void PixelateImage(
//            string inputPath,
//            string outputPath,
//            int targetSize)
//        {
//            using (Bitmap original =
//                new Bitmap(inputPath))
//            {
//                int originalWidth =
//                    original.Width;

//                int originalHeight =
//                    original.Height;

//                float aspectRatio =
//                    (float)originalWidth
//                    / originalHeight;

//                int reducedWidth;
//                int reducedHeight;

//                // mantém proporção automaticamente
//                if (originalWidth >= originalHeight)
//                {
//                    reducedWidth =
//                        targetSize;

//                    reducedHeight =
//                        Math.Max(
//                            1,
//                            (int)(targetSize / aspectRatio));
//                }
//                else
//                {
//                    reducedHeight =
//                        targetSize;

//                    reducedWidth =
//                        Math.Max(
//                            1,
//                            (int)(targetSize * aspectRatio));
//                }

//                // imagem reduzida
//                using (Bitmap small =
//                    new Bitmap(
//                        reducedWidth,
//                        reducedHeight))

//                // imagem final
//                using (Bitmap finalImage =
//                    new Bitmap(
//                        originalWidth,
//                        originalHeight))
//                {
//                    // REDUZ
//                    using (Graphics g =
//                        Graphics.FromImage(small))
//                    {
//                        g.InterpolationMode =
//                            InterpolationMode.NearestNeighbor;

//                        g.SmoothingMode =
//                            SmoothingMode.None;

//                        g.PixelOffsetMode =
//                            PixelOffsetMode.Half;

//                        g.CompositingQuality =
//                            CompositingQuality.HighSpeed;

//                        g.DrawImage(
//                            original,
//                            0,
//                            0,
//                            reducedWidth,
//                            reducedHeight);
//                    }

//                    // AMPLIA novamente
//                    using (Graphics g =
//                        Graphics.FromImage(finalImage))
//                    {
//                        g.InterpolationMode =
//                            InterpolationMode.NearestNeighbor;

//                        g.SmoothingMode =
//                            SmoothingMode.None;

//                        g.PixelOffsetMode =
//                            PixelOffsetMode.Half;

//                        g.CompositingQuality =
//                            CompositingQuality.HighSpeed;

//                        g.DrawImage(
//                            small,
//                            0,
//                            0,
//                            originalWidth,
//                            originalHeight);
//                    }

//                    finalImage.Save(
//                        outputPath,
//                        ImageFormat.Png);
//                }
//            }
//        }

//        private void numericUpDown1_ValueChanged(
//            object sender,
//            EventArgs e)
//        {

//        }
//    }
//}


////// Script para converter PNG em ICO - Form1.cs
//using System;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Drawing.Imaging;
//using System.IO;
//using System.Linq;
//using System.Windows.Forms;

//namespace PngParaIcoConverter
//{
//    public partial class Form1 : Form
//    {
//        public Form1()
//        {
//            InitializeComponent();
//        }

//        private void btnConverter_Click(object sender, EventArgs e)
//        {
//            OpenFileDialog openFileDialog = new OpenFileDialog();

//            openFileDialog.Filter = "PNG Image|*.png";

//            if (openFileDialog.ShowDialog() == DialogResult.OK)
//            {
//                string pngPath = openFileDialog.FileName;

//                SaveFileDialog saveFileDialog = new SaveFileDialog();

//                saveFileDialog.Filter = "Icon File|*.ico";
//                saveFileDialog.Title = "Salvar ícone";

//                saveFileDialog.FileName =
//                    Path.GetFileNameWithoutExtension(pngPath) + ".ico";

//                if (saveFileDialog.ShowDialog() == DialogResult.OK)
//                {
//                    string icoPath = saveFileDialog.FileName;

//                    // resolução escolhida
//                    int tamanho = (int)numericUpDown1.Value;

//                    ConvertToIco(pngPath, icoPath, tamanho);

//                    MessageBox.Show(
//                        $"Ícone {tamanho}x{tamanho} criado em:\n{icoPath}"
//                    );
//                }
//            }
//        }

//        public static void ConvertToIco(
//            string pngPath,
//            string icoPath,
//            int size)
//        {
//            using (Bitmap original = new Bitmap(pngPath))
//            using (Bitmap resized = new Bitmap(size, size))
//            {
//                using (Graphics g = Graphics.FromImage(resized))
//                {
//                    g.Clear(Color.Transparent);

//                    g.InterpolationMode =
//                        InterpolationMode.HighQualityBicubic;

//                    g.SmoothingMode =
//                        SmoothingMode.HighQuality;

//                    g.PixelOffsetMode =
//                        PixelOffsetMode.HighQuality;

//                    g.DrawImage(original, 0, 0, size, size);
//                }

//                using (MemoryStream pngStream = new MemoryStream())
//                {
//                    resized.Save(pngStream, ImageFormat.Png);

//                    byte[] pngBytes = pngStream.ToArray();

//                    using (BinaryWriter writer =
//                        new BinaryWriter(
//                            File.Open(icoPath, FileMode.Create)))
//                    {
//                        // ICONDIR
//                        writer.Write((short)0);
//                        writer.Write((short)1);
//                        writer.Write((short)1);

//                        // ICONDIRENTRY
//                        writer.Write((byte)(size >= 256 ? 0 : size));
//                        writer.Write((byte)(size >= 256 ? 0 : size));

//                        writer.Write((byte)0);
//                        writer.Write((byte)0);

//                        writer.Write((short)1);
//                        writer.Write((short)32);

//                        writer.Write(pngBytes.Length);

//                        writer.Write(22);

//                        // PNG DATA
//                        writer.Write(pngBytes);
//                    }
//                }
//            }
//        }

//        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
//        {

//        }
//    }
//}


//Script para converter PNG em ICO - Form1.cs com botao simples
// using System;
// using System.Drawing;
// using System.Drawing.Drawing2D;
// using System.Drawing.Imaging;
// using System.IO;
// using System.Windows.Forms;

// namespace PngParaIcoConverter
// {
//     public partial class Form1 : Form
//     {
//         public Form1()
//         {
//             InitializeComponent();
//         }

// 		// private void btnConverter_Click(object sender, EventArgs e)
// 		// {
// 			// OpenFileDialog openFileDialog = new OpenFileDialog();

// 			// openFileDialog.Filter = "PNG Image|*.png";

// 			// if (openFileDialog.ShowDialog() == DialogResult.OK)
// 			// {
// 				// string pngPath = openFileDialog.FileName;

// 				// string icoPath = Path.Combine(
// 					// Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
// 					// "logo.ico"
// 				// );

// 				// ConvertTo32x32Ico(pngPath, icoPath);

// 				// MessageBox.Show("Ícone criado em:\n" + icoPath);
// 			// }
// 		// }

// 		private void btnConverter_Click(object sender, EventArgs e)
// 		{
// 			OpenFileDialog openFileDialog = new OpenFileDialog();

// 			openFileDialog.Filter = "PNG Image|*.png";

// 			if (openFileDialog.ShowDialog() == DialogResult.OK)
// 			{
// 				string pngPath = openFileDialog.FileName;

// 				SaveFileDialog saveFileDialog = new SaveFileDialog();

// 				saveFileDialog.Filter = "Icon File|*.ico";
// 				saveFileDialog.Title = "Salvar ícone";

// 				// Nome padrão baseado no PNG
// 				saveFileDialog.FileName = Path.GetFileNameWithoutExtension(pngPath) + ".ico";

// 				if (saveFileDialog.ShowDialog() == DialogResult.OK)
// 				{
// 					string icoPath = saveFileDialog.FileName;

// 					ConvertTo32x32Ico(pngPath, icoPath);

// 					MessageBox.Show("Ícone criado em:\n" + icoPath);
// 				}
// 			}
// 		}

//         public static void ConvertTo32x32Ico(string pngPath, string icoPath)
//         {
//             using (Bitmap original = new Bitmap(pngPath))
//             using (Bitmap resized = new Bitmap(32, 32))
//             {
//                 using (Graphics g = Graphics.FromImage(resized))
//                 {
//                     g.Clear(Color.Transparent);

//                     g.InterpolationMode = InterpolationMode.HighQualityBicubic;
//                     g.SmoothingMode = SmoothingMode.HighQuality;
//                     g.PixelOffsetMode = PixelOffsetMode.HighQuality;

//                     g.DrawImage(original, 0, 0, 32, 32);
//                 }

//                 using (MemoryStream pngStream = new MemoryStream())
//                 {
//                     resized.Save(pngStream, ImageFormat.Png);
//                     byte[] pngBytes = pngStream.ToArray();

//                     using (BinaryWriter writer = new BinaryWriter(File.Open(icoPath, FileMode.Create)))
//                     {
//                         // ICONDIR
//                         writer.Write((short)0);
//                         writer.Write((short)1);
//                         writer.Write((short)1);

//                         // ICONDIRENTRY
//                         writer.Write((byte)32);
//                         writer.Write((byte)32);
//                         writer.Write((byte)0);
//                         writer.Write((byte)0);
//                         writer.Write((short)1);
//                         writer.Write((short)32);
//                         writer.Write(pngBytes.Length);
//                         writer.Write(22);

//                         // PNG
//                         writer.Write(pngBytes);
//                     }
//                 }
//             }
//         }
//     }
// }