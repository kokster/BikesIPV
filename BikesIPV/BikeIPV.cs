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
        private bool crankLeft;
        
        // Last instance of the found bike
        private List<PointF> centers;
        private Point centerofwheel;
        private List<CircleF> circleslist;
        private double distancebetweenwheels;
        private int radint;
        private double length;
        private Rectangle crank;
        private List<Rectangle> wheelOutlines = new List<Rectangle>();

        // If the tutorial point is set, 
        // then an line is going to show 
        // where the repair can potentially 
        // happen.
        Point tutorialPoint;
        
        public Rectangle Crank
        {
            get { return crank; }
            set { crank = value; }
        }


        public bool CrankLeft
        {
            get { return crankLeft; }
        }

        private int timer1 = 30;

        public bool SetCrank(String value)
        {
            this.crankLeft = (value == "Right" ? false : true);
            return this.crankLeft;
        }
       

        public BikeIPV(String value)
        {
            SetCrank(value);
        }

        /// <summary>
        /// Initialize the class preferences
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageBox1"></param>
        /// <returns></returns>
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
            // Threshold value
            Gray thresholdGray = new Gray(35);
            // Blocksize of the chuncks getting check to determine
            // average gray value each pixel.
            int blocksize = 77;
            // Apply the thresholding
            testImage = greyImage.ThresholdAdaptive(maxDepthColor, Emgu.CV.CvEnum.ADAPTIVE_THRESHOLD_TYPE.CV_ADAPTIVE_THRESH_GAUSSIAN_C, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY_INV, blocksize, thresholdGray);
            // Get the height and assume the tyres are on the lower part
            increaseStep += 5;
            

            int halfHeight = greyImage.Height / 2;
            int halfWidth = greyImage.Width / 2;

            this.Width = image.Width;
            this.Height = image.Height;

            greyImage.Dispose();
            return testImage;
        }

        /// <summary>
        /// Find the wheels and run the rest of code.
        /// </summary>
        /// <param name="imgToPro"></param>
        /// <param name="imgToDrawOn"></param>
        /// <param name="isRealTime"></param>
        /// <returns></returns>
        public Image<Gray, Byte> process(Image<Gray, Byte> imgToPro, Image<Bgr,Byte> imgToDrawOn,bool isRealTime, String repairSubject)
        { 
            // Detects the circles using the HoughCircles algorithm.
            // The important parameters are 
            // Circle accumulator threshold
            double cannyThreshold = 200;
            double circleAccumulatorThreshold =300;
            double resolutionOfAccumulator = 2.0;
            double minDist = 500;//300;
            int minRadius = 150;
            int maxRadius = 300;
            timer1--;
            if (isRealTime)
            {
                minRadius = 50;
            }


            imgToPro.SmoothGaussian(333);
            // Take 2 smaller pieces of the picture and test that on them
            Rectangle rect = new Rectangle(0, 0+(this.Height/3), this.Width, this.Height - this.Height/3 );
            imgToPro.ROI = rect;
            imgToDrawOn.ROI = rect;
            // Type avg = imgToPro.GetAverage(imgToPro);
            
                // The following is done for the sake of speed.
                // Since the wheels should be on the bottom part of the image,
                // then 1/4 of the picture height should be the maximum radius.
                maxRadius = rect.Height / 2;
                // minRadius = rect.Width/ 10;
            


            double[] valuesToTest = new double[] { 1,1.25,1.5,1.75, 2,2.5, 3, 4, 5, 6,7,8,9,10 };


            // The minimum radius is 
            List<CircleF[]> circles = new List<CircleF[]>();
            List<PointF> centers = new List<PointF>();

            foreach (int i in valuesToTest)
            {
                circleAccumulatorThreshold = rect.Width / i +1;
                circles.Add(imgToPro.HoughCircles(new Gray(cannyThreshold), new Gray(circleAccumulatorThreshold), resolutionOfAccumulator, minDist, minRadius, maxRadius)[0]);
                //Console.WriteLine(circles);
            }
            


            List<CircleF> circlesList = new List<CircleF>();
            int radInt = 0;
            // Draws point on the circles where the rays are
            // We are working in a two dimensional array [][].
            Image<Gray, Byte> circleImage = imgToPro.CopyBlank();
            Point centerOfWheel = new Point();
            for (int i = 0; i < circles.Count; i++)
            {
                for(int z = 0; z < circles[i].Length; z++)
                {
                    circlesList.Add(circles[i][z]);
                    //CircleF
                    PointF cpoint = circles[i][z].Center;
                   centers.Add(cpoint);
                   float rad = circles[i][z].Radius;
                   radInt = Convert.ToInt32(rad);
                    if (centers.Count > 2) break;
                   // Console.WriteLine(circles[i][z]);
                }
            }
            // Distance between the wheels
            double length = 0;
            // Connect the two points
            if (centers.Count > 2)
            {
                float startX = centers[0].X;
                float startY = centers[0].Y;
                float endX = centers[1].X;
                float endY = centers[1].Y;
                Point start = new Point((int)startX, (int)startY);
                Point end = new Point((int)endX, (int)endY);
                length = new LineSegment2DF(start, end).Length;
                
            }
            if (isRealTime)
            {

                // if the bike is found
                if (timer1 > 0)
                {
                    // draw the last bike
                    if (this.centers != null)
                    {
                        circleImage = drawEverything(this.centers, imgToPro, this.centerofwheel, imgToDrawOn, this.circleslist, this.length, radInt);
                    }
                }
                else
                {
                    // draw the new found bike
                    if (isBikeFound(length, imgToPro))
                    {
                        centerOfWheel = detectRightWheelCenter(centerOfWheel, centers, imgToPro);
                        circleImage = drawEverything(centers, imgToPro, centerOfWheel, imgToDrawOn, circlesList, length, radInt);
                        this.centers = centers;
                        this.centerofwheel = centerOfWheel;
                        this.circleslist = circlesList;
                        this.radint = radInt;
                        this.length = length;
                        this.distancebetweenwheels = length;
                        timer1 = 30;

                        // pass coloured image
                        imgToDrawOn = highlightPartThatNeedsFixing(repairSubject , imgToDrawOn);


                    }
                }
            }
            else
            {
                //Console.WriteLine("sadsadsa");
                if (isBikeFound(length, imgToPro))
                {
                    centerOfWheel = detectRightWheelCenter(centerOfWheel, centers, imgToPro);
                    circleImage = drawEverything(centers, imgToPro, centerOfWheel, imgToDrawOn, circlesList, length, radInt);
                    imgToDrawOn = highlightPartThatNeedsFixing(repairSubject, imgToDrawOn);


                }
            }



            // 


            
            
            return circleImage;
        }

        /// <summary>
        /// Draws on the image provided. 
        /// It draws wheels, frame and crank.
        /// </summary>
        /// <param name="centers"></param>
        /// <param name="imgToPro"></param>
        /// <param name="centerOfWheel"></param>
        /// <param name="imgToDrawOn"></param>
        /// <param name="circlesList"></param>
        /// <param name="distanceBetweenWheels"></param>
        /// <param name="radInt"></param>
        /// <returns></returns>
        private Image<Gray, Byte> drawEverything(List<PointF> centers, Image<Gray, Byte> imgToPro, Point centerOfWheel, Image<Bgr, Byte> imgToDrawOn, List<CircleF> circlesList, double distanceBetweenWheels, int radInt)
        {

            for (int i = 0; i < centers.Count; i++)
            {

                if (isBikeFound(distanceBetweenWheels, imgToPro))
                {
                    
                    PointF cpoint = centers[i];
                    imgToDrawOn.Draw(circlesList[i], new Bgr(0, 0, 255), 2);
                    imgToDrawOn.Draw(new Rectangle((int)cpoint.X, (int)cpoint.Y, 1, 1), new Bgr(0, 0, 0), 10);
                    //OUTLINES THE TIRES WITH A RECTANGLE
                    Rectangle rec;
                    imgToDrawOn.Draw((rec = new Rectangle((int)cpoint.X - radInt - 10, (int)cpoint.Y - radInt - 10, radInt * 2 + 10, radInt * 2 + 10)), new Bgr(0, 0, 255), 5);
                    wheelOutlines.Add(rec);
                    detectCrank(imgToDrawOn, centerOfWheel, radInt * 2);
                }
            }
            return imgToPro;
        }



        private Image<Bgr,Byte> highlightPartThatNeedsFixing(String tutorialSubject, Image<Bgr, Byte>img)
        {
            if(tutorialSubject == "Flat Front Tire")
            {
                img = highlightPartThatNeedsFixing(true, false, false, img);

            } else if (tutorialSubject == "Flat Back Tire")
            {
                img = highlightPartThatNeedsFixing(false, true, false, img);


            } else
            {
                img = highlightPartThatNeedsFixing(false, false, true, img);
            }

            return img;
        }


        public Image<Bgr, Byte> highlightPartThatNeedsFixing(bool leftTire, bool rightTire, bool crank, Image<Bgr,Byte> img)
        {
            int leftestX = img.Width/2;
            int rightestX = img.Width/2;
            foreach (Rectangle rec in wheelOutlines)
            {
                if (rec.X<leftestX)
                {
                    leftestX = rec.X;
                }
                if (rec.X>rightestX)
                {
                    rightestX = rec.X;
                }
            }

            if (leftTire)
            {
                img.Draw(new Rectangle(leftestX,wheelOutlines[0].Y,wheelOutlines[0].Width,wheelOutlines[0].Height), new Bgr(0, 255, 0), 8);
            }
            if (rightTire)
            {
                img.Draw(new Rectangle(rightestX, wheelOutlines[0].Y, wheelOutlines[0].Width, wheelOutlines[0].Height), new Bgr(0, 255, 0), 8);
            }
            if (crank)
            {
                int crankX = this.Crank.X;
                int crankY = this.Crank.Y;
                int crankWidth = this.Crank.Width;
                int crankHeight = this.Crank.Height;
                img.Draw(new Rectangle(crankX, crankY, crankWidth, crankHeight), new Bgr(0, 255, 0), 8);
                
                
            }

            return img;
        }



        private Image<Bgr,Byte> traceTutorialLine(Image<Bgr, Byte> imgToPro, Point start, Point endPoint)
        {
            imgToPro.Draw(new LineSegment2DF(start, endPoint), new Bgr(0,0,255), 2);
            return imgToPro;

        }

       
        /// <summary>
        /// Final check to know whether the bike is found. It takes the distance of the two wheels
        /// center to guess that. The distance should be greater than a given number. (in this case 
        /// the width of the image is taken into consideration and divided by n) 
        /// </summary>
        /// <param name="length"></param>
        /// <param name="imgToPro"></param>
        /// <returns></returns>
        private bool isBikeFound(double length,  Image<Gray, Byte> imgToPro)
        {
            if(length > (imgToPro.Width / 5))
            {
                
                Console.WriteLine("found bike ");
                return true;
            }

            return false;
           
        }

        /// <summary>
        /// It detects where the left or right wheel (based on the user preference) can be. 
        //  It will do that based on the width of the image. One wheel should be on the left and 
        //  the other one on the right hand side. 
        /// </summary>
        /// <param name="centerOfWheel"></param>
        /// <param name="centers"></param>
        /// <param name="imgToPro"></param>
        /// <returns></returns>
        private Point detectRightWheelCenter(Point centerOfWheel, List<PointF> centers, Image<Gray, Byte> imgToPro)
        {

            for (int i = 0; i < centers.Count; i++)
            {
                // crank is left
                if (this.crankLeft)
                {
                    // get leftest point
                    if (centers[i].X < imgToPro.Width / 2)
                    {
                        centerOfWheel.X = (int)centers[i].X;
                        centerOfWheel.Y = (int)centers[i].Y;
                        break;
                    }

                }
                else
                {
                    // get rightest point
                    if (centers[i].X > imgToPro.Width / 2)
                    {
                        centerOfWheel.X = (int)centers[i].X;
                        centerOfWheel.Y = (int)centers[i].Y;
                        break;
                    }
                }
            }
            return centerOfWheel;
        }

        // Tries to guess the crank being given the wheel diameter and their centers
        private void detectCrank(Image<Bgr, Byte> imgToPro, Point wheelsCenter, int wheelDiameter)
        {
            int crankWidth = Convert.ToInt32((wheelDiameter - (0.1* wheelDiameter)));
            int crankHeight = (int)crankWidth / 4;
                // Assume the crank is on the left hand side
                imgToPro.Draw(crank = new Rectangle(wheelsCenter.X - ( this.crankLeft ? 0 : crankWidth) , wheelsCenter.Y - wheelDiameter / 10 , crankWidth, Convert.ToInt32(crankHeight*1.5)), new Bgr(0,0,255), 4);
           
            
            // TODO -> fill the elemnts the user should be working in!     
            //if (this.tutorialPoint == "crank")
        //        imgToPro.Draw(new Rectangle(wheelsCenter.X - (this.crankLeft ? 0 : crankWidth), wheelsCenter.Y - wheelDiameter / 10, crankWidth, Convert.ToInt32(crankHeight * 1.5)), new Bgr(0, 0, 255), 4)
            // Draw where the center of the crank is 
            //imgToPro.Draw(new Rectangle(wheelsCenter.X - wheelDiameter / 10, wheelsCenter.Y - wheelDiameter / 10, 1, 1), new Gray(125), 1);

        }

        //Find the difference between 2 pictures to be tried with BIKE PICTURES!!
        // Test method: attempts to find the difference between two pictures. 
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
