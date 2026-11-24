using System;
using System.Data;
using System.Data.SqlClient;

namespace IPISv2.Report.ReportDataSets
{
    public partial class LEDMatrixFaultReportDataSet
    {
    }
}

namespace IPISv2.Report.ReportDataSets.LEDMatrixFaultReportDataSetTableAdapters
{
    public partial class LOGLED_SHORT_OPENTableAdapter
    {
        public virtual int FillByDateRange(
            LEDMatrixFaultReportDataSet.LOGLED_SHORT_OPENDataTable dataTable,
            DateTime? FromDate,
            DateTime? ToDate)
        {
            if (dataTable == null)
            {
                throw new ArgumentNullException(nameof(dataTable));
            }

            using (var cmd = new SqlCommand(
                "SELECT ID, Board_ID, Board_IP, Board_Name, PF_Number, ledMatrix, logDate " +
                "FROM dbo.LOGLED_SHORT_OPEN " +
                "WHERE (@FromDate IS NULL OR logDate >= @FromDate) " +
                "  AND (@ToDate IS NULL OR logDate <= @ToDate) " +
                "ORDER BY logDate DESC",
                this.Connection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime)
                {
                    Value = FromDate.HasValue ? (object)FromDate.Value : DBNull.Value
                });
                cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.DateTime)
                {
                    Value = ToDate.HasValue ? (object)ToDate.Value : DBNull.Value
                });

                if (this.Transaction != null)
                {
                    cmd.Transaction = this.Transaction;
                }

                SqlCommand previousCommand = this.Adapter.SelectCommand;
                try
                {
                    this.Adapter.SelectCommand = cmd;
                    if (this.ClearBeforeFill)
                    {
                        dataTable.Clear();
                    }
                    return this.Adapter.Fill(dataTable);
                }
                finally
                {
                    this.Adapter.SelectCommand = previousCommand;
                }
            }
        }

        public virtual LEDMatrixFaultReportDataSet.LOGLED_SHORT_OPENDataTable GetDataByDateRange(
            DateTime? FromDate,
            DateTime? ToDate)
        {
            var dataTable = new LEDMatrixFaultReportDataSet.LOGLED_SHORT_OPENDataTable();
            this.FillByDateRange(dataTable, FromDate, ToDate);
            return dataTable;
        }
    }
}
