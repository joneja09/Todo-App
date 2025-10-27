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
import { useToast } from 'vue-toastification';
import { useTaskStore } from '../stores/auth';
import './styles/TaskForm.css';

const toast = useToast();

const taskStore = useTaskStore();
const title = ref('');
const description = ref('');
const error = ref('');

const handleAdd = async () => {
  if (!title.value.trim()) {
    toast.error('Title is required');
    return;
  }
  try {
    await taskStore.addTask(title.value, description.value);
    toast.success('Task added successfully!');
    title.value = '';
    description.value = '';
  } catch (error) {
    toast.error('Failed to add task');
  }
};
</script>