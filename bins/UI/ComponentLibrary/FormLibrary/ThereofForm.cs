﻿using Core.CacheLibrary.FormCache;
using Core.DefaultData.DataLibrary;
using Core.StaticMethod.Method.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;
using UI.ComponentLibrary.MethodLibrary.Interface;

namespace UI.ComponentLibrary.FormLibrary {
    /// <summary>
    /// 关于窗体
    /// </summary>
    public partial class ThereofForm : Form, IComponentInitMode<Form>{
        private string github = "https://github.com/l575989285/EditCodeAssistant.git";
        internal ThereofForm() {
            InitializeComponent();
        }
        /// <summary>
        /// 打开单例模式下的添加字符窗口
        /// </summary>
        /// <param name="isShowTop">是否显示为顶层窗体</param>
        /// <returns></returns>
        public Form initSingleExample(bool isShowTop) {
            ThereofForm thereofForm = null;
            Form form = FormCacheFactory.getSingletonCache(DefaultNameEnum.THEREOF_FORM);
            if(form == null || form.IsDisposed || !(form is AddCharsForm)) { 
                thereofForm = this;
                thereofForm.Name = EnumUtils.GetDescription(DefaultNameEnum.THEREOF_FORM);
                thereofForm = FormCacheFactory.ininSingletonForm(thereofForm, false);
            } else {
                thereofForm = (ThereofForm)form;
                thereofForm.Activate();
            }
            if(isShowTop) FormCacheFactory.addTopFormCache(thereofForm);
            thereofForm.Visible = false;
            return thereofForm;
        }
        /// <summary>
        /// 打开多例模式下的添加字符窗口
        /// </summary>
        /// <param name="isShowTop">是否显示为顶层窗体</param>
        /// <returns></returns>
        public Form initPrototypeExample(bool isShowTop) {
            ThereofForm thereofForm = this;
            thereofForm.Name = EnumUtils.GetDescription(DefaultNameEnum.THEREOF_FORM)+DateTime.Now.Ticks.ToString();;
            // 加入到顶层窗体集合
            if(isShowTop) FormCacheFactory.addTopFormCache(thereofForm);
            // 加入到多例工厂
            FormCacheFactory.addPrototypeCache(DefaultNameEnum.THEREOF_FORM, thereofForm);
            thereofForm.Activate();
            thereofForm.Visible = false;
            return thereofForm;
        }
        // 窗体加载事件
        private void ThereofForm_Load(object sender, EventArgs e) {
            // 设置图标
            this.Icon = MessyUtils.IamgeToIcon(Core.ImageResource.关于,true);
            this.Text = "关于 " + EnumUtils.GetDescription(DefaultNameEnum.PROGRAM_NAME);
            this.programName.Text = "关于 " + EnumUtils.GetDescription(DefaultNameEnum.PROGRAM_NAME);
            setVersion();

            // 组装Tab容器
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("系统信息", getSystemMess());
            dic.Add("环境变量", getVariable());
            dic.Add("特殊目录", getSpecialcatalog());
            setTab(dic);
        }
        /// <summary>
        /// 设置程序的详细版本信息
        /// </summary>
        /// <returns></returns>
        private void setVersion() { 
            string version = EnumUtils.GetDescription(DefaultNameEnum.PROGRAM_NAME)+" 2018"+Environment.NewLine
                + "版本：1.0.2"+Environment.NewLine
                + "内部修订版本：1.0.0.2"+Environment.NewLine
                + "此应用是基于C#和.NET Framework 4.0开发，"
                + "用于解决程序开发工作中遇到的批量的，重复的字符串生成与处理"+Environment.NewLine
                + "GitHub地址："+github;
            head_textB.Text = version;
            head_textB.ForeColor = ColorTranslator.FromHtml("#E13D3D");
            系统信息_lab.Location = new Point(系统信息_lab.Location.X, head_textB.Location.Y + head_textB.Height+5);
            tabControl1.Location = new Point(tabControl1.Location.X, 系统信息_lab.Location.Y+系统信息_lab.Height+ 3);
            tabControl1.Height = ClientSize.Height - (tabControl1.Location.Y+5);
        }

