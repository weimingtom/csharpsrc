﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using _3d.Function;
using System.Runtime.InteropServices;

namespace _3d
{
    public partial class main : Form
    {
        Timer tLabelMove = null;
        Timer marqueeLabelMove = null;
        Timer setUsersOffline = new Timer();
        Timer refreshMyOnlineTime = new Timer();

        private Form1 f1 = new Form1();
        private Form3 f3 = new Form3();
        private Form4 f4 = new Form4();
        private Page4Form pf4 = new Page4Form();

        private one11xuan5 o11_1 = new one11xuan5();
        private two11xuan5 o11_2 = new two11xuan5();

        private OutputForm f2 = null;
        

        Login l = new Login();

        #region Timer控件设置
        System.Windows.Forms.Timer t = new Timer();//Form标题栏文字
        //标题栏时间
        private void time_Tick(object sender, EventArgs e)
        {
            string us = "";
            string ub = "";
            if (Global.user_province.IndexOf("省") >= 0)
            {
                us = Global.user_province.Substring(0, Global.user_province.IndexOf("省") + 1);
            }
            else
            {
                us = Global.user_province.Substring(0, Global.user_province.IndexOf("市") + 1);
            }

            if (Global.user_province.Contains("省") && Global.user_province.Contains("市"))
            {
                int us1 = 0;
                int us2 = 0;
                us1 = Global.user_province.IndexOf("省") + 1;
                us2 = Global.user_province.IndexOf("市") + 1;
                if (us2 > us1)
                {
                    ub = Global.user_province.Substring(us1, us2 - us1);
                }
            }

            string un = Global.user_realname;
            string uv = "尊敬的：";
            if (Global.user_vali.Equals("1"))
                uv = "尊贵的市场总监：";
            if (Global.user_vali.Equals("3"))
                uv = "尊贵的" + us + "代理：";
            if (Global.user_vali.Equals("4"))
                uv = "尊贵的" + ub + "代理：";
            if (Global.user_vali.Equals("5"))
                uv = "尊贵的区域代理：";
            if (Global.user_vali.Equals("6"))
                uv = "尊贵的市场专员：";
            if (Global.user_vali.Equals("7"))
                uv = "尊贵的业务员：";
            if (Global.user_vali.Equals("8"))
                uv = "尊贵的" + us + "代理：";
            //用户权限,1为总代理,2为普通用户,3为省级代理,4为市级代理,5为区域代理,6为市场专员,7为业务员,8为省代（虚）
            setFormText("恩泽天下 - 辅助运算软件 - 欢迎" + uv + un + "，当前版本：" + Global.version + "，系统时间：" + DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒"));
        }

        private delegate void WriteFormTextDelegate(string args);

        private void setFormText(string args)
        {
            if (this.IsHandleCreated)
            {
                this.Invoke(new WriteFormTextDelegate(writeFormText), args);
            }
        }

        private void writeFormText(string args)
        {
            this.Text = args;
        }

        #endregion

        #region 页面滚动文字

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (label8.Left > -this.label8.Width)
            {
                label8.Left -= 3;
            }
            else
            {
                label8.Left = this.panel2.Width;
            }
        }

        private void marqueeTimer_Tick(object sender, EventArgs e)
        {
            if (marqueeLabel.Left > -this.marqueeLabel.Width)
            {
                marqueeLabel.Left -= 3;
            }
            else
            {
                marqueeLabel.Left = this.marqueePanel.Width;
            }
        }

        private void label8_MouseHover(object sender, EventArgs e)
        {
            tLabelMove.Enabled = false;
        }

        private void label8_MouseLeave(object sender, EventArgs e)
        {
            tLabelMove.Enabled = true;
        }

        private void label8_MouseMove(object sender, MouseEventArgs e)
        {
            tLabelMove.Enabled = false;
        }

        private void marqueeLabel_MouseHover(object sender, EventArgs e)
        {
            marqueeLabelMove.Enabled = false;
        }

