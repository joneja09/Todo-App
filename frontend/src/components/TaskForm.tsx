import React, { useState } from 'react';
import { addTask } from '../services/api';
import './styles/TaskForm.css';

const TaskForm: React.FC<{ todoListId: number; onAdd: () => void }> = ({ todoListId, onAdd }) => {
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async () => {
    if (!title.trim()) {
      setError('Title is required');
      return;
    }
    setError('');
    await addTask(title, description, todoListId);
    setTitle('');
    setDescription('');
    onAdd();
  };

  return (
    <div className="task-form">
      <div className="form-group">
        <input
          value={title}
          onChange={e => setTitle(e.target.value)}
          placeholder="Title"
        />
        <input
          value={description}
          onChange={e => setDescription(e.target.value)}
          placeholder="Description (optional)"
        />
        <button className="add-btn" onClick={handleSubmit}>
          <i className="fas fa-plus"></i>
        </button>
      </div>
      {error && <div className="error-message">{error}</div>}
    </div>
  );
};

export default TaskForm;