        /// <summary>
        /// 向Tab容器中加入系统消息
        /// </summary>
        /// <param name="strArr"></param>
        private void setTab(Dictionary<string, string> dic) {
            tabControl1.ItemSize = new Size(130, 25);
            tabControl1.DrawMode = TabDrawMode.Normal;
            
            foreach(KeyValuePair<string, string> kvp in dic) { 
                TabPage page = new TabPage();
                page.Text= kvp.Key;
                page.Size = tabControl1.Size;
                TextBox t = new TextBox();
                t.ReadOnly = true;
                t.Multiline = true;
                t.BorderStyle = BorderStyle.None;
                t.ScrollBars = ScrollBars.Vertical;
                t.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                t.Size = page.Size;
                t.Text = kvp.Value;
                t.WordWrap = false;
                t.Font = new Font("微软雅黑", 10,FontStyle.Regular);
                page.Controls.Add(t);
                tabControl1.TabPages.Add(page);
            }
        }
        /// <summary>
        /// 设置程序的系统信息
        /// </summary>
        private string getSystemMess() {
            StringBuilder strB = new StringBuilder();
            string machineName = Environment.MachineName + Environment.NewLine;
            string servicePack = Environment.OSVersion.ServicePack + Environment.NewLine;
            string osVersionName = GetOsVersion()+servicePack+ Environment.NewLine;
            string userName = Environment.UserName+ Environment.NewLine;
            string domainName = Environment.UserDomainName+ Environment.NewLine;
            string tickCount = (Environment.TickCount / 1000).ToString() + "s"+ Environment.NewLine;
            string systemPageSize = (Environment.SystemPageSize / 1024).ToString() + "KB"+ Environment.NewLine;
            string systemDir = Environment.SystemDirectory+ Environment.NewLine;
            string stackTrace = Environment.StackTrace+ Environment.NewLine;
            string processorCounter = Environment.ProcessorCount.ToString()+ Environment.NewLine;
            string platform = Environment.OSVersion.Platform.ToString()+ Environment.NewLine;
            bool is64Os = Environment.Is64BitOperatingSystem;
            bool is64Process = Environment.Is64BitProcess;
            string ram = GetPhisicalMemory().ToString()+"MB"+Environment.NewLine;
            string currDir = Environment.CurrentDirectory+ Environment.NewLine;
            string cmdLine = Environment.CommandLine+ Environment.NewLine;
            string[] drives = Environment.GetLogicalDrives();
            
            strB.Append("机器名："+machineName)
                .Append("操作系统："+osVersionName)
                .Append("用户名："+userName)
                .Append("启动时间："+tickCount)
                .Append("内存页："+systemPageSize)
                .Append("系统目录："+systemDir)
                .Append("CPU数："+processorCounter)
                .Append("平台标识："+platform)
                .Append("系统类型："+ (is64Os? "64bit" : "32bit")).Append(Environment.NewLine)
                .Append("进程类型："+ (is64Process ? "64bit" : "32bit")).Append(Environment.NewLine)
                .Append("当前目录："+currDir)
                .Append("命令行："+cmdLine)
                .Append("内存："+ram)
                .Append("域名称："+servicePack)
                .Append("盘符："+string.Join("  ", drives));
            return strB.ToString();
        }

