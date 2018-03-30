using AddAssetIDTable.BusinessClasses;
using AddAssetIDTable.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddAssetIDTable
{
    public partial class AddAssetTable : Form
    {
    
        private BindingSource bindingSource1 = new BindingSource();
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        private bool _isDirty = false;

        public AddAssetTable()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //set the connection string, store in app.config
            GetConnectionString connString = new GetConnectionString(this.txtServer.Text, cboDbs.SelectedItem.ToString());
            string ConnStringValue = connString.ConnectionString;

            // Bind the DataGridView to the BindingSource
            // and load the data from the database.
            dgAssetTable.DataSource = bindingSource1;
            string TableName = this.cboTable.SelectedItem.ToString();

            //gets a list of columns in t-sql syntax so we can edit a table
            string SQL = GetSQL(ConnStringValue, TableName); 

            //go populate the data grid view
            GetData(SQL, ConnStringValue);



        }
         

        private void submitButton_Click(object sender, System.EventArgs e)
        {
            // Update the database with the user's changes.
            dataAdapter.Update((DataTable)bindingSource1.DataSource);
            _isDirty = false;
        }

        private string GetSQL(string ConnectionString, string TableName)
        {
            //given a connection string a table and the below procedure in that database it will get a select query, ommitting identity columns
            string returnSQL = string.Empty;

            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    string SQL = "exec dbo.usp_Arvada_GetEditableTables @ReturnType = 1, @TableNAme = '" + TableName + "'";
                    

                    // Set up a command with the given query and associate
                    // this with the current connection.
                    using (SqlCommand cmd = new SqlCommand(SQL, con))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                returnSQL = dr[0].ToString();
                            }
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return returnSQL;

        }

        private void GetData(string selectCommand, string connectionString)
        {
            //gets the data based of the sql query returned
            try
            {
                 
                
                // Create a new data adapter based on the specified query.
                dataAdapter = new SqlDataAdapter(selectCommand, connectionString);

                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                // Populate a new data table and bind it to the BindingSource.
                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);
                bindingSource1.DataSource = table;

                // Resize the DataGridView columns to fit the newly loaded content.
                dgAssetTable.AutoResizeColumns();

                
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dgAssetTable_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            //for the main purpose of this, editing the asset update table, try to default the schema, otherwise just skip this
            string PriorSchema;

            _isDirty = true;

            if (dgAssetTable.Columns.Contains("SchemaName") && dgAssetTable.Columns["SchemaName"].Visible)
            {
                //try to guess what the schema is by using the prior row's value
                if (this.dgAssetTable.Rows.Count - 2 > 0)
                {
                    DataGridViewRow schema = this.dgAssetTable.Rows[this.dgAssetTable.Rows.Count - 2];
                    PriorSchema = schema.Cells["SchemaName"].Value.ToString();
                }
                else
                {
                    PriorSchema = "dbo";
                }

                e.Row.Cells["SchemaName"].Value = PriorSchema;
                e.Row.Cells["LastAssetNumber"].Value = 0;
            }

          
        }

        private void AddAssetTable_FormClosing(object sender, FormClosingEventArgs e)
        {
            // upon closing, check if the form is dirty; if so, prompt
            // to save changes
            if (_isDirty)
            {
                DialogResult result
                    = (MessageBox.Show(
                       "Would you like to save changes before closing?"
                       , "Save Changes"
                       , MessageBoxButtons.YesNoCancel
                       , MessageBoxIcon.Question));

                switch (result)
                {
                    case DialogResult.Yes:
                        // save the document
                        submitButton_Click(sender, e);
                        break;

                    case DialogResult.No:
                        // just allow the form to close
                        // without saving
                        break;

                    case DialogResult.Cancel:
                        // cancel the close
                        e.Cancel = true;
                        break;
                }
            }
        }
         
        

        private void dgAssetTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //I should do this more cleanly but did it the hard way for now, just saying we need to make sure we have saved the data before we close the form
            _isDirty = true;
        }

        private void dgAssetTable_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            //I should do this more cleanly but did it the hard way for now, just saying we need to make sure we have saved the data before we close the form
            _isDirty = true;
        }

        private void cboDbs_Enter(object sender, EventArgs e)
        {
            //Get the list of databases on the server, a good way to see if windows auth connection works to the SQLServer typed in
            GetDatabaseList();


        }

        private void GetDatabaseList()
        {
            GetConnectionString connString2 = new GetConnectionString(this.txtServer.Text);
            string ConnStringValue2 = connString2.ConnectionStringStart;

            List<string> list = new List<string>();

            try
            {
                using (SqlConnection con = new SqlConnection(ConnStringValue2))
                {
                    con.Open();

                    // Set up a command with the given query and associate
                    // this with the current connection.
                    using (SqlCommand cmd = new SqlCommand("SELECT name from sys.databases where database_id > 4 order by name", con))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                list.Add(dr[0].ToString());
                            }
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            this.cboDbs.DisplayMember = "name";
            this.cboDbs.ValueMember = "name";
            this.cboDbs.DataSource = list;
            this.cboDbs.BindingContext = this.BindingContext;
        }

        private void cboTable_Enter(object sender, EventArgs e)
        {
            //get possible tables to edit, exclude large ones and ones better managed in arcCatalog etc...
            GetTableList();
        }

        private bool CheckProcedure()
        {
            //checks if the procedure this app needs exist, if it does return true else false
            GetConnectionString connString2 = new GetConnectionString(this.txtServer.Text, cboDbs.SelectedItem.ToString());
            string ConnStringValue2 = connString2.ConnectionString;

            int ProcExists = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(ConnStringValue2))
                {
                    con.Open();

                    string SQL = "select 1 from sys.objects where name = 'usp_Arvada_GetEditableTables' ";


                    // Set up a command with the given query and associate
                    // this with the current connection.
                    using (SqlCommand cmd = new SqlCommand(SQL, con))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                ProcExists = (int)dr[0];
                            }
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (ProcExists == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private void CreateProcedure()
        {
            //create the procedure we need to get tables and table sql
            string sqltext = Resources.MyStoredProcedure;
            GetConnectionString connString2 = new GetConnectionString(this.txtServer.Text, cboDbs.SelectedItem.ToString());
            string ConnStringValue2 = connString2.ConnectionString;

            List<string> list = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(
                  ConnStringValue2))
                    {
                        SqlCommand command = new SqlCommand(sqltext, connection);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                    }

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        private void GetTableList()
        {
            //get possible tables to edit, exclude large ones and ones better managed in arcCatalog etc...
            GetConnectionString connString2 = new GetConnectionString(this.txtServer.Text, cboDbs.SelectedItem.ToString());
            string ConnStringValue2 = connString2.ConnectionString;

            List<string> list = new List<string>();

            if (!CheckProcedure())
            {
                CreateProcedure();
            }

            try
            {
                using (SqlConnection con = new SqlConnection(ConnStringValue2))
                {
                    con.Open();

                    // Set up a command with the given query and associate
                    // this with the current connection.
                    using (SqlCommand cmd = new SqlCommand("exec dbo.usp_Arvada_GetEditableTables @ReturnType = 0", con))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                list.Add(dr[0].ToString());
                            }
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            this.cboTable.DisplayMember = "TableName";
            this.cboTable.ValueMember = "TableName";
            this.cboTable.DataSource = list;
            this.cboTable.BindingContext = this.BindingContext;
            if (list.Contains("dbo.AssetIDTableMap"))
            {
                this.cboTable.SelectedIndex = cboTable.FindStringExact("dbo.AssetIDTableMap");
                cboTable.Select();
                cboTable.Focus();  
            }
        }

       
    }
}
