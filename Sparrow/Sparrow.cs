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

            rftObj = new RealFourierTransformation(TransformationConvention.Matlab);                    
            ni6251OptionsObj = new NI6251_Options();
            sparrowOptionsObj = new Sparrow_Options();
            UpdateAmpUnitLabels();

            timerObj = new Timer();
            timerObj.Interval = 100;        // number of miliseconds until timer clicks
            timerObj.Tick += new EventHandler(TimerTick);
            timerObj.Start();

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
    }

    public class DownSampler
    {
        private int mNumDecades;
        /// <summary>
        /// Must be power of 2
        /// </summary>
        private int mDownsamplingFactor;
        int mPointsPerDecade;
        private DataSeries origionalDataSeries;
        private DataSeriesNode[] downsampledNodes;
        AmpUnits ampUnits;
        double mResistance;

        double[] mFLogArr;      // the frequency scale for the downsampled data
        double[] y_logf = null;            // the data points on a log(f) scale

        // points per update
        private int pointsPerUpdate = 0;

        public DownSampler(DataSeries origioalDataSeriesObj, int downsamplingFactor, int pointsPerDecade, int numDecadesToDownSample, AmpUnits fourierAmpUnits, double resistance)
        {
            origionalDataSeries = origioalDataSeriesObj;

            mNumDecades = numDecadesToDownSample;
            mDownsamplingFactor = downsamplingFactor;
            mPointsPerDecade = pointsPerDecade;
            ampUnits = fourierAmpUnits;
            mResistance = resistance;
            InitDownsampledDataSeries();
        }

        public DataSeries OrigionalDataSeries
        {
            get
            {
                return (origionalDataSeries);
            }
            set
            {
                origionalDataSeries = value;
            }
        }

        public int NumDecades
        {
            get
            {
                return (mNumDecades);
            }
            set
            {
                InitDownsampledDataSeries();
                mNumDecades = value;
            }
        }

        public int DownsamplingFactor
        {
            get
            {
                return(mDownsamplingFactor);
            }
            set
            {
                InitDownsampledDataSeries();
                mDownsamplingFactor = value;
            }
        }

        public double[] DownsampledLogFrequency
        {
            get
            {
                return (mFLogArr);
            }
        }

        public double[] Y_fLog
        {
            get
            {
                y_logf = GetDownsampledY_fLog();
                return (y_logf);
            }
        }

        public void UpdateDownsampledData()
        {
            // because the data is coming in in 2^n multiple powers
            // and the downsampling factor must be a power of 2
            // the downsampled data sets can only add 1, 2, 4,.. 2^n points per  update
            // update the lowest downsampled data series
            for(int i = 0; i < pointsPerUpdate; i++)
            {
                downsampledNodes[0].AddPoint(
                    downsampledNodes[0].GetAverageFromDoubleArray(
                    origionalDataSeries.Y_t, i*mDownsamplingFactor, mDownsamplingFactor));    
            }

        }

        public DataSeries GetDownsampledDataSeries(int decade)
        {
            return ((DataSeries) downsampledNodes[decade - 1]);
        }

        private void InitDownsampledDataSeries()
        {
            downsampledNodes = new DataSeriesNode[mNumDecades];
            
            // these data series nodes need to be created in reverse order so 
            // that the previous one can know about the next one
            for (int i = mNumDecades-1; i >=0 ; i--)
            {
                // the last decade has no next node pointer
                if (i == (mNumDecades - 1))
                    downsampledNodes[i] = new DataSeriesNode(mPointsPerDecade, origionalDataSeries.SampleRate / (Math.Pow((double)mDownsamplingFactor, (double)(i + 1))),
                        ampUnits, mResistance, false, mDownsamplingFactor, null);

                else
                    downsampledNodes[i] = new DataSeriesNode(mPointsPerDecade, origionalDataSeries.SampleRate / (Math.Pow((double)mDownsamplingFactor, (double)(i + 1))),
                        ampUnits, mResistance, false, mDownsamplingFactor, downsampledNodes[i + 1]);
                
            }
            
            // calculate the number of points updated per data set
            pointsPerUpdate = OrigionalDataSeries.Y_t.Length / mDownsamplingFactor;

            mFLogArr = CreateDownsampledFrequency();
        }

        private double [] CreateDownsampledFrequency()
        {
            int iNumLogfPts = (origionalDataSeries.NumPoints/2)*(mDownsamplingFactor-1)/mDownsamplingFactor +
                (mNumDecades-1)*(mPointsPerDecade/2*(mDownsamplingFactor-1))/mDownsamplingFactor +
                mPointsPerDecade/2;

            double[] fLogArr = new double[iNumLogfPts];

            //copy backwards
            int fIndex = iNumLogfPts-1;      // index to use for the fLogArr

            // copy the origial data frequencies (do not copy this index)
            int stopIndex = (origionalDataSeries.NumPoints / 2) / mDownsamplingFactor;

            for (int i = origionalDataSeries.NumPoints / 2-1; i >= stopIndex; i--)
            {
                fLogArr[fIndex] = origionalDataSeries.FrequencyHalfArr[i];
                fIndex--;
            }
            
            // for each decade copy out the frequencies
            stopIndex = (mPointsPerDecade / 2) / mDownsamplingFactor;

            for (int i = 0; i < (mNumDecades - 1); i++)
            {                
                for (int j = mPointsPerDecade/2 - 1; j >= stopIndex; j--)
                {
                    fLogArr[fIndex] = downsampledNodes[i].FrequencyHalfArr[j];
                    fIndex--;
                }
            }

            for (int j = mPointsPerDecade/2 -1; j >= 0; j--)
            {
                fLogArr[fIndex] = downsampledNodes[mNumDecades - 1].FrequencyHalfArr[j];
                fIndex--;
            }

            return (fLogArr);
        }

        private double[] GetDownsampledY_fLog()
        {
            int iNumLogfPts = mFLogArr.Length;

            double[] y_fLog = new double[iNumLogfPts];

            //copy backwards
            int yIndex = iNumLogfPts - 1;      // index to use for the fLogArr

            // update the FFT first
            origionalDataSeries.UpdateFFT();
            // copy the origial data frequencies (do not copy this index)
            int stopIndex = (origionalDataSeries.NumPoints / 2) / mDownsamplingFactor;
            for (int i = origionalDataSeries.NumPoints / 2 - 1; i >= stopIndex; i--)
            {
                double[] y_temp = origionalDataSeries.YAbs_fHalf;
                y_fLog[yIndex] = y_temp[i];
                yIndex--;
            }

            // for each decade copy out the frequencies
            stopIndex = (mPointsPerDecade / 2) / mDownsamplingFactor;

            for (int i = 0; i < (mNumDecades - 1); i++)
            {
                downsampledNodes[i].UpdateFFT();        // update the fft for this node
                for (int j = mPointsPerDecade / 2 - 1; j >= stopIndex; j--)
                {
                    double[] y_temp = downsampledNodes[i].YAbs_fHalf;
                    y_fLog[yIndex] = y_temp[j];
                    yIndex--;
                }
            }

            downsampledNodes[mNumDecades - 1].UpdateFFT();
            for (int j = mPointsPerDecade / 2 - 1; j >= 0; j--)
            {
                double[] y_temp = downsampledNodes[mNumDecades - 1].YAbs_fHalf;
                y_fLog[yIndex] = y_temp[j];
                yIndex--;
            }

            return (y_fLog);
        }
    }

    public class DataSeries
    {
        // related to the data
        private double[] tArr;
        private double[] fArr;
        private double[] fHalfArr; 
        protected double[] y_t;
        private bool bUpdateFFT = false;
        private double[] yAbs_f;
        private double[] yReal_f;
        private double[] yImag_f;
        protected int ptIndex = 0;
        private int mNumPts;
        private double fSample;
        private double mResistance;

        // the transformer
        private RealFourierTransformation rftObj = new RealFourierTransformation(TransformationConvention.Matlab);

        AmpUnits mAmpUnits;

        public DataSeries(int numPts, double sampleRate, AmpUnits fourierAmpUnits, double resistance)
        {
            mNumPts = numPts;
            fSample = sampleRate;

            // create the data arrays.
            CreateTimeArr();
            fArr = rftObj.GenerateFrequencyScale(fSample, numPts);
            fHalfArr = GetFreqHalfArr(fArr);

            y_t = new double[numPts];
            mAmpUnits = fourierAmpUnits;
            mResistance = resistance;
            UpdateFFT();
        }

        public DataSeries(int numPts, double sampleRate, AmpUnits fourierAmpUnits, double resistance, bool updateFFT)
        {
            mNumPts = numPts;
            fSample = sampleRate;

            // create the data arrays.
            CreateTimeArr();
            fArr = rftObj.GenerateFrequencyScale(fSample, numPts);
            fHalfArr = GetFreqHalfArr(fArr);

            y_t = new double[numPts];
            mAmpUnits = fourierAmpUnits;
            mResistance = resistance;
            UpdateFFT();

            bUpdateFFT = updateFFT;
        }

        public double[] Y_t
        {
            get
            {
                return (y_t);
            }
            set
            {
                y_t = value;

                if (bUpdateFFT)
                    UpdateFFT();
            }
        }

        public double[] TimeArr
        {
            get
            {
                return (tArr);
            }
        }

        public double[] FrequencyArr
        {
            get
            {
                return (fArr);
            }   
        }

        public double[] FrequencyHalfArr
        {
            get
            {
                return (fHalfArr);
            }
        }

        public int NumPoints
        {
            get
            {
                return (mNumPts);
            }
            set
            {
                mNumPts = value;
                // create the data arrays.
                CreateTimeArr();
                fArr = rftObj.GenerateFrequencyScale(fSample, mNumPts);
                fHalfArr = GetFreqHalfArr(fArr);
                // update FFT
                UpdateFFT();
            }
        }

        public double SampleRate
        {
            get
            {
                return (fSample);
            }
            set
            {
                fSample = value;
                // Update the time and frequency arrays
                // create the data arrays.
                CreateTimeArr();
                fArr = rftObj.GenerateFrequencyScale(fSample, mNumPts);
                fHalfArr = GetFreqHalfArr(fArr);

            }
        }

        public double[] YAbs_f
        {
            get
            {
                return (yAbs_f);
            }
        }

        public double [] YAbs_fHalf
        {
            get
            {
                double[] halfArr = new double[yAbs_f.Length / 2];
                for (int i = 0; i < (yAbs_f.Length / 2); i++)
                {
                    halfArr[i] = yAbs_f[i+1];
                }

                return (halfArr);
            }
        }

            public void UpdateFFT()
        {           
            rftObj.TransformForward(Y_t, out yReal_f, out yImag_f);
            // any math that needs to be done should just be done in this function
            yAbs_f = GetModulous(yReal_f, yImag_f);
            // TODO: Implement Phase retrevial
        }

        public virtual void AddPoint(double pt)
        {
            y_t[ptIndex] = pt;

            ptIndex++;
            // check that we haven't reached the end of the array
            if (ptIndex >= mNumPts)
                ptIndex = 0;
        }

        public int PtIndex
        {
            get
            {
                return (ptIndex);
            }

            set
            {
                ptIndex = value;
            }
        }

        private void CreateTimeArr()
        {
            tArr = new double[mNumPts];

            tArr[0] = 0;
            double tSample = 1 / fSample;     // find the time between samples

            for (int i = 1; i < mNumPts; i++)
            {
                tArr[i] = tArr[i - 1] + tSample;
            }
        }

        private double[] GetModulous(double[] realArr, double[] imagArr)
        {
            double scale_factor = .5 / (Convert.ToDouble(realArr.Length)); // this scale factor asummes the sampled window is repeated for a full second

            // the scale factor is need because of implicit windowing of the transform                    
            double[] modArr = new double[realArr.Length];

            /*
            maxBroadArr[0] = -1E8;
            maxBroadArr[1] = -1E8;
            maxBroadArr[2] = -1E8;

            */
            switch (mAmpUnits)
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
                        
                        /* Max Capture Code
                        if (graphNum == 1)
                        {
                            if (modArr[i] > maxBroadArr[0])
                                maxBroadArr[0] = modArr[i];
                            else if (modArr[i] > maxBroadArr[1])
                                maxBroadArr[1] = modArr[i];
                            else if (modArr[i] > maxBroadArr[2])
                                maxBroadArr[2] = modArr[i];
                        }
                        */ 
                    }
                    break;

                case AmpUnits.V:
                    for (int i = 0; i < realArr.Length; i++)
                    {
                        modArr[i] = scale_factor * Math.Sqrt((realArr[i] * realArr[i] + imagArr[i] * imagArr[i]));
                        /* Max Capture Code
                        if (graphNum == 1)
                        {
                            if (modArr[i] > maxBroadArr[0])
                                maxBroadArr[0] = modArr[i];
                            else if (modArr[i] > maxBroadArr[1])
                                maxBroadArr[1] = modArr[i];
                            else if (modArr[i] > maxBroadArr[2])
                                maxBroadArr[2] = modArr[i];
                        }*/
                    }
                    break;

                case AmpUnits.dBm:
                    // overwrite scale factor with the addtion factor for dBm
                    scale_factor = 20 * Math.Log10(scale_factor) + -10 * Math.Log10(mResistance) + 30;

                    for (int i = 0; i < realArr.Length; i++)
                    {
                        modArr[i] = 10 * Math.Log10(realArr[i] * realArr[i] + imagArr[i] * imagArr[i]) + scale_factor;
                        // Check that the abs was not zero
                        if (modArr[i] < -120)
                            modArr[i] = -120;

                        /* Max Caputure Code
                        if (graphNum == 1)
                        {
                            if (modArr[i] > maxBroadArr[0])
                                maxBroadArr[0] = modArr[i];
                            else if (modArr[i] > maxBroadArr[1])
                                maxBroadArr[1] = modArr[i];
                            else if (modArr[i] > maxBroadArr[2])
                                maxBroadArr[2] = modArr[i];
                        }
                        */
                    }
                    break;

            }

            return (modArr);
        }

        private double[] GetFreqHalfArr(double[] fArr)
        {
            double [] retArr = new double[fArr.Length/2];

            for (int i = 0; i < retArr.Length; i++)
            {
                retArr[i] = fArr[i+1];
            }

            return (retArr);
        }
    }

    public class DataSeriesNode : DataSeries
    {
        private DataSeriesNode mNextNode;
        private int mDownsamplingFactor = 0;

        public DataSeriesNode(int numPts, double sampleRate, AmpUnits fourierAmpUnits, double resistance, bool updateFFT,
            int downsamplingFactor, DataSeriesNode nextNode)  : base(numPts, sampleRate, fourierAmpUnits, resistance)      
        {
            mNextNode = nextNode;
            mDownsamplingFactor = downsamplingFactor;
        }

        public DataSeriesNode NextNode
        {
            get
            {
                return (mNextNode);
            }         
            set
            {
                mNextNode = NextNode;
            }
         }

        public override void AddPoint(double pt)
        {
            // add the point to the array
            base.y_t[ptIndex] = pt;

            if ((base.ptIndex % mDownsamplingFactor) == (mDownsamplingFactor-1))
            {
                if (mNextNode != null)
                {
                    // average the last mDowsamplingFactor points and add them to the next node
                    mNextNode.AddPoint(GetAverageFromDoubleArray(base.y_t,
                        ptIndex - mDownsamplingFactor+1, mDownsamplingFactor));
                }
            }

            base.ptIndex++;

            // reset the point index if it exceeds the length of the array
            if (base.ptIndex >= base.NumPoints)
                ptIndex = 0;
        }

        /// <summary>
        /// Calculate the average of all elements in a double array.
        /// </summary>
        /// <param name="dblArray">The double array to get the 
        /// average from.</param>
        /// <returns>The average of the double array</returns>
        public double GetAverageFromDoubleArray(double[] dblArray, int startIndex, int len)
        {
            double dblResult = 0;
            int endIndex = startIndex + len;

            for (int i = startIndex; i < endIndex; i++)
            {
                dblResult += dblArray[i];
            }

            return dblResult / (len);
        }
    }
}