        private void marqueeLabel_MouseLeave(object sender, EventArgs e)
        {
            marqueeLabelMove.Enabled = true;
        }

        private void marqueeLabel_MouseMove(object sender, MouseEventArgs e)
        {
            marqueeLabelMove.Enabled = false;
        }

        #endregion

        public main()
        {
            InitializeComponent();
        }

        //时时彩“生成”按钮点击
        private void button1_Click(object sender, EventArgs e)
        {
            if (f2 == null || f2.IsDisposed)//判断窗口是否打开
            {
                f2 = new OutputForm();

            }

            if (f1.chuHaoGeShu() == true && f3.chuHaoGeShu() == true && f4.chuHaoGeShu() == true)
            {
                getData();
            }
        }

        //时时彩最终生成
        private void getData()
        {
            //将第二个页面f3与第一个页面f1的数据传递结合
            f3.getF1Data(f1.fenJieShi());//f1.fenJieShi()       f3.noLocHeExecute()

            //将第三个页面f4与第二个页面f3的数据传递结合
            f4.getF3Data(f3.sumZhi());
            string ans = "";
            string[] allNum = f4.pingHengZhiShu();
            for (int i = 0; i < allNum.Count(); i++)
            {
                ans += allNum[i];
            }
            //f2.Text = "共计  " + (ans.Length / 4).ToString() + "   注"; f2.textBox1.Text = ans.Substring(0, ans.Length - 1);

            try { f2.Text = "共计  " + (ans.Length / 4).ToString() + "   注,当前运算结果为 时时彩 - 3D - 排列三 - 快乐3 - 时时乐"; f2.textBox1.Text = ans.Substring(0, ans.Length - 1); }
            catch { f2.Text = "出错"; f2.textBox1.Text = "运算出错，请您检查您设置的条件！"; }
            f2.Show();
        }

        //“时时彩清空”按钮点击  
        private void button2_Click(object sender, EventArgs e)
        {
            f1.clearCheckbox();
            f3.clearCheckBox();
            f4.clearCheckBox();
            pf4.clearCheckBox(pf4.Controls);
        }

        //十一选五“生成”按钮点击
        private void computingButton_Click(object sender, EventArgs e)
        {
            if (f2 == null || f2.IsDisposed)//判断窗口是否打开
            {
                f2 = new OutputForm();
            }

            get11Data();
        }

        //十一选五清空按钮点击
        private void clearButton_Click(object sender, EventArgs e)
        {
            o11_1.clearCheckbox();
            o11_2.clearCheckbox();
        }

        //十一选五最终生成
        private void get11Data()
        {
            List<string> li = new List<string>();
            li = o11_1.sendData();
            li = o11_2.sendData(li);

            string res = "";
            for (int i = 0; i < li.Count; i++)
            {
                res += (li[i] + "\r\n");
            }
            if (res.Length != 0)
            {
                res = res.Substring(0, res.Length - 1);
            }

            int countNum = res.Split('\n').Length;
            if (res == "")
            {
                countNum = 0;
                res = "运算出错，请您检查您设置的条件！";
            }
            try { f2.Text = "共计  " + countNum + "   注,当前运算结果为 十一选五"; f2.textBox1.Text = res; }
            catch { f2.Text = "出错"; f2.textBox1.Text = "运算出错，请您检查您设置的条件！"; }
            f2.zhiZhuanZu.Visible = false;//十一选五禁用“直选转组选”按钮
            f2.button1.Location = new Point(f2.button1.Location.X + 95, f2.button1.Location.Y);//把“复制”按钮右移至中央
            f2.Show();
        }

        //清内存
        private void timer3_Tick(object sender, EventArgs e)
        {
            ClearMemory.Clear();
        }

        ToolTip changeButtonToolTip = new ToolTip();
        private string changeButtonText = "";
        private string changeButton1Text = "杀码,出号,分解式,临码,组合,凹凸,密码修改,信息修改";
        private string changeButton2Text = "两码和,两码差,和值";
        private string changeButton3Text = "合值,杀合值,跨距(跨度),差合,龙头凤尾,号码属性,平衡指数";
        private string changeButton4Text = "东北专供：两码差、两位数组胆、走势图";

