import React, { useState, useEffect } from 'react';
import { getTasks, updateTask, deleteTask } from '../services/api';
import TaskForm from './TaskForm';
import './styles/TaskList.css';

interface Task {
  id: number;
  title: string;
  description?: string;
  isCompleted: boolean;
  todoListId: number;
}

const TaskList: React.FC<{ todoListId: number }> = ({ todoListId }) => {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [editTitle, setEditTitle] = useState('');
  const [editDesc, setEditDesc] = useState('');
  const [showTaskForm, setShowTaskForm] = useState(false);

  useEffect(() => {
    fetchTasks();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [todoListId]);

  const fetchTasks = async () => {
    const data = await getTasks(todoListId);
    setTasks(data);
  };

  const handleDelete = async (id: number) => {
    await deleteTask(id);
    fetchTasks();
  };

  const handleToggle = async (task: Task) => {
    await updateTask(task.id, task.title, task.description || '', !task.isCompleted, task.todoListId);
    fetchTasks();
  };

  const startEdit = (task: Task) => {
    setEditingId(task.id);
    setEditTitle(task.title);
    setEditDesc(task.description || '');
  };

  const saveEdit = async (id: number) => {
    await updateTask(id, editTitle, editDesc, tasks.find(t => t.id === id)!.isCompleted, todoListId);
    setEditingId(null);
    fetchTasks();
  };

  const cancelEdit = () => {
    setEditingId(null);
    setEditTitle('');
    setEditDesc('');
  };

  return (
    <div className="task-list">
      <div className="task-list-header">
        <h3>Tasks</h3>
        <button
          onClick={() => setShowTaskForm(!showTaskForm)}
          className={showTaskForm ? "cancel-btn" : "add-task-btn"}
          aria-label={showTaskForm ? 'Hide add task form' : 'Show add task form'}
        >
          <i className={showTaskForm ? "fas fa-times" : "fas fa-plus"}></i>
        </button>
      </div>
      {showTaskForm && <TaskForm todoListId={todoListId} onAdd={fetchTasks} />}
      <ul>
        {tasks.map(task => (
          <li key={task.id} className="task-item">
            {editingId === task.id ? (
              <div className="edit-form">
                <input
                  value={editTitle}
                  onChange={e => setEditTitle(e.target.value)}
                />
                <input
                  value={editDesc}
                  onChange={e => setEditDesc(e.target.value)}
                />
                <div className="task-actions">
                  <button onClick={() => saveEdit(task.id)} className="save-btn" aria-label="Save task">
                    <i className="fas fa-save"></i>
                  </button>
                  <button onClick={cancelEdit} className="cancel-btn" aria-label="Cancel edit">
                    <i className="fas fa-times"></i>
                  </button>
                </div>
              </div>
            ) : (
              <div className="task-content">
                <span className={task.isCompleted ? 'completed' : ''}>
                  {task.title} {task.description && `- ${task.description}`}
                </span>
                <div className="task-actions">
                  <button onClick={() => handleToggle(task)} className="toggle-btn" aria-label={task.isCompleted ? 'Mark as incomplete' : 'Mark as complete'}>
                    <i className={task.isCompleted ? 'fas fa-check' : 'far fa-square'}></i>
                  </button>
                  <button onClick={() => startEdit(task)} className="edit-btn" aria-label="Edit task">
                    <i className="fas fa-edit"></i>
                  </button>
                  <button onClick={() => handleDelete(task.id)} className="delete-btn" aria-label="Delete task">
                    <i className="fas fa-trash"></i>
                  </button>
                </div>
              </div>
            )}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default TaskList;