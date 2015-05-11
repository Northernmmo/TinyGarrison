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
			this.CraftSecrets = new System.Windows.Forms.CheckBox();
			this.TransmuteBlood = new System.Windows.Forms.CheckBox();
			this.BuySavageBlood = new System.Windows.Forms.CheckBox();
			this.Save = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.OpenGearTokens = new System.Windows.Forms.CheckBox();
			this.OpenFollowerUpgrades = new System.Windows.Forms.CheckBox();
			this.UseRushOrders = new System.Windows.Forms.CheckBox();
			this.SkipJewelcraftingWOs = new System.Windows.Forms.CheckBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// CraftSecrets
			// 
			this.CraftSecrets.AutoSize = true;
			this.CraftSecrets.Location = new System.Drawing.Point(6, 19);
			this.CraftSecrets.Name = "CraftSecrets";
			this.CraftSecrets.Size = new System.Drawing.Size(87, 17);
			this.CraftSecrets.TabIndex = 0;
			this.CraftSecrets.Text = "Craft Secrets";
			this.CraftSecrets.UseVisualStyleBackColor = true;
			// 
			// TransmuteBlood
			// 
			this.TransmuteBlood.AutoSize = true;
			this.TransmuteBlood.Checked = true;
			this.TransmuteBlood.CheckState = System.Windows.Forms.CheckState.Checked;
			this.TransmuteBlood.Location = new System.Drawing.Point(6, 42);
			this.TransmuteBlood.Name = "TransmuteBlood";
			this.TransmuteBlood.Size = new System.Drawing.Size(155, 17);
			this.TransmuteBlood.TabIndex = 1;
			this.TransmuteBlood.Text = "Transmute Blood (Alchemy)";
			this.TransmuteBlood.UseVisualStyleBackColor = true;
			// 
			// BuySavageBlood
			// 
			this.BuySavageBlood.AutoSize = true;
			this.BuySavageBlood.Checked = true;
			this.BuySavageBlood.CheckState = System.Windows.Forms.CheckState.Checked;
			this.BuySavageBlood.Location = new System.Drawing.Point(202, 109);
			this.BuySavageBlood.Name = "BuySavageBlood";
			this.BuySavageBlood.Size = new System.Drawing.Size(114, 17);
			this.BuySavageBlood.TabIndex = 2;
			this.BuySavageBlood.Text = "Buy Savage Blood";
			this.BuySavageBlood.UseVisualStyleBackColor = true;
			// 
			// Save
			// 
			this.Save.Location = new System.Drawing.Point(271, 141);
			this.Save.Name = "Save";
			this.Save.Size = new System.Drawing.Size(75, 23);
			this.Save.TabIndex = 3;
			this.Save.Text = "Save";
			this.Save.UseVisualStyleBackColor = true;
			this.Save.Click += new System.EventHandler(this.Save_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.CraftSecrets);
			this.groupBox1.Controls.Add(this.TransmuteBlood);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(166, 71);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Crafting";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.OpenGearTokens);
			this.groupBox2.Controls.Add(this.OpenFollowerUpgrades);
			this.groupBox2.Location = new System.Drawing.Point(12, 89);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(166, 75);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Salvage";
			// 
			// OpenGearTokens
			// 
			this.OpenGearTokens.AutoSize = true;
			this.OpenGearTokens.Checked = true;
			this.OpenGearTokens.CheckState = System.Windows.Forms.CheckState.Checked;
			this.OpenGearTokens.Location = new System.Drawing.Point(7, 44);
			this.OpenGearTokens.Name = "OpenGearTokens";
			this.OpenGearTokens.Size = new System.Drawing.Size(117, 17);
			this.OpenGearTokens.TabIndex = 1;
			this.OpenGearTokens.Text = "Open Gear Tokens";
			this.OpenGearTokens.UseVisualStyleBackColor = true;
			// 
			// OpenFollowerUpgrades
			// 
			this.OpenFollowerUpgrades.AutoSize = true;
			this.OpenFollowerUpgrades.Checked = true;
			this.OpenFollowerUpgrades.CheckState = System.Windows.Forms.CheckState.Checked;
			this.OpenFollowerUpgrades.Location = new System.Drawing.Point(7, 20);
			this.OpenFollowerUpgrades.Name = "OpenFollowerUpgrades";
			this.OpenFollowerUpgrades.Size = new System.Drawing.Size(143, 17);
			this.OpenFollowerUpgrades.TabIndex = 0;
			this.OpenFollowerUpgrades.Text = "Open Follower Upgrades";
			this.OpenFollowerUpgrades.UseVisualStyleBackColor = true;
			// 
			// UseRushOrders
			// 
			this.UseRushOrders.AutoSize = true;
			this.UseRushOrders.Checked = true;
			this.UseRushOrders.CheckState = System.Windows.Forms.CheckState.Checked;
			this.UseRushOrders.Location = new System.Drawing.Point(6, 19);
			this.UseRushOrders.Name = "UseRushOrders";
			this.UseRushOrders.Size = new System.Drawing.Size(107, 17);
			this.UseRushOrders.TabIndex = 6;
			this.UseRushOrders.Text = "Use Rush Orders";
			this.UseRushOrders.UseVisualStyleBackColor = true;
			// 
			// SkipJewelcraftingWOs
			// 
			this.SkipJewelcraftingWOs.AutoSize = true;
			this.SkipJewelcraftingWOs.Checked = true;
			this.SkipJewelcraftingWOs.CheckState = System.Windows.Forms.CheckState.Checked;
			this.SkipJewelcraftingWOs.Location = new System.Drawing.Point(6, 42);
			this.SkipJewelcraftingWOs.Name = "SkipJewelcraftingWOs";
			this.SkipJewelcraftingWOs.Size = new System.Drawing.Size(139, 17);
			this.SkipJewelcraftingWOs.TabIndex = 7;
			this.SkipJewelcraftingWOs.Text = "Skip Jewelcrafting WOs";
			this.SkipJewelcraftingWOs.UseVisualStyleBackColor = true;
			this.SkipJewelcraftingWOs.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.UseRushOrders);
			this.groupBox3.Controls.Add(this.SkipJewelcraftingWOs);
			this.groupBox3.Location = new System.Drawing.Point(196, 12);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(150, 71);
			this.groupBox3.TabIndex = 8;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Profession Buildings";
			// 
			// TinyGarrisonGUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(356, 173);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.Save);
			this.Controls.Add(this.BuySavageBlood);
			this.Name = "TinyGarrisonGUI";
			this.Text = "TinyGarrisonGUI";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox CraftSecrets;
		private System.Windows.Forms.CheckBox TransmuteBlood;
		private System.Windows.Forms.CheckBox BuySavageBlood;
		private System.Windows.Forms.Button Save;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox OpenGearTokens;
		private System.Windows.Forms.CheckBox OpenFollowerUpgrades;
		private System.Windows.Forms.CheckBox UseRushOrders;
		private System.Windows.Forms.CheckBox SkipJewelcraftingWOs;
		private System.Windows.Forms.GroupBox groupBox3;
	}
}