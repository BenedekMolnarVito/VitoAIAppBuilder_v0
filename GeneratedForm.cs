using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AIAppBuilder_v0
{
    public partial class GeneratedForm : Form
    {
        public GeneratedForm()
        {
            InitializeComponent();
            InitContent();
        }

        private void InitContent()
        {
            // Create labels and textboxes for each field
            Label lblName = new() { Text = "Név / Name", Location = new System.Drawing.Point(60, 10), AutoSize = true };
            System.Windows.Forms.TextBox txtName = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(200, 10), Width = 200 };

            Label lblPlaceOfBirth = new Label { Text = "Születési hely / Place of birth", Location = new System.Drawing.Point(60, 40), AutoSize = true };
            System.Windows.Forms.TextBox txtPlaceOfBirth = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(200, 40), Width = 200 };

            Label lblNameAtBirth = new Label { Text = "Születési név / Name at birth", Location = new System.Drawing.Point(60, 70), AutoSize = true };
            System.Windows.Forms.TextBox txtNameAtBirth = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(200, 70), Width = 200 };

            Label lblDateOfBirth = new Label { Text = "Születési idő / Date of birth", Location = new System.Drawing.Point(60, 600), AutoSize = true };
            System.Windows.Forms.TextBox txtDateOfBirth = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(200, 600), Width = 200 };

            Label lblMotherName = new Label { Text = "Anyja neve / Mother\'s name", Location = new System.Drawing.Point(60, 130), AutoSize = true };
            System.Windows.Forms.TextBox txtMotherName = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(200, 130), Width = 200 };

            Label lblPermanentAddress = new Label { Text = "Állandó lakcím / Permanent address", Location = new System.Drawing.Point(60, 160), AutoSize = true };
            System.Windows.Forms.TextBox txtPermanentAddress = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(200, 160), Width = 200 };

            Label lblPostalCode = new Label { Text = "Irányítószám / Postal code", Location = new System.Drawing.Point(60, 190), AutoSize = true };
            System.Windows.Forms.TextBox txtPostalCode = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(200, 190), Width = 200 };

            Label lblEmail = new Label { Text = "E-mail cím / E-mail address", Location = new System.Drawing.Point(60, 220), AutoSize = true };
            System.Windows.Forms.TextBox txtEmail = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(200, 220), Width = 200 };

            Label lblMobileNumber = new Label { Text = "Mobiltelefonszám / Mobile phone number", Location = new System.Drawing.Point(60, 250), AutoSize = true };
            System.Windows.Forms.TextBox txtMobileNumber = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(200, 250), Width = 200 };

            // Save button
            System.Windows.Forms.Button btnSave = new System.Windows.Forms.Button { Text = "Save", Location = new System.Drawing.Point(150, 280) };
            btnSave.Click += (sender, e) => SaveForm(txtName.Text, txtPlaceOfBirth.Text, txtNameAtBirth.Text, txtDateOfBirth.Text,
                                                    txtMotherName.Text, txtPermanentAddress.Text, txtPostalCode.Text,
                                                    txtEmail.Text, txtMobileNumber.Text);

            // Add controls to the form
            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblPlaceOfBirth);
            this.Controls.Add(txtPlaceOfBirth);
            this.Controls.Add(lblNameAtBirth);
            this.Controls.Add(txtNameAtBirth);
            this.Controls.Add(lblDateOfBirth);
            this.Controls.Add(txtDateOfBirth);
            this.Controls.Add(lblMotherName);
            this.Controls.Add(txtMotherName);
            this.Controls.Add(lblPermanentAddress);
            this.Controls.Add(txtPermanentAddress);
            this.Controls.Add(lblPostalCode);
            this.Controls.Add(txtPostalCode);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblMobileNumber);
            this.Controls.Add(txtMobileNumber);
            this.Controls.Add(btnSave);

            // Form properties
            this.Text = "OTP SZÉCHENYI PIHENŐKÁRTYA APPLICATION FORM";
            this.ClientSize = new System.Drawing.Size(400, 320);
        }

        private void SaveForm(string name, string placeOfBirth, string nameAtBirth, string dateOfBirth,
                              string motherName, string permanentAddress, string postalCode,
                              string email, string mobileNumber)
        {
            string content = @$"Név / Name: {name}\
" +
                             @$"Születési hely / Place of birth: {placeOfBirth}\
" +
                             @$"Születési név / Name at birth: {nameAtBirth}\
" +
                             @$"Születési idő / Date of birth: {dateOfBirth}\
" +
                             @$"Anyja neve / Mother\'s name: {motherName}\
" +
                             @$"Állandó lakcím / Permanent address: {permanentAddress}\
" +
                             @$"Irányítószám / Postal code: {postalCode}\
" +
                             @$"E-mail cím / E-mail address: {email}\
" +
                             @$"Mobiltelefonszám / Mobile phone number: {mobileNumber}\
";

            string path = @"C:\tmp\test1.pdf";

            // Ensure directory exists
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));

            using (PdfWriter writer = new PdfWriter(path))
            {
                // Initialize PDF document
                PdfDocument pdf = new PdfDocument(writer);

                // Create a document to add elements
                Document document = new Document(pdf);

                // Add the content to the document
                document.Add(new Paragraph(content));

                // Close the document
                document.Close();
            }

            MessageBox.Show("Form saved successfully!");
        }
    }
}