﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Core.StaticMethod.Inter;
using Core.StaticMethod.Method.Redraw;
using Core.StaticMethod.Method.Utils;
using Ui.ControlEventLibrary;
using UI.ComponentLibrary.FormLibrary;
using Core.CacheLibrary.ControlCache;
using Core.CacheLibrary.OperateCache.TextBoxOperateCache;
using UI_TopMenuBar.TopMenuEvent;
using UI_OptionForm;
using Core.DefaultData.DataLibrary;

namespace UI_TopMenuBar
{
    public partial class TopMenuContainer : UserControl, MenuItemAopInter
    {
        private TopMenuContainer()
        {
            // 初始化
            InitializeComponent();
            // 初始化配置
            initTopMenuConfig();
            //遍历顶部菜单的子菜单并执行对应的AOP方法
            this.traverseItems();
        }
        // 定义菜单的执行方法的委托
        private delegate object methodDelegate(Dictionary<Type , object> data);
        /// <summary>
        /// 遍历顶部菜单的子菜单并执行对应的AOP方法
        /// </summary>
        private void traverseItems()
        {
            foreach(ToolStripMenuItem tool in this.topMenuStrip.Items.OfType<ToolStripMenuItem>())
            {
                new MenuItemUtilsMet().isDownItemAop(tool,this);
            }
        }
        /// <summary>
        /// 当菜单项有子项时的执行方法
        /// </summary>
        public virtual void haveDownItem(ToolStripMenuItem tool)
        {
            
        }
        /// <summary>
        /// 当菜单项无子项时的执行方法
        /// </summary>
        public virtual void noDownItem(ToolStripMenuItem tool)
        {
            tool.Click+=new EventHandler(this.MenuItem_Click);
            //MenuItemUtilsMet.fontCentered(tool);
        }
        /// <summary>
        /// 全部菜单项的执行方法
        /// </summary>
        
