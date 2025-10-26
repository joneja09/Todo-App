import React, { useState } from 'react';
import { login, register } from '../services/api';
import './styles/Login.css';

const Login: React.FC<{ onLogin: (token: string) => void }> = ({ onLogin }) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [isRegister, setIsRegister] = useState(false);

  const handleSubmit = async () => {
    try {
      const token = isRegister ? await register(email, password) : await login(email, password);
      localStorage.setItem('token', token);
      onLogin(token);
    } catch (err) {
      alert('Error: ' + (err as Error).message);
    }
  };

  return (
    <div className="login-container">
      <div className="login-card">
        <h2>{isRegister ? 'Register' : 'Login'}</h2>
        <div className="form-group">
          <input
            type="text"
            value={email}
            onChange={e => setEmail(e.target.value)}
            placeholder="Email"
          />
          <input
            type="password"
            value={password}
            onChange={e => setPassword(e.target.value)}
            placeholder="Password"
          />
          <button onClick={handleSubmit} className="primary-btn">
            {isRegister ? 'Register' : 'Login'}
          </button>
          <button onClick={() => setIsRegister(!isRegister)} className="secondary-btn">
            Switch to {isRegister ? 'Login' : 'Register'}
          </button>
        </div>
      </div>
    </div>
  );
};

export default Login;
