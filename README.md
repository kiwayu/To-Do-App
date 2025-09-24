# Todo List Application

A comprehensive Windows Forms-based todo list application built with C# and .NET 6.

## Features

### Task Management
- ✅ **Add new tasks** with title, description, due date, priority, and category
- ✅ **View all tasks** in a detailed list view with color coding
- ✅ **Edit task details** by double-clicking or using the Edit button
- ✅ **Delete tasks** with confirmation dialog
- ✅ **Mark tasks as completed/incomplete** with toggle functionality

### Task Organization
- ✅ **Filter tasks** by status:
  - All tasks
  - Pending tasks only
  - Completed tasks only
  - Overdue tasks only
- ✅ **Sort tasks** by:
  - Creation date
  - Due date
  - Priority (High to Low)
  - Title (alphabetical)
- ✅ **Categorize tasks** with custom categories/tags

### Visual Features
- **Color coding**: 
  - Gray text for completed tasks
  - Red text for overdue tasks
  - Orange text for high/critical priority tasks
- **Status bar** showing task statistics
- **Responsive layout** that adapts to window resizing

## How to Run

### Prerequisites
- .NET 9.0 or later
- Windows operating system

### Running the Application

1. **Build and run from command line:**
   ```bash
   dotnet build
   dotnet run
   ```

2. **Or build and run from Visual Studio:**
   - Open the project in Visual Studio
   - Press F5 or click "Start Debugging"

## How to Use

### Adding a New Task
1. Click the **"Add Task"** button
2. Fill in the task details:
   - **Title** (required): A brief description of the task
   - **Description** (optional): Additional details about the task
   - **Due Date** (optional): When the task should be completed
   - **Priority**: Low, Medium, High, or Critical
   - **Category** (required): Organize tasks by project or type
3. Click **"OK"** to save the task

### Managing Tasks
- **Edit**: Select a task and click "Edit Task" or double-click the task
- **Delete**: Select a task and click "Delete Task" (with confirmation)
- **Toggle Complete**: Select a task and click "Toggle Complete" to mark as done/undone

### Filtering and Sorting
- Use the **Filter** dropdown to show only certain types of tasks
- Use the **Sort by** dropdown to change the order of tasks
- The status bar shows current statistics about your tasks

### Sample Data
The application includes sample tasks to demonstrate functionality:
- A work-related task with high priority
- A personal task that's already completed
- An overdue health-related task

## Project Structure

```
ToDoApp/
├── Task.cs                  # Task model with properties and enums
├── MainForm.cs              # Main application window
├── TaskDialog.cs            # Add/Edit task dialog
├── Program.cs               # Application entry point
├── ToDoApp.csproj          # Project configuration
└── README.md               # This file
```

## Technical Details

- **Framework**: .NET 9.0 Windows Forms
- **Language**: C# 12
- **UI Framework**: Windows Forms
- **Architecture**: Simple desktop application with in-memory data storage
