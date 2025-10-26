import React, { useState, useEffect } from 'react';
import { getTodoLists, addTodoList, updateTodoList, deleteTodoList } from '../services/api';
import './styles/TodoListSelector.css';

interface TodoList {
  id: number;
  name: string;
}

const TodoListSelector: React.FC<{ onSelect: (id: number) => void }> = ({ onSelect }) => {
  const [lists, setLists] = useState<TodoList[]>([]);
  const [selectedId, setSelectedId] = useState<number | null>(null);
  const [newListName, setNewListName] = useState('');
  const [editingId, setEditingId] = useState<number | null>(null);
  const [editName, setEditName] = useState('');

  useEffect(() => {
    fetchLists();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const fetchLists = async () => {
    const data = await getTodoLists();
    setLists(data);
    if (data.length > 0 && (!selectedId || !data.find((list : TodoList) => list.id === selectedId))) {
      setSelectedId(data[0].id);
      onSelect(data[0].id);
    }
  };

  const handleAdd = async () => {
    if (!newListName) return;
    await addTodoList(newListName);
    setNewListName('');
    fetchLists();
  };

  const handleUpdate = async (id: number) => {
    await updateTodoList(id, editName);
    setEditingId(null);
    fetchLists();
  };

  const handleDelete = async (id: number) => {
    await deleteTodoList(id);
    fetchLists();
    if (selectedId === id) {
      setSelectedId(lists[0]?.id || null);
      onSelect(lists[0]?.id || 0);
    }
  };

  const startEdit = (list: TodoList) => {
    setEditingId(list.id);
    setEditName(list.name);
  };

  const cancelEdit = () => {
    setEditingId(null);
    setEditName('');
  };

  return (
    <div className="todo-list-selector">
      <h3>Lists</h3>
      <select
        id="todolist"
        aria-label="Select a to-do list"
        name="todolist"
        value={selectedId || ''}
        onChange={e => {
          setSelectedId(Number(e.target.value));
          onSelect(Number(e.target.value));
        }}
      >
        {lists.map(list => (
          <option id={`todolist-${list.id}`} key={list.id} value={list.id}>
            {list.name}
          </option>
        ))}
      </select>
      <div className="add-form">
        <input
          value={newListName}
          onChange={e => setNewListName(e.target.value)}
          placeholder="Add a list"
        />
        <button onClick={handleAdd} aria-label="Add list">
          <i className="fas fa-plus"></i>
        </button>
      </div>
      {selectedId && (
        <div className="list-actions">
          {editingId === selectedId ? (
            <div className="action-buttons">
              <input
                value={editName}
                onChange={e => setEditName(e.target.value)}
              />
              <button onClick={() => handleUpdate(selectedId)}
                className="save-btn"
                aria-label="Save list">
                <i className="fas fa-save"></i>
              </button>
              <button onClick={cancelEdit} className="cancel-btn" aria-label="Cancel edit">
                <i className="fas fa-times"></i>
              </button>
            </div>
          ) : (
            <div className="action-buttons">
              <button
                onClick={() => startEdit(lists.find(list => list.id === selectedId)!)}
                className="edit-btn"
                aria-label="Edit selected list"
              >
                <i className="fas fa-edit"></i>
              </button>
              <button onClick={() => handleDelete(selectedId)} className="delete-btn" aria-label="Delete selected list">
                <i className="fas fa-trash"></i>
              </button>
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default TodoListSelector;