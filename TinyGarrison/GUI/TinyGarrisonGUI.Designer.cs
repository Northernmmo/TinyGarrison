namespace TinyGarrison.GUI
{
	partial class TinyGarrisonGUI
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
			this.GarrisonCache = new System.Windows.Forms.CheckBox();
			this.Save = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// GarrisonCache
			// 
			this.GarrisonCache.AutoSize = true;
			this.GarrisonCache.Location = new System.Drawing.Point(13, 13);
			this.GarrisonCache.Name = "GarrisonCache";
			this.GarrisonCache.Size = new System.Drawing.Size(99, 17);
			this.GarrisonCache.TabIndex = 0;
			this.GarrisonCache.Text = "Garrison Cache";
			this.GarrisonCache.UseVisualStyleBackColor = true;
			this.GarrisonCache.CheckedChanged += new System.EventHandler(this.GarrisonCache_CheckedChanged);
			// 
			// Save
			// 
			this.Save.Location = new System.Drawing.Point(168, 9);
			this.Save.Name = "Save";
			this.Save.Size = new System.Drawing.Size(75, 23);
			this.Save.TabIndex = 1;
			this.Save.Text = "Save";
			this.Save.UseVisualStyleBackColor = true;
			// 
			// TinyGarrisonGUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(255, 42);
			this.Controls.Add(this.Save);
			this.Controls.Add(this.GarrisonCache);
			this.Name = "TinyGarrisonGUI";
			this.Text = "TinyGarrisonGUI";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox GarrisonCache;
		private System.Windows.Forms.Button Save;
	}
}