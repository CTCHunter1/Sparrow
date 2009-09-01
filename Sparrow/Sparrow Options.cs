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

            broadSpecUnitsComboBox.SelectedIndex = 2;
            narrowSpecUnitsComboBox.SelectedIndex = 2;
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
                if ((string) broadSpecUnitsComboBox.SelectedItem == "V")
                    return AmpUnits.V;

                if ((string) broadSpecUnitsComboBox.SelectedItem == "dBmV")
                    return AmpUnits.dBmV;

                return AmpUnits.dBm;
            }
        }

        public AmpUnits NarrowAmpUnits
        {
            get
            {
                if ((string) narrowSpecUnitsComboBox.SelectedItem == "V")
                    return AmpUnits.V;

                if ((string) narrowSpecUnitsComboBox.SelectedItem == "dBmV")
                    return AmpUnits.dBmV;

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