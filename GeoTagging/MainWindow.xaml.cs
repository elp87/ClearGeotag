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
                //LoadedImage.Source = new BitmapImage(new Uri(filename, UriKind.Absolute));

                FileStream photoStream = File.Open(
                    filename,
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite
                    );



                BitmapDecoder decoder = JpegBitmapDecoder.Create(
                    photoStream,
                    BitmapCreateOptions.IgnoreColorProfile,
                    BitmapCacheOption.Default
                    );
                BitmapSource frame = decoder.Frames[0].Clone();
                var thumbnail = decoder.Frames[0].Thumbnail.Clone();
                var colorContexts = decoder.Frames[0].ColorContexts;
                BitmapMetadata exif = (BitmapMetadata)frame.Metadata.Clone();

                

                exif.RemoveQuery("/app1/ifd/gps/{ushort=1}");
                exif.RemoveQuery("/app1/ifd/gps/{ushort=2}");
                exif.RemoveQuery("/app1/ifd/gps/{ushort=3}");
                exif.RemoveQuery("/app1/ifd/gps/{ushort=4}");
                exif.RemoveQuery("/app1/ifd/gps/{ushort=5}");
                exif.RemoveQuery("/app1/ifd/gps/{ushort=6}");
                //photoStream.Close();

                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(
                    BitmapFrame.Create(
                        frame,
                        thumbnail,
                        exif,
                        colorContexts
                        )
                    );
                //photoStream.Close();
                encoder.Save(photoStream);


                /*object data = exif.GetQuery("/app1/ifd/gps/{ushort=1}");

                string dataValue = data.ToString();
                InfoLabel.Content = dataValue;*/

                /*Image image = new Bitmap(filename);

                PropertyItem piLatitudeRef = image.GetPropertyItem(1);
                PropertyItem piLatitude = image.GetPropertyItem(2);
                PropertyItem piLongtitudeRef = image.GetPropertyItem(3);
                PropertyItem piLongtitude = image.GetPropertyItem(4);
                PropertyItem piAltitudeRef = image.GetPropertyItem(5);
                PropertyItem piAltitude = image.GetPropertyItem(6);

                GpsValue latitude = new GpsValue(piLatitudeRef, piLatitude);
                GpsValue longtitude = new GpsValue(piLongtitudeRef, piLongtitude);
                InfoLabel.Content = "Latitude - " + latitude.ToString() + "\n";
                InfoLabel.Content += "Longtitude - " + longtitude.ToString() + "\n";



                UInt32 altN = BitConverter.ToUInt32(piAltitude.Value, 0);
                UInt32 altD = BitConverter.ToUInt32(piAltitude.Value, 4);
                float alt = (float)altN / (float)altD;
                InfoLabel.Content += "Altitude - " + alt.ToString();*/



            }
        }
    }
}
