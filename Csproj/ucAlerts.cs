using DataBaseHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPISv2
{
    public partial class ucAlerts : UserControl
    { 
        public ucAlerts()
        {
            InitializeComponent();
            this.Load += ucAlerts_Load;
        }
        private void ucAlerts_Load(object sender, EventArgs e)
        {
            LoadDemoAlerts(); 
        }
        private void AddAlertCard(AlertItem alert)
        {
            Panel card = new Panel
            {
                Width = 340,
                Height = 110,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10)
            };

            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2
            };

            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 8));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            Panel colorStrip = new Panel
            {
                Dock = DockStyle.Fill
            };

            switch (alert.Severity.ToLower())
            {
                case "severe":
                    colorStrip.BackColor = Color.Red;
                    break;

                case "moderate":
                    colorStrip.BackColor = Color.Goldenrod;
                    break;

                default:
                    colorStrip.BackColor = Color.Green;
                    break;
            }
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
                Padding = new Padding(5)
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));

            layout.Controls.Add(new Label { Text = $"Message: {alert.Message}", AutoSize = true }, 0, 0);
            layout.Controls.Add(new Label { Text = $"Audio: {(alert.AudioAvailable ? "Yes" : "No")}", AutoSize = true }, 0, 1);
            layout.Controls.Add(new Label { Text = $"Urgency: {alert.Urgency}", AutoSize = true }, 0, 2);
            layout.Controls.Add(new Label { Text = $"Severity: {alert.Severity}", AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) }, 0, 3);
            layout.Controls.Add(new Label { Text = $"Time: {alert.StartTime:t} - {alert.EndTime:t}", AutoSize = true }, 0, 4);

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill
            };

            Button btnOverride = new Button { Text = "Override", Width = 80 };
            Button btnPause = new Button { Text = "Pause", Width = 80 };
            Button btnClose = new Button { Text = "Close", Width = 80 };
            Button btnPreview = new Button { Text = "Preview", Width = 80 };

            btnClose.Click += (s, e) =>
            {
                flowLayoutPanel1.Controls.Remove(card);
                card.Dispose();
            };

            buttonPanel.Controls.Add(btnOverride);
            //buttonPanel.Controls.Add(btnPause);
            buttonPanel.Controls.Add(btnPreview);
            buttonPanel.Controls.Add(btnClose);

            layout.Controls.Add(buttonPanel, 1, 0);
            layout.SetRowSpan(buttonPanel, 5);

            mainLayout.Controls.Add(colorStrip, 0, 0);
            mainLayout.Controls.Add(layout, 1, 0);

            card.Controls.Add(mainLayout);
            flowLayoutPanel1.Controls.Add(card);
        }
        private void LoadDemoAlerts()
        {
            List<AlertItem> demoAlerts = new List<AlertItem>()
                {
                    new AlertItem
                    {
                        Message = "Train delayed due to technical issue",
                        AudioAvailable = true,
                        Urgency = "High",
                        Severity = "Severe",
                        Certainty = "Observed",
                        StartTime = DateTime.Now,
                        EndTime = DateTime.Now.AddHours(2)
                    },
                    new AlertItem
                    {
                        Message = "Platform change for Train 12345",
                        AudioAvailable = false,
                        Urgency = "Medium",
                        Severity = "Moderate",
                        Certainty = "Likely",
                        StartTime = DateTime.Now,
                        EndTime = DateTime.Now.AddHours(1)
                    },
                    new AlertItem
                    {
                        Message = "cap alert cyclone today",
                        AudioAvailable = false,
                        Urgency = "Medium",
                        Severity = "Moderate",
                        Certainty = "Likely",
                        StartTime = DateTime.Now,
                        EndTime = DateTime.Now.AddHours(1)
                    },
                    new AlertItem
                    {
                        Message = "cap alert cyclone today",
                        AudioAvailable = false,
                        Urgency = "Medium",
                        Severity = "Moderate",
                        Certainty = "Likely",
                        StartTime = DateTime.Now,
                        EndTime = DateTime.Now.AddHours(1)
                    },
                };

            foreach (var alert in demoAlerts)
            {
                AddAlertCard(alert);
            }

        }

        public void RefreshData()
        {
            flowLayoutPanel1.Controls.Clear();
            LoadDemoAlerts();
        }
    }
    public class AlertItem
    {
        public string Message { get; set; }
        public bool AudioAvailable { get; set; }
        public string Urgency { get; set; }
        public string Severity { get; set; }
        public string Certainty { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
