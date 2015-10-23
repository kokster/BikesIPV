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


    //MAKE ROI 3.5
    //draw on transparrent panel
    //Limit the number of times the method for calling the circles is called

    public partial class Form1 : Form
    {
        // Capture object 
        private Capture capture;
        private bool captureInProgress = false;
        private int framecounter;
        BikeIPV bIPV;
        private string[,] arrayProblems;
        private int problemIndex;
        private int stepIndex;
        private int picIndex;




        public Form1()
        {
            InitializeComponent();
            capture = new Capture();
            // comboBox2.Items.Add(RemoteMgr.ExposeProperty
            // comboBox2.Items.Add(Properties.Resources.ResourceManager.BaseName);
            getMeTheResources();
            framecounter = 0;
            bIPV = new BikeIPV("Left");
            //problems.Enabled = false;
            btn_next.Enabled = false;
            arrayProblems = new string[3,10];
            arrayProblems[0, 0] = "Step1:Remove The Tire \n Hook the rounded end of one tire lever under the bead (the outer edge) of the tire to unseat it. Hook the other end onto a spoke to hold the lever in place and to keep the unseated tire from popping back into the rim. Hook the rounded end of the second lever under the bead next to the first and walk it around the tire/rim clockwise until one side of the tire is off the rim." ;
            arrayProblems[0, 1] = "Step2:Find The Culprit \n Remove the tube and pump air into it to find the leak. Two holes side by side is a pinch-flat--the tube got pinched between the tire and rim. A single hole was most likely caused by a sharp object such as a thorn or a piece of glass. Carefully run your fingers along the inside of the tire to make sure the foreign object is no longer there. If you don't, it could cause another flat.";
            arrayProblems[0, 2] = "Step3:If You Patch \n Clean the punctured area of the flat tire with an alcohol prep pad and rough the surface with an emery cloth. For a glueless patch, simply stick it over the hole and press firmly. For a patch that requires glue, apply a thin layer of glue to the tube and patch. Wait for the glue to get tacky, then apply the patch and press firmly until it adheres. If you don't patch, stuff the tube into your bag and fix it when you get home. It could be good for another season or more of use.";
            arrayProblems[0, 3] = "Step4:Install the Tube \n Inflate either your patched or new tube until it holds its shape, then insert it into the tire. With the valve stem installed straight, work the tire back into the rim with your hands by rolling the bead away from yourself. (Do not use levers to reseat the tire, as you could puncture the tube.) When you get to the valve stem, tuck both sides of the tire bead low into the rim then push upward on the stem to get the tube up inside the tire. Inflate completely, checking that the bead is seated correctly.";
            arrayProblems[1, 0] = "Step1:Remove The Tire \n Hook the rounded end of one tire lever under the bead (the outer edge) of the tire to unseat it. Hook the other end onto a spoke to hold the lever in place and to keep the unseated tire from popping back into the rim. Hook the rounded end of the second lever under the bead next to the first and walk it around the tire/rim clockwise until one side of the tire is off the rim.";
            arrayProblems[1, 1] = "Step2:Find The Culprit \n Remove the tube and pump air into it to find the leak. Two holes side by side is a pinch-flat--the tube got pinched between the tire and rim. A single hole was most likely caused by a sharp object such as a thorn or a piece of glass. Carefully run your fingers along the inside of the tire to make sure the foreign object is no longer there. If you don't, it could cause another flat.";
            arrayProblems[1, 2] = "Step3:If You Patch \n Clean the punctured area of the flat tire with an alcohol prep pad and rough the surface with an emery cloth. For a glueless patch, simply stick it over the hole and press firmly. For a patch that requires glue, apply a thin layer of glue to the tube and patch. Wait for the glue to get tacky, then apply the patch and press firmly until it adheres. If you don't patch, stuff the tube into your bag and fix it when you get home. It could be good for another season or more of use.";
            arrayProblems[1, 3] = "Step4:Install the Tube \n Inflate either your patched or new tube until it holds its shape, then insert it into the tire. With the valve stem installed straight, work the tire back into the rim with your hands by rolling the bead away from yourself. (Do not use levers to reseat the tire, as you could puncture the tube.) When you get to the valve stem, tuck both sides of the tire bead low into the rim then push upward on the stem to get the tube up inside the tire. Inflate completely, checking that the bead is seated correctly.";
            arrayProblems[2, 0] = "Step1:Unscrew The Bolt/nut \n Depending on the fitting of the square tapered crank bolt or nut, use either a socket or hex key to unscrew it. Make sure you remove any washer that’s under the head of the bolt.";
            arrayProblems[2, 1] = "Step2:Retract The Bolt From The Crank-puller \n A crank-puller is a unit that screws into the threads on the inside of the square tapered crank holes. A bolt that runs through it that jams against the axle to force the square tapered crank off the tapered axle. Retract the bolt from the crank puller. Almost all square tapered cranks use the Italian standard 22mm thread but some old French ‘Stronglight’ or ‘T.A.’ cranks need a different crank-puller.";
            arrayProblems[2, 2] = "Step3:Mount The Crank-puller \n Check the threads on the inside of the square tapered crank hole are clean and apply a smear of grease. Screw the crank-puller into the threads, do this carefully as the threads on the aluminium square tapered crank are soft and easily damaged. Finger strength is usually enough.";
            arrayProblems[2, 3] = "Step4:Drive The Bolt into the square tapered crank \n When the crank-puller is fully in the square tapered crank, screw in the bolt on the crank-puller, it will jam against the axle and force the crank off the taper.";
            arrayProblems[2, 4] = "Step5:Remove The Tool And Repeat \n Unscrew the tool from thesquare tapered crank arm and repeat the operation on the other side.";

        }

        private void button4_Click(object sender, EventArgs e)
        {

            //TODO: TRY TO DISPLAY ORIGINAL PICTURE IN IMAGEBOX 2

            String direction = "Left";
            String selectedImg = comboBox2.SelectedItem.ToString();

            
            direction = leftRightCombo.SelectedItem.ToString();
            // TODO 
            // Resize the picture .resize() 
            // Dummy data
            //Image<Bgr, Byte> image = new Image<Bgr, byte>(Application.StartupPath + @"\..\..\Resources" + selectedImg);
            //Image<Bgr, Byte> image = new Image<Bgr, Byte>(Properties.Resources.bike3);
            string imgPath = Application.StartupPath.Replace("bin\\Debug", "Resources") + "\\" + selectedImg;
            Image<Bgr, Byte> image = new Image<Bgr, Byte>(@imgPath).Resize(1024, 768, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true);

           
            //Resize(500, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true );
            bIPV.SetCrank(direction);
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



           String repairSubject = "";
            //repairSubject = problems.SelectedItem.ToString();


            imageBox1.Image = bIPV.process(imageReady, image, false, repairSubject );//.Resize(400, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true) ;
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


        public void realTimeCircleFinder(Image<Bgr, Byte> imgFrame)
        {
            String repairSubject = "";

            //repairSubject = problems.SelectedItem.ToString();

            Image<Gray, Byte> grayImg = bIPV.init(imgFrame, imageBox2);
            bIPV.process(grayImg, imgFrame,true, repairSubject );
            // Garbage collect
            grayImg.Dispose();
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
            framecounter++;
            //if (framecounter == 20)
            //{
                framecounter = 0;
                realTimeCircleFinder(ImageFrame);
            //}
            ImageFrame.Dispose();

            

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


        private void problems_SelectedIndexChanged(object sender, EventArgs e)
        {
            
             
        }

        private void btn_help_Click(object sender, EventArgs e)
        {
            Image<Bgr, Byte> image = (Image<Bgr, Byte>)imageBox2.Image;
            if (problems.Text == "Flat Front Tire")
            {
                problemIndex = 0;
                stepIndex = 0;
                richTextBox1.Text = arrayProblems[problemIndex, stepIndex];
                bIPV.highlightPartThatNeedsFixing(false, true, false, image);
                imageBox2.Refresh();
                pb_tutorial.Image = null;

            }
            else if (problems.Text == "Flat Back Tire")
            {
                problemIndex = 1;
                stepIndex = 0;
                richTextBox1.Text = arrayProblems[problemIndex, stepIndex];
                bIPV.highlightPartThatNeedsFixing(true, false, false, image);
                imageBox2.Refresh();
                pb_tutorial.Image = null;
            }
            else if (problems.Text == "Crank Replacement")
            {
                problemIndex = 2;
                stepIndex = 0;
                picIndex = 1;
                richTextBox1.Text = arrayProblems[problemIndex, stepIndex];
                // pb_tutorial.Image = BikesIPV.Properties.Resources.
                pb_tutorial.Load(@"..\..\Resources\RC1.jpg");
                
                bIPV.highlightPartThatNeedsFixing(false, false, true, image);
                imageBox2.Refresh();
                
            }
            btn_next.Enabled = true;
            
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            stepIndex++;
            picIndex++;
            richTextBox1.Text = arrayProblems[problemIndex, stepIndex];
            if (picIndex <= 5 && problemIndex==2 )
            {
                pb_tutorial.Load(@"..\..\Resources\RC" + picIndex + ".jpg");
            }
        }
     }
    }

