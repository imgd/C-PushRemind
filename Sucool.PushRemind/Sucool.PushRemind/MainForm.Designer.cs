namespace Sucool.PushRemind
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btn_gbkremind_start = new System.Windows.Forms.Button();
            this.btn_ordertimeremind_start = new System.Windows.Forms.Button();
            this.btn_notloginremind_start = new System.Windows.Forms.Button();
            this.timer_goldbookRemid = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.btn_gbkremind_stop = new System.Windows.Forms.Button();
            this.btn_ordertimeoutremind_stop = new System.Windows.Forms.Button();
            this.btn_notloginremind_stop = new System.Windows.Forms.Button();
            this.timer_notuserloginRemind = new System.Windows.Forms.Timer(this.components);
            this.timer_ordertimeout = new System.Windows.Forms.Timer(this.components);
            this.timer_ordertimeoutJush = new System.Windows.Forms.Timer(this.components);
            this.timer_notloginuserJush = new System.Windows.Forms.Timer(this.components);
            this.timer_goldbookRemindJush = new System.Windows.Forms.Timer(this.components);
            this.timer_addgoldbookJush = new System.Windows.Forms.Timer(this.components);
            this.timer_timingJush = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_timingJush_stop = new System.Windows.Forms.Button();
            this.btn_timingJush_start = new System.Windows.Forms.Button();
            this.btn_addgbkremindJush_stop = new System.Windows.Forms.Button();
            this.btn_addgbkremindJush_start = new System.Windows.Forms.Button();
            this.btn_gbkremindJush_stop = new System.Windows.Forms.Button();
            this.btn_gbkremindJush_start = new System.Windows.Forms.Button();
            this.btn_notloginuserremind_End = new System.Windows.Forms.Button();
            this.btn_NotLoginUserRemindJush_Start = new System.Windows.Forms.Button();
            this.btn_ordertimeoutremindJush_stop = new System.Windows.Forms.Button();
            this.btn_ordertimeremindJush_start = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_gbkremind_start
            // 
            this.btn_gbkremind_start.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_gbkremind_start.Location = new System.Drawing.Point(22, 46);
            this.btn_gbkremind_start.Margin = new System.Windows.Forms.Padding(4);
            this.btn_gbkremind_start.Name = "btn_gbkremind_start";
            this.btn_gbkremind_start.Size = new System.Drawing.Size(283, 52);
            this.btn_gbkremind_start.TabIndex = 0;
            this.btn_gbkremind_start.Text = ">> 启动优惠卷快过期提醒";
            this.btn_gbkremind_start.UseVisualStyleBackColor = true;
            this.btn_gbkremind_start.Click += new System.EventHandler(this.btn_gbkremind_start_Click);
            // 
            // btn_ordertimeremind_start
            // 
            this.btn_ordertimeremind_start.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_ordertimeremind_start.Location = new System.Drawing.Point(340, 46);
            this.btn_ordertimeremind_start.Margin = new System.Windows.Forms.Padding(4);
            this.btn_ordertimeremind_start.Name = "btn_ordertimeremind_start";
            this.btn_ordertimeremind_start.Size = new System.Drawing.Size(283, 53);
            this.btn_ordertimeremind_start.TabIndex = 1;
            this.btn_ordertimeremind_start.Text = ">> 启动订单即将超时提醒";
            this.btn_ordertimeremind_start.UseVisualStyleBackColor = true;
            this.btn_ordertimeremind_start.Click += new System.EventHandler(this.btn_ordertimeremind_start_Click);
            // 
            // btn_notloginremind_start
            // 
            this.btn_notloginremind_start.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_notloginremind_start.Location = new System.Drawing.Point(22, 117);
            this.btn_notloginremind_start.Margin = new System.Windows.Forms.Padding(4);
            this.btn_notloginremind_start.Name = "btn_notloginremind_start";
            this.btn_notloginremind_start.Size = new System.Drawing.Size(283, 52);
            this.btn_notloginremind_start.TabIndex = 2;
            this.btn_notloginremind_start.Text = ">> 启动长期内未登录提醒";
            this.btn_notloginremind_start.UseVisualStyleBackColor = true;
            this.btn_notloginremind_start.Click += new System.EventHandler(this.btn_notloginremind_start_Click);
            // 
            // timer_goldbookRemid
            // 
            this.timer_goldbookRemid.Tag = "0";
            // 
            // btn_gbkremind_stop
            // 
            this.btn_gbkremind_stop.BackColor = System.Drawing.Color.PaleGreen;
            this.btn_gbkremind_stop.BackgroundImage = global::Sucool.PushRemind.Properties.Resources._5_121204193950;
            this.btn_gbkremind_stop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_gbkremind_stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_gbkremind_stop.ForeColor = System.Drawing.SystemColors.Control;
            this.btn_gbkremind_stop.Location = new System.Drawing.Point(22, 46);
            this.btn_gbkremind_stop.Margin = new System.Windows.Forms.Padding(4);
            this.btn_gbkremind_stop.Name = "btn_gbkremind_stop";
            this.btn_gbkremind_stop.Size = new System.Drawing.Size(283, 52);
            this.btn_gbkremind_stop.TabIndex = 4;
            this.btn_gbkremind_stop.Text = ">>优惠卷到期任务中...点击【停止】";
            this.btn_gbkremind_stop.UseVisualStyleBackColor = false;
            this.btn_gbkremind_stop.Visible = false;
            this.btn_gbkremind_stop.Click += new System.EventHandler(this.btn_gbkremind_stop_Click);
            // 
            // btn_ordertimeoutremind_stop
            // 
            this.btn_ordertimeoutremind_stop.BackColor = System.Drawing.Color.PaleGreen;
            this.btn_ordertimeoutremind_stop.BackgroundImage = global::Sucool.PushRemind.Properties.Resources._5_121204193950;
            this.btn_ordertimeoutremind_stop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_ordertimeoutremind_stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_ordertimeoutremind_stop.ForeColor = System.Drawing.SystemColors.Control;
            this.btn_ordertimeoutremind_stop.Location = new System.Drawing.Point(340, 46);
            this.btn_ordertimeoutremind_stop.Margin = new System.Windows.Forms.Padding(4);
            this.btn_ordertimeoutremind_stop.Name = "btn_ordertimeoutremind_stop";
            this.btn_ordertimeoutremind_stop.Size = new System.Drawing.Size(283, 52);
            this.btn_ordertimeoutremind_stop.TabIndex = 5;
            this.btn_ordertimeoutremind_stop.Text = ">>订单快超时任务中...点击【停止】";
            this.btn_ordertimeoutremind_stop.UseVisualStyleBackColor = false;
            this.btn_ordertimeoutremind_stop.Visible = false;
            this.btn_ordertimeoutremind_stop.Click += new System.EventHandler(this.btn_ordertimeoutremind_stop_Click);
            // 
            // btn_notloginremind_stop
            // 
            this.btn_notloginremind_stop.BackColor = System.Drawing.Color.PaleGreen;
            this.btn_notloginremind_stop.BackgroundImage = global::Sucool.PushRemind.Properties.Resources._5_121204193950;
            this.btn_notloginremind_stop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_notloginremind_stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_notloginremind_stop.ForeColor = System.Drawing.SystemColors.Control;
            this.btn_notloginremind_stop.Location = new System.Drawing.Point(22, 117);
            this.btn_notloginremind_stop.Margin = new System.Windows.Forms.Padding(4);
            this.btn_notloginremind_stop.Name = "btn_notloginremind_stop";
            this.btn_notloginremind_stop.Size = new System.Drawing.Size(283, 52);
            this.btn_notloginremind_stop.TabIndex = 6;
            this.btn_notloginremind_stop.Text = ">>长期未登录任务中...点击【停止】";
            this.btn_notloginremind_stop.UseVisualStyleBackColor = false;
            this.btn_notloginremind_stop.Visible = false;
            this.btn_notloginremind_stop.Click += new System.EventHandler(this.btn_notloginremind_stop_Click);
            // 
            // timer_notuserloginRemind
            // 
            this.timer_notuserloginRemind.Tag = "0";
            // 
            // timer_ordertimeout
            // 
            this.timer_ordertimeout.Tag = "0";
            // 
            // timer_ordertimeoutJush
            // 
            this.timer_ordertimeoutJush.Tag = "0";
            // 
            // timer_notloginuserJush
            // 
            this.timer_notloginuserJush.Tag = "0";
            // 
            // timer_goldbookRemindJush
            // 
            this.timer_goldbookRemindJush.Tag = "0";
            // 
            // timer_addgoldbookJush
            // 
            this.timer_addgoldbookJush.Tag = "0";
            // 
            // timer_timingJush
            // 
            this.timer_timingJush.Tag = "0";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.btn_gbkremind_stop);
            this.groupBox1.Controls.Add(this.btn_gbkremind_start);
            this.groupBox1.Controls.Add(this.btn_ordertimeremind_start);
            this.groupBox1.Controls.Add(this.btn_notloginremind_start);
            this.groupBox1.Controls.Add(this.btn_ordertimeoutremind_stop);
            this.groupBox1.Controls.Add(this.btn_notloginremind_stop);
            this.groupBox1.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(652, 202);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "短信推送";           
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.btn_timingJush_stop);
            this.groupBox2.Controls.Add(this.btn_timingJush_start);
            this.groupBox2.Controls.Add(this.btn_addgbkremindJush_stop);
            this.groupBox2.Controls.Add(this.btn_addgbkremindJush_start);
            this.groupBox2.Controls.Add(this.btn_gbkremindJush_stop);
            this.groupBox2.Controls.Add(this.btn_gbkremindJush_start);
            this.groupBox2.Controls.Add(this.btn_notloginuserremind_End);
            this.groupBox2.Controls.Add(this.btn_NotLoginUserRemindJush_Start);
            this.groupBox2.Controls.Add(this.btn_ordertimeoutremindJush_stop);
            this.groupBox2.Controls.Add(this.btn_ordertimeremindJush_start);
            this.groupBox2.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(13, 229);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(652, 272);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "APP推送";
            // 
            // btn_timingJush_stop
            // 
            this.btn_timingJush_stop.BackColor = System.Drawing.Color.PaleGreen;
            this.btn_timingJush_stop.BackgroundImage = global::Sucool.PushRemind.Properties.Resources._5_121204193950;
            this.btn_timingJush_stop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_timingJush_stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_timingJush_stop.ForeColor = System.Drawing.SystemColors.Control;
            this.btn_timingJush_stop.Location = new System.Drawing.Point(22, 189);
            this.btn_timingJush_stop.Margin = new System.Windows.Forms.Padding(4);
            this.btn_timingJush_stop.Name = "btn_timingJush_stop";
            this.btn_timingJush_stop.Size = new System.Drawing.Size(283, 52);
            this.btn_timingJush_stop.TabIndex = 31;
            this.btn_timingJush_stop.Text = ">>APP定时推送任务中...点击【停止】";
            this.btn_timingJush_stop.UseVisualStyleBackColor = false;
            this.btn_timingJush_stop.Visible = false;
            this.btn_timingJush_stop.Click += new System.EventHandler(this.btn_timingJush_stop_Click);
            // 
            // btn_timingJush_start
            // 
            this.btn_timingJush_start.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_timingJush_start.Location = new System.Drawing.Point(22, 189);
            this.btn_timingJush_start.Margin = new System.Windows.Forms.Padding(4);
            this.btn_timingJush_start.Name = "btn_timingJush_start";
            this.btn_timingJush_start.Size = new System.Drawing.Size(283, 52);
            this.btn_timingJush_start.TabIndex = 30;
            this.btn_timingJush_start.Text = ">> 启动APP定时推送";
            this.btn_timingJush_start.UseVisualStyleBackColor = true;
            this.btn_timingJush_start.Click += new System.EventHandler(this.btn_timingJush_start_Click);
            // 
            // btn_addgbkremindJush_stop
            // 
            this.btn_addgbkremindJush_stop.BackColor = System.Drawing.Color.PaleGreen;
            this.btn_addgbkremindJush_stop.BackgroundImage = global::Sucool.PushRemind.Properties.Resources._5_121204193950;
            this.btn_addgbkremindJush_stop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_addgbkremindJush_stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_addgbkremindJush_stop.ForeColor = System.Drawing.SystemColors.Control;
            this.btn_addgbkremindJush_stop.Location = new System.Drawing.Point(22, 45);
            this.btn_addgbkremindJush_stop.Margin = new System.Windows.Forms.Padding(4);
            this.btn_addgbkremindJush_stop.Name = "btn_addgbkremindJush_stop";
            this.btn_addgbkremindJush_stop.Size = new System.Drawing.Size(283, 52);
            this.btn_addgbkremindJush_stop.TabIndex = 28;
            this.btn_addgbkremindJush_stop.Text = ">>优惠卷新增APP推送任务中...点击【停止】";
            this.btn_addgbkremindJush_stop.UseVisualStyleBackColor = false;
            this.btn_addgbkremindJush_stop.Visible = false;
            this.btn_addgbkremindJush_stop.Click += new System.EventHandler(this.btn_addgbkremindJush_stop_Click);
            // 
            // btn_addgbkremindJush_start
            // 
            this.btn_addgbkremindJush_start.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_addgbkremindJush_start.Location = new System.Drawing.Point(22, 45);
            this.btn_addgbkremindJush_start.Margin = new System.Windows.Forms.Padding(4);
            this.btn_addgbkremindJush_start.Name = "btn_addgbkremindJush_start";
            this.btn_addgbkremindJush_start.Size = new System.Drawing.Size(283, 52);
            this.btn_addgbkremindJush_start.TabIndex = 27;
            this.btn_addgbkremindJush_start.Text = ">> 启动优惠卷新增APP推送";
            this.btn_addgbkremindJush_start.UseVisualStyleBackColor = true;
            this.btn_addgbkremindJush_start.Click += new System.EventHandler(this.btn_addgbkremindJush_start_Click);
            // 
            // btn_gbkremindJush_stop
            // 
            this.btn_gbkremindJush_stop.BackColor = System.Drawing.Color.PaleGreen;
            this.btn_gbkremindJush_stop.BackgroundImage = global::Sucool.PushRemind.Properties.Resources._5_121204193950;
            this.btn_gbkremindJush_stop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_gbkremindJush_stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_gbkremindJush_stop.ForeColor = System.Drawing.SystemColors.Control;
            this.btn_gbkremindJush_stop.Location = new System.Drawing.Point(340, 45);
            this.btn_gbkremindJush_stop.Margin = new System.Windows.Forms.Padding(4);
            this.btn_gbkremindJush_stop.Name = "btn_gbkremindJush_stop";
            this.btn_gbkremindJush_stop.Size = new System.Drawing.Size(283, 52);
            this.btn_gbkremindJush_stop.TabIndex = 26;
            this.btn_gbkremindJush_stop.Text = ">>优惠卷到期APP推送任务中...点击【停止】";
            this.btn_gbkremindJush_stop.UseVisualStyleBackColor = false;
            this.btn_gbkremindJush_stop.Visible = false;
            this.btn_gbkremindJush_stop.Click += new System.EventHandler(this.btn_gbkremindJush_stop_Click);
            // 
            // btn_gbkremindJush_start
            // 
            this.btn_gbkremindJush_start.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_gbkremindJush_start.Location = new System.Drawing.Point(340, 45);
            this.btn_gbkremindJush_start.Margin = new System.Windows.Forms.Padding(4);
            this.btn_gbkremindJush_start.Name = "btn_gbkremindJush_start";
            this.btn_gbkremindJush_start.Size = new System.Drawing.Size(283, 52);
            this.btn_gbkremindJush_start.TabIndex = 25;
            this.btn_gbkremindJush_start.Text = ">> 启动优惠卷快过期APP推送";
            this.btn_gbkremindJush_start.UseVisualStyleBackColor = true;
            this.btn_gbkremindJush_start.Click += new System.EventHandler(this.btn_gbkremindJush_start_Click);
            // 
            // btn_notloginuserremind_End
            // 
            this.btn_notloginuserremind_End.BackColor = System.Drawing.Color.PaleGreen;
            this.btn_notloginuserremind_End.BackgroundImage = global::Sucool.PushRemind.Properties.Resources._5_121204193950;
            this.btn_notloginuserremind_End.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_notloginuserremind_End.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_notloginuserremind_End.ForeColor = System.Drawing.SystemColors.Control;
            this.btn_notloginuserremind_End.Location = new System.Drawing.Point(340, 118);
            this.btn_notloginuserremind_End.Margin = new System.Windows.Forms.Padding(4);
            this.btn_notloginuserremind_End.Name = "btn_notloginuserremind_End";
            this.btn_notloginuserremind_End.Size = new System.Drawing.Size(283, 52);
            this.btn_notloginuserremind_End.TabIndex = 24;
            this.btn_notloginuserremind_End.Text = ">>长期未登录APP推送任务中...点击【停止】";
            this.btn_notloginuserremind_End.UseVisualStyleBackColor = false;
            this.btn_notloginuserremind_End.Visible = false;
            this.btn_notloginuserremind_End.Click += new System.EventHandler(this.btn_notloginuserremind_End_Click);
            // 
            // btn_NotLoginUserRemindJush_Start
            // 
            this.btn_NotLoginUserRemindJush_Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_NotLoginUserRemindJush_Start.Location = new System.Drawing.Point(340, 117);
            this.btn_NotLoginUserRemindJush_Start.Margin = new System.Windows.Forms.Padding(4);
            this.btn_NotLoginUserRemindJush_Start.Name = "btn_NotLoginUserRemindJush_Start";
            this.btn_NotLoginUserRemindJush_Start.Size = new System.Drawing.Size(283, 52);
            this.btn_NotLoginUserRemindJush_Start.TabIndex = 23;
            this.btn_NotLoginUserRemindJush_Start.Text = ">> 启动长期内未登录APP推送";
            this.btn_NotLoginUserRemindJush_Start.UseVisualStyleBackColor = true;
            this.btn_NotLoginUserRemindJush_Start.Click += new System.EventHandler(this.btn_NotLoginUserRemindJush_Start_Click);
            // 
            // btn_ordertimeoutremindJush_stop
            // 
            this.btn_ordertimeoutremindJush_stop.BackColor = System.Drawing.Color.PaleGreen;
            this.btn_ordertimeoutremindJush_stop.BackgroundImage = global::Sucool.PushRemind.Properties.Resources._5_121204193950;
            this.btn_ordertimeoutremindJush_stop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_ordertimeoutremindJush_stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_ordertimeoutremindJush_stop.ForeColor = System.Drawing.SystemColors.Control;
            this.btn_ordertimeoutremindJush_stop.Location = new System.Drawing.Point(22, 118);
            this.btn_ordertimeoutremindJush_stop.Margin = new System.Windows.Forms.Padding(4);
            this.btn_ordertimeoutremindJush_stop.Name = "btn_ordertimeoutremindJush_stop";
            this.btn_ordertimeoutremindJush_stop.Size = new System.Drawing.Size(283, 52);
            this.btn_ordertimeoutremindJush_stop.TabIndex = 22;
            this.btn_ordertimeoutremindJush_stop.Text = ">>订单快超时APP推送任务中...点击【停止】";
            this.btn_ordertimeoutremindJush_stop.UseVisualStyleBackColor = false;
            this.btn_ordertimeoutremindJush_stop.Visible = false;
            this.btn_ordertimeoutremindJush_stop.Click += new System.EventHandler(this.btn_ordertimeoutremindJush_stop_Click);
            // 
            // btn_ordertimeremindJush_start
            // 
            this.btn_ordertimeremindJush_start.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_ordertimeremindJush_start.Location = new System.Drawing.Point(22, 117);
            this.btn_ordertimeremindJush_start.Margin = new System.Windows.Forms.Padding(4);
            this.btn_ordertimeremindJush_start.Name = "btn_ordertimeremindJush_start";
            this.btn_ordertimeremindJush_start.Size = new System.Drawing.Size(283, 53);
            this.btn_ordertimeremindJush_start.TabIndex = 21;
            this.btn_ordertimeremindJush_start.Text = ">> 启动订单即将超时APP推送";
            this.btn_ordertimeremindJush_start.UseVisualStyleBackColor = true;
            this.btn_ordertimeremindJush_start.Click += new System.EventHandler(this.btn_ordertimeremindJush_start_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(681, 522);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "速库消息推送后台系统V2.0";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Leave += new System.EventHandler(this.MainForm_Leave);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_gbkremind_start;
        private System.Windows.Forms.Button btn_ordertimeremind_start;
        private System.Windows.Forms.Button btn_notloginremind_start;
        private System.Windows.Forms.Timer timer_goldbookRemid;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button btn_gbkremind_stop;
        private System.Windows.Forms.Button btn_ordertimeoutremind_stop;
        private System.Windows.Forms.Button btn_notloginremind_stop;
        private System.Windows.Forms.Timer timer_notuserloginRemind;
        private System.Windows.Forms.Timer timer_ordertimeout;
        private System.Windows.Forms.Timer timer_ordertimeoutJush;
        private System.Windows.Forms.Timer timer_notloginuserJush;
        private System.Windows.Forms.Timer timer_goldbookRemindJush;
        private System.Windows.Forms.Timer timer_addgoldbookJush;
        private System.Windows.Forms.Timer timer_timingJush;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_timingJush_stop;
        private System.Windows.Forms.Button btn_timingJush_start;
        private System.Windows.Forms.Button btn_addgbkremindJush_stop;
        private System.Windows.Forms.Button btn_addgbkremindJush_start;
        private System.Windows.Forms.Button btn_gbkremindJush_stop;
        private System.Windows.Forms.Button btn_gbkremindJush_start;
        private System.Windows.Forms.Button btn_notloginuserremind_End;
        private System.Windows.Forms.Button btn_NotLoginUserRemindJush_Start;
        private System.Windows.Forms.Button btn_ordertimeoutremindJush_stop;
        private System.Windows.Forms.Button btn_ordertimeremindJush_start;
    }
}

