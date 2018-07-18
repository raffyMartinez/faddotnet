/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/19/2016
 * Time: 1:45 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace FAD3
{
	/// <summary>
	/// Description of frmCatch.
	/// </summary>
	public partial class frmCatch : Form
	{
		
		private sampling _Sampling = new sampling();
		private string _CatchLineGUID = "";
		private string _CatchNameGUID = "";
		private string _CatchCompRowGUID = "";
		private Dictionary<string, string> _CatchCompRow = new Dictionary<string, string>();
		private double _CatchWt = 0;
		private double _CatchSampleWt = 0;
		private double _TotalWtFromTotal = 0;
		private bool _isNew = false;
		private frmMain _ParentForm;


		public new frmMain ParentForm
		{
			set { _ParentForm = value; }
			get { return _ParentForm; }
		}

		public double TotalWtFromTotal
		{
			get { return _TotalWtFromTotal; }
			set { _TotalWtFromTotal = value; }
		}

		public string CatchNameGUID
		{
			get { return _CatchNameGUID; }
			set { _CatchNameGUID = value; }
		}

		public string CatchLineGUID
		{
			get { return _CatchLineGUID; }
			set
			{
				_CatchLineGUID = value;
				_CatchCompRow = _Sampling.CatchCompRow(_CatchLineGUID);
				FillForm();
				_isNew = false;
			}
		}
		
		public void AddNew()
		{
			_isNew = true;
			radioSciName.Checked = true;
			checkFromTotal.Checked = false;
			chkSubsampled.Checked = false;
			txtWt.Text = "";
			txtCt.Text = "";
			txtSubWt.Text = "";
			txtSubCt.Text = "";
			comboName1.Text = "";
			comboName2.Text = "";
			comboLocal.Text = "";
			comboName1.Focus();
			Text = "Adding new catch";
			lblCompWt.Text = "Computed weight:";
			lblCompCt.Text = "Computed count:";
			lblWtCatch.Text = "Weight of catch: " + _Sampling.TotalCatchWt.ToString();
			lblWtSample.Text = "Weight of sample: " + _Sampling.SampleWtFromTotalCatch.ToString();
		}

		public sampling Sampling{
			get{return _Sampling;}
			set
			{
				_Sampling = value;
				lblWtCatch.Text = "Weight of catch: " + _Sampling.TotalCatchWt.ToString();
				lblWtSample.Text = "Weight of sample: " + _Sampling.SampleWtFromTotalCatch.ToString();
				lblTitle.Text = "Reference no: " + _Sampling.ReferenceNo;
				//lblTitle.Text = "Reference no: " + _Sampling.re
			}
		}
		
		public frmCatch()
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

		private void frmCatch_Load(object sender, EventArgs e)
		{
			txtSubCt.Enabled = chkSubsampled.Checked;
			txtSubWt.Enabled = chkSubsampled.Checked;
			label3.Enabled = chkSubsampled.Checked;
			label4.Enabled = chkSubsampled.Checked;
			FillComboGenus();
			FillComboLocal();
			comboName1.Text = "";
			comboName2.Text = "";
			comboLocal.Text = "";
			comboLocal.Location = comboName1.Location;
			lblFB.Enabled = false;
			lblInFishbase.Text = "";  
			lblCatchTaxa.Text = "";
		}

		private void FillComboGenus()
		{
			//radioSciName.Checked = true;
			comboName1.DataSource = new BindingSource(names.GenusList, null);
			comboName1.DisplayMember = "Name";
			comboName1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			comboName1.AutoCompleteSource = AutoCompleteSource.ListItems;
			comboName2.Visible = true;
			comboName2.Text = "";
			
		}

		private void FillComboLocal()
		{
			comboLocal.DataSource = new BindingSource(names.LocalNameListDict, null);
			comboLocal.DisplayMember = "Value";
			comboLocal.ValueMember = "Key";
			comboLocal.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			comboLocal.AutoCompleteSource = AutoCompleteSource.ListItems;
			comboName2.Visible = false;
		}

		private void FillForm()
		{

			txtWt.Text = _CatchCompRow["wt"];
			txtCt.Text = _CatchCompRow["ct"];
			txtSubWt.Text = _CatchCompRow["swt"];
			txtSubCt.Text = _CatchCompRow["sct"];
			chkSubsampled.Checked = txtSubWt.Text.Length>0 && double.Parse(txtSubWt.Text)>0;
			
				
	   
			_CatchNameGUID = _CatchCompRow["NameGUID"];
			_CatchCompRowGUID = _CatchCompRow["DetailRow"];
			lblWtCatch.Text = "Weight of catch: " + _CatchCompRow["WtCatch"];
			lblWtSample.Text = "Weight of sample: " + _CatchCompRow["WtSample"];

			if (_CatchCompRow["NameType"]=="1")
			{
				//FillComboGenus();
				FillSpeciesCombo(_CatchCompRow["Name1"]);
				radioSciName.Checked = true;
				comboName1.Text = _CatchCompRow["Name1"];
				comboName2.Text = _CatchCompRow["Name2"];
				comboName2.Tag = comboName1.Text;
				comboLocal.Text = "";
			}
			else
			{
				radioLocalName.Checked = true;
				comboName1.Text = "";
				comboName2.Text = "";
				comboLocal.Text = _CatchCompRow["Name1"];
			}

			string myTaxa = global.TaxaFromCatchNameGUID(_CatchNameGUID).ToString();
			lblCatchTaxa.Text = myTaxa;
			if (myTaxa == "Fish")
			{
				lblFB.Enabled = true;
				if (names.IsListedInFishBase(_CatchNameGUID))
				{
					lblInFishbase.Text = "Yes";
				}
				else
				{
					lblInFishbase.Text = "No";
				}
			}
			else
			{
				lblInFishbase.Text = "No";
			}

			checkFromTotal.Checked =  _CatchCompRow["FromTotal"]=="True";
			checkLiveFish.Checked = _CatchCompRow["Live"] == "True";

			double num;
			if(double.TryParse(_CatchCompRow["WtCatch"],out num))
			{
				_CatchWt = num;
			}
			if (double.TryParse(_CatchCompRow["WtSample"], out num))
			{
				_CatchSampleWt = num;
			}

			if(ComputeSubValues()==false)
			{

			}

		}

		private bool ComputeSubValues()
		{
			double TotalCatchWt = 0;
			double SampleCatchWt = 0;
			double RowCatchWt = 0;
			double RowCatchCt = 0;
			double RowSubSampleWt = 0;
			long RowSubSampleCt = 0;
			bool Proceed = false;
			double num;
			long num2;

			//get numeric values in the dictionary
			if (double.TryParse(_CatchCompRow["WtSample"].ToString(), out num))
			{
				SampleCatchWt = num;
			}

			if (double.TryParse(_CatchCompRow["WtCatch"].ToString(), out num))
			{
				TotalCatchWt = num;
			}

			if (double.TryParse(_CatchCompRow["wt"].ToString(), out num))
			{
				RowCatchWt = num;
			}

			if (double.TryParse(_CatchCompRow["swt"].ToString(), out num))
			{
				RowSubSampleWt = num;
			}

			if (long.TryParse(_CatchCompRow["ct"].ToString(), out num2))
			{
				RowCatchCt = num2;
			}

			if (long.TryParse(_CatchCompRow["sct"].ToString(), out num2))
			{
				RowSubSampleCt = num2;
			}



			//if from total then computed wt is the row wt. When there is no row count
			// then we compute it from the subsample.
			if (checkFromTotal.Checked)
			{
				lblCompWt.Text = "Computed weight: " + _CatchCompRow["wt"];
				lblCompCt.Text = "Computed count: " + _CatchCompRow["ct"];

				if (_CatchCompRow["ct"] == "")
				{
					// we check if the required numbers for the computation
					// are there. If one of the values is zero then we dont
					// compute and leave the corresponding textbox blank.
					Proceed = RowCatchCt > 0 && RowCatchWt > 0 && RowCatchWt > 0;
					if (Proceed)
					{
						lblCompCt.Text = "Computed count: " +  (int)(RowCatchWt / RowSubSampleWt) * RowSubSampleCt;
					}
				}
				else
				{
					Proceed = true;
				}
			}

			// if not from total then we raise the computed weight from the sample wt.
			// using a simple ratio and proportion.
			else
			{
				// but first we check if the required values are there.
				Proceed = RowCatchWt > 0 && TotalCatchWt > 0 && SampleCatchWt > 0;
				if (Proceed)
				{
					lblCompWt.Text = "Computed weight: " + TotalCatchWt / (SampleCatchWt / RowCatchWt);
					if (_CatchCompRow["ct"].ToString()=="")
					{
						Proceed = RowSubSampleCt > 0 && RowSubSampleWt > 0;
						if (Proceed)
						{
							lblCompCt.Text = "Computed count: " + (int)((RowCatchWt / RowSubSampleWt) * RowSubSampleCt) * (_CatchSampleWt / RowCatchWt);
						}
					}
					else
					{
						Proceed = RowCatchCt > 0;
						if (Proceed)
						{
							lblCompCt.Text = "Computed count: " + (int)(TotalCatchWt / RowCatchWt) * RowCatchCt;
						}
					}
				}
			}
			return Proceed;
		}

		private void FillSpeciesCombo(string Genus)
		{
			names.Genus = Genus;
			comboName2.DataSource = new BindingSource(names.speciesList, null);
			comboName2.DisplayMember = "Value";
			comboName2.ValueMember = "key";
			comboName2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			comboName2.AutoCompleteSource = AutoCompleteSource.ListItems;
		}
		private void chkSubsampled_CheckedChanged(object sender, EventArgs e)
		{
			txtSubCt.Enabled = chkSubsampled.Checked;
			txtSubWt.Enabled = chkSubsampled.Checked;
			label3.Enabled = chkSubsampled.Checked;
			label4.Enabled = chkSubsampled.Checked;
			if (chkSubsampled.Checked==false)
			{
				txtSubCt.Text = "";
				txtSubWt.Text = "";
			}
		}

		private void comboname2_validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (comboName2.Text != "")
			{
				string s;
				try { s = comboName2.SelectedItem.ToString(); }
				catch { s = ""; }

				if (s == "")
				{
					e.Cancel = true;
					MessageBox.Show("Item  not found");
				}
				else
				{
					_CatchNameGUID = ((KeyValuePair<string, string>)comboName2.SelectedItem).Key;
					if (_CatchNameGUID.Length>0)
					{
						string myTaxa = global.TaxaFromCatchNameGUID(_CatchNameGUID).ToString();
						lblCatchTaxa.Text = myTaxa;
						if (myTaxa == "Fish")
						{
							if (names.IsListedInFishBase(_CatchNameGUID))
							{
								lblInFishbase.Text = "Yes";
							}
							else
							{
								lblInFishbase.Text = "No";
							}
							lblFB.Enabled = true;
						}
						else
						{
							lblFB.Enabled = false;
						}
						comboName2.Tag = comboName1.Text;
					}
				}
				
			}
		}


		private void comboname1_validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string s = "";
			if (comboName1.Text != "")
			{
				try
				{
					s = comboName1.SelectedItem.ToString();
				}
				catch
				{
					s = "";
				}

				if (s != "" )
				{
					if (comboName2.Tag.ToString() != comboName1.Text) {
						string selected = comboName1.SelectedItem.ToString();
						if (radioSciName.Checked)
						{
							FillSpeciesCombo(selected);
						}
					}
				}
				else 
				{
					MessageBox.Show("Item not found");
					e.Cancel = true;
				}
			}
		}
		private void comboName2_Validating_1(object sender, System.ComponentModel.CancelEventArgs e)
		{

		}
		private void radioSciName_CheckedChanged(object sender, EventArgs e)
		{
			RadioButton rb;
			
			foreach(Control c in groupBox1.Controls)
			{
				if (c is RadioButton)
				{
					rb = (RadioButton)c;
					if (rb.Checked)
					{
						switch (rb.Name)
						{
							case "radioSciName":
								comboName1.Visible = true;
								comboName2.Visible = true;
								comboName1.Text = _CatchCompRow["Name1"];
								comboName2.Text = _CatchCompRow["Name2"];
								comboLocal.Visible = false;
								comboName1.Focus();
								lblCatchTaxa.Text = "";
								break;
							case "radioLocalName":
								comboLocal.Visible = true;
								comboName1.Text = _CatchCompRow["Name1"];
								comboName2.Text = "";
								comboName1.Visible = false;
								comboName2.Visible = false;
								comboLocal.Focus();
								lblCatchTaxa.Text = "Not applicable";
								break;
						}
						lblCatchTaxa.Enabled = rb.Name =="radioSciName";
						lblTaxa.Enabled = lblCatchTaxa.Enabled;
						lblFB.Enabled = lblCatchTaxa.Enabled;
						
					}
				}
			}            
		}

		private bool ValidateData()
		{
			bool Success = false;
			string msg = "";
			Success = _CatchNameGUID.Length > 0 && txtWt.Text.Length > 0;
			if (Success)
			{
				if (txtCt.Text.Length==0)
				{
					Success = txtSubCt.Text.Length > 0 && txtSubWt.Text.Length > 0;
					if (Success==false)
					{
						msg = "Subsample count and subsample weight is requiered if catch count is not provided";
					}
				}
			}
			else
			{
				msg = "Name of catch and catch weight is required";
			}

			if (msg.Length>0)
			{
				MessageBox.Show(msg);
			}
			return Success;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (ValidateData())
			{
				long? CatchCount = null;

				if (txtCt.Text.Length > 0)
				{
					CatchCount = long.Parse(txtCt.Text);
				}

				string myName = "";
				if (radioSciName.Checked)
				{
					myName = comboName1.Text + " " + comboName2.Text;
					_CatchNameGUID = ((KeyValuePair<string, string>)comboName2.SelectedItem).Key;
				}
				else if (radioLocalName.Checked)
				{
					myName = comboLocal.Text;
					_CatchNameGUID = ((KeyValuePair<string, string>)comboLocal.SelectedItem).Key;
				}

				string myGUID = "";
				if (_isNew)
				{
					myGUID = Guid.NewGuid().ToString();
				}
				else
				{
					myGUID = _CatchCompRow["RowGUID"];
				}
				sampling.CatchLine myLine = new sampling.CatchLine(myName,
																	_Sampling.SamplingGUID,
																	myGUID,
																	_CatchNameGUID,
																	double.Parse(txtWt.Text));

				if (radioLocalName.Checked)
				{
					myLine.NameType = sampling.CatchLine.Identification.LocalName;
				}
				else if (radioSciName.Checked)
				{
					myLine.NameType = sampling.CatchLine.Identification.Scientific;
				}

				myLine.FromTotalCatch = checkFromTotal.Checked;

				myLine.FromTotalCatch = checkFromTotal.Checked;
				myLine.CatchDetailRowGUID = _CatchCompRowGUID;

				if (CatchCount != null)
				{
					myLine.CatchCount = (long)CatchCount;
				}
				else
				{
					if (txtSubCt.Text.Length > 0 && txtSubWt.Text.Length > 0)
					{
						myLine.CatchSubsampleCount = long.Parse(txtSubCt.Text);
						myLine.CatchSubsampleWt = double.Parse(txtSubWt.Text);
					}
				}

				if (myLine.Save(_isNew))
				{
					_ParentForm.UpdatedCatchLine(_isNew, myLine);
					_isNew = false;
					_CatchCompRow.Clear();
					_CatchCompRow = _Sampling.CatchCompRow(myGUID);
				}
			}
		}

		private void comboLocal_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (comboLocal.Text != "")
			{
				string s;
				try
				{
					s = comboLocal.SelectedItem.ToString();
				}
				catch
				{
					s = "";
				}

				if(s != "")
				{
					_CatchNameGUID = ((KeyValuePair<string, string>)comboLocal.SelectedItem).Key;
				}
				else
				{
					e.Cancel = true;
					MessageBox.Show("Item not found");
				}
			}
		}

		private void buttonNew_Click(object sender, EventArgs e)
		{
			AddNew();
		}

		private void txtWt_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			TextBox t = (TextBox)sender;
			string msg = "";
			if(t.Text.Length > 0)
			{
				if (t.Tag.ToString()=="wt")
				{
					double num;
					if (double.TryParse(t.Text,out num)==false)
					{
						e.Cancel = true;
						msg = "Valid input is a number greater than zero";
					}
					else
					{
						if (txtWt.TextLength > 0 && txtSubWt.TextLength>0)
						{
							e.Cancel = double.Parse(txtSubWt.Text) > double.Parse(txtWt.Text);
							if (e.Cancel)
							{
								msg = "Subsample weight cannot be more than catch weight";
							}
						}
					}
					
				}
				else if (t.Tag.ToString() == "ct")
				{
					long ct;
					if (long.TryParse(t.Text,out ct))
					{
						if (ct % 1 != 0)
						{
							e.Cancel = true;
							msg = "Valid input is a whole number greater than zero";
						}

					}
					else
					{
						e.Cancel = true;
						msg = "Valid input is a whole number greater than zero";
					}
				}

			if (e.Cancel)
				{
					MessageBox.Show(msg);
				}
			}
		}
	}
}
