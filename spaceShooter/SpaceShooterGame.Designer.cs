namespace spaceShooter
{
    partial class SpaceShooterGame
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
            components = new System.ComponentModel.Container();
            gameLoopTimer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // gameLoopTimer
            // 
            gameLoopTimer.Enabled = true;
            gameLoopTimer.Interval = 10;
            gameLoopTimer.Tick += gameLoopTimer_Tick;
            // 
            // SpaceShooterGame
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightGray;
            ClientSize = new Size(978, 744);
            ForeColor = Color.LightGray;
            MaximumSize = new Size(1000, 800);
            Name = "SpaceShooterGame";
            Text = "SpaceShooterGame";
            Load += SpaceShooterGame_Load;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer gameLoopTimer;
    }
}