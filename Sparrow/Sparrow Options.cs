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
        private int backupNarrowIndex;
        private decimal backupNumSlowTimePts;
        private decimal backupResistance;

        public Sparrow_Options()
        {
            InitializeComponent();

            broadSpecUnitsComboBox.SelectedIndex = 1;
            narrowSpecUnitsComboBox.SelectedIndex = 1;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            broadSpecUnitsComboBox.SelectedIndex = backupBroadIndex;
            narrowSpecUnitsComboBox.SelectedIndex = backupNarrowIndex;
            numSlowTimePtsNumeric.Value = backupNumSlowTimePts;
            resistanceNumeric.Value = backupResistance;
        }

        public AmpUnits BroadAmpUnits
        {
            get
            {
                if (broadSpecUnitsComboBox.SelectedIndex == 0)
                    return AmpUnits.V;

                return AmpUnits.dBm;
            }
        }

        public AmpUnits NarrowAmpUnits
        {
            get
            {
                if (narrowSpecUnitsComboBox.SelectedIndex == 0)
                    return AmpUnits.V;

                return AmpUnits.dBm;
            }        
        }

        public int NumSlowTimePoints
        {
            get
            {
                return (Convert.ToInt32(numSlowTimePtsNumeric.Value));
            }
        }

        public double Resistance
        {
            get
            {
                return (Convert.ToDouble(resistanceNumeric.Value));
            }
        }

        private void View_Options_Shown(object sender, EventArgs e)
        {
            backupBroadIndex = broadSpecUnitsComboBox.SelectedIndex;
            backupNarrowIndex = narrowSpecUnitsComboBox.SelectedIndex;
            backupNumSlowTimePts = numSlowTimePtsNumeric.Value;
            backupResistance = resistanceNumeric.Value;
        }
    }
}