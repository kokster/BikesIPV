﻿using System;
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
            double circleAccumulatorThreshold =300;
            double resolutionOfAccumulator = 2.0;
            double minDist = 400;
            int minRadius = 50;
            int maxRadius = 300;


            imgToPro.SmoothGaussian(333);
            // Take 2 smaller pieces of the picture and test that on them
            Rectangle rect = new Rectangle(0, 0+(this.Height/3), this.Width, this.Height - this.Height/3 );
            imgToPro.ROI = rect;
           // Type avg = imgToPro.GetAverage(imgToPro);

            // The following is done for the sake of speed.
            // Since the wheels should be on the bottom part of the image,
            // then 1/4 of the picture height should be the maximum radius.
            maxRadius = rect.Height/2;
           // minRadius = rect.Width/ 10;


            double[] valuesToTest = new double[] { 1,1.25,1.5,1.75, 2,2.5, 3, 4, 5, 6,7,8,9,10 };


            // The minimum radius is 
            List<CircleF[]> circles = new List<CircleF[]>();
            List<PointF> centers = new List<PointF>();

            foreach (int i in valuesToTest)
            {
                circleAccumulatorThreshold = rect.Width / i;
                circles.Add(imgToPro.HoughCircles(new Gray(cannyThreshold), new Gray(circleAccumulatorThreshold), resolutionOfAccumulator, minDist, minRadius, maxRadius)[0]);
                
                Console.WriteLine(circles);
                
            }

             
            //CircleF[] circles = imgToPro.HoughCircles(new Gray(125), new Gray(255),2,100,50,500)[0];
            Image<Gray, Byte> circleImage = imgToPro.CopyBlank();
            for (int i = 0; i < circles.Count; i++)
            {
                for(int z = 0; z < circles[i].Length; z++)
                {
                    PointF cpoint = circles[i][z].Center;
                    centers.Add(cpoint);
                    float rad = circles[i][z].Radius;
                    circleImage.Draw(circles[i][z], new Gray(255), 2);
                    circleImage.Draw(new Rectangle((int)cpoint.X, (int)cpoint.Y, 1,1 ), new Gray(255), 10);
                    Console.WriteLine(circles[i][z]);
                    


                }
            }
            
            if (centers.Count > 2)
            {
                float startX = centers[1].X;
                float startY = centers[1].Y;
                float endX = centers[2].X;
                float endY = centers[2].Y;
                Point start = new Point((int)startX, (int)startY);
                Point end = new Point((int)endX, (int)endY);
                circleImage.Draw(new LineSegment2DF(start, end),new Gray(255),2);
                

            }
            
                
           // circleImage.Draw(rect, new Gray(255), 30);


            return circleImage;
        }


        //Find the difference between 2 pictures to be tried with BIKE PICTURES!!
        public Image<Bgr,Byte> FindDifference(Image<Bgr, Byte> Frame, Image<Bgr,Byte> Previous_Frame)
        {
        Image<Bgr, Byte> Difference = null; //Difference between the two frames

        double ContourThresh = 0.003; //stores alpha for thread access
        int Threshold = 60; //stores threshold for thread access

            if (Frame == null) //we need at least one fram to work out running average so acquire one before doing anything
            {
               
                
                
                Previous_Frame = Frame.Copy(); //copy the frame to act as the previous

            }
            else
            {
               

                Difference = Frame.AbsDiff(Previous_Frame); //find the absolute difference 
                /*Play with the value 60 to set a threshold for movement*/
                Difference = Difference.ThresholdBinary(new Bgr(Threshold, Threshold, Threshold), new Bgr(255, 255, 255)); //if value > 60 set to 255, 0 otherwise 
                

                Previous_Frame = Frame.Copy(); //copy the frame to act as the previous frame

                #region Draw the contours of difference
                //this is tasken from the ShapeDetection Example
                using (MemStorage storage = new MemStorage()) //allocate storage for contour approximation
                                                              //detect the contours and loop through each of them
                    for (Contour<Point> contours = Difference.Convert<Gray, Byte>().FindContours(
                          Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                          Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST,
                          storage);
    contours != null;
                       contours = contours.HNext)
                    {
                        //Create a contour for the current variable for us to work with
                        Contour<Point> currentContour = contours.ApproxPoly(contours.Perimeter * 0.05, storage);

                        //Draw the detected contour on the image
                        if (currentContour.Area > ContourThresh) //only consider contours with area greater than 100 as default then take from form control
                        {
                            Frame.Draw(currentContour.BoundingRectangle, new Bgr(Color.Red), 2);
                        }
                    }
                #endregion


              
            }
            return Difference;


        }

        



}
}
