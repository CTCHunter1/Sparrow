using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using NationalInstruments;
using NationalInstruments.DAQmx;

using MathNet.Numerics;
using MathNet.Numerics.Transformations;

namespace Sparrow
{
    public enum AmpUnits { dBm, dBmV, V };

    public partial class Sparrow : Form
    {
        private AnalogSingleChannelReader analogInReader;
        private AsyncCallback analogCallback;
        private IAsyncResult iAsyncResultObj; 

        private NI6251_Options ni6251OptionsObj;
        private Sparrow_Options sparrowOptionsObj;

        private RealFourierTransformation rftObj;

        private AnalogWaveform<double> ch1WaveformFast;

        DataSeries ch1VObj;
        DownSampler downSamplerObj; 

        private bool aquireData = false;

        // update timer
        Timer timerObj;
        bool bDisplayUpdate = false;

        private double [] maxBroadArr = new double[3]{-1E8, -1E8, -1E8};

        public Sparrow()
        {
            
            InitializeComponent();
            statusStrip.Items[1].Text = "Ready";

            sparrowOptionsObj = new Sparrow_Options();
            
            try
            {
                rftObj = new RealFourierTransformation(TransformationConvention.Matlab);
                ni6251OptionsObj = new NI6251_Options();
                UpdateAmpUnitLabels();
            }   
            catch (DaqException ex)
            {
                if (ex.Error == -201003)
                {
                    MessageBox.Show("Error: No DAQ detected.");
                }
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message);
            }

            timerObj = new Timer();
            timerObj.Interval = 100;        // number of miliseconds until timer clicks
            timerObj.Tick += new EventHandler(TimerTick);

        }

        private void AnalogInCallback(IAsyncResult ar)
        {
            try
            {
                // Get the waveform object
                ch1WaveformFast = analogInReader.EndReadWaveform(ar);
                ch1VObj.Y_t = ch1WaveformFast.GetScaledData();      // get array of doubles the values in Volts
                               
                downSamplerObj.UpdateDownsampledData();

                // Only Update the graphs when the display update bool is true
                if (bDisplayUpdate == true)
                {
                    // update the time graphs
                    if (sparrowOptionsObj.TimeDecGraph1 == 0)
                    {
                        // create slower waveform by averaging each half of the sampled signal
                        timeGraph1.Plot("axis1", ch1VObj.TimeArr, ch1VObj.Y_t, Color.Blue);
                    }
                    else
                    {
                        timeGraph1.Plot("axis1", downSamplerObj.GetDownsampledDataSeries(sparrowOptionsObj.TimeDecGraph1).TimeArr,
                            downSamplerObj.GetDownsampledDataSeries(sparrowOptionsObj.TimeDecGraph1).Y_t, Color.Blue);
                    }

                    // update the time graphs
                    if (sparrowOptionsObj.TimeDecGraph2 == 0)
                    {
                        // create slower waveform by averaging each half of the sampled signal
                        timeGraph2.Plot("axis1", ch1VObj.TimeArr, ch1VObj.Y_t, Color.Blue);
                    }
                    else
                    {
                        timeGraph2.Plot("axis1", downSamplerObj.GetDownsampledDataSeries(sparrowOptionsObj.TimeDecGraph2).TimeArr,
                            downSamplerObj.GetDownsampledDataSeries(sparrowOptionsObj.TimeDecGraph2).Y_t, Color.Blue);
                    }
              
                    freqGraph1.Semilogx("axis1", downSamplerObj.DownsampledLogFrequency, downSamplerObj.Y_fLog, Color.Red);
                    bDisplayUpdate = false;
                }

                // keep the aquisition running
                if (aquireData == true)
                    iAsyncResultObj = analogInReader.BeginReadWaveform(ch1VObj.NumPoints, AnalogInCallback, ni6251OptionsObj.TaskObj);
          
            }

            catch (DaqException ex)
            {
                StopAquire();
                

                switch (ex.Error)
                {
                    case -200279:
                        //underread error, restart
                        StartAquire();
                        break;

                    default:
                        MessageBox.Show(ex.Message);
                        StartAquire();
                        break;
                }                
            } 
        }

        private void TimerTick(Object o, EventArgs e)
        {
            // set the update bool to true
            bDisplayUpdate = true;
        }

        private void nI6251ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
            if (ni6251OptionsObj == null)
                ni6251OptionsObj = new NI6251_Options();
           
                StopAquire();
                // We dont' care what changed
                ni6251OptionsObj.ShowDialog();

