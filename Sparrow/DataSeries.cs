using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

using MathNet.Numerics;
using MathNet.Numerics.Transformations;

namespace Sparrow
{
    public class DataSeries
    {
        // related to the data
        private double[] tArr;
        private double T0;
        private double[] fArr;
        private double[] fHalfArr;
        protected double[] y_t;
        private bool bUpdateFFT = false;
        private double[] yAbs_f;
        private double[] yReal_f;
        private double[] yImag_f;
        // new variables for FFT averaging
        protected bool bFFTAveraging = false;
        private double[] yRealAvg_f;
        private double[] yImagAvg_f;
        private double[] yAbsAvg_f;
        int numFFTs = 0;        // number of points averaged in for yRealAvg_f
        int pointsToFFTAverage;

        protected int ptIndex = 0;
        protected int mNumPts;
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
            fHalfArr = GetHalfArr(fArr);

            y_t = new double[numPts];
            mAmpUnits = fourierAmpUnits;
            mResistance = resistance;

            // create the averaging arrays
            yRealAvg_f = new double[numPts];
            yImagAvg_f = new double[numPts];
            yAbsAvg_f = new double[numPts];

            UpdateFFT();
        }

        public DataSeries(int numPts, double sampleRate, AmpUnits fourierAmpUnits, double resistance, bool updateFFT)
        {
            mNumPts = numPts;
            fSample = sampleRate;

            // create the data arrays.
            CreateTimeArr();
            fArr = rftObj.GenerateFrequencyScale(fSample, numPts);
            fHalfArr = GetHalfArr(fArr);

            y_t = new double[numPts];
            mAmpUnits = fourierAmpUnits;
            mResistance = resistance;           

            UpdateFFT();

            // create the averaging arrays
            yRealAvg_f = new double[numPts];
            yImagAvg_f = new double[numPts];
            yAbsAvg_f = new double[numPts];

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
                fHalfArr = GetHalfArr(fArr);
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
                fHalfArr = GetHalfArr(fArr);

            }
        }

        public double[] YAbs_f
        {
            get
            {
                return (yAbs_f);
            }
        }

        public double[] YAbsAvg_f
        {
            get
            {
                return (yAbsAvg_f);
            }
        }

        public double[] YAbs_fHalf
        {
            get
            {
                return GetHalfArr(yAbs_f);
                /*
                double[] halfArr = new double[yAbs_f.Length / 2];
                for (int i = 0; i < (yAbs_f.Length / 2); i++)
                {
                    halfArr[i] = yAbs_f[i + 1];
                }

                return (halfArr);*/
            }
        }

        public double[] YAbsAvg_fHalf
        {
            get
            {
                return (GetHalfArr(yAbsAvg_f));
            }
        }

        public AmpUnits FrequncyUnits
        {
            get 
            {
                return mAmpUnits;    
            }
        }

        public bool FFTAveraging
        {
            set
            {
                bFFTAveraging = value;
            }
            get 
            {
                return (bFFTAveraging);
            }
        }

        public void UpdateFFT()
        {
            rftObj.TransformForward(Y_t, out yReal_f, out yImag_f);
            // any math that needs to be done should just be done in this function
            yAbs_f = GetModulous(yReal_f, yImag_f);
            // TODO: Implement Phase retrevial

            // Check Power Conservation
            double PTime = GetPowerTimeDomain(y_t);
            double PFreq = GetPowerFrequencyDomaindBmV(yAbs_f);

        }

        public virtual void AddPoint(double pt, ToolStripProgressBar bPar)
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

            // the duration
            T0 = tSample*mNumPts;
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
                        modArr[i] = 10 * Math.Log10((realArr[i] * realArr[i] + imagArr[i] * imagArr[i])/(fSample*fSample*T0)) + 60; // +scale_factor      
                        //modArr[i] = (realArr[i] * realArr[i] + imagArr[i] * imagArr[i])/(fSample*fSample*T0*T0);
                        // Check that the abs was not zero
                        if (modArr[i] < -140)
                            modArr[i] = -140;

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
                        if (modArr[i] < -140)
                            modArr[i] = -140;

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

        private double[] GetHalfArr(double[] arr)
        {
            double[] retArr = new double[arr.Length / 2];

            for (int i = 0; i < retArr.Length; i++)
            {
                retArr[i] = arr[i + 1];
            }

            return (retArr);
        }

        public void UpdateFFTAverage()
        {
            
            // for each new point requires the addition of a linear phase factor because of the
            // shifting of the sample window
            // add the latest value of the fft to the accumulator, then average each point
            double [] yNew = GetModulous(yReal_f, yImag_f);
            
            for (int i = 0; i < yReal_f.Length; i++)
            {
                double c = Math.Cos(2 * Math.PI * numFFTs * T0 * fArr[i]);
                double d = Math.Sin(2 * Math.PI * numFFTs * T0 * fArr[i]);
                yRealAvg_f[i] = (numFFTs * yRealAvg_f[i] + (yReal_f[i]*c-yImag_f[i]*d)) / (double)(numFFTs + 1);
                yImagAvg_f[i] = (numFFTs * yImagAvg_f[i] + (yReal_f[i]*d+yImag_f[i]*c)) / (double)(numFFTs + 1);
                //yRealAvg_f[i] =  ;
                //yImagAvg_f[i] = (yReal_f[i] * d + yImag_f[i] * c) ;
                switch (mAmpUnits)
                {
                    case AmpUnits.dBm:
                    case AmpUnits.dBmV:
                        yAbsAvg_f[i] = 20*Math.Log10((numFFTs * Math.Pow(10,yAbsAvg_f[i]/20) + Math.Pow(10,yNew[i]/20)) / (double)(numFFTs + 1));
                        break;

                    case AmpUnits.V:
                        yAbsAvg_f[i] = (numFFTs * yAbsAvg_f[i] + yNew[i]) / (double)(numFFTs + 1);                        
                        break;
                } 
            }
            
            numFFTs++;

           ///yAbsAvg_f = GetModulous(yRealAvg_f, yImagAvg_f);
        }
        
        double GetPowerTimeDomain(double [] x_t)
        {
            double accum = 0;

            for(int i=0; i < mNumPts; i++)
            {
                accum += x_t[i] * x_t[i];
            }

            return (accum / mNumPts);
        }

        double GetPowerFrequencyDomaindBmV(double[] ydBmV_f)
        {
            double accum = 0;

            for (int i = 0; i < mNumPts; i++)
            {
                accum += Math.Pow(10, (ydBmV_f[i]-60) / 10);
            }

            return (accum);
        }
    }
}
