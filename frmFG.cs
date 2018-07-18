/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/19/2016
 * Time: 3:30 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace FAD3
{
	/// <summary>
	/// Description of frmFG.
	/// </summary>
	public partial class frmFG : Form
	{
		List<string> _FishingGrounds = new List<string>();
		private string _MajorGrid = "";

		public List<string> FishingGrounds
		{
			get { return _FishingGrounds; }
			set 
			{
				_FishingGrounds = value;
				foreach(var item in _FishingGrounds)
				{
					listBox1.Items.Add(item.ToString());
				}
			}
		}
		
		public void MajorGridList(string theList){
			_MajorGrid = theList;
			string[] arr = _MajorGrid.Split(',');
			for(int i=0; i<arr.Length;i++){
				comboBox1.Items.Add(arr[i]);
			}
			comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
		}
		
		public frmFG()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		

		
		void Button2_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void txtMajorGrid_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string s = txtMajorGrid.Text;
			bool isCancel = false;
			if (s.Length>0)
			{
				if(s.Length>1 && s.Length <4)
				{
					byte[] arr = Encoding.ASCII.GetBytes(s.ToUpper());
					if (arr[0] >= 65 && arr[0]<90)
					{
						if (arr[1] > 48 && arr[1] < 58 )
						{
							if (arr.Length>2)
							{
								if (arr[1]==49)
								{
									if (arr[2] < 48 || arr[2] > 57)
									{
										isCancel = true;
									}
								}
								else if (arr[1]==50)
								{
									if (arr[2] < 48 || arr[2] > 53)
									{
										isCancel = true;
									}
								}
								else
								{
									isCancel = true;
								}
							}
						}
						else
						{
							isCancel = true;
						}
					}
					else
					{
						isCancel = true;
					}
				 

				}
				else
				{
					isCancel = true;
				}
			}

			if (isCancel)
			{
				e.Cancel = true;
				MessageBox.Show("Please provide correct minor grid");
			}
		}

		private void comboBox1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (comboBox1.Text!="") {
				if (!comboBox1.Items.Contains(comboBox1.Text))
				{
					MessageBox.Show("Item not in list");
					e.Cancel = true;
				}
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			AddToList();
		}

		private void AddToList()
		{
			if (comboBox1.Text != "" && txtMajorGrid.Text != "")
			{
				string newItem = comboBox1.Text + "-" + txtMajorGrid.Text.ToUpper();
				if (listBox1.Items.Contains(newItem) == false)
				{
					listBox1.Items.Add(comboBox1.Text + "-" + txtMajorGrid.Text.ToUpper());
					comboBox1.Text = "";
					txtMajorGrid.Text = "";
					comboBox1.Focus();
				}
				else
				{
					MessageBox.Show("Item already in list");
				}
			}
		}
			

		private void button1_Click(object sender, EventArgs e)
		{

			if (listBox1.Items.Count>0) {
				foreach (var item in listBox1.Items)
				{
					_FishingGrounds.Add(item.ToString());
				}
				this.Close();
			}
            else if (comboBox1.Text.Length>0 && txtMajorGrid.Text.Length>0)
            {
                _FishingGrounds.Add(comboBox1.Text + "-" + txtMajorGrid.Text);
                this.Close();
            }
		}

		private void txtMajorGrid_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Return)
			{
				if (ValidateChildren())
				{
					AddToList();
				} 
			}
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
	}
}
