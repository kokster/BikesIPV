using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu;


namespace BikesIPV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();



            Image<Bgr, Byte> image = new Image<Bgr, byte>(@"C:\Users\preslav\Desktop\BikesIPV\BikesIPV\bin\Debug\bike2.jpg");

            this.Width = image.Width;
            imageBox1.Width = image.Width;
            this.Height = image.Height;
            imageBox1.Height = image.Height;

            // First turn the color depth to 8bits
            Image<Gray, Byte> greyImage;
            greyImage = image.Convert<Gray, Byte>();

            // Value for which the colour depth of each single pixel is taken into 
            // consideration. (Now <= than 255 are taken into consideration)
            Gray maxDepthColor = new Gray(255);

            // Threshold value
            Gray thresholdValue = new Gray(30);
            // Blocksize of the chuncks getting check to determine
            // average gray value each pixel.
            int blocksize = 77;
            // Apply the thresholding
            greyImage = greyImage.ThresholdAdaptive(maxDepthColor ,  Emgu.CV.CvEnum.ADAPTIVE_THRESHOLD_TYPE.CV_ADAPTIVE_THRESH_GAUSSIAN_C, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY_INV, blocksize , thresholdValue);
            

            



            imageBox1.Image = image;

        }

    }
}
