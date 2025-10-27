<template>
  <div className="task-list">
    <div className="task-list-header">
      <h3>Tasks</h3>
      <button
        @click="showTaskForm = !showTaskForm"
        className="add-task-btn"
        :aria-label="showTaskForm ? 'Hide add task form' : 'Show add task form'"
      >
        <i className="fas fa-plus"></i>
      </button>
    </div>
    <TaskForm v-if="showTaskForm" />
    <ul>
      <li v-for="task in taskStore.tasks" :key="task.id" className="task-item">
        <template v-if="editingId === task.id">
          <div className="edit-form">
            <input v-model="editTitle" />
            <input v-model="editDesc" />
            <div className="edit-actions">
              <button @click="saveEdit(task.id)" className="save-btn" aria-label="Save task">
                <i className="fas fa-save"></i>
              </button>
              <button @click="cancelEdit" className="cancel-btn" aria-label="Cancel edit">
                <i className="fas fa-times"></i>
              </button>
            </div>
          </div>
        </template>
        <template v-else>
          <div className="task-content">
            <span :class="{ completed: task.isCompleted }">
              {{ task.title }} {{ task.description && `- ${task.description}` }}
            </span>
            <div className="task-actions">
              <button @click="handleToggleTask(task)" className="toggle-btn" :aria-label="task.isCompleted ? 'Mark as incomplete' : 'Mark as complete'">
                <i :class="task.isCompleted ? 'fas fa-check' : 'far fa-square'"></i>
              </button>
              <button @click="startEdit(task)" className="edit-btn" aria-label="Edit task">
                <i className="fas fa-edit"></i>
              </button>
              <button @click="handleDeleteTask(task.id)" className="delete-btn" aria-label="Delete task">
                <i className="fas fa-trash"></i>
              </button>
            </div>
          </div>
        </template>
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useToast } from 'vue-toastification';
import { useTaskStore } from '../stores/auth';
import TaskForm from './TaskForm.vue';
import './styles/TaskList.css';

const toast = useToast();

const taskStore = useTaskStore();
const editingId = ref<number | null>(null);
const editTitle = ref('');
const editDesc = ref('');
const showTaskForm = ref(false);

onMounted(() => taskStore.fetchTasks());

const startEdit = (task: { id: number; title: string; description?: string }) => {
  editingId.value = task.id;
  editTitle.value = task.title;
  editDesc.value = task.description || '';
};

const saveEdit = async (id: number) => {
  try {
    const task = taskStore.tasks.find(t => t.id === id);
    if (task) {
      await taskStore.updateTask(id, editTitle.value, editDesc.value, task.isCompleted);
      toast.success('Task updated successfully!');
    }
    editingId.value = null;
  } catch (error) {
    toast.error('Failed to update task');
  }
};

const cancelEdit = () => {
  editingId.value = null;
  editTitle.value = '';
  editDesc.value = '';
};

const handleToggleTask = async (task: { id: number; isCompleted: boolean }) => {
  try {
    await taskStore.toggleTask(task.id, !task.isCompleted);
    toast.success(task.isCompleted ? 'Task marked as incomplete' : 'Task completed!');
  } catch (error) {
    toast.error('Failed to update task');
  }
};

const handleDeleteTask = async (id: number) => {
  try {
    await taskStore.deleteTask(id);
    toast.success('Task deleted successfully!');
  } catch (error) {
    toast.error('Failed to delete task');
  }
};
</script>