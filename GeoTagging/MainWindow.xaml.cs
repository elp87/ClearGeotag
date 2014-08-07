using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Globalization;

namespace GeoTagging
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".jpg";
            ofd.Filter = "Jpg Files (*.jpg; *.jpeg)|*.jpg; *.jpeg";
            if (ofd.ShowDialog() == true)
            {
                string filename = ofd.FileName;

                BitmapDecoder decoder = null;
                BitmapFrame frame = null;
                BitmapMetadata metadata = null;

                using (FileStream photoStream = File.Open(
                    filename,
                    FileMode.Open,
                    FileAccess.ReadWrite,
                    FileShare.None
                    ))
                {
                    decoder = JpegBitmapDecoder.Create(
                    photoStream,
                    BitmapCreateOptions.PreservePixelFormat,
                    BitmapCacheOption.OnLoad
                    );
                }

                frame = decoder.Frames[0];
                metadata = (BitmapMetadata)frame.Metadata;

                if (metadata != null)
                {
                    BitmapMetadata newMetaData = (BitmapMetadata)frame.Metadata.Clone();

                    newMetaData.RemoveQuery("/app1/ifd/gps/{ushort=1}");
                    newMetaData.RemoveQuery("/app1/ifd/gps/{ushort=2}");
                    newMetaData.RemoveQuery("/app1/ifd/gps/{ushort=3}");
                    newMetaData.RemoveQuery("/app1/ifd/gps/{ushort=4}");
                    newMetaData.RemoveQuery("/app1/ifd/gps/{ushort=5}");
                    newMetaData.RemoveQuery("/app1/ifd/gps/{ushort=6}");

                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(
                        frame,
                        frame.Thumbnail,
                        newMetaData,
                        frame.ColorContexts
                        ));

                    File.Delete(filename);

                    using (Stream photoStream = File.Open(
                        filename,
                        FileMode.CreateNew,
                        FileAccess.ReadWrite
                        ))
                    {
                        encoder.Save(photoStream);
                    }
                }
            }
        }
    }
}