                StartAquire();     // restart the data acquistion
            }catch (DaqException ex)
            {
                if (ex.Error == -201003)
                {
                    if (ni6251OptionsObj != null)
                        ni6251OptionsObj.Dispose();

                    MessageBox.Show("Error: No DAQ detected.");
                }
                else
                    throw (ex);
            }
        }

        private void StartAquire()
        {
            try
            {
                // stop any already running aquisitions
                ni6251OptionsObj.TaskObj.Control(TaskAction.Stop);

                // create the channel 1 data object
                ch1VObj = new DataSeries(ni6251OptionsObj.SamplesPerChannel,
                    ni6251OptionsObj.Rate, sparrowOptionsObj.BroadAmpUnits, sparrowOptionsObj.Resistance);

                UpdateAmpUnitLabels();

                // create the downsampler object
                downSamplerObj = new DownSampler(ch1VObj, sparrowOptionsObj.DownsampleFactor,
                    sparrowOptionsObj.PointsPerDecade, sparrowOptionsObj.NumDecades, sparrowOptionsObj.BroadAmpUnits, sparrowOptionsObj.Resistance);

                downSamplerObj.OrigionalDataSeries = ch1VObj;

                // Setup the NI-DAQ for the configured task and start aquisition to the analogCallback
                analogInReader = new AnalogSingleChannelReader(ni6251OptionsObj.TaskObj.Stream);
                analogCallback = new AsyncCallback(AnalogInCallback);

                analogInReader.SynchronizeCallbacks = true;
                iAsyncResultObj = analogInReader.BeginReadWaveform(ni6251OptionsObj.SamplesPerChannel, analogCallback, ni6251OptionsObj.TaskObj);

                timerObj.Start();

                aquireData = true;
                statusStrip.Items[1].Text = "Aquiring";
                stopToolStripMenuItem.Enabled = true;
                startToolStripMenuItem.Enabled = false;
            }
            catch (NullReferenceException)
            {

            } 
        }

        private void StopAquire()
        {
            if (aquireData == true)
            {
                aquireData = false;
                // spin for operation to complete
                iAsyncResultObj.AsyncWaitHandle.WaitOne();
                statusStrip.Items[1].Text = "Stoped";
                stopToolStripMenuItem.Enabled = false;
                startToolStripMenuItem.Enabled = true;
            } 
        }

        // fs - sample rate
        private double[] CreateTimeArr(double fSample, int len)
        {
            double [] tArr = new double[len];

            tArr[0] = 0;
            double tSample = 1 / fSample;     // find the time between samples

            for(int i=1; i < len; i++)
            {
                tArr[i] = tArr[i - 1] + tSample; 
            }

            return (tArr);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                StopAquire();               
            }
            catch (Exception)
            { };

            Close();
        }

        private Complex [] CopyComplex(double[] realArr, double[] imagArr)
        {
            Complex[] complexArr = new Complex[realArr.Length];

            for (int i = 0; i < realArr.Length; i++)
            {
                complexArr[i].Real = realArr[i];
                complexArr[i].Imag = imagArr[i];
            }

            return (complexArr);
        }



        /// <summary>
        /// Calculate the average of all elements in a double array.
        /// </summary>
        /// <param name="dblArray">The double array to get the 
        /// average from.</param>
        /// <returns>The average of the double array</returns>
        double GetAverageFromDoubleArray(double[] dblArray)
        {
            double dblResult = 0;
            foreach (double dblValue in dblArray)
                dblResult += dblValue;

            return dblResult / dblArray.Length;
        }

        /// <summary>
        /// Calculate the average of all elements in a double array.
        /// </summary>
        /// <param name="dblArray">The double array to get the 
        /// average from.</param>
        /// <returns>The average of the double array</returns>
        double GetAverageFromDoubleArray(double[] dblArray, int startIndex, int len)
        {
            double dblResult = 0;
            int endIndex = startIndex + len;

            for (int i = startIndex; i < endIndex; i++)
            {
                dblResult += dblArray[i];
            }
            
            return dblResult / (len);
        }

        private double[] GetModulous(Complex[] cmpArr)
        {
            double [] modArr = new double[cmpArr.Length];

            for(int i = 0; i < cmpArr.Length; i++)
            {
                modArr[i] = cmpArr[i].Modulus;
            }

            return (modArr);
            
        }

        private void Sparrow_Shown(object sender, EventArgs e)
        {
            // start taking data
            StartAquire();
        }

        private void UpdateAmpUnitLabels()
        {
            //narrowAmpUnitsLabel.Text = "(" + sparrowOptionsObj.NarrowAmpUnits + ")";
            broadAmpUnitsLabel.Text = "(" + sparrowOptionsObj.BroadAmpUnits + ")";
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                StopAquire();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Error: No DAQ Connected");
            }
            sparrowOptionsObj.ShowDialog();

            StartAquire();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartAquire();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopAquire();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopAquire();
            StartAquire();           
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    } 
}