        private string changeButton1TextFor11x5 = "出号,偶数,合数,小数,连号,平衡,号码配置";
        private string changeButton2TextFor11x5 = "必下两码,和值,跨度,最大邻码跨距,反边球,边临和,龙头凤尾,合值,临码和,任意两码差";
        Font changeButtonFont = new Font("微软雅黑", 13.0F, FontStyle.Regular);

        private void main_Load(object sender, EventArgs e)
        {

            //设置时时彩及十一选五页面最上方的滚动文字
            tLabelMove = new Timer();
            tLabelMove.Interval = 100;
            tLabelMove.Tick += new EventHandler(timer2_Tick);
            tLabelMove.Enabled = true;
            marqueeLabelMove = new Timer();
            marqueeLabelMove.Interval = 100;
            marqueeLabelMove.Tick += new EventHandler(marqueeTimer_Tick);
            marqueeLabelMove.Enabled = true;

            //启用标题栏计时
            t.Tick += new EventHandler(time_Tick);
            t.Interval = 1000;//设置是执行一次（false）还是一直执行(true)； 
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；

            //启用内存回收
            timer3.Interval = 60000;
            timer3.Enabled = true;

            //将tabpage隐藏

            //启动下线机制
            //如30分钟 30 * 60 *1000=1800000
            this.offlineUserTimer.Interval = 5 * 60000;
            this.offlineUserTimer.Enabled = true;

            //***刷新全员列表，谁离线过久就下线 10分钟一次
            //setUsersOffline.Tick += new EventHandler(setUsersOffline_Tick);
            //this.setUsersOffline.Interval = 10 * 60000;
            //this.setUsersOffline.Enabled = true;

            //刷新在线时间
            refreshMyOnlineTime.Tick += new EventHandler(refreshMyOnlineTime_Tick);
            refreshMyOnlineTime.Interval = 6 * 60000;
            refreshMyOnlineTime.Enabled = true;

            EnableDoubleBuffering();//启用双缓冲
            this.label8.Text = Global.main_msg;//获取时时彩界面跑马灯文字信息
            this.marqueeLabel.Text = Global.main_msg;//获取时时彩界面跑马灯文字信息
            this.changeButton1_Click(null, null);//触发时时彩的按钮1事件，为了将page添加进框体
            this.pageOneBtn_Click(null, null);//触发十一选五的按钮1事件，为了将page添加进框体

            // Set up the delays for the ToolTip.   
            changeButtonToolTip.OwnerDraw = true;
            changeButtonToolTip.Draw += new DrawToolTipEventHandler(changeButtonToolTip_Draw);
            changeButtonToolTip.Popup += new PopupEventHandler(changeButtonToolTip_Popup);
            changeButtonToolTip.AutoPopDelay = 0;   //tooltip的显示时间(无用)
            changeButtonToolTip.ReshowDelay = 0;  //出现tooltip的延迟显示时间(无用)
            changeButtonToolTip.SetToolTip(this.changeButton1, changeButton1Text);  //绑定tooltip到控件
            changeButtonToolTip.SetToolTip(this.changeButton2, changeButton2Text);  //绑定tooltip到控件
            changeButtonToolTip.SetToolTip(this.changeButton3, changeButton3Text);  //绑定tooltip到控件
            changeButtonToolTip.SetToolTip(this.changeButton4, changeButton4Text);  //绑定tooltip到控件
            changeButtonToolTip.SetToolTip(this.pageOneBtn, changeButton1TextFor11x5);  //绑定tooltip到控件
            changeButtonToolTip.SetToolTip(this.pageTwoBtn, changeButton2TextFor11x5);  //绑定tooltip到控件

            //走势图页面WebBrowser
            string url = Global.soft_server_url + "/ZstNavigate.html?t=" + DateTime.Now.Ticks;//用随机数防止IE缓存
            this.zstWebBrowser.Navigate(new System.Uri(url, System.UriKind.Absolute));

            //倍投计算器页面
            loadBeiTouCal();

            // 隐藏倍投计算器
            mainPages.TabPages.Remove(mainPages.TabPages[2]);
        }

