using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SETSReport.Reports
{
    public partial class rptIndividualTestStatistic : DevExpress.XtraReports.UI.XtraReport
    {
        public rptIndividualTestStatistic()
        {
            InitializeComponent();
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
              if (RowCount > 0 ){
                 int userScore = Convert.ToInt32(GetCurrentColumnValue("UserScore"));
                 int totalScore = Convert.ToInt32(GetCurrentColumnValue("TotalScore"));
                 double percentScore = (userScore / totalScore) * 100;
                 txtScorePerSubject.Text = String.Format("{0:00.00}% ({1}/{2})", percentScore, userScore, totalScore);
                 PercentBar.Visible = percentScore > 0;
                 PercentBar.WidthF = Convert.ToSingle(percentScore * 2.460);
              }
        }

        private void ActualTestHeader_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           if (RowCount > 0 ){
                txtTimeTaken.Text = TimeSpan.FromSeconds(double.Parse(GetCurrentColumnValue("TimeTakenSec").ToString())) + " (hr:min:sec)";

                txtTotalScorePerSubject.Text = GetCurrentColumnValue("TestScore").ToString();

                Double scorePercent = Convert.ToDouble(GetCurrentColumnValue("TotalPercent"));

                TotalPercentBar.BackColor = Color.White;
                TotalPercentBar.WidthF = Convert.ToSingle(scorePercent * 2.46);

                if (scorePercent > 0 ){
                    int lvl2Min = Convert.ToInt32(GetCurrentColumnValue("Level2MarkMin"));
                    int lvl3Min = Convert.ToInt32(GetCurrentColumnValue("Level3MarkMin"));
                    TotalPercentBar.BackColor = scorePercent < lvl2Min ? Color.Red : scorePercent < lvl3Min ? Color.Yellow : Color.Green;
            }
           }
        }

        private void DateTaken_EvaluateBinding(object sender, BindingEventArgs e)
        {
            try
            {
                if (e.Value != null)
                {
                    e.Value = Convert.ToDateTime(e.Value).ToString("dd-MMM-yyyy hh:mm tt");
                }
            }
            catch (Exception ex) { }
        }

    }
}
