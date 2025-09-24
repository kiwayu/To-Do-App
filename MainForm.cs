using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ToDoApp
{
    public partial class MainForm : Form
    {
        private List<TodoTask> tasks;
        private List<TodoTask> filteredTasks;
        private int nextTaskId = 1;
        
        // UI Controls
        private ListView taskListView = null!;
        private ComboBox filterComboBox = null!;
        private ComboBox sortComboBox = null!;
        private Button addButton = null!;
        private Button editButton = null!;
        private Button deleteButton = null!;
        private Button toggleCompleteButton = null!;
        private Button themeToggleButton = null!;
        private Label statusLabel = null!;
        private GroupBox filterGroupBox = null!;
        private GroupBox actionGroupBox = null!;
        
        private bool isDarkMode = false;
        
        public MainForm()
        {
            InitializeComponent();
            tasks = new List<TodoTask>();
            filteredTasks = new List<TodoTask>();
            LoadSampleData();
            ApplyFiltersAndSort();
            
            // Apply theme after everything is initialized
            ApplyTheme();
        }
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "Todo List Application";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 500);
            
            // Filter GroupBox
            filterGroupBox = new GroupBox
            {
                Text = "Filter & Sort",
                Location = new Point(10, 10),
                Size = new Size(860, 60),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            
            // Filter ComboBox
            Label filterLabel = new Label
            {
                Text = "Filter:",
                Location = new Point(10, 25),
                Size = new Size(40, 20)
            };
            
            filterComboBox = new ComboBox
            {
                Location = new Point(55, 22),
                Size = new Size(120, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            filterComboBox.Items.AddRange(["All", "Pending", "Completed", "Overdue"]);
            filterComboBox.SelectedIndex = 0;
            filterComboBox.SelectedIndexChanged += FilterComboBox_SelectedIndexChanged;
            
            // Sort ComboBox
            Label sortLabel = new Label
            {
                Text = "Sort by:",
                Location = new Point(190, 25),
                Size = new Size(50, 20)
            };
            
            sortComboBox = new ComboBox
            {
                Location = new Point(245, 22),
                Size = new Size(120, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            sortComboBox.Items.AddRange(["Creation Date", "Due Date", "Priority", "Title"]);
            sortComboBox.SelectedIndex = 0;
            sortComboBox.SelectedIndexChanged += SortComboBox_SelectedIndexChanged;
            
            // Theme Toggle Button
            themeToggleButton = new Button
            {
                Text = "🌙 Dark",
                Location = new Point(380, 22),
                Size = new Size(80, 23),
                FlatStyle = FlatStyle.Flat
            };
            themeToggleButton.Click += ThemeToggleButton_Click;
            
            filterGroupBox.Controls.AddRange([filterLabel, filterComboBox, sortLabel, sortComboBox, themeToggleButton]);
            
            // Task ListView
            taskListView = new ListView
            {
                Location = new Point(10, 80),
                Size = new Size(860, 400),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                MultiSelect = false
            };
            
            // ListView columns
            taskListView.Columns.Add("Title", 200);
            taskListView.Columns.Add("Description", 250);
            taskListView.Columns.Add("Due Date", 100);
            taskListView.Columns.Add("Priority", 80);
            taskListView.Columns.Add("Category", 100);
            taskListView.Columns.Add("Status", 80);
            
            taskListView.SelectedIndexChanged += TaskListView_SelectedIndexChanged;
            taskListView.DoubleClick += EditButton_Click;
            
            // Action GroupBox
            actionGroupBox = new GroupBox
            {
                Text = "Actions",
                Location = new Point(10, 490),
                Size = new Size(860, 60),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            
            // Action Buttons
            addButton = new Button
            {
                Text = "Add Task",
                Location = new Point(10, 22),
                Size = new Size(80, 30)
            };
            addButton.Click += AddButton_Click;
            
            editButton = new Button
            {
                Text = "Edit Task",
                Location = new Point(100, 22),
                Size = new Size(80, 30),
                Enabled = false
            };
            editButton.Click += EditButton_Click;
            
            deleteButton = new Button
            {
                Text = "Delete Task",
                Location = new Point(190, 22),
                Size = new Size(80, 30),
                Enabled = false
            };
            deleteButton.Click += DeleteButton_Click;
            
            toggleCompleteButton = new Button
            {
                Text = "Toggle Complete",
                Location = new Point(280, 22),
                Size = new Size(100, 30),
                Enabled = false
            };
            toggleCompleteButton.Click += ToggleCompleteButton_Click;
            
            // Status Label
            statusLabel = new Label
            {
                Text = "Ready",
                Location = new Point(400, 27),
                Size = new Size(400, 20),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            
            actionGroupBox.Controls.AddRange([addButton, editButton, deleteButton, toggleCompleteButton, statusLabel]);
            
            // Add controls to form
            this.Controls.AddRange([filterGroupBox, taskListView, actionGroupBox]);
            
            this.ResumeLayout(false);
        }
        
        private void LoadSampleData()
        {
            tasks.Add(new TodoTask
            {
                Id = nextTaskId++,
                Title = "Complete project documentation",
                Description = "Write comprehensive documentation for the todo app project",
                DueDate = DateTime.Today.AddDays(3),
                Priority = Priority.High,
                Category = "Work"
            });
            
            tasks.Add(new TodoTask
            {
                Id = nextTaskId++,
                Title = "Buy groceries",
                Description = "Get milk, bread, and eggs from the store",
                DueDate = DateTime.Today.AddDays(1),
                Priority = Priority.Medium,
                Category = "Personal",
                IsCompleted = true
            });
            
            tasks.Add(new TodoTask
            {
                Id = nextTaskId++,
                Title = "Call dentist",
                Description = "Schedule dental checkup appointment",
                DueDate = DateTime.Today.AddDays(-2),
                Priority = Priority.Low,
                Category = "Health"
            });
        }
        
        private void ApplyFiltersAndSort()
        {
            filteredTasks = new List<TodoTask>(tasks);
            
            // Apply filter
            string selectedFilter = filterComboBox.SelectedItem?.ToString() ?? "All";
            switch (selectedFilter)
            {
                case "Pending":
                    filteredTasks = filteredTasks.Where(t => !t.IsCompleted).ToList();
                    break;
                case "Completed":
                    filteredTasks = filteredTasks.Where(t => t.IsCompleted).ToList();
                    break;
                case "Overdue":
                    filteredTasks = filteredTasks.Where(t => t.IsOverdue).ToList();
                    break;
            }
            
            // Apply sort
            string selectedSort = sortComboBox.SelectedItem?.ToString() ?? "Creation Date";
            switch (selectedSort)
            {
                case "Due Date":
                    filteredTasks = filteredTasks.OrderBy(t => t.DueDate ?? DateTime.MaxValue).ToList();
                    break;
                case "Priority":
                    filteredTasks = filteredTasks.OrderByDescending(t => (int)t.Priority).ToList();
                    break;
                case "Title":
                    filteredTasks = filteredTasks.OrderBy(t => t.Title).ToList();
                    break;
                default: // Creation Date
                    filteredTasks = filteredTasks.OrderBy(t => t.CreatedDate).ToList();
                    break;
            }
            
            RefreshTaskList();
            UpdateStatusLabel();
        }
        
        private void RefreshTaskList()
        {
            taskListView.Items.Clear();
            
            foreach (var task in filteredTasks)
            {
                var item = new ListViewItem(task.Title);
                item.SubItems.Add(task.Description);
                item.SubItems.Add(task.DueDateText);
                item.SubItems.Add(task.PriorityText);
                item.SubItems.Add(task.Category);
                item.SubItems.Add(task.StatusText);
                item.Tag = task;
                
                // Color coding based on theme
                if (task.IsCompleted)
                {
                    item.ForeColor = isDarkMode ? Color.FromArgb(120, 120, 120) : Color.Gray;
                }
                else if (task.IsOverdue)
                {
                    item.ForeColor = isDarkMode ? Color.FromArgb(255, 100, 100) : Color.Red;
                }
                else if (task.Priority == Priority.High || task.Priority == Priority.Critical)
                {
                    item.ForeColor = isDarkMode ? Color.FromArgb(255, 165, 0) : Color.DarkOrange;
                }
                else
                {
                    item.ForeColor = isDarkMode ? Color.White : Color.Black;
                }
                
                taskListView.Items.Add(item);
            }
        }
        
        private void UpdateStatusLabel()
        {
            int totalTasks = tasks.Count;
            int completedTasks = tasks.Count(t => t.IsCompleted);
            int overdueTasks = tasks.Count(t => t.IsOverdue);
            
            statusLabel.Text = $"Total: {totalTasks} | Completed: {completedTasks} | Overdue: {overdueTasks} | Showing: {filteredTasks.Count}";
        }
        
        private void ApplyTheme()
        {
            try
            {
                if (isDarkMode)
                {
                    // Dark theme colors
                    this.BackColor = Color.FromArgb(45, 45, 48);
                    this.ForeColor = Color.White;
                    
                    if (taskListView != null)
                    {
                        taskListView.BackColor = Color.FromArgb(37, 37, 38);
                        taskListView.ForeColor = Color.White;
                    }
                    
                    if (filterGroupBox != null)
                        ApplyControlTheme(filterGroupBox, Color.FromArgb(60, 60, 60), Color.White);
                    if (actionGroupBox != null)
                        ApplyControlTheme(actionGroupBox, Color.FromArgb(60, 60, 60), Color.White);
                    
                    if (themeToggleButton != null)
                        themeToggleButton.Text = "☀️ Light";
                }
                else
                {
                    // Light theme colors
                    this.BackColor = SystemColors.Control;
                    this.ForeColor = SystemColors.ControlText;
                    
                    if (taskListView != null)
                    {
                        taskListView.BackColor = SystemColors.Window;
                        taskListView.ForeColor = SystemColors.WindowText;
                    }
                    
                    if (filterGroupBox != null)
                        ApplyControlTheme(filterGroupBox, SystemColors.Control, SystemColors.ControlText);
                    if (actionGroupBox != null)
                        ApplyControlTheme(actionGroupBox, SystemColors.Control, SystemColors.ControlText);
                    
                    if (themeToggleButton != null)
                        themeToggleButton.Text = "🌙 Dark";
                }
                
                // Only refresh if we have filtered tasks
                if (filteredTasks != null)
                    RefreshTaskList(); // Refresh to apply color coding with new theme
            }
            catch (Exception ex)
            {
                // Silently ignore theme application errors during initialization
                System.Diagnostics.Debug.WriteLine($"Theme application error: {ex.Message}");
            }
        }
        
        private void ApplyControlTheme(Control parent, Color backColor, Color foreColor)
        {
            parent.BackColor = backColor;
            parent.ForeColor = foreColor;
            
            foreach (Control control in parent.Controls)
            {
                if (control is Button button)
                {
                    button.BackColor = isDarkMode ? Color.FromArgb(70, 70, 70) : SystemColors.ButtonFace;
                    button.ForeColor = foreColor;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = isDarkMode ? Color.FromArgb(100, 100, 100) : SystemColors.ButtonShadow;
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.BackColor = isDarkMode ? Color.FromArgb(60, 60, 60) : SystemColors.Window;
                    comboBox.ForeColor = foreColor;
                }
                else if (control is TextBox textBox)
                {
                    textBox.BackColor = isDarkMode ? Color.FromArgb(60, 60, 60) : SystemColors.Window;
                    textBox.ForeColor = foreColor;
                }
                else
                {
                    control.BackColor = backColor;
                    control.ForeColor = foreColor;
                }
            }
        }
        
        // Event Handlers
        private void ThemeToggleButton_Click(object? sender, EventArgs e)
        {
            isDarkMode = !isDarkMode;
            ApplyTheme();
        }
        
        private void FilterComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ApplyFiltersAndSort();
        }
        
        private void SortComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ApplyFiltersAndSort();
        }
        
        private void TaskListView_SelectedIndexChanged(object? sender, EventArgs e)
        {
            bool hasSelection = taskListView.SelectedItems.Count > 0;
            editButton.Enabled = hasSelection;
            deleteButton.Enabled = hasSelection;
            toggleCompleteButton.Enabled = hasSelection;
        }
        
        private void AddButton_Click(object? sender, EventArgs e)
        {
            using var dialog = new TaskDialog();
            if (dialog.ShowDialog() == DialogResult.OK && dialog.Task != null)
            {
                var newTask = dialog.Task;
                newTask.Id = nextTaskId++;
                tasks.Add(newTask);
                ApplyFiltersAndSort();
            }
        }
        
        private void EditButton_Click(object? sender, EventArgs e)
        {
            if (taskListView.SelectedItems.Count == 0) return;
            
            if (taskListView.SelectedItems[0].Tag is TodoTask selectedTask)
            {
                using var dialog = new TaskDialog(selectedTask);
                if (dialog.ShowDialog() == DialogResult.OK && dialog.Task != null)
                {
                    var editedTask = dialog.Task;
                    var index = tasks.FindIndex(t => t.Id == selectedTask.Id);
                    if (index >= 0)
                    {
                        editedTask.Id = selectedTask.Id;
                        editedTask.CreatedDate = selectedTask.CreatedDate;
                        tasks[index] = editedTask;
                        ApplyFiltersAndSort();
                    }
                }
            }
        }
        
        private void DeleteButton_Click(object? sender, EventArgs e)
        {
            if (taskListView.SelectedItems.Count == 0) return;
            
            if (taskListView.SelectedItems[0].Tag is TodoTask selectedTask)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete the task '{selectedTask.Title}'?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    tasks.RemoveAll(t => t.Id == selectedTask.Id);
                    ApplyFiltersAndSort();
                }
            }
        }
        
        private void ToggleCompleteButton_Click(object? sender, EventArgs e)
        {
            if (taskListView.SelectedItems.Count == 0) return;
            
            if (taskListView.SelectedItems[0].Tag is TodoTask selectedTask)
            {
                var index = tasks.FindIndex(t => t.Id == selectedTask.Id);
                if (index >= 0)
                {
                    tasks[index].IsCompleted = !tasks[index].IsCompleted;
                    ApplyFiltersAndSort();
                }
            }
        }
    }
}
