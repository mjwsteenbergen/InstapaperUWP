﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Instapaper
{
    public sealed partial class Article : UserControl
    {
        

        public Article()
        {
            this.InitializeComponent();
        }

        public void SetText(string html)
        {
            if(html == null)
            {
                RichText.Blocks.Clear();
            }

            var res = HtmlRichTextBlockv3.SetHtml(RichText, html);
#if DEBUG
            Tree.Text = ParagraphTree.Parse(res);
#endif
        }


    }
}