        private void changeButton1_MouseMove(object sender, MouseEventArgs e)
        {
            changeButtonText = changeButton1Text;
        }

        private void changeButton2_MouseMove(object sender, MouseEventArgs e)
        {
            changeButtonText = changeButton2Text;
        }

        private void changeButton3_MouseMove(object sender, MouseEventArgs e)
        {
            changeButtonText = changeButton3Text;
        }

        private void changeButton4_MouseMove(object sender, MouseEventArgs e)
        {
            changeButtonText = changeButton4Text;
        }

        private void changeButtonToolTip_Popup(object sender, PopupEventArgs e)
        {
            // on popip set the size of tool tip
            e.ToolTipSize = TextRenderer.MeasureText(changeButtonText, changeButtonFont);
        }

        private void changeButtonToolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.DrawBackground();
            e.DrawBorder();
            e.Graphics.DrawString(e.ToolTipText, changeButtonFont, Brushes.Black, new PointF(0, 0));
        }

        #region 防止控件或图像过多卡

        [DllImport("user32.dll")]
        static extern bool LockWindowUpdate(IntPtr hWndLock);
        private void main_ResizeBegin(object sender, EventArgs e)
        {
            LockWindowUpdate(this.Handle);
        }

        private void main_ResizeEnd(object sender, EventArgs e)
        {
            LockWindowUpdate(IntPtr.Zero);
        }

        //双缓冲
        public void EnableDoubleBuffering()
        {
            // Set the value of the double-buffering style bits to true.
            this.SetStyle(ControlStyles.DoubleBuffer |
               ControlStyles.UserPaint |
               ControlStyles.AllPaintingInWmPaint,
               true);
            this.UpdateStyles();
        }
        #endregion

