import { defineStore } from 'pinia';
import axios from 'axios';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'https://localhost:58273',
});

api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

// Response interceptor to handle token expiration
api.interceptors.response.use(
  response => response,
  error => {
    if (error.response?.status === 401) {
      // Token expired or invalid, clear storage and reload to show login
      localStorage.removeItem('token');
      window.location.reload();
    }
    return Promise.reject(error);
  }
);

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token') || null,
  }),
  actions: {
    async login(email: string, password: string) {
      const res = await api.post('/login', { email, password });
      const token = res.data.accessToken;
      localStorage.setItem('token', token);
      this.token = token;
    },
    async register(email: string, password: string) {
      await api.post('/register', { email, password });
      // After successful registration, immediately log in
      await this.login(email, password);
    },
    logout() {
      localStorage.removeItem('token');
      this.token = null;
    },
  },
});

export const useTaskStore = defineStore('tasks', {
  state: () => ({
    tasks: [] as Task[],
    todoLists: [] as TodoList[],
    selectedListId: 0,
  }),
  actions: {
    async fetchTodoLists() {
      const res = await api.get('/api/todolists');
      this.todoLists = res.data.data;
      if (this.todoLists.length > 0 && !this.selectedListId) {
        this.selectedListId = this.todoLists[0].id;
      }
    },
    async addTodoList(name: string) {
      await api.post('/api/todolists', { name });
      await this.fetchTodoLists();
    },
    async updateTodoList(id: number, name: string) {
      await api.put(`/api/todolists/${id}`, { id, name });
      await this.fetchTodoLists();
    },
    async deleteTodoList(id: number) {
      await api.delete(`/api/todolists/${id}`);
      await this.fetchTodoLists();
      if (this.selectedListId === id) {
        this.selectedListId = this.todoLists[0]?.id || 0;
      }
    },
    async fetchTasks() {
      if (this.selectedListId) {
        const res = await api.get(`/api/tasks/list/${this.selectedListId}`);
        this.tasks = res.data.data;
      }
    },
    async addTask(title: string, description?: string) {
      if (!this.selectedListId) return;
      await api.post('/api/tasks', { title, description, todoListId: this.selectedListId });
      await this.fetchTasks();
    },
    async updateTask(id: number, title: string, description: string, isCompleted: boolean) {
      await api.put(`/api/tasks/${id}`, { id, title, description, isCompleted, todoListId: this.selectedListId });
      await this.fetchTasks();
    },
    async deleteTask(id: number) {
      await api.delete(`/api/tasks/${id}`);
      await this.fetchTasks();
    },
    async toggleTask(id: number, isCompleted: boolean) {
      const task = this.tasks.find(t => t.id === id);
      if (task) {
        await this.updateTask(id, task.title, task.description || '', isCompleted);
      }
    },
    setSelectedListId(id: number) {
      this.selectedListId = id;
      this.fetchTasks();
    },
  },
});

interface Task {
  id: number;
  title: string;
  description?: string;
  isCompleted: boolean;
  todoListId: number;
}

interface TodoList {
  id: number;
  name: string;
}
