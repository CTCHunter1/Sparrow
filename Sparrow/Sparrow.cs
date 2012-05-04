using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Reflection;
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
        SaveFileDialog saveFileDialogObj;

        private RealFourierTransformation rftObj;

        private AnalogWaveform<double> ch1WaveformFast;

        DataSeries ch1VObj;
        DataSeries singleShotVObj = null;
        DownSampler downSamplerObj;

        private bool bSingleShot = false;
        private bool aquireData = false;

        private int singleShotCalls = 0;
        private int iRestart = 0;

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
                ch1WaveformFast = analogInReader.EndReadWaveform(ar);
                ch1VObj.Y_t = ch1WaveformFast.GetScaledData();      // get array of doubles the values in Volts
                
                downSamplerObj.UpdateDownsampledData(toolStripProgressBar);

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
                    freqGraph1.Semilogx("axis2", downSamplerObj.DownsampledLogFrequency, downSamplerObj.YAvg_fLog, Color.Blue);
                    bDisplayUpdate = false;
                }

                // keep the aquisition running
                if (aquireData == true )
                {
                    iAsyncResultObj = analogInReader.BeginReadWaveform(ch1VObj.NumPoints, AnalogInCallback, ni6251OptionsObj.TaskObj);
                }
            }

            catch (DaqException ex)
            {
                switch (ex.Error)
                {
                    case -200279:
                        //underread error, restart
                        StopAquire();
                        iRestart++;
                        if (iRestart < 10)
                        {
                            StreamWriter sw_obj = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Restart" + iRestart + ".csv");

                            WriteDataFile(sw_obj);

                            sw_obj.Close();
                        }
                        StartAquire();
                        break;

                        /*
                    case -200284:
                        iAsyncResultObj = analogInReader.BeginReadWaveform(ch1VObj.NumPoints, AnalogInCallback, ni6251OptionsObj.TaskObj);
                        break;
                        */

                    default:
                        MessageBox.Show(ex.Message);
                        StopAquire();
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
                ni6251OptionsObj.SingleShot = false;

                if(ni6251OptionsObj.Rate >1000)
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

                ch1VObj.FFTAveraging = sparrowOptionsObj.FFTAveraging;

                UpdateAmpUnitLabels();

                // create the downsampler object
                downSamplerObj = new DownSampler(ch1VObj, sparrowOptionsObj.DownsampleFactor,
                    sparrowOptionsObj.PointsPerDecade, sparrowOptionsObj.NumDecades, sparrowOptionsObj.BroadAmpUnits, sparrowOptionsObj.Resistance);

                toolStripProgressBar.Maximum = downSamplerObj.PointsPerDecade;

                downSamplerObj.OrigionalDataSeries = ch1VObj;
                downSamplerObj.FFTAveraging = sparrowOptionsObj.FFTAveraging;

                // Setup the NI-DAQ for the configured task and start aquisition to the analogCallback
                analogInReader = new AnalogSingleChannelReader(ni6251OptionsObj.TaskObj.Stream);
                analogCallback = new AsyncCallback(AnalogInCallback);

                //analogInReader.SynchronizeCallbacks = true;


                aquireData = true;

                if (bSingleShot)
                {
                    iAsyncResultObj = analogInReader.BeginReadWaveform(ni6251OptionsObj.SamplesPerChannel, SingleShotCallback, ni6251OptionsObj.TaskObj);
                    ni6251OptionsObj.TaskObj.Stream.Timeout = -1;
                }
                else
                {
                    iAsyncResultObj = analogInReader.BeginReadWaveform(ni6251OptionsObj.SamplesPerChannel, analogCallback, ni6251OptionsObj.TaskObj);
                    
                    timerObj.Start();

                    statusStrip.Items[1].Text = "Aquiring";
                    stopToolStripMenuItem.Enabled = true;
                    startToolStripMenuItem.Enabled = false;
                }
            }
            catch (NullReferenceException)
            {

            }
        }

        private void StopAquire()
        {
            timerObj.Stop();
            if(toolStripProgressBar != null)
                toolStripProgressBar.Value = 0;

            if (aquireData == true)
            {
                aquireData = false;
                // spin for operation to complete
                iAsyncResultObj.AsyncWaitHandle.WaitOne();
                ni6251OptionsObj.TaskObj.Stop();
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
            StreamWriter streamWriterObj;

            if (saveFileDialogObj == null)
            {
                saveFileDialogObj = new SaveFileDialog();
            }

            if (saveFileDialogObj.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // .NET Framwork issue *** The base path for the application changes
                    // to be where ever the SaveFileDialog points to
                    // .NET can not handle multiple network file access
                    // Open the file
                    streamWriterObj = new StreamWriter(saveFileDialogObj.OpenFile());

                    WriteDataFile(streamWriterObj);

                    streamWriterObj.Close();
                }
                catch (IOException Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void WriteDataFile(StreamWriter streamWriterObj)
        {
            
            streamWriterObj.NewLine = "\r\n";
            streamWriterObj.WriteLine("Timestamp:," + DateTime.Now);
            streamWriterObj.WriteLine("Title:," + "Sparrow Data File");
            streamWriterObj.WriteLine("Sample Rate:,{0:E}", ch1VObj.SampleRate);
            streamWriterObj.WriteLine("Number of Points:,{0}", ch1VObj.NumPoints);
            streamWriterObj.WriteLine("Downsampling Factor M:,{0}", downSamplerObj.DownsamplingFactor);
            streamWriterObj.WriteLine("Points Per Decade:,{0}", downSamplerObj.PointsPerDecade);

            double[] y_fLogTemp = downSamplerObj.Y_fLog;
            double[] y_fLogTemp2 = downSamplerObj.YAvg_fLog;  

            int maxLength = y_fLogTemp.Length;

            if(maxLength < ch1VObj.NumPoints)
                maxLength = ch1VObj.NumPoints;

            if(maxLength < downSamplerObj.PointsPerDecade)
                maxLength = downSamplerObj.PointsPerDecade;

            // write the header
            streamWriterObj.Write("Frequency Log (Hz),");
            streamWriterObj.Write("Magnitude (" + downSamplerObj.FrequencyUnits.ToString() + "),");
            streamWriterObj.Write("Magnitude Avg (" + downSamplerObj.FrequencyUnits.ToString() + "),");
            streamWriterObj.Write("Channel 1 Time (s),");
            streamWriterObj.Write("Channel 1 (V),");
            streamWriterObj.Write("Channel 1 Freq (Hz),");
            streamWriterObj.Write("Channel 1 (" + ch1VObj.FrequncyUnits.ToString() + "),");
            streamWriterObj.Write("Channel 1 Avg(" + ch1VObj.FrequncyUnits.ToString() + "),");

            for(int i = 1; i <= downSamplerObj.NumDecades; i++)
            {
                streamWriterObj.Write("Dec {0} Time (s),", i);            
                streamWriterObj.Write("Dec {0} (V),", i);
                streamWriterObj.Write("Dec {0} Freq (Hz),", i);
                streamWriterObj.Write("Dec {0} (" + downSamplerObj.FrequencyUnits.ToString() + "),", i);
                streamWriterObj.Write("Dec {0} Avg (" + downSamplerObj.FrequencyUnits.ToString() + "),", i);

            }

            streamWriterObj.Write("\r\n");

            // write the data
            for (int i = 0; i < maxLength; i++)
            {
                if (i < y_fLogTemp.Length)
                {
                    streamWriterObj.Write("{0:E10},", downSamplerObj.DownsampledLogFrequency[i]);
                    streamWriterObj.Write("{0:E10},", y_fLogTemp[i]);
                    streamWriterObj.Write("{0:E10},", y_fLogTemp2[i]);
                }
                else
                {
                    streamWriterObj.Write(",,,");
                }

                if (i < ch1VObj.NumPoints)
                {
                    streamWriterObj.Write("{0:E10},", ch1VObj.TimeArr[i]);
                    streamWriterObj.Write("{0:E10},", ch1VObj.Y_t[i]);
                    streamWriterObj.Write("{0:E10},", ch1VObj.FrequencyArr[i]);
                    streamWriterObj.Write("{0:E10},", ch1VObj.YAbs_f[i]);
                    streamWriterObj.Write("{0:E10},", ch1VObj.YAbsAvg_f[i]);                    
                }

                if(i < downSamplerObj.PointsPerDecade)
                {
                    for(int j = 0; j < downSamplerObj.NumDecades; j++)
                    {
                        streamWriterObj.Write("{0:E10},", downSamplerObj.GetDownsampledDataSeries(j+1).TimeArr[i]);
                        streamWriterObj.Write("{0:E10},", downSamplerObj.GetDownsampledDataSeries(j+1).Y_t[i]);
                        streamWriterObj.Write("{0:E10},", downSamplerObj.GetDownsampledDataSeries(j+1).FrequencyArr[i]);
                        streamWriterObj.Write("{0:E10},", downSamplerObj.GetDownsampledDataSeries(j + 1).YAbs_f[i]);
                        streamWriterObj.Write("{0:E10},", downSamplerObj.GetDownsampledDataSeries(j + 1).YAbsAvg_f[i]);
                    }
                }

                streamWriterObj.Write("\r\n");
            }    

        }

        private void singleShotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // turn on single shot
            StopAquire();
            bSingleShot = true;
            ni6251OptionsObj.SingleShot = true;

            saveSingleShotToolStripMenuItem.Enabled = true;

            singleShotVObj = new DataSeries(ni6251OptionsObj.SamplesPerChannel * sparrowOptionsObj.SingleShotNumPoints,
                ni6251OptionsObj.Rate, AmpUnits.dBmV, sparrowOptionsObj.Resistance);

            StartAquire();
        
        }

        private void SingleShotCallback(IAsyncResult ar)
        {   
            try
            {
                ni6251OptionsObj.TaskObj.WaitUntilDone();

                ch1WaveformFast = analogInReader.EndReadWaveform(ar);
                ch1VObj.Y_t = ch1WaveformFast.GetScaledData();      // get array of doubles the values in Volts

         
                // copy Y_t into the sigle shot array
                for (int i = 0; i < ch1VObj.NumPoints; i++)
                {
                    singleShotVObj.AddPoint(ch1VObj.Y_t[i], toolStripProgressBar);
                }

                singleShotVObj.UpdateFFT();

                ch1VObj.UpdateFFT();
                // create slower waveform by averaging each half of the sampled signal
                timeGraph1.Plot("axis1", singleShotVObj.TimeArr, singleShotVObj.Y_t, Color.Blue);
                timeGraph2.Plot("axis1", singleShotVObj.FrequencyArr, singleShotVObj.YAbs_f, Color.Blue);
                

                singleShotCalls++;

                if(singleShotCalls < sparrowOptionsObj.SingleShotNumPoints)
                     iAsyncResultObj = analogInReader.BeginReadWaveform(ni6251OptionsObj.SamplesPerChannel, SingleShotCallback, ni6251OptionsObj.TaskObj);
                else
                {
                    bSingleShot = false;
                    singleShotCalls = 0;
                }
                                
                
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

            bSingleShot = false;
        }

        private void saveSingleShotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter streamWriterObj;

            if (saveFileDialogObj == null)
            {
                saveFileDialogObj = new SaveFileDialog();
            }

            if (saveFileDialogObj.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // .NET Framwork issue *** The base path for the application changes
                    // to be where ever the SaveFileDialog points to
                    // .NET can not handle multiple network file access
                    // Open the file
                    streamWriterObj = new StreamWriter(saveFileDialogObj.OpenFile());

                    WriteSingleShotFile(streamWriterObj);

                    streamWriterObj.Close();
                }
                catch (IOException Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void WriteSingleShotFile(StreamWriter streamWriterObj)
        {
            streamWriterObj.NewLine = "\r\n";
            streamWriterObj.WriteLine("Timestamp:," + DateTime.Now);
            streamWriterObj.WriteLine("Title:," + "Sparrow Data File,");
            streamWriterObj.WriteLine("Sample Rate:,{0:E}.", singleShotVObj.SampleRate);
            streamWriterObj.WriteLine("Number of Points:,{0}.", singleShotVObj.NumPoints);


            // Write Header
            streamWriterObj.Write("Channel 1 Time (s),");
            streamWriterObj.Write("Channel 1 (V),");
            streamWriterObj.Write("Channel 1 Freq (Hz),");
            streamWriterObj.Write("Channel 1 (" + ch1VObj.FrequncyUnits.ToString() + "),");
            streamWriterObj.Write("Channel 1 Avg(" + ch1VObj.FrequncyUnits.ToString() + "),\r\n");

            // Write Data
            for (int i = 0; i < singleShotVObj.NumPoints; i++)
            {
                streamWriterObj.Write("{0:E10},", singleShotVObj.TimeArr[i]);
                streamWriterObj.Write("{0:E10},", singleShotVObj.Y_t[i]);
                streamWriterObj.Write("{0:E10},", singleShotVObj.FrequencyArr[i]);
                streamWriterObj.Write("{0:E10},", singleShotVObj.YAbs_f[i]);
                streamWriterObj.Write("{0:E10},\r\n", singleShotVObj.YAbsAvg_f[i]);
            }          
        }
    }
}