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
    public enum AmpUnits { dBm, V };

    public partial class Sparrow : Form
    {
        private AnalogSingleChannelReader analogInReader;
        private AsyncCallback analogCallback;

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

        double fSampleRateSlow;

        public Sparrow()
        {
            
            InitializeComponent();

            rftObj = new RealFourierTransformation(TransformationConvention.Matlab);                    
            ni6251OptionsObj = new NI6251_Options();
            sparrowOptionsObj = new Sparrow_Options();
        }

        private void AnalogInCallback(IAsyncResult ar)
        {
            try
            {
                ch1WaveformFast = analogInReader.EndReadWaveform(ar);

                ch1VFast_t = ch1WaveformFast.GetScaledData();      // selected channel waveform as a fuction of time in (Volts)

                // FFT the data
                rftObj.TransformForward(ch1VFast_t, out ch1RealBroad_f, out ch1ImagBroad_f);
                // plot the modulous
                ch1ModBroad_f = GetModulous(ch1RealBroad_f, ch1ImagBroad_f, sparrowOptionsObj.BroadAmpUnits);

                UpdateSlowTimeSeries();
                // FFT the slow time data
                rftObj.TransformForward(ch1Vslow_t, out ch1RealNarrow_f, out ch1ImagNarrow_f);
                ch1ModNarrow_f = GetModulous(ch1RealNarrow_f, ch1ImagNarrow_f, sparrowOptionsObj.NarrowAmpUnits);
                
                // create slower waveform by averaging each half of the sampled signal
                fastTimeGraph.Plot("axis1", tArrFast, ch1VFast_t, Color.Blue);
                broadFreqGraph.Plot("axis2", fArrBroad, ch1ModBroad_f, Color.Red);
                slowTimeGraph.Plot("axis3", tArrSlow, ch1Vslow_t, Color.Blue);
                narrowFreqGraph.Plot("axis4", fArrNarrow, ch1ModNarrow_f, Color.Red);

                analogInReader.BeginReadWaveform(ni6251OptionsObj.SamplesPerChannel, AnalogInCallback, ni6251OptionsObj.TaskObj);
            }

            catch (DaqException ex)
            {
                // throw the exception
                MessageBox.Show(ex.Message);
            }
        }

        private void nI6251ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //NI6251_Options ni6251OptionsObj = new NI6251_Options();
            
            if(ni6251OptionsObj.ShowDialog() == DialogResult.OK)
            {
                //analogInReader.EndReadWaveform(); // terminate the current read
                TakeData();     // restart the data acquistion
            }
        }

        private void TakeData()
        {
            // create the time and frequencie arrays
            // create a time series for the sampling positions
            tArrFast = CreateTimeArr(ni6251OptionsObj.Rate, ni6251OptionsObj.SamplesPerChannel);
            fArrBroad = rftObj.GenerateFrequencyScale(ni6251OptionsObj.Rate, ni6251OptionsObj.SamplesPerChannel);
            // the slow sampling rate
            fSampleRateSlow = ni6251OptionsObj.Rate * 2 / ni6251OptionsObj.SamplesPerChannel;
            
            // the time array for the slow sampling
            tArrSlow = CreateTimeArr(fSampleRateSlow, sparrowOptionsObj.NumSlowTimePoints);
            fArrNarrow = rftObj.GenerateFrequencyScale(fSampleRateSlow, sparrowOptionsObj.NumSlowTimePoints);
            ch1Vslow_t = new double[sparrowOptionsObj.NumSlowTimePoints];
            halfLen = ni6251OptionsObj.SamplesPerChannel / 2;

            // all the calls to start data aquisition
            analogInReader = new AnalogSingleChannelReader(ni6251OptionsObj.TaskObj.Stream);
            analogCallback = new AsyncCallback(AnalogInCallback);

            analogInReader.SynchronizeCallbacks = true;
            analogInReader.BeginReadWaveform(ni6251OptionsObj.SamplesPerChannel, analogCallback, ni6251OptionsObj.TaskObj);  
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

        private double[] GetModulous(double[] realArr, double[] imagArr, AmpUnits units)
        {
            double scale_factor = 1.0/(Convert.ToDouble(realArr.Length));

            // the scale factor is need because of implicit windowing of the transform                    
            double[] modArr = new double[realArr.Length];

            for (int i = 0; i < realArr.Length; i++)
            {
                if (units == AmpUnits.dBm)
                {
                    scale_factor = scale_factor/ sparrowOptionsObj.Resistance;
                    modArr[i] = 10 * Math.Log10(scale_factor * (realArr[i] * realArr[i] + imagArr[i] * imagArr[i]));
                    // we need to be careful here, if the value is less than -120 dBm just set it to -120 dBm
                    // we can have -inf dBm
                    if (modArr[i] < -120)
                        modArr[i] = -120;
                }

                if (units == AmpUnits.V)
                    modArr[i] = scale_factor * Math.Sqrt(realArr[i] * realArr[i] + imagArr[i] * imagArr[i]);                
                
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
            TakeData();
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

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sparrowOptionsObj.ShowDialog() == DialogResult.OK)
            {
                TakeData();
            }
        }

    }
}