using System;
using System.Drawing;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace AIAppBuilder_v0
{
    public class ApplicationForm : Form
    {
        private TextBox txtName;
        private TextBox txtPlaceOfBirth;
        private TextBox txtNameAtBirth;
        private DateTimePicker dtpDateOfBirth;
        private TextBox txtPermanentAddress;
        private TextBox txtPostalCode;
        private TextBox txtEmail;
        private TextBox txtMobile;
        private TextBox txtMailingAddress;
        private TextBox txtMailingPostalCode;
        private TextBox txtMotherName;
        private TextBox txtCardName;
        private Button btnSave;

        public ApplicationForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "OTP Széchenyi Pihenőkártya Application Form";
            this.Size = new Size(500, 800);

            // Initialize Labels and TextBoxes
            Label lblName = new Label { Text = "Név / Name", Location = new Point(10, 20) };
            txtName = new TextBox { Location = new Point(150, 20), Width = 300 };

            Label lblPlaceOfBirth = new Label { Text = "Születési hely / Place of birth", Location = new Point(10, 60) };
            txtPlaceOfBirth = new TextBox { Location = new Point(150, 60), Width = 300 };

            Label lblNameAtBirth = new Label { Text = "Születési név / Name at birth", Location = new Point(10, 100) };
            txtNameAtBirth = new TextBox { Location = new Point(150, 100), Width = 300 };

            Label lblDateOfBirth = new Label { Text = "Születési idő / Date of birth", Location = new Point(10, 140) };
            dtpDateOfBirth = new DateTimePicker { Location = new Point(150, 140), Width = 300 };

            Label lblPermanentAddress = new Label { Text = "Állandó lakcím / Permanent address", Location = new Point(10, 180) };
            txtPermanentAddress = new TextBox { Location = new Point(150, 180), Width = 300 };

            Label lblPostalCode = new Label { Text = "Irányítószám / Postal code", Location = new Point(10, 220) };
            txtPostalCode = new TextBox { Location = new Point(150, 220), Width = 300 };

            Label lblEmail = new Label { Text = "E-mail cím* / E-mail address*", Location = new Point(10, 260) };
            txtEmail = new TextBox { Location = new Point(150, 260), Width = 300 };

            Label lblMobile = new Label { Text = "Mobiltelefonszám* / Mobile phone number*", Location = new Point(10, 300) };
            txtMobile = new TextBox { Location = new Point(150, 300), Width = 300 };

            Label lblMailingAddress = new Label { Text = "Levelezési cím / Mailing Address", Location = new Point(10, 340) };
            txtMailingAddress = new TextBox { Location = new Point(150, 340), Width = 300 };

            Label lblMailingPostalCode = new Label { Text = "Irányítószám / Postal code", Location = new Point(10, 380) };
            txtMailingPostalCode = new TextBox { Location = new Point(150, 380), Width = 300 };

            Label lblMotherName = new Label { Text = "Anyja neve / Mother's name", Location = new Point(10, 420) };
            txtMotherName = new TextBox { Location = new Point(150, 420), Width = 300 };

            Label lblCardName = new Label { Text = "Kártyán szerepeltetni kívánt név / Name to be featured on the card", Location = new Point(10, 460) };
            txtCardName = new TextBox { Location = new Point(150, 460), Width = 300 };

            // Initialize Save Button
            btnSave = new Button { Text = "Save", Location = new Point(200, 500) };
            btnSave.Click += new EventHandler(SaveForm);

            // Add controls to the form
            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblPlaceOfBirth);
            this.Controls.Add(txtPlaceOfBirth);
            this.Controls.Add(lblNameAtBirth);
            this.Controls.Add(txtNameAtBirth);
            this.Controls.Add(lblDateOfBirth);
            this.Controls.Add(dtpDateOfBirth);
            this.Controls.Add(lblPermanentAddress);
            this.Controls.Add(txtPermanentAddress);
            this.Controls.Add(lblPostalCode);
            this.Controls.Add(txtPostalCode);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblMobile);
            this.Controls.Add(txtMobile);
            this.Controls.Add(lblMailingAddress);
            this.Controls.Add(txtMailingAddress);
            this.Controls.Add(lblMailingPostalCode);
            this.Controls.Add(txtMailingPostalCode);
            this.Controls.Add(lblMotherName);
            this.Controls.Add(txtMotherName);
            this.Controls.Add(lblCardName);
            this.Controls.Add(txtCardName);
            this.Controls.Add(btnSave);
        }

        private void SaveForm(object sender, EventArgs e)
        {
            // Define the output file path
            string outputPath = @"C:\Users\User\source\repos\AIAppBuilder_v0\CodeGenerationOutput\test1.pdf";

            // Create a PDF writer
            using (PdfWriter writer = new(outputPath))
            {
                // Initialize the PDF document
                using PdfDocument pdfDoc = new(writer);
                // Initialize the document layout
                Document doc = new(pdfDoc);

                // Add content to the PDF
                doc.Add(new Paragraph("IGÉNYLŐLAP / APPLICATION FORM").SetBold().SetFontSize(16));
                doc.Add(new Paragraph("OTP SZÉCHENYI PIHENŐKÁRTYA KERETSZERZŐDÉS MEGKÖTÉSÉRE").SetBold().SetFontSize(14));
                doc.Add(new Paragraph("Név: " + txtName.Text));
                doc.Add(new Paragraph("Születési hely: " + txtPlaceOfBirth.Text));
                doc.Add(new Paragraph("Születési név: " + txtNameAtBirth.Text));
                doc.Add(new Paragraph("Születési idő: " + dtpDateOfBirth.Value.ToShortDateString()));
                doc.Add(new Paragraph("Állandó lakcím: " + txtPermanentAddress.Text));
                doc.Add(new Paragraph("Irányítószám: " + txtPostalCode.Text));
                doc.Add(new Paragraph("E-mail cím: " + txtEmail.Text));
                doc.Add(new Paragraph("Mobiltelefonszám: " + txtMobile.Text));
                doc.Add(new Paragraph("Levelezési cím: " + txtMailingAddress.Text));
                doc.Add(new Paragraph("Irányítószám: " + txtMailingPostalCode.Text));
                doc.Add(new Paragraph("Anyja neve: " + txtMotherName.Text));
                doc.Add(new Paragraph("Kártyán szerepeltetni kívánt név: " + txtCardName.Text));

                // Close the document
                doc.Close();
            }

            // Show confirmation message
            MessageBox.Show("Form saved successfully!");
        }
    }
}