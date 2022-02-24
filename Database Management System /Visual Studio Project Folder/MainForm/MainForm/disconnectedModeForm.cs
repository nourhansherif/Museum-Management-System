using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace MainForm
{
    public partial class disconnectedModeForm : Form
    {
        OracleDataAdapter adapter;
        OracleCommandBuilder builder;
        DataSet ds;
        string info = "Data Source=orcl;User Id=hr;Password=hr;";
        Dictionary<string, int> sections = new Dictionary<string, int>();
        public disconnectedModeForm()
        {
            InitializeComponent();
        }

        private void disconnectedModeForm_Load(object sender, EventArgs e)
        {

           
            string command = "Select sectionname, sectionID from sections";
            adapter = new OracleDataAdapter(command, info);
            ds = new DataSet();
            adapter.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count;i++) {
               sectionToViewExhibtsComboBox.Items.Add( ds.Tables[0].Rows[i].ItemArray[0].ToString());
               sections[ds.Tables[0].Rows[i].ItemArray[0].ToString()] = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[1].ToString());
            }
            

        }

        private void showExhibtsButton_Click(object sender, EventArgs e)
        {
            string command = @"select ex.exhibitname, ex.typeexhibit, ex.arrivaldate,
                              ex.countryoffoundation, ex.material, ex.historicalperiod
                              from exhibts ex, sections s
                              where s.sectionid = ex.sectionid and s.sectionname = :name";
            adapter = new OracleDataAdapter(command, info);
            adapter.SelectCommand.Parameters.Add("name", sectionToViewExhibtsComboBox.Text);
            
            ds = new DataSet();
            adapter.Fill(ds);
   
            exhibtsGridView.DataSource = ds.Tables[0];
            

        }

        private void saveChangesButton_Click(object sender, EventArgs e)
        {

            builder = new OracleCommandBuilder(adapter);
            adapter.Update(ds.Tables[0]);



            OracleConnection conn = new OracleConnection(info);
            conn.Open();
            OracleCommand c;
      
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                c = new OracleCommand();
                c.Connection = conn;
                c.CommandText = "update exhibts set sectionID=:id where exhibitName=:name";
                c.Parameters.Add("id", sections[sectionToViewExhibtsComboBox.Text]);
                c.Parameters.Add("name", ds.Tables[0].Rows[i].ItemArray[0].ToString());
                int r = c.ExecuteNonQuery();
               
            }

        }

        private void exhibtsGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
        }

        private void exhibtsGridView_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
        }

        private void exhibtsGridView_NewRowNeeded(object sender, DataGridViewRowEventArgs e)
        {
        }
    }
}
