﻿using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using ReaLTaiizor.Controls;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace ReaLTaiizor.Translate
{
    public partial class Translate : Form
    {
        private const string URL = "https://translate.yandex.net/api/v1.5/tr.json/translate?key=";
        private const string APIKey = "";
        private static string Lang = "tr";

        public Translate()
        {
            InitializeComponent();
            foreverRadioButton1.Checked = true;
        }

        private async void Language_CheckedChanged(object sender)
        {
            ForeverRadioButton Language = sender as ForeverRadioButton;
            if (Language.Checked && Lang != Language.Text.ToLowerInvariant())
            {
                Lang = Language.Text.ToLowerInvariant();
                foreverTextBox2.Text = await TranslateText(foreverTextBox1.Text, Lang).ConfigureAwait(false);
            }
        }

        private async void ForeverToggle1_CheckedChanged(object sender)
        {
            foreverButton1.Enabled = !foreverButton1.Enabled;
            if (foreverToggle1.Checked)
            {
                foreverTextBox2.Text = await TranslateText(foreverTextBox1.Text, Lang).ConfigureAwait(false);
            }
        }

        private async void ForeverTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (foreverToggle1.Checked)
            {
                foreverTextBox2.Text = await TranslateText(foreverTextBox1.Text, Lang).ConfigureAwait(false);
            }
        }

        private async void ForeverButton1_Click(object sender, EventArgs e)
        {
            foreverTextBox2.Text = await TranslateText(foreverTextBox1.Text, Lang).ConfigureAwait(false);
        }

        private async Task<string> TranslateText(string Text, string Lang2, string Lang1 = null)
        {
            try
            {
                if (string.IsNullOrEmpty(Text))
                {
                    Text = "ReaLTaiizor";
                }

                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                using (WebClient Client = new WebClient())
                {
                    NameValueCollection Post = new NameValueCollection()
                    {
                        { "" , "" }
                    };
                    string JSON;
                    if (!string.IsNullOrEmpty(Lang1))
                    {
                        JSON = Encoding.UTF8.GetString(await Client.UploadValuesTaskAsync(URL + APIKey + "&text=" + Text + "&lang=" + Lang1 + "-" + Lang2, Post));
                    }
                    else
                    {
                        JSON = Encoding.UTF8.GetString(await Client.UploadValuesTaskAsync(URL + APIKey + "&text=" + Text + "&lang=" + Lang2, Post));
                    }

                    dynamic DJSON = JsonConvert.DeserializeObject(JSON);
                    int i = 0;
                    foreach (dynamic FJSON in DJSON)
                    {
                        foreach (dynamic CJSON in FJSON)
                        {
                            if (i == 2)
                            {
                                string Result = CJSON.ToString();
                                return Result.Substring(6, Result.Length - 10).Replace(" \\n", Environment.NewLine).Replace("\\n", Environment.NewLine).Replace(" \\r", "").Replace("\\r", "");
                            }
                            else
                            {
                                i++;
                            }
                        }
                    }
                    return "null";
                }
            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }
        }
    }
}