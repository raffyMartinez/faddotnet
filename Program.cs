/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/6/2016
 * Time: 12:03 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;

namespace FAD3
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);


            global.TestRequiredFilesExists();
            try
            {
                Application.Run(new frmMain());
            }
            catch (ObjectDisposedException) { }
            //TODO: error when closing the file when file close on the menu bar is used 
            
            //System.ObjectDisposedException: 'Cannot access a disposed object.
            //Object name: 'ToolStripDropDownMenu'.'


        }

    }
}
