using DataBaseHandler;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPISv2
{
    public partial class ucVendorDetails : UserControl
    {
        // ---------------------------------------------------------------
        // Fields
        // ---------------------------------------------------------------
        private const string DefaultModeText = "Adding new vendor";

        private List<Vendor> _vendors = new List<Vendor>();
        private Vendor _editingVendor;   // null = Add mode

        // ---------------------------------------------------------------
        // Constructor
        // ---------------------------------------------------------------
        public ucVendorDetails()
        {
            InitializeComponent();
            SubscribeEvents();
            ConfigureVendorsGrid();
            Load += async (_, __) => await LoadVendorsAsync();
        }

        // ---------------------------------------------------------------
        // Event wiring
        // ---------------------------------------------------------------
        private void SubscribeEvents()
        {
            btnSave.Click += SaveButton_Click;
            dgvVendors.CellContentClick += VendorsGrid_CellContentClick;
        }

        // ---------------------------------------------------------------
        // Data loading
        // ---------------------------------------------------------------
        private async Task LoadVendorsAsync()
        {
            try
            {
                var vendors = await _dbService.GetVendorDetails();
                _vendors = vendors ?? new List<Vendor>();
                RefreshVendorsGrid();
            }
            catch (Exception ex)
            {
                ShowError("Failed to load vendors.", ex);
            }
        }

        // ---------------------------------------------------------------
        // Grid configuration
        // ---------------------------------------------------------------
        private void ConfigureVendorsGrid()
        {
            dgvVendors.AutoGenerateColumns = false;
            dgvVendors.AllowUserToAddRows = false;
            dgvVendors.ReadOnly = true;
            dgvVendors.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVendors.MultiSelect = false;
            dgvVendors.RowHeadersVisible = false;
            dgvVendors.BackgroundColor = System.Drawing.Color.White;

            // ---------------------------------------------------------------
            // NEW: Force black text and remove blue selection highlight
            // ---------------------------------------------------------------
            dgvVendors.EnableHeadersVisualStyles = false;                    // NEW

            // Grid overall
            dgvVendors.GridColor = System.Drawing.Color.LightGray;           // NEW

            // Column headers
            dgvVendors.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;      // NEW
            dgvVendors.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Black;      // NEW
            dgvVendors.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.White; // NEW
            dgvVendors.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black; // NEW
            dgvVendors.ColumnHeadersDefaultCellStyle.Font =
                new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);            // NEW

            // Rows (normal state)
            dgvVendors.DefaultCellStyle.BackColor = System.Drawing.Color.White;                  // NEW
            dgvVendors.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;                  // NEW
            dgvVendors.DefaultCellStyle.Font =
                new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);        // NEW

            // Rows (selected state) — keep black text, NO blue background
            dgvVendors.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;         // NEW
            dgvVendors.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;         // NEW

            // Alternating row style — keeps readability without blue
            dgvVendors.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 245, 245); // NEW
            dgvVendors.AlternatingRowsDefaultCellStyle.ForeColor = System.Drawing.Color.Black;   // NEW
            dgvVendors.AlternatingRowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(225, 225, 225); // NEW
            dgvVendors.AlternatingRowsDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black; // NEW

            dgvVendors.Columns.Clear();

            AddTextColumn("colId", null, "Id", visible: false);
            AddTextColumn("colName", "Vendor Name", "VendorName", width: 260);
            AddTextColumn("colCode", "Vendor Code", "VendorCode", width: 160);
            AddCheckBoxColumn("colActive", "Is Active", "IsActive", width: 90);
            AddButtonColumn("colEdit", "Edit", System.Drawing.Color.White);
            AddButtonColumn("colDelete", "Delete", System.Drawing.Color.White);
        }
        private void AddTextColumn(string name, string header, string property,
                                   int width = 100, bool visible = true)
        {
            dgvVendors.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                DataPropertyName = property,
                Width = width,
                Visible = visible
            });
        }

        private void AddCheckBoxColumn(string name, string header, string property, int width)
        {
            dgvVendors.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = name,
                HeaderText = header,
                DataPropertyName = property,
                Width = width
            });
        }

        private void AddButtonColumn(string name, string text, System.Drawing.Color backColor)
        {
            var column = new DataGridViewButtonColumn
            {
                Name = name,
                HeaderText = text,
                Text = text,
                UseColumnTextForButtonValue = true,
                Width = 80,
                FlatStyle = FlatStyle.Flat
            };

            column.DefaultCellStyle.BackColor = backColor;
            column.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;           // BLACK text
            column.DefaultCellStyle.SelectionBackColor = backColor;
            column.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;  // BLACK text on selection
            column.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            dgvVendors.Columns.Add(column);
        }

        // ---------------------------------------------------------------
        // Save (Insert when Id = 0, otherwise Update)
        // ---------------------------------------------------------------
        private async void SaveButton_Click(object sender, EventArgs e)
        {
            if (!TryReadFormInputs(out string vendorName, out string vendorCode, out bool isActive))
                return;

            var vendor = new Vendor
            {
                Id = _editingVendor?.Id ?? 0,
                VendorName = vendorName,
                VendorCode = vendorCode,
                IsActive = isActive
            };

            int resultCode = await _dbService.SaveVendorDetail(vendor);

            switch (resultCode)
            {
                case 0:
                    ShowInfo(_editingVendor == null
                        ? "Vendor added successfully."
                        : "Vendor updated successfully.");
                    await LoadVendorsAsync();
                    ResetForm();
                    break;

                case -1:
                    ShowWarning("Vendor code already exists.");
                    txtVendorCode.Focus();
                    break;

                default:
                    ShowError("Failed to save vendor.");
                    break;
            }
        }

        // ---------------------------------------------------------------
        // Grid clicks (Edit / Delete)
        // ---------------------------------------------------------------
        private void VendorsGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string columnName = dgvVendors.Columns[e.ColumnIndex].Name;

            switch (columnName)
            {
                case "colEdit":
                    BeginEditVendor(e.RowIndex);
                    break;

                case "colDelete":
                    _ = DeleteVendorAsync(e.RowIndex);
                    break;
            }
        }

        private void BeginEditVendor(int rowIndex)
        {
            int vendorId = Convert.ToInt32(dgvVendors.Rows[rowIndex].Cells["colId"].Value);
            var vendor = _vendors.FirstOrDefault(v => v.Id == vendorId);
            if (vendor == null) return;

            _editingVendor = vendor;

            txtVendorName.Text = vendor.VendorName;
            txtVendorCode.Text = vendor.VendorCode;
            chkIsActive.Checked = vendor.IsActive;

            lblMode.Text = $"Editing: {vendor.VendorName} (ID {vendor.Id})";
            btnSave.Text = "SAVE";
            txtVendorName.Focus();
        }

        private async Task DeleteVendorAsync(int rowIndex)
        {
            int vendorId = Convert.ToInt32(dgvVendors.Rows[rowIndex].Cells["colId"].Value);
            var vendor = _vendors.FirstOrDefault(v => v.Id == vendorId);
            if (vendor == null) return;

            var confirmation = MessageBox.Show(
                $"Delete vendor \"{vendor.VendorName}\" ({vendor.VendorCode})?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes) return;

            string query =
                $"DELETE FROM Vendors WHERE Id = {vendorId}; " +
                "SELECT 0;";

            var result = _dbService.ExecuteScalar(query);
            int resultCode = result == null ? -1 : Convert.ToInt32(result);

            if (resultCode == 0)
            {
                ShowInfo("Vendor deleted successfully.");
                await LoadVendorsAsync();
                ResetForm();
            }
            else
            {
                ShowError("Failed to delete vendor.");
            }
        }
        // ---------------------------------------------------------------
        // Form helpers
        // ---------------------------------------------------------------
        private bool TryReadFormInputs(out string vendorName, out string vendorCode, out bool isActive)
        {
            vendorName = txtVendorName.Text.Trim();
            vendorCode = txtVendorCode.Text.Trim();
            isActive = chkIsActive.Checked;

            if (string.IsNullOrWhiteSpace(vendorCode))
            {
                ShowWarning("Vendor Code is required.");
                txtVendorCode.Focus();
                return false;
            }

            return true;
        }

        private void ResetForm()
        {
            txtVendorName.Clear();
            txtVendorCode.Clear();
            chkIsActive.Checked = true;
            _editingVendor = null;

            lblMode.Text = DefaultModeText;
            btnSave.Text = "Save";
            txtVendorName.Focus();
        }

        private void RefreshVendorsGrid()
        {
            dgvVendors.DataSource = null;
            dgvVendors.DataSource = _vendors.ToList();
        }

        // ---------------------------------------------------------------
        // Message helpers
        // ---------------------------------------------------------------
        private static void ShowInfo(string message) =>
            MessageBox.Show(message, "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

        private static void ShowWarning(string message) =>
            MessageBox.Show(message, "Validation",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private static void ShowError(string message, Exception ex = null)
        {
            string text = ex == null ? message : $"{message}\n{ex.Message}";
            MessageBox.Show(text, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // ---------------------------------------------------------------
        // Public API
        // ---------------------------------------------------------------
        public IReadOnlyList<Vendor> GetVendors() => _vendors.AsReadOnly();
        public void RefreshData() => RefreshVendorsGrid();
    }
}