import axios from 'axios';

const api = axios.create({
  baseURL: process.env.REACT_APP_API_URL || 'https://localhost:58273',
});

api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

export const login = async (email: string, password: string) => {
  const res = await api.post('/login', { email, password, twoFactorCode: "fakeTwoFactCode", twoFactorRecoveryCode: "fakeTwoFactRecoveryCode" });
  return res.data.accessToken;
};

export const register = async (email: string, password: string) => {
  const res = await api.post('/register', { email, password });
  return res.data;
};

export const getTodoLists = async () => {
  const res = await api.get('/api/todolists');
  return res.data.data;
};

export const addTodoList = async (name: string) => {
  await api.post('/api/todolists', { name });
};

export const updateTodoList = async (id: number, name: string) => {
  await api.put(`/api/todolists/${id}`, { id, name });
};

export const deleteTodoList = async (id: number) => {
  await api.delete(`/api/todolists/${id}`);
};

export const getTasks = async (todoListId: number) => {
  const res = await api.get(`/api/tasks/list/${todoListId}`);
  return res.data.data;
};

export const addTask = async (title: string, description: string | undefined, todoListId: number) => {
  await api.post('/api/tasks', { title, description, todoListId });
};

export const updateTask = async (id: number, title: string, description: string, isCompleted: boolean, todoListId: number) => {
  await api.put(`/api/tasks/${id}`, { id, title, description, isCompleted, todoListId });
};

export const deleteTask = async (id: number) => {
  await api.delete(`/api/tasks/${id}`);
};