        public virtual void allItem(ToolStripMenuItem tool)
        {
            tool.BackColor = Color.White;
            tool.AutoToolTip = true;
            tool.ToolTipText = tool.Text;
            // tool.Font = topMenuStrip.Font;
            // 绑定Image
            setItemImage(tool);
        }
        /// <summary>
        /// 初始化顶部菜单配置
        /// </summary>
        private void initTopMenuConfig() {
            topMenuStrip.Name = DefaultNameCof.TOP_MENU;
            topMenuStrip.TabStop = false;
            // 设置大小
            topMenuStrip.Size = new Size(this.Size.Width,10);
            // 设置停靠到顶端
            topMenuStrip.Dock = DockStyle.Top;
            // 设置相对距离
            topMenuStrip.Location = new Point(0,0);
            // 设置背景颜色
            topMenuStrip.BackColor = Color.Red;
            // 字体
            topMenuStrip.Font = new Font("微软雅黑",9,FontStyle.Regular);
            // 绑定重绘函数
            topMenuStrip.Renderer =  new TopMenuRenderer();    
        }
        /// <summary>
        /// 菜单子选项的总绑定类，执行选项name对应的绑定类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem tool = (ToolStripMenuItem)sender;
                //获取当前获得焦点的文本框
                TextBox t = TextBoxCache.getFocusTextBox()[MainFormDataLib.ROOT_FORM_NAME];
                foreach (KeyValuePair<string, Delegate> kvp in this.eventBinding())
                {//遍历对应关系字典
                    if (kvp.Key.Equals(tool.Name))
                    {//判断当前点击的选项名是否与关系字典中的选项名对应，对应则执行关系字典中的对应方法
                        if (this.topMenuStrip.IsHandleCreated)
                        {//判断当前控件是否有与其关联的句柄
                            Dictionary<Type, object> data = new Dictionary<Type, object>();
                            data.Add(typeof(TextBox), t);
                            data.Add(typeof(ToolStripMenuItem), tool);

                            this.topMenuStrip.Invoke(kvp.Value, new object[]{data});
                        }
                    }
                }
                //dLLLoad.TextBoxUtilsMet.textFixStartIndex(t, TextBoxDataLibcs.ExpTextLength, TextBoxDataLibcs.ExpStartIndex);//确定文本框光标位置
            }
            catch (InvalidCastException ie)
            {//强制类型转化异常
                MessageBox.Show(ie.ToString());
            }
            catch(NullReferenceException ne)
            {//空指针异常
                MessageBox.Show(ne.ToString());
            }
        }

        /// <summary>
        /// 设置菜单项的Image
        /// </summary>
        /// <param name="item"></param>
        private void setItemImage(ToolStripMenuItem item) {
            System.Drawing.Image image = getItemImageDic(item.Name);
            if(image != null) { 
                item.Image = image;
                item.ImageScaling = ToolStripItemImageScaling.None;
                item.ImageAlign = ContentAlignment.MiddleRight;
            }
        }
        /// <summary>
        /// 菜单项对应Image的字典
        /// </summary>
        /// <returns></returns>
        private System.Drawing.Image getItemImageDic(string name) { 
            
            Dictionary<string, System.Drawing.Image> toolImageDic = new Dictionary<string, System.Drawing.Image>();
            toolImageDic.Add(this.打开Item.Name, Core.ImageResource.打开);
            toolImageDic.Add(this.保存Item.Name, Core.ImageResource.保存);
            toolImageDic.Add(this.另存为Item.Name, Core.ImageResource.另存为);
            toolImageDic.Add(this.用记事本打开Item.Name, Core.ImageResource.记事本);
            toolImageDic.Add(this.退出Item.Name, Core.ImageResource.退出);
            
            toolImageDic.Add(this.撤销Item.Name, Core.ImageResource.撤销);
            toolImageDic.Add(this.恢复Item.Name, Core.ImageResource.重做);
            
            toolImageDic.Add(this.全选Item.Name, Core.ImageResource.全选_反相);
            toolImageDic.Add(this.剪切Item.Name, Core.ImageResource.裁剪);
            toolImageDic.Add(this.复制Item.Name, Core.ImageResource.复制);
            toolImageDic.Add(this.粘贴Item.Name, Core.ImageResource.粘贴);
            toolImageDic.Add(this.删除Item.Name, Core.ImageResource.删除del);

            toolImageDic.Add(this.查找替换Item.Name, Core.ImageResource.查找替换);
            toolImageDic.Add(this.转到行Item.Name, Core.ImageResource.转到行);
            toolImageDic.Add(this.统计字符Item.Name, Core.ImageResource.统计);

            toolImageDic.Add(this.分列Item.Name, Core.ImageResource.分列);
            toolImageDic.Add(this.首选项Item.Name, Core.ImageResource.设置);

            toolImageDic.Add(this.字体Item.Name, Core.ImageResource.字体);
            toolImageDic.Add(this.关于Item.Name, Core.ImageResource.关于);

            if (toolImageDic.ContainsKey(name)) { 
                return toolImageDic[name];
            } else { 
                return null;    
            }
        }
        /// <summary>
        /// 顶部菜单选项对应的执行类
        /// </summary>
        /// <param name="t">需要操作的文本框</param>
        /// <returns>Dictionary的对应关系，key为右键菜单选项的name,value为委托类</returns>
        private Dictionary<string, Delegate> eventBinding()
        {
            Dictionary<string, Delegate> toolBindingDic = new Dictionary<string, Delegate>();
            toolBindingDic.Add(this.打开Item.Name, new methodDelegate(TopMenuEventMet.openFileMethod));
            toolBindingDic.Add(this.另存为Item.Name, new methodDelegate(TopMenuEventMet.saveFileMethod));
            toolBindingDic.Add(this.保存Item.Name, new methodDelegate(TopMenuEventMet.saveOrSaveas));
            toolBindingDic.Add(this.用记事本打开Item.Name, new methodDelegate(TopMenuEventMet.notepadOpenFile));
            toolBindingDic.Add(this.退出Item.Name, new methodDelegate(TopMenuEventMet.exitProgram));

            toolBindingDic.Add(this.撤销Item.Name, new methodDelegate(TopMenuEventMet.cancelTextBoxCache));
            toolBindingDic.Add(this.恢复Item.Name, new methodDelegate(TopMenuEventMet.restoreTextBoxCache));


            toolBindingDic.Add(this.全选Item.Name, new methodDelegate((Dictionary<Type , object> data) =>{
                TextBox t = (TextBox)data[typeof(TextBox)];
                TextBoxUtilsMet.textAllSelect(t);
                return null;}));
            toolBindingDic.Add(this.剪切Item.Name, new methodDelegate((Dictionary<Type , object> data) =>{
                TextBox t = (TextBox)data[typeof(TextBox)];
                TextBoxUtilsMet.textSelectCut(t);
                return null;}));
            toolBindingDic.Add(this.复制Item.Name, new methodDelegate((Dictionary<Type , object> data) =>{
                TextBox t = (TextBox)data[typeof(TextBox)];
                TextBoxUtilsMet.textCopy(t);
                return null;}));
            toolBindingDic.Add(this.粘贴Item.Name, new methodDelegate((Dictionary<Type , object> data) =>{
                TextBox t = (TextBox)data[typeof(TextBox)];
                TextBoxUtilsMet.textPaste(t);
                return null;}));
            toolBindingDic.Add(this.删除Item.Name, new methodDelegate((Dictionary<Type , object> data) =>{
                TextBox t = (TextBox)data[typeof(TextBox)];
                TextBoxUtilsMet.textSelectDelect(t);
                return null;}));
            toolBindingDic.Add(this.查找替换Item.Name, new methodDelegate((Dictionary<Type , object> data) =>{
                TextBox t = (TextBox)data[typeof(TextBox)];
                FindAndReplace.initSingleFindAndReplace(t);
                return null;}));
            toolBindingDic.Add(this.转到行Item.Name, new methodDelegate((Dictionary<Type , object> data) =>{
                TextBox t = (TextBox)data[typeof(TextBox)];
                RowGoToForm.openRowGoToForm(t);
                return null;}));
            toolBindingDic.Add(this.统计字符Item.Name, new methodDelegate((Dictionary<Type , object> data) =>{
                TextBox t = (TextBox)data[typeof(TextBox)];
                CharsStatistics.openCharsStatistics(t);
                return null;}));
            toolBindingDic.Add(this.时间日期Item.Name, new methodDelegate((Dictionary<Type , object> data) =>{
                TextBox t = (TextBox)data[typeof(TextBox)];
                TextBoxUtilsMet.textInsertDate(t);
                return null;}));

            toolBindingDic.Add(this.自动换行Item.Name, new methodDelegate(TopMenuEventMet.isAutoLine));
            toolBindingDic.Add(this.状态栏Item.Name, new methodDelegate(TopMenuEventMet.isStartBarDisplay));

            toolBindingDic.Add(this.分列Item.Name, new methodDelegate((Dictionary<Type , object> data) =>{
                TextBox t = (TextBox)data[typeof(TextBox)];
                SplitCharsForm.initSingleSplitCharsForm(t);
                return null;}));
            toolBindingDic.Add(this.添加字符Item.Name, new methodDelegate((Dictionary<Type , object> data) =>{
                TextBox t = (TextBox)data[typeof(TextBox)];
                AddCharsForm.initSingleAddCharsForm(t);
                return null;}));

            toolBindingDic.Add(this.首选项Item.Name, new methodDelegate((Dictionary<Type , object> data) =>{
                SetUpMain setUpMain = new SetUpMain();
                setUpMain.ShowDialog();
                return null;}));

            toolBindingDic.Add(this.字体Item.Name, new methodDelegate(TopMenuEventMet.fontDialogMethod));
            // 实例化关于窗体
            toolBindingDic.Add(this.关于Item.Name, new methodDelegate((Dictionary<Type , object> data) =>{
                TextBox t = (TextBox)data[typeof(TextBox)];
                ThereofForm.openThereofForm();
                return null;}));
            return toolBindingDic;
        }
        /// <summary>
        /// 重绘菜单的边框
        /// </summary>
        /// <param name="menu">需要重绘的菜单</param>
        private static void paintMenuFrame(object sender, PaintEventArgs e)
        {
            MenuStrip menu = (MenuStrip)sender;
            ControlsUtilsMet.setCOntrolBorderStyle(e.Graphics, menu.ClientRectangle
                ,ButtonBorderStyle.Solid
                ,0,0,0,0
                , Color.FromArgb(160, 160, 160));
        }
        /// <summary>
        /// 获取右键菜单
        /// </summary>
        /// <returns></returns>
        public static MenuStrip getTopMenuStrip() { 
            Control con = ControlCache.getSingletonCache(DefaultNameCof.TOP_MENU);
            if(con == null) {
                TopMenuContainer topMenuContainer = new TopMenuContainer();
                ControlCache.addSingletonCache(topMenuContainer.topMenuStrip);
                return topMenuContainer.topMenuStrip;
            } else { 
                return (MenuStrip)con;
            }
        }
        
    }
}