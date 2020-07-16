using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Management;
using System.IO;

namespace WindowsUpdateForm
{

    public partial class Form1 : Form
    {
        PowerStatus powerStatus = SystemInformation.PowerStatus;
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        public static bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }

        public Form1()
        {
            InitializeComponent();
            power();
        }

        public string WhichONe
        {
            get
            {
                string connected = SystemInformation.PowerStatus.BatteryChargeStatus == BatteryChargeStatus.NoSystemBattery ? "PC Plugin" : "Laptop On Battery";
                return connected;
            }
        }

        // verificare pc sau laptop
        public enum ChassisTypes
        {
            Other = 1,
            Unknown,
            Desktop,
            LowProfileDesktop,
            PizzaBox,
            MiniTower,
            Tower,
            Portable,
            Laptop,
            Notebook,
            Handheld,
            DockingStation,
            AllInOne,
            SubNotebook,
            SpaceSaving,
            LunchBox,
            MainSystemChassis,
            ExpansionChassis,
            SubChassis,
            BusExpansionChassis,
            PeripheralChassis,
            StorageChassis,
            RackMountChassis,
            SealedCasePC
        }

        public static ChassisTypes GetCurrentChassisType()
        {
            ManagementClass systemEnclosures = new ManagementClass("Win32_SystemEnclosure");
            foreach (ManagementObject obj in systemEnclosures.GetInstances())
            {
                foreach (int i in (UInt16[])(obj["ChassisTypes"]))
                {
                    if (i > 0 && i <25)
                    {
                        return (ChassisTypes)i;
                    }
                }
            }
            return ChassisTypes.Unknown;
        }

        // verificare baterie (ac adaptor)
        private Boolean baterie()
        {
            if (powerStatus.PowerLineStatus == PowerLineStatus.Online)
            {
                label4.Text = "Plugin On Power "  + Convert.ToString(powerStatus.BatteryLifePercent * 100) + "%";
                return true;
            }
            else
            {
                label4.Text = "On Battery " + Convert.ToString(powerStatus.BatteryLifePercent * 100) + "%";
                return false;
            }
        }

        private void power()
        {
            label3.Text = GetCurrentChassisType().ToString();
            baterie();
            label6.Text = IsConnectedToInternet().ToString();


            if (baterie().Equals(true) && IsConnectedToInternet().Equals(true))
            {
                btnNext.Enabled = true;
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //buton install 
        }



    }
}
