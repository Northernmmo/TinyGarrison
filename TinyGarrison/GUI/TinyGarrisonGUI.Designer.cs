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
			this.Save = new System.Windows.Forms.Button();
			this.GarrisonCache = new System.Windows.Forms.CheckBox();
			this.GardenMine = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// Save
			// 
			this.Save.Location = new System.Drawing.Point(168, 33);
			this.Save.Name = "Save";
			this.Save.Size = new System.Drawing.Size(75, 23);
			this.Save.TabIndex = 1;
			this.Save.Text = "Save";
			this.Save.UseVisualStyleBackColor = true;
			this.Save.Click += new System.EventHandler(this.Save_Click);
			// 
			// GarrisonCache
			// 
			this.GarrisonCache.AutoSize = true;
			this.GarrisonCache.Location = new System.Drawing.Point(13, 13);
			this.GarrisonCache.Name = "GarrisonCache";
			this.GarrisonCache.Size = new System.Drawing.Size(99, 17);
			this.GarrisonCache.TabIndex = 2;
			this.GarrisonCache.Text = "Garrison Cache";
			this.GarrisonCache.UseVisualStyleBackColor = true;
			// 
			// GardenMine
			// 
			this.GardenMine.AutoSize = true;
			this.GardenMine.Location = new System.Drawing.Point(13, 37);
			this.GardenMine.Name = "GardenMine";
			this.GardenMine.Size = new System.Drawing.Size(95, 17);
			this.GardenMine.TabIndex = 3;
			this.GardenMine.Text = "Garden / Mine";
			this.GardenMine.UseVisualStyleBackColor = true;
			// 
			// TinyGarrisonGUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(255, 67);
			this.Controls.Add(this.GardenMine);
			this.Controls.Add(this.GarrisonCache);
			this.Controls.Add(this.Save);
			this.Name = "TinyGarrisonGUI";
			this.Text = "TinyGarrisonGUI";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button Save;
		private System.Windows.Forms.CheckBox GarrisonCache;
		private System.Windows.Forms.CheckBox GardenMine;
	}
}