        /// <summary>
        /// 获取环境变量
        /// </summary>
        /// <returns></returns>
        private string getVariable() {
            StringBuilder strB = new StringBuilder();
            string retStr = "";
            //环境变量
            // HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Session Manager\Environment
            IDictionary dicMachine = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);
            strB.Append("机器环境变量："+Environment.NewLine);
            foreach (string str in dicMachine.Keys) {
                string val = dicMachine[str].ToString();
                strB.Append(string.Format("{0}： {1}{2}", str, val, Environment.NewLine));
            }
            strB.Append(string.Format("{0}{1}", ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>", Environment.NewLine));
            // 环境变量存储在 Windows 操作系统注册表的 HKEY_CURRENT_USER\Environment 项中，或从其中检索。
            IDictionary dicUser = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User);
            strB.Append("用户环境变量"+Environment.NewLine);
            foreach (string str in dicUser.Keys) {
                string val = dicUser[str].ToString();
                strB.Append(string.Format("{0}： {1}{2}", str, val, Environment.NewLine));
            }
            strB.Append(string.Format("{0}{1}", ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>", Environment.NewLine));
            IDictionary dicProcess = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
            strB.Append(string.Format("{0}： {1}", "进程环境变量", Environment.NewLine));
            foreach (string str in dicProcess.Keys) {
                string val = dicProcess[str].ToString();
                strB.Append(string.Format("{0}： {1}{2}", str, val, Environment.NewLine));
            }
            retStr = StringUtils.TrimEndNewLine(strB.ToString());
            return retStr;
        }

        /// <summary>
        /// 获取特殊目录
        /// </summary>
        /// <returns></returns>
        private string getSpecialcatalog() {
            StringBuilder strB = new StringBuilder();
            string retStr = "";
            //特殊目录 
            string[] names = Enum.GetNames(typeof(Environment.SpecialFolder));
            foreach (string name in names){
                Environment.SpecialFolder sf;
                if (Enum.TryParse<Environment.SpecialFolder>(name, out sf))
                {
                    string folder = Environment.GetFolderPath(sf);
                    strB.Append(string.Format("{0}： {1}{2}", name, folder, Environment.NewLine));
                }
            }
            retStr = StringUtils.TrimEndNewLine(strB.ToString());
            return retStr;
        }
        /// <summary>
        /// 判断操作系统版本
        /// </summary>
        /// <param name="ver"></param>
        /// <returns></returns>
        private string GetOsVersion() {
            string str = "未知";
            switch (Environment.OSVersion.Version.Major + "." + Environment.OSVersion.Version.Minor)
            {
                case "5.0" :
                    str = "Windows2000";
                    break;
                case "5.1" :
                    str = "WindowsXP";
                    break;
                case "5.2" :
                    str = "Windows2003";
                    break;
                case "6.0" :
                    str = "Windows2008";
                    break;
                case "6.1" :
                    str = "Windows7";
                    break;
                case "6.2" :
                    str = "Windows8";
                    break;
                case "6.3" :
                    str = "Windows8.1";
                    break;
                case "10.0" :
                    str = "Windows10";
                    break;
            }
            return str;
        }
        /// <summary>
        /// 获取系统内存大小
        /// </summary>
        /// <returns>内存大小（单位M）</returns>
        private int GetPhisicalMemory()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher();   //用于查询一些如系统信息的管理对象 
            searcher.Query = new SelectQuery("Win32_PhysicalMemory ", "", new string[] { "Capacity" });//设置查询条件 
            ManagementObjectCollection collection = searcher.Get();   //获取内存容量 
            ManagementObjectCollection.ManagementObjectEnumerator em = collection.GetEnumerator();
 
            long capacity = 0;
            while (em.MoveNext())
            {
                ManagementBaseObject baseObj = em.Current;
                if (baseObj.Properties["Capacity"].Value != null)
                {
                    try
                    {
                        capacity += long.Parse(baseObj.Properties["Capacity"].Value.ToString());
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
            return (int)(capacity / 1024 / 1024);
        }

        private void 复制信息_but_Click(object sender, EventArgs e) {
            try { 
                TextBox t = this.tabControl1.SelectedTab.Controls.OfType<TextBox>().ToArray()[0];
                t.SelectAll();
                t.Copy();
                t.SelectionLength = 0;
                MessageBox.Show("复制"+this.tabControl1.SelectedTab.Text+"成功");
            } catch(Exception e1) { 
                MessageBox.Show("复制失败,异常："+e1.Message);
            }
            
        }
        private void 系统信息_but_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("msinfo32.exe");
        }

        private void github_but_Click(object sender, EventArgs e) {
            Clipboard.SetDataObject(github);
            MessageBox.Show("复制GitHub地址成功");
        }
    }
}
