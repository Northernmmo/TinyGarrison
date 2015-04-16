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
			this.BuySavageBlood = new System.Windows.Forms.CheckBox();
			this.ProfessionDailies = new System.Windows.Forms.CheckBox();
			this.ProfessionBuildings = new System.Windows.Forms.CheckBox();
			this.Salvage = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// Save
			// 
			this.Save.Location = new System.Drawing.Point(162, 129);
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
			// BuySavageBlood
			// 
			this.BuySavageBlood.AutoSize = true;
			this.BuySavageBlood.Location = new System.Drawing.Point(13, 61);
			this.BuySavageBlood.Name = "BuySavageBlood";
			this.BuySavageBlood.Size = new System.Drawing.Size(114, 17);
			this.BuySavageBlood.TabIndex = 4;
			this.BuySavageBlood.Text = "Buy Savage Blood";
			this.BuySavageBlood.UseVisualStyleBackColor = true;
			// 
			// ProfessionDailies
			// 
			this.ProfessionDailies.AutoSize = true;
			this.ProfessionDailies.Location = new System.Drawing.Point(13, 85);
			this.ProfessionDailies.Name = "ProfessionDailies";
			this.ProfessionDailies.Size = new System.Drawing.Size(109, 17);
			this.ProfessionDailies.TabIndex = 5;
			this.ProfessionDailies.Text = "Profession Dailies";
			this.ProfessionDailies.UseVisualStyleBackColor = true;
			// 
			// ProfessionBuildings
			// 
			this.ProfessionBuildings.AutoSize = true;
			this.ProfessionBuildings.Location = new System.Drawing.Point(13, 109);
			this.ProfessionBuildings.Name = "ProfessionBuildings";
			this.ProfessionBuildings.Size = new System.Drawing.Size(120, 17);
			this.ProfessionBuildings.TabIndex = 6;
			this.ProfessionBuildings.Text = "Profession Buildings";
			this.ProfessionBuildings.UseVisualStyleBackColor = true;
			// 
			// Salvage
			// 
			this.Salvage.AutoSize = true;
			this.Salvage.Location = new System.Drawing.Point(13, 133);
			this.Salvage.Name = "Salvage";
			this.Salvage.Size = new System.Drawing.Size(65, 17);
			this.Salvage.TabIndex = 7;
			this.Salvage.Text = "Salvage";
			this.Salvage.UseVisualStyleBackColor = true;
			// 
			// TinyGarrisonGUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(252, 164);
			this.Controls.Add(this.Salvage);
			this.Controls.Add(this.ProfessionBuildings);
			this.Controls.Add(this.ProfessionDailies);
			this.Controls.Add(this.BuySavageBlood);
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
		private System.Windows.Forms.CheckBox BuySavageBlood;
		private System.Windows.Forms.CheckBox ProfessionDailies;
		private System.Windows.Forms.CheckBox ProfessionBuildings;
		private System.Windows.Forms.CheckBox Salvage;
	}
}