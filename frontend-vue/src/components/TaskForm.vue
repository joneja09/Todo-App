<template>
  <div className="task-form">
    <div className="form-group">
      <input v-model="title" placeholder="Task title" />
      <input v-model="description" placeholder="Description (optional)" />
      <button @click="handleAdd" aria-label="Add task">
        <i className="fas fa-plus"></i>
      </button>
    </div>
    <div v-if="error" className="error-message" role="alert">{{ error }}</div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useTaskStore } from '../stores/auth';
import './styles/TaskForm.css';

const taskStore = useTaskStore();
const title = ref('');
const description = ref('');
const error = ref('');

const handleAdd = async () => {
  if (!title.value.trim()) {
    error.value = 'Title is required';
    return;
  }
  error.value = '';
  await taskStore.addTask(title.value, description.value);
  title.value = '';
  description.value = '';
};
</script>