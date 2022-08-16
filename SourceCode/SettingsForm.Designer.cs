namespace StellarSimulation {
    partial class SettingsForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.systemsLabel = new System.Windows.Forms.Label();
            this.slowParticles = new System.Windows.Forms.Button();
            this.fastParticles = new System.Windows.Forms.Button();
            this.distributionTest = new System.Windows.Forms.Button();
            this.massiveBody = new System.Windows.Forms.Button();
            this.planetarySystem = new System.Windows.Forms.Button();
            this.orbitalSystem = new System.Windows.Forms.Button();
            this.binarySystem = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.none = new System.Windows.Forms.Button();
            this.pause = new System.Windows.Forms.Button();
            this.showTree = new System.Windows.Forms.Button();
            this.resetCamera = new System.Windows.Forms.Button();
            this.typeGroupBox = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.addParamBox = new System.Windows.Forms.GroupBox();
            this.changeC = new System.Windows.Forms.Button();
            this.changeG = new System.Windows.Forms.Button();
            this.changeN = new System.Windows.Forms.Button();
            this.typeGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.addParamBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // systemsLabel
            // 
            this.systemsLabel.AutoSize = true;
            this.systemsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.systemsLabel.Location = new System.Drawing.Point(8, 25);
            this.systemsLabel.Name = "systemsLabel";
            this.systemsLabel.Size = new System.Drawing.Size(504, 20);
            this.systemsLabel.TabIndex = 9;
            this.systemsLabel.Text = "Выберите необходимую гравитационную систему для симуляции";
            // 
            // slowParticles
            // 
            this.slowParticles.Location = new System.Drawing.Point(6, 153);
            this.slowParticles.Name = "slowParticles";
            this.slowParticles.Size = new System.Drawing.Size(130, 40);
            this.slowParticles.TabIndex = 1;
            this.slowParticles.Text = "Медленные частицы";
            this.slowParticles.UseVisualStyleBackColor = true;
            this.slowParticles.Click += new System.EventHandler(this.SlowStars_Click);
            // 
            // fastParticles
            // 
            this.fastParticles.Location = new System.Drawing.Point(6, 97);
            this.fastParticles.Name = "fastParticles";
            this.fastParticles.Size = new System.Drawing.Size(130, 40);
            this.fastParticles.TabIndex = 2;
            this.fastParticles.Text = "Быстрые частицы";
            this.fastParticles.UseVisualStyleBackColor = true;
            this.fastParticles.Click += new System.EventHandler(this.FastStars_Click);
            // 
            // distributionTest
            // 
            this.distributionTest.Location = new System.Drawing.Point(6, 384);
            this.distributionTest.Name = "distributionTest";
            this.distributionTest.Size = new System.Drawing.Size(130, 40);
            this.distributionTest.TabIndex = 8;
            this.distributionTest.Text = "Тест дистрибутивности";
            this.distributionTest.UseVisualStyleBackColor = true;
            this.distributionTest.Click += new System.EventHandler(this.DistribTest_Click);
            // 
            // massiveBody
            // 
            this.massiveBody.Location = new System.Drawing.Point(6, 267);
            this.massiveBody.Name = "massiveBody";
            this.massiveBody.Size = new System.Drawing.Size(130, 40);
            this.massiveBody.TabIndex = 4;
            this.massiveBody.Text = "Массивные тела";
            this.massiveBody.UseVisualStyleBackColor = true;
            this.massiveBody.Click += new System.EventHandler(this.MassStars_Click);
            // 
            // planetarySystem
            // 
            this.planetarySystem.Location = new System.Drawing.Point(6, 41);
            this.planetarySystem.Name = "planetarySystem";
            this.planetarySystem.Size = new System.Drawing.Size(130, 40);
            this.planetarySystem.TabIndex = 7;
            this.planetarySystem.Text = "Планетная система";
            this.planetarySystem.UseVisualStyleBackColor = true;
            this.planetarySystem.Click += new System.EventHandler(this.PlanetSys_Click);
            // 
            // orbitalSystem
            // 
            this.orbitalSystem.Location = new System.Drawing.Point(6, 326);
            this.orbitalSystem.Name = "orbitalSystem";
            this.orbitalSystem.Size = new System.Drawing.Size(130, 40);
            this.orbitalSystem.TabIndex = 5;
            this.orbitalSystem.Text = "Орбитальная система";
            this.orbitalSystem.UseVisualStyleBackColor = true;
            this.orbitalSystem.Click += new System.EventHandler(this.OrSys_Click);
            // 
            // binarySystem
            // 
            this.binarySystem.Location = new System.Drawing.Point(6, 210);
            this.binarySystem.Name = "binarySystem";
            this.binarySystem.Size = new System.Drawing.Size(130, 40);
            this.binarySystem.TabIndex = 6;
            this.binarySystem.Text = "Двойная система";
            this.binarySystem.UseVisualStyleBackColor = true;
            this.binarySystem.Click += new System.EventHandler(this.DoubleSys_Click);
            // 
            // none
            // 
            this.none.BackColor = System.Drawing.Color.White;
            this.none.Image = global::StellarSimulation.Properties.Resources.STOP;
            this.none.Location = new System.Drawing.Point(172, 26);
            this.none.Name = "none";
            this.none.Size = new System.Drawing.Size(135, 135);
            this.none.TabIndex = 0;
            this.toolTip1.SetToolTip(this.none, "Остановить симуляцию");
            this.none.UseVisualStyleBackColor = false;
            this.none.Click += new System.EventHandler(this.StopButtonClick);
            // 
            // pause
            // 
            this.pause.BackColor = System.Drawing.Color.White;
            this.pause.Image = global::StellarSimulation.Properties.Resources.PAUSE;
            this.pause.Location = new System.Drawing.Point(172, 178);
            this.pause.Name = "pause";
            this.pause.Size = new System.Drawing.Size(135, 135);
            this.pause.TabIndex = 10;
            this.toolTip1.SetToolTip(this.pause, "Приостановить, продолжить симуляцию");
            this.pause.UseVisualStyleBackColor = false;
            this.pause.Click += new System.EventHandler(this.PauseClick);
            // 
            // showTree
            // 
            this.showTree.BackColor = System.Drawing.Color.Transparent;
            this.showTree.BackgroundImage = global::StellarSimulation.Properties.Resources.SHOWTREE;
            this.showTree.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.showTree.Location = new System.Drawing.Point(18, 26);
            this.showTree.Name = "showTree";
            this.showTree.Size = new System.Drawing.Size(135, 135);
            this.showTree.TabIndex = 19;
            this.toolTip1.SetToolTip(this.showTree, "Показать/Спрятать дерево");
            this.showTree.UseVisualStyleBackColor = false;
            this.showTree.Click += new System.EventHandler(this.ShowBarnesHut_Click);
            // 
            // resetCamera
            // 
            this.resetCamera.BackColor = System.Drawing.Color.White;
            this.resetCamera.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resetCamera.BackgroundImage")));
            this.resetCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.resetCamera.Location = new System.Drawing.Point(18, 178);
            this.resetCamera.Name = "resetCamera";
            this.resetCamera.Size = new System.Drawing.Size(135, 135);
            this.resetCamera.TabIndex = 18;
            this.toolTip1.SetToolTip(this.resetCamera, "Сбросить камеру");
            this.resetCamera.UseVisualStyleBackColor = false;
            this.resetCamera.Click += new System.EventHandler(this.ResetView_Click);
            // 
            // typeGroupBox
            // 
            this.typeGroupBox.Controls.Add(this.planetarySystem);
            this.typeGroupBox.Controls.Add(this.binarySystem);
            this.typeGroupBox.Controls.Add(this.orbitalSystem);
            this.typeGroupBox.Controls.Add(this.massiveBody);
            this.typeGroupBox.Controls.Add(this.distributionTest);
            this.typeGroupBox.Controls.Add(this.fastParticles);
            this.typeGroupBox.Controls.Add(this.slowParticles);
            this.typeGroupBox.Location = new System.Drawing.Point(12, 65);
            this.typeGroupBox.Name = "typeGroupBox";
            this.typeGroupBox.Size = new System.Drawing.Size(180, 456);
            this.typeGroupBox.TabIndex = 20;
            this.typeGroupBox.TabStop = false;
            this.typeGroupBox.Text = "Типы гравитационных систем";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.none);
            this.groupBox1.Controls.Add(this.pause);
            this.groupBox1.Controls.Add(this.showTree);
            this.groupBox1.Controls.Add(this.resetCamera);
            this.groupBox1.Location = new System.Drawing.Point(236, 65);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(327, 330);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Элементы управления";
            // 
            // addParamBox
            // 
            this.addParamBox.Controls.Add(this.changeC);
            this.addParamBox.Controls.Add(this.changeG);
            this.addParamBox.Controls.Add(this.changeN);
            this.addParamBox.Location = new System.Drawing.Point(236, 401);
            this.addParamBox.Name = "addParamBox";
            this.addParamBox.Size = new System.Drawing.Size(327, 120);
            this.addParamBox.TabIndex = 27;
            this.addParamBox.TabStop = false;
            this.addParamBox.Text = "Параметры";
            // 
            // changeC
            // 
            this.changeC.Location = new System.Drawing.Point(6, 63);
            this.changeC.Name = "changeC";
            this.changeC.Size = new System.Drawing.Size(130, 40);
            this.changeC.TabIndex = 29;
            this.changeC.Text = "Максимально допустимая скорость (C)";
            this.changeC.UseVisualStyleBackColor = true;
            this.changeC.Click += new System.EventHandler(this.ChangeCClick);
            // 
            // changeG
            // 
            this.changeG.Location = new System.Drawing.Point(154, 14);
            this.changeG.Name = "changeG";
            this.changeG.Size = new System.Drawing.Size(130, 40);
            this.changeG.TabIndex = 27;
            this.changeG.Text = "Гравитационная постоянная (G)";
            this.changeG.UseVisualStyleBackColor = true;
            this.changeG.Click += new System.EventHandler(this.ChangeGClick);
            // 
            // changeN
            // 
            this.changeN.Location = new System.Drawing.Point(6, 14);
            this.changeN.Name = "changeN";
            this.changeN.Size = new System.Drawing.Size(130, 40);
            this.changeN.TabIndex = 28;
            this.changeN.Text = "Количество частиц (N)";
            this.changeN.UseVisualStyleBackColor = true;
            this.changeN.Click += new System.EventHandler(this.ChangeNClick);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(586, 522);
            this.Controls.Add(this.addParamBox);
            this.Controls.Add(this.typeGroupBox);
            this.Controls.Add(this.systemsLabel);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Настройки симуляции";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Settings_FormClosed);
            this.typeGroupBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.addParamBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button showTree;
        private System.Windows.Forms.Button resetCamera;
        private System.Windows.Forms.Button pause;
        private System.Windows.Forms.Label systemsLabel;
        private System.Windows.Forms.Button none;
        private System.Windows.Forms.Button slowParticles;
        private System.Windows.Forms.Button fastParticles;
        private System.Windows.Forms.Button distributionTest;
        private System.Windows.Forms.Button massiveBody;
        private System.Windows.Forms.Button planetarySystem;
        private System.Windows.Forms.Button orbitalSystem;
        private System.Windows.Forms.Button binarySystem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox typeGroupBox;
        private System.Windows.Forms.GroupBox addParamBox;
        private System.Windows.Forms.Button changeC;
        private System.Windows.Forms.Button changeG;
        private System.Windows.Forms.Button changeN;

    }
}