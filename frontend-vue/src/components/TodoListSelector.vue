<template>
  <div className="todo-list-selector">
    <h3>To-Do Lists</h3>
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
import { useTaskStore } from '../stores/auth';
import './styles/TodoListSelector.css';

const taskStore = useTaskStore();
const newListName = ref('');
const editingId = ref<number | null>(null);
const editName = ref('');

onMounted(() => taskStore.fetchTodoLists());

const handleAdd = async () => {
  if (!newListName.value) return;
  await taskStore.addTodoList(newListName.value);
  newListName.value = '';

};

const handleUpdate = async (id: number) => {
  await taskStore.updateTodoList(id, editName.value);
  editingId.value = null;
};

const handleDelete = async (id: number) => {
  await taskStore.deleteTodoList(id);
  await taskStore.fetchTasks();
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