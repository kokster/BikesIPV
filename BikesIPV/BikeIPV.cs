using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu;
using Emgu.CV.UI;
using System.Drawing;

namespace BikesIPV
{
    class BikeIPV
    {
        // picture
        public int Width { get; set; }
        public int Height { get; set; }
        


        public Image<Gray,Byte> init(Image<Bgr, Byte>  image, ImageBox imageBox1)
        {
            // First turn the color depth to 8bits
            Image<Gray, Byte> greyImage;
            greyImage = image.Convert<Gray, Byte>().PyrUp().PyrDown();

            // Value for which the colour depth of each single pixel is taken into 
            // consideration. (Now <= than 255 are taken into consideration)
            Gray maxDepthColor = new Gray(255);


            Image<Gray, Byte> testImage = greyImage;
            int thresholdValue = 25;
            int increaseStep = 5;
           
            // TODO -> check the average color of the picture. 
            //while ( increaseStep < 20 ) {
                // Threshold value
//                Gray thresholdGray = new Gray(5 + increaseStep);
              Gray thresholdGray = new Gray(35);

            // Blocksize of the chuncks getting check to determine
            // average gray value each pixel.
            int blocksize = 77;
                // Apply the thresholding
                testImage = greyImage.ThresholdAdaptive(maxDepthColor, Emgu.CV.CvEnum.ADAPTIVE_THRESHOLD_TYPE.CV_ADAPTIVE_THRESH_GAUSSIAN_C, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY_INV, blocksize, thresholdGray);
                // Get the height and assume the tyres are on the lower part
                increaseStep += 5;
           // }
            

            int halfHeight = greyImage.Height / 2;
            int halfWidth = greyImage.Width / 2;

            this.Width = image.Width;
            this.Height = image.Height;

            return testImage;
        }

        public Image<Gray, Byte> findWheels(Image<Gray, Byte> imgToPro)
        { 
            // Detects the circles using the HoughCircles algorithm.
            // The important parameters are 
            // Circle accumulator threshold
            double cannyThreshold = 200;
            double circleAccumulatorThreshold = 200;
            double resolutionOfAccumulator = 2.0;
            double minDist = 20;
            int minRadius = 50;
            int maxRadius = 500;

            

            // Take 2 smaller pieces of the picture and test that on them
            Rectangle rect = new Rectangle(0, 0+(this.Height/3), this.Width, this.Height - this.Height/3 );
            imgToPro.ROI = rect;
           // Type avg = imgToPro.GetAverage(imgToPro);

            // The following is done for the sake of speed.
            // Since the wheels should be on the bottom part of the image,
            // then 1/4 of the picture height should be the maximum radius.
            maxRadius = rect.Height/2;
            minRadius = rect.Width/ 10;


            int[] valuesToTest = new int[] { 10, 9, 8, 5, 3, 12 };


            // The minimum radius is 
            List<CircleF[]> circles = new List<CircleF[]>();

            foreach (int i in valuesToTest)
            {
                minRadius = rect.Width / i;
                circles.Add(imgToPro.HoughCircles(new Gray(cannyThreshold), new Gray(circleAccumulatorThreshold), resolutionOfAccumulator, minDist, minRadius, maxRadius)[0]);
                Console.WriteLine("Hello");
            }

             
            //CircleF[] circles = imgToPro.HoughCircles(new Gray(125), new Gray(255),2,100,50,500)[0];
            Image<Gray, Byte> circleImage = imgToPro.CopyBlank();

            for(int i = 0; i < circles.Count; i++)
            {
                for(int z = 0; z < circles[i].Length; z++)
                {
                    circleImage.Draw(circles[i][z], new Gray(255), 2);
                    Console.WriteLine(circles[i][z]);
                }
            }
            
                
           // circleImage.Draw(rect, new Gray(255), 30);


            return circleImage;
        }





    }



}
