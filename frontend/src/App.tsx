import React, { useState } from 'react';
import toast, { Toaster } from 'react-hot-toast';
import Login from './components/Login';
import TaskList from './components/TaskList';
import TodoListSelector from './components/TodoListSelector';
import './components/styles/App.css';

const App: React.FC = () => {
  const [token, setToken] = useState(localStorage.getItem('token'));
  const [selectedListId, setSelectedListId] = useState(0);
    
  const handleLogin = (newToken: string) => {
    setToken(newToken);
  };

  const [isDarkMode, setIsDarkMode] = useState(() => {
    const saved = localStorage.getItem('darkMode');
    console.log('Saved dark mode:', saved);
    if (!saved) return false;
    const newMode = JSON.parse(saved);
    document.body.classList.toggle('dark', newMode);
    return newMode;
  });

  const toggleDarkMode = () => {
    setIsDarkMode((prev: Boolean) => {
      const newMode = !prev;
      localStorage.setItem('darkMode', JSON.stringify(newMode));
      document.body.classList.toggle('dark', newMode);
      return newMode;
    });
  };

  return (
    <div className="app-container">
      <Toaster
        position="top-right"
        toastOptions={{
          duration: 4000,
        }}
      />
      {token ? (
        <>
          <div className="header">
            <h1>To-Do App</h1>
            <button onClick={toggleDarkMode} className="theme-toggle">
              <i className={isDarkMode ? 'fas fa-sun' : 'fas fa-moon'}></i>
              {isDarkMode ? ' Light Mode' : ' Dark Mode'}
            </button>
          </div>
          <TodoListSelector onSelect={setSelectedListId} />
          {selectedListId > 0 && (
            <>
              <TaskList todoListId={selectedListId} />
            </>
          )}
          <button
            onClick={() => {
              localStorage.removeItem('token');
              setToken(null);
              toast.success('Logged out successfully!');
            }}
            className="logout-btn"
          >
            Logout
          </button>
        </>
      ) : (
        <Login onLogin={handleLogin} />
      )}
    </div>
  );
};

export default App;