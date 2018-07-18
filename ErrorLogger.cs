/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/12/2016
 * Time: 8:24 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace FAD3
{
	/// <summary>
	/// Description of ErrorLogger.
	/// </summary>
	public class ErrorLogger
	{
		
		public  ErrorLogger(){
			
		}
		public static void Log(Exception ex, Boolean ShowMessage = false){
			string filepath = Path.GetDirectoryName(global.AppPath) + "\\error.log";
			using (StreamWriter writer = new StreamWriter(filepath, true))
			{
				    writer.WriteLine("Message: " + ex.Message + "<br/>" + Environment.NewLine + 
				                 "StackTrace: " + ex.StackTrace +
                                  "" + Environment.NewLine + "Date: " + DateTime.Now.ToString() + 
                                  System.Environment.NewLine) ;
			}
			
			
			if(Debugger.IsAttached){
				if(global.ShowErrorMessage){
					
					StackTrace st = new StackTrace(ex,true);
					StackFrame frame = st.GetFrame(st.FrameCount - 1);
			        string fileName = frame.GetFileName(); //returns null
			
			        //Get the method name
			        string methodName = frame.GetMethod().Name; //returns PermissionDemand
			
			        //Get the line number from the stack frame
			        int line = frame.GetFileLineNumber(); //returns 0
			
			        //Get the column number
			        int col = frame.GetFileColumnNumber(); //returns 0  
					string errorMessage = "Filename: " + fileName + Environment.NewLine + 
										  "Method: " + methodName + Environment.NewLine +
										  "Line: " + line + Environment.NewLine; 
					errorMessage = ex.Message + Environment.NewLine + errorMessage;
					MessageBox.Show(errorMessage,"Error");
				}
				else if(ShowMessage){
					MessageBox.Show(ex.Message);
				}
			}

		}
		
		public static void Log(string s){
			string filepath = Path.GetDirectoryName(global.AppPath) + "\\error.log";
			using (StreamWriter writer = new StreamWriter(filepath, true))
			{
				writer.WriteLine("Messagee: " + s + "<br/>" +   "Date :" + DateTime.Now.ToString());
			}
		}
		
	}
}