        #region Tab2页面的LinkLabel点击

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clickMethod("iexplore.exe", "http://tool.cailele.com/zs/14_125.htm");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clickMethod("iexplore.exe", "http://zst.cjcp.com.cn/cjwssc_3xing/view/ssc_aotu.php?lotteryType=ssc");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clickMethod("iexplore.exe", "http://tool.cailele.com/zs/13_121.htm");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clickMethod("iexplore.exe", "http://tool.cailele.com/zs/1_1.htm");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clickMethod("iexplore.exe", "http://tool.cailele.com/zs/2_14.htm");
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clickMethod("iexplore.exe", "http://tool.cailele.com/zs/16_132.htm");
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clickMethod("iexplore.exe", "http://tool.cailele.com/zs/4_26.htm");
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clickMethod("iexplore.exe", "http://tool.cailele.com/zs/31_214.htm");
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clickMethod("iexplore.exe", "http://www.eztx.cn");
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clickMethod("iexplore.exe", "http://tool.cailele.com/zs/18_141.htm");
        }

        /// <summary>
        /// linkLabel点击方法
        /// </summary>
        /// <param name="startProgram"></param>
        /// <param name="url"></param>
        private void clickMethod(string startProgram, string url)
        {
            try
            {
                System.Diagnostics.Process.Start(startProgram, url);
            }
            catch
            {
                Clipboard.SetText(url);
                MessageBox.Show("打开网址失败，可能是您的IE浏览器存在问题，现已将网址复制，您可以直接打开浏览器粘贴进行浏览。");
            }
        }
        #endregion

        //启动下线机制
        private void offlineUserTimer_Tick(object sender, EventArgs e)
        {
            DataTable tb = LinkMySql.MySqlQuery("select allowlogin,`online`,isdel from " + Global.sqlUserTable + " where user_name='" + Global.user_name + "'");
            if (tb != null && tb.Rows.Count > 0)
            {
                DataRow dr = tb.Rows[0];
                string allowlogin = dr["allowlogin"].ToString();
                string online = dr["online"].ToString();
                string isdel = dr["isdel"].ToString();

                if (allowlogin.Equals("0"))
                {
                    MessageBox.Show("账号已被禁止登录，软件即将关闭\r\n请重新登录或者检查账号安全");
                }
                else if (isdel.Equals("-1"))
                {
                    MessageBox.Show("账号已被删除，软件即将关闭\r\n请重新登录或者检查账号安全");
                }
                else if (online.Equals("0"))
                {
                    MessageBox.Show("账号已被下线，软件即将关闭\r\n请重新登录或者检查账号安全");
                }
                else if (online.Equals("2") && Global.loginType.Equals("1"))//机器码登录下线机制
                {
                    MessageBox.Show("账号已从机器码登录，软件即将关闭\r\n请重新登录或者检查账号安全");
                }
                else
                {
                    return;
                }

                Global.isNormalStatus = false;// 设置为非正常关闭
                this.offlineUserTimer.Enabled = false;
                this.Dispose();
                this.Close();
            }
        }

        /// <summary>
        /// 刷新自己的在线时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshMyOnlineTime_Tick(object sender, EventArgs e)
        {
            LinkMySql.MySqlExcute("update " + Global.sqlUserTable + " set onlinetime=now() where user_name='" + Global.user_name + "' ");
        }

        //启动全员下线机制
        private void setUsersOffline_Tick(object sender, EventArgs e)
        {
            try
            {
                DataTable tb = LinkMySql.MySqlQuery("select user_name,`onlinetime`,user_realname from " + Global.sqlUserTable + " where `online`!='0' and isdel!='-1' and allowlogin!='0' and user_name!='" + Global.user_name + "'");
                if (tb != null && tb.Rows.Count > 0)
                {
                    foreach (DataRow dr in tb.Rows)
                    {
                        string user_name = dr["user_name"].ToString();
                        string user_realname = dr["user_realname"].ToString();
                        string onlinetime = dr["onlinetime"].ToString();
                        if (string.IsNullOrEmpty(onlinetime))
                        {
                            continue;
                        }

                        TimeSpan ts1 = new TimeSpan(Convert.ToDateTime(onlinetime).Ticks);//服务器最后更新时间
                        TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);//现在时间
                        TimeSpan ts = ts1.Subtract(ts2).Duration();//绝对值
                        int diffHours = Convert.ToInt32(ts.TotalMinutes);
                        //如果相差时间超过10分钟，那么就把该用户下线
                        if (diffHours > 10)
                        {
                            //MessageBox.Show(diffHours + ":::" + user_realname + "");
                            LinkMySql.MySqlExcute("update " + Global.sqlUserTable + " set `online`='0' where user_name='" + user_name + "' ");
                        }
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 选中按钮颜色
        /// </summary>
        Color _changeButtonColorFocus = Color.FromArgb(122, 185, 0);//122,185,0

        /// <summary>
        /// 未选中按钮颜色，默认背景色
        /// </summary>
        Color _changeButtonColor = Color.GreenYellow;


        /// <summary>
        /// 页面一
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeButton1_Click(object sender, EventArgs e)
        {
            if (f1 != null)
            {
                f1.FormBorderStyle = FormBorderStyle.None; // 无边框
                f1.TopLevel = false; // 不是最顶层窗体
                panel1.Controls.Add(f1);  // 添加到 Panel中
                pageHideOrShow(f1, changeButton1);
            }

            panel1.Focus();
        }

        /// <summary>
        /// 页面二
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeButton2_Click(object sender, EventArgs e)
        {
            if (f3 != null)
            {
                f3.FormBorderStyle = FormBorderStyle.None; // 无边框
                f3.TopLevel = false; // 不是最顶层窗体
                panel1.Controls.Add(f3);  // 添加到 Panel中
                pageHideOrShow(f3, changeButton2);
            }

            panel1.Focus();
        }

        /// <summary>
        /// 页面三
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeButton3_Click(object sender, EventArgs e)
        {
            if (f4 != null)
            {
                f4.FormBorderStyle = FormBorderStyle.None; // 无边框
                f4.TopLevel = false; // 不是最顶层窗体
                panel1.Controls.Add(f4);  // 添加到 Panel中
                pageHideOrShow(f4, changeButton3);
            }

            panel1.Focus();
        }

        /// <summary>
        /// 页面四
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeButton4_Click(object sender, EventArgs e)
        {
            if (pf4 != null)
            {
                pf4.FormBorderStyle = FormBorderStyle.None; // 无边框
                pf4.TopLevel = false; // 不是最顶层窗体
                panel1.Controls.Add(pf4);  // 添加到 Panel中
                pageHideOrShow(pf4, changeButton4);
            }

            panel1.Focus();
        }

        /// <summary>
        /// 进行时时彩隐藏或者显示form
        /// </summary>
        /// <param name="f"></param>
        private void pageHideOrShow(Form f, Button btn)
        {
            //切换Form
            Form[] forms = { f1, f3, f4, pf4 };
            foreach (Form _f in forms)
            {
                if (_f.Equals(f))
                {
                    _f.Show();
                    continue;
                }
                _f.Hide();
            }

            //给切换Form的按钮换色
            Button[] btns = { changeButton1, changeButton2, changeButton3, changeButton4 };
            foreach (Button _btn in btns)
            {
                if (_btn.Equals(btn))
                {
                    _btn.BackColor = _changeButtonColorFocus;
                    continue;
                }
                _btn.BackColor = _changeButtonColor;
            }
        }

        /// <summary>
        /// 点击webbrowser链接后，会调用系统默认浏览器打开网页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zstWebBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            string currentUri = ((WebBrowser)sender).Document.ActiveElement.GetAttribute("href");
            System.Diagnostics.Process.Start(currentUri);
        }

        private void pageOneBtn_Click(object sender, EventArgs e)
        {
            if (o11_1 != null)
            {
                o11_1.FormBorderStyle = FormBorderStyle.None; // 无边框
                o11_1.TopLevel = false; // 不是最顶层窗体
                mainPanel.Controls.Add(o11_1);  // 添加到 Panel中
                pageHideOrShowFor11x5(o11_1, pageOneBtn);
            }

            mainPanel.Focus();
        }

        private void pageTwoBtn_Click(object sender, EventArgs e)
        {
            if (o11_2 != null)
            {
                o11_2.FormBorderStyle = FormBorderStyle.None; // 无边框
                o11_2.TopLevel = false; // 不是最顶层窗体
                mainPanel.Controls.Add(o11_2);  // 添加到 Panel中
                pageHideOrShowFor11x5(o11_2, pageTwoBtn);
            }

            mainPanel.Focus();
        }

        /// <summary>
        /// 进行十一选5隐藏或者显示form
        /// </summary>
        /// <param name="f"></param>
        private void pageHideOrShowFor11x5(Form f, Button btn)
        {
            //切换Form
            Form[] forms = { o11_1, o11_2 };
            foreach (Form _f in forms)
            {
                if (_f.Equals(f))
                {
                    _f.Show();
                    continue;
                }
                _f.Hide();
            }

            //给切换Form的按钮换色
            Button[] btns = { pageOneBtn, pageTwoBtn };
            foreach (Button _btn in btns)
            {
                if (_btn.Equals(btn))
                {
                    _btn.BackColor = _changeButtonColorFocus;
                    continue;
                }
                _btn.BackColor = _changeButtonColor;
            }
        }

        private void pageOneBtn_MouseMove(object sender, MouseEventArgs e)
        {
            changeButtonText = changeButton1TextFor11x5;
        }

        private void pageTwoBtn_MouseMove(object sender, MouseEventArgs e)
        {
            changeButtonText = changeButton2TextFor11x5;
        }

        /// <summary>
        /// 载入倍投计算器
        /// </summary>
        private void loadBeiTouCal()
        {
            ColumnHeader ch = new ColumnHeader();


        }
    }
}
