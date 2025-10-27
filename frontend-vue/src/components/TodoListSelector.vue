<template>
  <div className="todo-list-selector">
    <h3>Lists</h3>
    <select
      v-model="taskStore.selectedListId"
      @change="taskStore.setSelectedListId(taskStore.selectedListId)"
    >
      <option v-for="list in taskStore.todoLists" :key="list.id" :value="list.id">
        {{ list.name }}
      </option>
    </select>
    <div className="add-form">
      <input
        v-model="newListName"
        placeholder="Add a list"
      />
      <button @click="handleAdd" aria-label="Add list">
        <i className="fas fa-plus"></i>
      </button>
    </div>
    <div className="list-actions" v-if="taskStore.selectedListId">
      <template v-if="editingId === taskStore.selectedListId">
        <div className="edit-form">
          <input v-model="editName" />
          <button @click="handleUpdate(taskStore.selectedListId)" className="save-btn" aria-label="Save list">
            <i className="fas fa-save"></i>
          </button>
          <button @click="cancelEdit" className="cancel-btn" aria-label="Cancel edit">
            <i className="fas fa-times"></i>
          </button>
        </div>
      </template>
      <template v-else>
        <div className="action-buttons">
          <button @click="startEdit(taskStore.todoLists.find(list => list.id === taskStore.selectedListId)!)" className="edit-btn" aria-label="Edit selected list">
            <i className="fas fa-edit"></i>
          </button>
          <button @click="handleDelete(taskStore.selectedListId)" className="delete-btn" aria-label="Delete selected list">
            <i className="fas fa-trash"></i>
          </button>
        </div>
      </template>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useToast } from 'vue-toastification';
import { useTaskStore } from '../stores/auth';
import './styles/TodoListSelector.css';

const toast = useToast();

const taskStore = useTaskStore();
const newListName = ref('');
const editingId = ref<number | null>(null);
const editName = ref('');

onMounted(() => taskStore.fetchTodoLists());

const handleAdd = async () => {
  if (!newListName.value.trim()) {
    toast.error('List name is required');
    return;
  }
  try {
    await taskStore.addTodoList(newListName.value);
    toast.success('Todo list added successfully!');
    newListName.value = '';
  } catch (error) {
    toast.error('Failed to add todo list');
  }
};

const handleUpdate = async (id: number) => {
  try {
    await taskStore.updateTodoList(id, editName.value);
    toast.success('Todo list updated successfully!');
    editingId.value = null;
  } catch (error) {
    toast.error('Failed to update todo list');
  }
};

const handleDelete = async (id: number) => {
  try {
    await taskStore.deleteTodoList(id);
    toast.success('Todo list deleted successfully!');
    await taskStore.fetchTasks();
  } catch (error) {
    toast.error('Failed to delete todo list');
  }
};

const startEdit = (list: { id: number; name: string }) => {
  editingId.value = list.id;
  editName.value = list.name;
};

const cancelEdit = () => {
  editingId.value = null;
  editName.value = '';
};
</script>