﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastFoodPOS.Models;
using FastFoodPOS.Components;

namespace FastFoodPOS.Forms.AdminForms
{
    partial class FormUpdateProduct : UserControl, IKeepable
    {
        ProductCardComponent pcc;
        Product product;
        FormManageProducts context;
        Validator NameValidator;
        public FormUpdateProduct(FormManageProducts context, ProductCardComponent pcc)
        {
            InitializeComponent();
            this.pcc = pcc;
            this.product = pcc.product;
            this.context = context;
            NameValidator = new Validator(TextName, LabelName, "Name", "required");
            ResetData();            
        }

        void ResetData()
        {
            PictureProductImage.ImageLocation = product.Image;
            TextName.Text = product.Name;
            ComboBoxType.Text = product.Category;
            TextPrice.Text = product.Price.ToString();
            NameValidator.Reset();
        }

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            FormAdminPanel.GetInstance().LoadFormControl(context);
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            ResetData();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            if(NameValidator.IsValid())
            {
                Product uProduct = product.Clone();
                uProduct.Name = TextName.Text;
                uProduct.Category = ComboBoxType.Text;
                uProduct.Price = TextPrice.Value;
                uProduct.newImage = PictureProductImage.ImageLocation;
                uProduct.Update();
                product.Copy(uProduct);

                Log.AddLog("Make changes to product["+uProduct.Id+"]");

                MessageBox.Show("Product updated successfully");
                pcc.UpdateData();
                ButtonBack.PerformClick();
            }
        }

        private void ButtonChangeImage_Click(object sender, EventArgs e)
        {
            if (OpenFileDialogChangeImage.ShowDialog() == DialogResult.OK)
            {
                PictureProductImage.ImageLocation = OpenFileDialogChangeImage.FileName;
            }
        }

        public bool ShouldKeepForm { get; set; }

        public void OnMounted()
        {

        }

        public void OnUnMounted(ref UserControl next)
        {
            context.ShouldKeepForm = false;
            if (next is FormManageProducts && next != context)
            {
                next.Dispose();
                next = context;
            }
            else if (next != context)
            {
                context.Dispose();
            }
        }

        private void TextPrice_Leave(object sender, EventArgs e)
        {
            if (TextPrice.Text.Length == 0)
            {
                TextPrice.Text = "0";
                TextPrice.Value = 0;
            }
        }
    }
}
