using DataModel;
using Helper;
using System.Data.SqlClient;


namespace IPIS
{
    public partial class ucTVDisplay : UserControl
    {
        public ucTVDisplay()
        {
            InitializeComponent();



        }

        private int editingMessageId = -1;
        private void ucTVDisplay_Load(object sender, EventArgs e)
        {
            LoadMessages();
        }
        private void LoadMessages()
        {
            try
            {
                lstbxMsgEntry.Items.Clear();

                string query = "SELECT MESSAGE_ID, MESSAGE FROM TVMESSAGE ORDER BY MESSAGE_ID ASC";

                using (SqlConnection conn = new SqlConnection(Config._dbSelfConnection))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string message = reader.GetString(1);

                            lstbxMsgEntry.Items.Add(new MessageItem { Id = id, Message = message });
                        }
                    }
                }

                lstbxMsgEntry.ClearSelected();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading messages: " + ex.Message);
            }
        }

        private void txtbxEnterMsg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string newMsg = txtbxEnterMsg.Text.Trim();
                if (string.IsNullOrWhiteSpace(newMsg))
                {
                    MessageBox.Show("Message cannot be empty.");
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(Config._dbSelfConnection))
                    {
                        conn.Open();

                        if (editingMessageId == -1)
                        {
                            string query = "INSERT INTO TVMESSAGE (MESSAGE) VALUES (@msg)";
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@msg", newMsg);
                                cmd.ExecuteNonQuery();
                            }
                            MessageBox.Show("Message added successfully!");
                        }
                        else
                        {
                            string query = "UPDATE TVMESSAGE SET MESSAGE = @msg WHERE MESSAGE_ID = @id";
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@msg", newMsg);
                                cmd.Parameters.AddWithValue("@id", editingMessageId);
                                cmd.ExecuteNonQuery();
                            }
                            MessageBox.Show("Message updated successfully!");
                            editingMessageId = -1;
                        }
                    }


                    txtbxEnterMsg.Clear();
                    LoadMessages();
                }
                catch (SqlException ex) when (ex.Number == 2627)
                {
                    MessageBox.Show("This message already exists!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving message: " + ex.Message);
                }

                e.SuppressKeyPress = true;
            }
        }

        private void btnEditTV_Click(object sender, EventArgs e)
        {
            if (lstbxMsgEntry.SelectedItem is MessageItem selected)
            {
                editingMessageId = selected.Id;
                txtbxEnterMsg.Text = selected.Message;
                txtbxEnterMsg.Focus();
            }
            else
            {
                MessageBox.Show("Please select a message to edit.");
            }
        }

        private void btnDeleteTV_Click(object sender, EventArgs e)
        {
            if (lstbxMsgEntry.SelectedItem is MessageItem selected)
            {
                DialogResult dr = MessageBox.Show(
                    $"Are you sure you want to delete this message?\n\n“{selected.Message}”",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(Config._dbSelfConnection))
                        {
                            conn.Open();
                            string query = "DELETE FROM TVMESSAGE WHERE MESSAGE_ID = @id";
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@id", selected.Id);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Message deleted successfully!", "Delete Message",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadMessages();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting message: " + ex.Message,
                            "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a message to delete.");
            }
        }

        

        private void lstbxMsgEntry_DoubleClick(object sender, EventArgs e)
        {
            if (lstbxMsgEntry.SelectedItem is MessageItem selectedItem)
            {
                string msg = selectedItem.Message;

                if (!lstbxDisplayMsg.Items.Contains(msg))
                {
                    lstbxDisplayMsg.Items.Add(msg);
                }
            }
        }

        private void lstbxDisplayMsg_DoubleClick(object sender, EventArgs e)
        {
            if (lstbxDisplayMsg.SelectedItem != null)
            {
                lstbxDisplayMsg.Items.Remove(lstbxDisplayMsg.SelectedItem);
            }

        }

        private void btnClearTV_Click(object sender, EventArgs e)
        {
            if (lstbxDisplayMsg.Items.Count == 0)
            {
                MessageBox.Show("No messages to clear.", "Clear Messages",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult dr = MessageBox.Show(
                "Are you sure you want to remove all messages?",
                "Confirm Clear",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                lstbxDisplayMsg.Items.Clear();
            }

        }

        private void txtbxEnterMsg_TextChanged(object sender, EventArgs e)
        {

        }

        private void lstbxMsgEntry_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstbxDisplayMsg_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void RefreshData()
        {
            LoadMessages();
        }
    }
}
