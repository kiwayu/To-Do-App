using System;
using System.Drawing;
using System.Windows.Forms;

namespace ToDoApp
{
    public partial class TaskDialog : Form
    {
        public TodoTask? Task { get; private set; }
        
        // UI Controls
        private TextBox titleTextBox = null!;
        private TextBox descriptionTextBox = null!;
        private DateTimePicker dueDatePicker = null!;
        private CheckBox noDueDateCheckBox = null!;
        private ComboBox priorityComboBox = null!;
        private TextBox categoryTextBox = null!;
        private CheckBox isCompletedCheckBox = null!;
        private Button okButton = null!;
        private Button cancelButton = null!;
        
        public TaskDialog() : this(null)
        {
        }
        
        public TaskDialog(TodoTask? existingTask)
        {
            InitializeComponent();
            
            if (existingTask != null)
            {
                this.Text = "Edit Task";
                PopulateFields(existingTask);
            }
            else
            {
                this.Text = "Add New Task";
                SetDefaultValues();
            }
        }
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Size = new Size(450, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            // Title
            Label titleLabel = new Label
            {
                Text = "Title:",
                Location = new Point(12, 15),
                Size = new Size(50, 20)
            };
            
            titleTextBox = new TextBox
            {
                Location = new Point(12, 35),
                Size = new Size(400, 23),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            
            // Description
            Label descriptionLabel = new Label
            {
                Text = "Description:",
                Location = new Point(12, 70),
                Size = new Size(80, 20)
            };
            
            descriptionTextBox = new TextBox
            {
                Location = new Point(12, 90),
                Size = new Size(400, 60),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            
            // Due Date
            Label dueDateLabel = new Label
            {
                Text = "Due Date:",
                Location = new Point(12, 165),
                Size = new Size(70, 20)
            };
            
            dueDatePicker = new DateTimePicker
            {
                Location = new Point(12, 185),
                Size = new Size(200, 23),
                Format = DateTimePickerFormat.Short
            };
            
            noDueDateCheckBox = new CheckBox
            {
                Text = "No due date",
                Location = new Point(220, 185),
                Size = new Size(100, 23)
            };
            noDueDateCheckBox.CheckedChanged += NoDueDateCheckBox_CheckedChanged;
            
            // Priority
            Label priorityLabel = new Label
            {
                Text = "Priority:",
                Location = new Point(12, 220),
                Size = new Size(60, 20)
            };
            
            priorityComboBox = new ComboBox
            {
                Location = new Point(12, 240),
                Size = new Size(120, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            priorityComboBox.Items.AddRange(["Low", "Medium", "High", "Critical"]);
            priorityComboBox.SelectedIndex = 1; // Default to Medium
            
            // Category
            Label categoryLabel = new Label
            {
                Text = "Category:",
                Location = new Point(150, 220),
                Size = new Size(70, 20)
            };
            
            categoryTextBox = new TextBox
            {
                Location = new Point(150, 240),
                Size = new Size(120, 23)
            };
            
            // Completed checkbox
            isCompletedCheckBox = new CheckBox
            {
                Text = "Mark as completed",
                Location = new Point(12, 280),
                Size = new Size(150, 23)
            };
            
            // Buttons
            okButton = new Button
            {
                Text = "OK",
                Location = new Point(256, 320),
                Size = new Size(75, 30),
                DialogResult = DialogResult.OK
            };
            okButton.Click += OkButton_Click;
            
            cancelButton = new Button
            {
                Text = "Cancel",
                Location = new Point(337, 320),
                Size = new Size(75, 30),
                DialogResult = DialogResult.Cancel
            };
            
            // Add controls to form
            this.Controls.AddRange([
                titleLabel, titleTextBox,
                descriptionLabel, descriptionTextBox,
                dueDateLabel, dueDatePicker, noDueDateCheckBox,
                priorityLabel, priorityComboBox,
                categoryLabel, categoryTextBox,
                isCompletedCheckBox,
                okButton, cancelButton
            ]);
            
            this.AcceptButton = okButton;
            this.CancelButton = cancelButton;
            
            this.ResumeLayout(false);
        }
        
        private void SetDefaultValues()
        {
            titleTextBox.Text = "";
            descriptionTextBox.Text = "";
            dueDatePicker.Value = DateTime.Today.AddDays(1);
            noDueDateCheckBox.Checked = false;
            priorityComboBox.SelectedIndex = 1; // Medium
            categoryTextBox.Text = "General";
            isCompletedCheckBox.Checked = false;
        }
        
        private void PopulateFields(TodoTask task)
        {
            titleTextBox.Text = task.Title;
            descriptionTextBox.Text = task.Description;
            
            if (task.DueDate.HasValue)
            {
                dueDatePicker.Value = task.DueDate.Value;
                noDueDateCheckBox.Checked = false;
            }
            else
            {
                dueDatePicker.Value = DateTime.Today.AddDays(1);
                noDueDateCheckBox.Checked = true;
            }
            
            priorityComboBox.SelectedIndex = (int)task.Priority - 1;
            categoryTextBox.Text = task.Category;
            isCompletedCheckBox.Checked = task.IsCompleted;
        }
        
        private void NoDueDateCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            dueDatePicker.Enabled = !noDueDateCheckBox.Checked;
        }
        
        private void OkButton_Click(object? sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(titleTextBox.Text))
            {
                MessageBox.Show("Please enter a title for the task.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                titleTextBox.Focus();
                return;
            }
            
            if (string.IsNullOrWhiteSpace(categoryTextBox.Text))
            {
                MessageBox.Show("Please enter a category for the task.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                categoryTextBox.Focus();
                return;
            }
            
            // Create task object
            Task = new TodoTask
            {
                Title = titleTextBox.Text.Trim(),
                Description = descriptionTextBox.Text.Trim(),
                DueDate = noDueDateCheckBox.Checked ? null : dueDatePicker.Value.Date,
                Priority = (Priority)(priorityComboBox.SelectedIndex + 1),
                Category = categoryTextBox.Text.Trim(),
                IsCompleted = isCompletedCheckBox.Checked
            };
        }
    }
}
