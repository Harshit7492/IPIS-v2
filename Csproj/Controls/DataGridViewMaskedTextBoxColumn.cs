using System;
using System.Windows.Forms;

namespace IPISv2.Controls
{
    public class DataGridViewMaskedTextBoxColumn : DataGridViewColumn
    {
        public string Mask { get; set; }

        public DataGridViewMaskedTextBoxColumn() : base(new DataGridViewMaskedTextBoxCell()) { }

        public override object Clone()
        {
            var col = (DataGridViewMaskedTextBoxColumn)base.Clone();
            col.Mask = this.Mask;
            return col;
        }

        public override DataGridViewCell CellTemplate
        {
            get => base.CellTemplate;
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewMaskedTextBoxCell)))
                {
                    throw new InvalidCastException("CellTemplate must be a DataGridViewMaskedTextBoxCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class DataGridViewMaskedTextBoxCell : DataGridViewTextBoxCell
    {
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            if (DataGridView.EditingControl is MaskedTextBox mtb)
            {
                var col = (DataGridViewMaskedTextBoxColumn)OwningColumn;
                mtb.Mask = col.Mask;
                mtb.Text = this.Value?.ToString() ?? "";
            }
        }

        public override Type EditType => typeof(DataGridViewMaskedTextBoxEditingControl);
        public override Type ValueType => typeof(string);
        public override object DefaultNewRowValue => "";
    }

    public class DataGridViewMaskedTextBoxEditingControl : MaskedTextBox, IDataGridViewEditingControl
    {
        private DataGridView dataGridView;
        private bool valueChanged;
        private int rowIndex;

        public DataGridViewMaskedTextBoxEditingControl()
        {
            this.Mask = "00:00";  
        }

        public object EditingControlFormattedValue
        {
            get => this.Text;
            set
            {
                if (value is string str)
                    this.Text = str;
            }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context) => EditingControlFormattedValue;
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
        }

        public int EditingControlRowIndex { get => rowIndex; set => rowIndex = value; }
        public bool EditingControlValueChanged { get => valueChanged; set => valueChanged = value; }
        public Cursor EditingPanelCursor => base.Cursor;
        public DataGridView EditingControlDataGridView { get => dataGridView; set => dataGridView = value; }
        public bool RepositionEditingControlOnValueChange => false;

        public void PrepareEditingControlForEdit(bool selectAll) { }
        public void NotifyDataGridViewOfValueChange()
        {
            valueChanged = true;
            dataGridView?.NotifyCurrentCellDirty(true);
        }
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.valueChanged = true;
            this.dataGridView?.NotifyCurrentCellDirty(true);
        }

        protected override void OnValidating(System.ComponentModel.CancelEventArgs e)
        {
            base.OnValidating(e);

            if (!string.IsNullOrWhiteSpace(this.Text))
            {
                if (TimeSpan.TryParse(this.Text, out var ts))
                {
                    if (ts.Hours > 23 || ts.Minutes > 59)
                    {
                        this.Text = "00:00";
                        e.Cancel = true;
                    }
                }
                else
                {
                    this.Text = "00:00";
                    e.Cancel = true;
                }
            }
        }
        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.Back:
                case Keys.Delete:
                case Keys.Tab:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

    }
}
