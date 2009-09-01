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
        double[] ch1VFast_t;
        double[] ch1RealBroad_f;
        double[] ch1ImagBroad_f;
        double[] ch1ModBroad_f;
        
        double[] ch1Vslow_t;
        double[] ch1RealNarrow_f;
        double[] ch1ImagNarrow_f;
        double[] ch1ModNarrow_f;
        
        int ch1SlowIndex = 0;
        int halfLen = 0;

        double[] tArrFast;
        double[] tArrSlow;


        double[] fArrBroad;
        double[] fArrNarrow;

        int samplesPerChannel;
        int fSampleRateFast;
        double fSampleRateSlow;

        private bool aquireData = false;
        
        private double [] maxBroadArr = new double[3]{-1E8, -1E8, -1E8};

        public Sparrow()
        {
            
            InitializeComponent();

            rftObj = new RealFourierTransformation(TransformationConvention.Matlab);                    
            ni6251OptionsObj = new NI6251_Options();
            sparrowOptionsObj = new Sparrow_Options();
            UpdateAmpUnitLabels();
        }

        private void AnalogInCallback(IAsyncResult ar)
        {
            try
            {
                // Get the waveform object
                ch1WaveformFast = analogInReader.EndReadWaveform(ar);
                ch1VFast_t = ch1WaveformFast.GetScaledData();      // get array of doubles the values in Volts

                // FFT the data
                rftObj.TransformForward(ch1VFast_t, out ch1RealBroad_f, out ch1ImagBroad_f);
                // find the modulous of the data
                ch1ModBroad_f = GetModulous(ch1RealBroad_f, ch1ImagBroad_f, sparrowOptionsObj.BroadAmpUnits, 1);

                UpdateMax123();

                // update the slower time series with the new points
                UpdateSlowTimeSeries();
                // FFT the slow time data
                rftObj.TransformForward(ch1Vslow_t, out ch1RealNarrow_f, out ch1ImagNarrow_f);
                ch1ModNarrow_f = GetModulous(ch1RealNarrow_f, ch1ImagNarrow_f, sparrowOptionsObj.NarrowAmpUnits, 2);
                
                // create slower waveform by averaging each half of the sampled signal
                fastTimeGraph.Plot("axis1", tArrFast, ch1VFast_t, Color.Blue);
                broadFreqGraph.Plot("axis2", fArrBroad, ch1ModBroad_f, Color.Red);
                slowTimeGraph.Plot("axis3", tArrSlow, ch1Vslow_t, Color.Blue);
                narrowFreqGraph.Plot("axis4", fArrNarrow, ch1ModNarrow_f, Color.Red);
                
                // keep the aquisition running
                if (aquireData == true)
                    iAsyncResultObj = analogInReader.BeginReadWaveform(samplesPerChannel, AnalogInCallback, ni6251OptionsObj.TaskObj);
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

        private void nI6251ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopAquire();
            
            // We dont' care what changed
            ni6251OptionsObj.ShowDialog();
            
            StartAquire();     // restart the data acquistion
            
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopAquire();

            sparrowOptionsObj.ShowDialog();

            StartAquire();
        }

        private void StartAquire()
        {
            // stop any already running aquisitions
            ni6251OptionsObj.TaskObj.Control(TaskAction.Stop);

            // save the rate and the # of samples per channel in case user opens dialog box
            samplesPerChannel = ni6251OptionsObj.SamplesPerChannel;
            fSampleRateFast = ni6251OptionsObj.Rate;
            UpdateAmpUnitLabels();

            // create the time and frequency arrays
            tArrFast = CreateTimeArr(fSampleRateFast, samplesPerChannel);
            fArrBroad = rftObj.GenerateFrequencyScale(fSampleRateFast, samplesPerChannel);
            // the slow sampling rate
            fSampleRateSlow = fSampleRateFast * 2 / samplesPerChannel;
            
            // create time and frequency arrays for slow sampling
            tArrSlow = CreateTimeArr(fSampleRateSlow, sparrowOptionsObj.NumSlowTimePoints);
            fArrNarrow = rftObj.GenerateFrequencyScale(fSampleRateSlow, sparrowOptionsObj.NumSlowTimePoints);
            // allocate memory for slow sampling time array
            ch1Vslow_t = new double[sparrowOptionsObj.NumSlowTimePoints];
            halfLen = ni6251OptionsObj.SamplesPerChannel / 2;
            // reset the slow index index could be anywhere
            ch1SlowIndex = 0;

            // Setup the NI-DAQ for the configured task and start aquisition to the analogCallback
            analogInReader = new AnalogSingleChannelReader(ni6251OptionsObj.TaskObj.Stream);
            analogCallback = new AsyncCallback(AnalogInCallback);

            analogInReader.SynchronizeCallbacks = true;
            iAsyncResultObj = analogInReader.BeginReadWaveform(ni6251OptionsObj.SamplesPerChannel, analogCallback, ni6251OptionsObj.TaskObj);
            
            aquireData = true;
        }

        private void StopAquire()
        {
            aquireData = false;
            // spin for operation to complete
            iAsyncResultObj.AsyncWaitHandle.WaitOne();           
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
            StopAquire();
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

        private double[] GetModulous(double[] realArr, double[] imagArr, AmpUnits units, int graphNum)
        {
            double scale_factor = .5/(Convert.ToDouble(realArr.Length)); // this scale factor asummes the sampled window is repeated for a full second
            
            // the scale factor is need because of implicit windowing of the transform                    
            double[] modArr = new double[realArr.Length];

            maxBroadArr[0] = -1E8;
            maxBroadArr[1] = -1E8;
            maxBroadArr[2] = -1E8;

            switch (units)
            {
                case AmpUnits.dBmV:
                    // overwrite the scale factor with the addition for dBmV
                    scale_factor = 20 * Math.Log10(scale_factor) + 60;

                    for (int i = 0; i < realArr.Length; i++)
                    {
                        modArr[i] = 10 * Math.Log10((realArr[i] * realArr[i] + imagArr[i] * imagArr[i])) + scale_factor;
                        // Check that the abs was not zero
                        if (modArr[i] < -120)
                            modArr[i] = -120;

                        if (graphNum == 1)
                        {
                            if(modArr[i] > maxBroadArr[0])
                                maxBroadArr[0] = modArr[i];
                            else if(modArr[i] > maxBroadArr[1])
                                maxBroadArr[1] = modArr[i];
                            else if(modArr[i] > maxBroadArr[2])
                                maxBroadArr[2] = modArr[i];
                        }
                    }
                    break;

                case AmpUnits.V:
                    for (int i = 0; i < realArr.Length; i++)
                    {
                        modArr[i] = scale_factor * Math.Sqrt((realArr[i] * realArr[i] + imagArr[i] * imagArr[i]));
                        if (graphNum == 1)
                        {
                            if (modArr[i] > maxBroadArr[0])
                                maxBroadArr[0] = modArr[i];
                            else if (modArr[i] > maxBroadArr[1])
                                maxBroadArr[1] = modArr[i];
                            else if (modArr[i] > maxBroadArr[2])
                                maxBroadArr[2] = modArr[i];
                        }
                    }
                    break;

                case AmpUnits.dBm:
                    // overwrite scale factor with the addtion factor for dBm
                    scale_factor = 20*Math.Log10(scale_factor) + - 10 * Math.Log10(sparrowOptionsObj.Resistance) + 30;

                    for (int i = 0; i < realArr.Length; i++)
                    {
                        modArr[i] = 10*Math.Log10(realArr[i] * realArr[i] + imagArr[i] * imagArr[i]) + scale_factor;
                        // Check that the abs was not zero
                        if (modArr[i] < -120)
                            modArr[i] = -120;

                        if (graphNum == 1)
                        {
                            if (modArr[i] > maxBroadArr[0])
                                maxBroadArr[0] = modArr[i];
                            else if (modArr[i] > maxBroadArr[1])
                                maxBroadArr[1] = modArr[i];
                            else if (modArr[i] > maxBroadArr[2])
                                maxBroadArr[2] = modArr[i];
                        }

                    }
                    break;

               }

            return (modArr);
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

        private void UpdateSlowTimeSeries()
        {
            ch1Vslow_t[ch1SlowIndex] = GetAverageFromDoubleArray(ch1VFast_t, 0, halfLen);
            ch1Vslow_t[ch1SlowIndex + 1] = GetAverageFromDoubleArray(ch1VFast_t, halfLen, halfLen);

            // increment the slow index by 2
            ch1SlowIndex += 2;

            // if the slow index is equal to the length of the slow array reset it
            if (ch1SlowIndex >= ch1Vslow_t.Length)
                ch1SlowIndex = 0;
        }

        private void UpdateAmpUnitLabels()
        {
            narrowAmpUnitsLabel.Text = "(" + sparrowOptionsObj.NarrowAmpUnits + ")";
            broadAmpUnitsLabel.Text = "(" + sparrowOptionsObj.BroadAmpUnits + ")";
        }

        void UpdateMax123()
        {
            max1Label.Text = maxBroadArr[0].ToString("0.00000");
            max2Label.Text = maxBroadArr[1].ToString("0.00000");
            max3Label.Text = maxBroadArr[2].ToString("0.00000");
        }
    }
}