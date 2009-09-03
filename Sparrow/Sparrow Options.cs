using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sparrow
{
    public partial class Sparrow_Options : Form
    {
        private int backupBroadIndex;
        private decimal backupTimeDecGraph1;
        private decimal backupTimeDecGraph2;
        private decimal backupResistance;
        private decimal backupNumDownsampledPtsPow2;
        private decimal backupDownsapledFactorPow2;
        private decimal backupNumDecades;

        public Sparrow_Options()
        {
            InitializeComponent();

            broadSpecUnitsComboBox.SelectedIndex = 2;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            // restore the backed up settings
            broadSpecUnitsComboBox.SelectedIndex = backupBroadIndex;
            timeDec1Numeric.Value = backupTimeDecGraph1;
            timeDec2Numeric.Value = backupTimeDecGraph2;
            numDownsampledPtsPow2Numeric.Value = backupNumDownsampledPtsPow2;
            downsampleFactorPow2Numeric.Value = backupDownsapledFactorPow2;
            resistanceNumeric.Value = backupResistance;
            numDecadesNumeric.Value = backupNumDecades;
        }

        public AmpUnits BroadAmpUnits
        {
            get
            {
                if ((string) broadSpecUnitsComboBox.SelectedItem == "V")
                    return AmpUnits.V;

                if ((string) broadSpecUnitsComboBox.SelectedItem == "dBmV")
                    return AmpUnits.dBmV;

                return AmpUnits.dBm;
            }
        }

        public int TimeDecGraph1
        {
            get{
                return (Convert.ToInt32(timeDec1Numeric.Value));
            }
        }

        public int TimeDecGraph2
        {
            get{
                return (Convert.ToInt32(timeDec2Numeric.Value));
            }
        }

        public int PointsPerDecade
        {
            get
            {
                return ((int) Math.Pow(2.0, Convert.ToDouble(numDownsampledPtsPow2Numeric.Value)));
            }
        }

        public int DownsampleFactor
        {
            get
            {
                return ((int) Math.Pow(2.0, Convert.ToDouble(downsampleFactorPow2Numeric.Value)));
            }
        }

        public double Resistance
        {
            get
            {
                return (Convert.ToDouble(resistanceNumeric.Value));
            }        
        }

        public int NumDecades
        {
            get
            {
                return (Convert.ToInt32(numDecadesNumeric.Value));
            }
        }

        private void View_Options_Shown(object sender, EventArgs e)
        {
            backupBroadIndex = broadSpecUnitsComboBox.SelectedIndex;
            backupTimeDecGraph1 = timeDec1Numeric.Value;
            backupTimeDecGraph2 = timeDec2Numeric.Value;
            backupNumDownsampledPtsPow2 = numDownsampledPtsPow2Numeric.Value;
            backupDownsapledFactorPow2 = downsampleFactorPow2Numeric.Value;
            backupResistance = resistanceNumeric.Value;
            backupNumDecades = numDecadesNumeric.Value;

            // update the power of 2 labels
            downsampleFactorLabel.Text = Math.Pow(2.0, Convert.ToDouble(downsampleFactorPow2Numeric.Value)).ToString("0");
            numDownsapledPointsLabel.Text = Math.Pow(2.0, Convert.ToDouble(numDownsampledPtsPow2Numeric.Value)).ToString("0");
        }

        private void numDownsampledPtsPow2Numeric_ValueChanged(object sender, EventArgs e)
        {
            numDownsapledPointsLabel.Text = Math.Pow(2.0, Convert.ToDouble(numDownsampledPtsPow2Numeric.Value)).ToString("0");        
        }

        private void downsampleFactorPow2Numeric_ValueChanged(object sender, EventArgs e)
        {
            downsampleFactorLabel.Text = Math.Pow(2.0, Convert.ToDouble(downsampleFactorPow2Numeric.Value)).ToString("0");            
        }

        private void numDecadesNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (timeDec1Numeric.Value >= numDecadesNumeric.Value)
            {
                timeDec1Numeric.Value = numDecadesNumeric.Value;
            }

            if (timeDec2Numeric.Value >= numDecadesNumeric.Value)
            {
                timeDec2Numeric.Value = numDecadesNumeric.Value;
            }

            timeDec1Numeric.Maximum = numDecadesNumeric.Value;
            timeDec2Numeric.Maximum = numDecadesNumeric.Value;
        }


    }
}