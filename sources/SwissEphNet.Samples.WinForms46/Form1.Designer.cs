namespace SwissEphNet.Samples.WinForms46
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnRunTest = new System.Windows.Forms.Button();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.btnRunTestLoadAsync = new System.Windows.Forms.Button();
            this.btnRunTestAsync = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnRunTest
            // 
            this.btnRunTest.Location = new System.Drawing.Point(12, 12);
            this.btnRunTest.Name = "btnRunTest";
            this.btnRunTest.Size = new System.Drawing.Size(112, 23);
            this.btnRunTest.TabIndex = 0;
            this.btnRunTest.Text = "Run test";
            this.btnRunTest.UseVisualStyleBackColor = true;
            this.btnRunTest.Click += new System.EventHandler(this.btnRunTest_Click);
            // 
            // tbResult
            // 
            this.tbResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResult.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbResult.Location = new System.Drawing.Point(12, 41);
            this.tbResult.Multiline = true;
            this.tbResult.Name = "tbResult";
            this.tbResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbResult.Size = new System.Drawing.Size(710, 459);
            this.tbResult.TabIndex = 1;
            // 
            // pbProgress
            // 
            this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbProgress.Location = new System.Drawing.Point(366, 12);
            this.pbProgress.MarqueeAnimationSpeed = 25;
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(356, 23);
            this.pbProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbProgress.TabIndex = 2;
            // 
            // btnRunTestLoadAsync
            // 
            this.btnRunTestLoadAsync.Location = new System.Drawing.Point(130, 12);
            this.btnRunTestLoadAsync.Name = "btnRunTestLoadAsync";
            this.btnRunTestLoadAsync.Size = new System.Drawing.Size(112, 23);
            this.btnRunTestLoadAsync.TabIndex = 3;
            this.btnRunTestLoadAsync.Text = "Run test load async";
            this.btnRunTestLoadAsync.UseVisualStyleBackColor = true;
            this.btnRunTestLoadAsync.Click += new System.EventHandler(this.btnRunTestLoadAsync_Click);
            // 
            // btnRunTestAsync
            // 
            this.btnRunTestAsync.Location = new System.Drawing.Point(248, 12);
            this.btnRunTestAsync.Name = "btnRunTestAsync";
            this.btnRunTestAsync.Size = new System.Drawing.Size(112, 23);
            this.btnRunTestAsync.TabIndex = 4;
            this.btnRunTestAsync.Text = "Run test async";
            this.btnRunTestAsync.UseVisualStyleBackColor = true;
            this.btnRunTestAsync.Click += new System.EventHandler(this.btnRunTestAsync_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 512);
            this.Controls.Add(this.btnRunTestAsync);
            this.Controls.Add(this.btnRunTestLoadAsync);
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.tbResult);
            this.Controls.Add(this.btnRunTest);
            this.Name = "Form1";
            this.Text = "Swiss Ephemeris Tests";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRunTest;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Button btnRunTestLoadAsync;
        private System.Windows.Forms.Button btnRunTestAsync;
    }
}

