﻿namespace FAD3
{
    partial class SpeciesSamplingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeList = new System.Windows.Forms.TreeView();
            this.lvSamplings = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // treeList
            // 
            this.treeList.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeList.Location = new System.Drawing.Point(0, 0);
            this.treeList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.treeList.Name = "treeList";
            this.treeList.Size = new System.Drawing.Size(259, 639);
            this.treeList.TabIndex = 0;
            this.treeList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeList_AfterSelect);
            // 
            // lvSamplings
            // 
            this.lvSamplings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSamplings.Location = new System.Drawing.Point(259, 0);
            this.lvSamplings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lvSamplings.Name = "lvSamplings";
            this.lvSamplings.Size = new System.Drawing.Size(761, 639);
            this.lvSamplings.TabIndex = 1;
            this.lvSamplings.UseCompatibleStateImageBehavior = false;
            // 
            // SpeciesSamplingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 639);
            this.Controls.Add(this.lvSamplings);
            this.Controls.Add(this.treeList);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "SpeciesSamplingsForm";
            this.Text = "SpeciesSamplingsForm";
            this.Load += new System.EventHandler(this.SpeciesSamplingsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeList;
        private System.Windows.Forms.ListView lvSamplings;
    }
}