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
using System.IO;
using Emgu;
using System.Resources;

namespace BikesIPV
{



    public partial class Form1 : Form
    {
        // Capture object 
        private Capture capture;
        private bool captureInProgress = false;



        public Form1()
        {
            InitializeComponent();
            capture = new Capture();
            // comboBox2.Items.Add(RemoteMgr.ExposeProperty
            // comboBox2.Items.Add(Properties.Resources.ResourceManager.BaseName);
            getMeTheResources();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            //TODO: TRY TO DISPLAY ORIGINAL PICTURE IN IMAGEBOX 2

            String direction = "Left";
            String selectedImg = comboBox2.SelectedItem.ToString();
            try
            {
                direction = leftRightCombo.SelectedValue.ToString();
                //selectedImg 
            }
            catch (NullReferenceException ev)
            {
                Console.WriteLine(ev);
            }
            BikeIPV bIPV = new BikeIPV(direction);
            // TODO 
            // Resize the picture .resize() 
            // Dummy data
            //Image<Bgr, Byte> image = new Image<Bgr, byte>(Application.StartupPath + @"\..\..\Resources" + selectedImg);
            //Image<Bgr, Byte> image = new Image<Bgr, Byte>(Properties.Resources.bike3);
            string imgPath = Application.StartupPath.Replace("bin\\Debug", "Resources") + "\\" + selectedImg;
            Image<Bgr, Byte> image = new Image<Bgr, Byte>(@imgPath);//.Resize(400, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true);
            
            //.Resize(500, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true );
            Image <Gray, Byte> imageReady = bIPV.init(image, imageBox1);
            imageBox1.Image = imageReady;


            //  this.Width = bIPV.Width;
            imageBox1.Width = bIPV.Width;
            //  this.Height = bIPV.Height;
            imageBox1.Height = bIPV.Height;
            // this.Width = bIPV.Width;
            imageBox2.Width = bIPV.Width;
            // this.Height = bIPV.Height;
            imageBox2.Height = bIPV.Height;






            imageBox1.Image = bIPV.findWheels(imageReady);//.Resize(400, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true) ;
            imageBox2.Image = image; //Ready;
         
            //Image<Bgr, Byte> imageTest1 = new Image<Bgr, Byte>(BikesIPV.Properties.Resources.Test_With);
            // Image<Bgr, Byte> imageTest2 = new Image<Bgr, Byte>(BikesIPV.Properties.Resources.Test_Without);
             //Image<Bgr, Byte> processed;
            
             //processed = bIPV.FindDifference(imageTest1, imageTest2);
            // grayProcessed = processed.Convert<Gray, Byte>();

             //imageBox2.Image = processed;
            //   imageBox1.Image = bIPV.findWheels(grayProcessed);

        }

        //generates a list of all the bike resource pictures and adds them to comboBox2.
        private void getMeTheResources()
        {
            string[] fileEntries = Directory.GetFiles(Application.StartupPath.Replace("bin\\Debug", "Resources"));
            foreach (string fileName in fileEntries)
            {
                comboBox2.Items.Add(Path.GetFileName(fileName));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (capture == null)
            {
                try
                {
                    capture = new Capture();
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
            }

            if (capture != null)
            {
                if (captureInProgress)
                {  //if camera is getting frames then stop the capture and set button Text
                    // "Start" for resuming capture
                    button1.Text = "Start!"; //
                    Application.Idle -= ProcessFrame;
                }
                else
                {
                    //if camera is NOT getting frames then start the capture and set button
                    // Text to "Stop" for pausing capture
                    button1.Text = "Stop";
                    Application.Idle += ProcessFrame;
                }

                captureInProgress = !captureInProgress;
            }
        }

       private void ProcessFrame(object sender, EventArgs arg)
        {
            Image<Bgr, Byte> ImageFrame = capture.QueryFrame();  //line 1
            imageBox1.Image = ImageFrame;        //line 2
            //Image<Gray, Byte> grayFrame = 
            //ImageFrame[0, 20] = new Bgr(Color.Red);
            //createSquare(ImageFrame);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string path;
            openFileDialog1.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            openFileDialog1.InitialDirectory = 
            path = openFileDialog1.InitialDirectory;
            openFileDialog1.Title = "Please select an image file to encrypt.";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Encrypt the selected file. I'll do this later. :)
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            String direction = "Left";
            try
            {
                direction = leftRightCombo.SelectedValue.ToString();
            }
            catch (NullReferenceException ev)
            {
                Console.WriteLine(ev);
            }
            BikeIPV bIPV = new BikeIPV(direction);

            if (comboBox2.SelectedIndex != -1)
            {
                Image<Bgr, Byte> image = new Image<Bgr, Byte>(BikesIPV.Properties.Resources.bike_white2);//.Resize(500, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true );
                Image<Gray, Byte> imageReady = bIPV.init(image, imageBox1);
                imageBox1.Image = imageReady;
            }
        }
    }
    }

