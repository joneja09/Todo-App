import React, { useState } from 'react';
import toast from 'react-hot-toast';
import { login, register } from '../services/api';
import './styles/Login.css';

const Login: React.FC<{ onLogin: (token: string) => void }> = ({ onLogin }) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [isRegister, setIsRegister] = useState(false);

  const handleSubmit = async () => {
    try {
      const loadingToast = toast.loading(isRegister ? 'Creating account...' : 'Signing in...');
      let token;

      if (isRegister) {
        await register(email, password);
        token = await login(email, password); // Follow registration with login
        toast.success('Account created and logged in successfully!');
      } else {
        token = await login(email, password);
        toast.success('Welcome back!');
      }

      localStorage.setItem('token', token);
      toast.dismiss(loadingToast);
      onLogin(token);
    } catch (err) {
      toast.error('Error: ' + (err as Error).message);
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
