<template>
  <div className="app-container">
    <div className="header">
      <h1>To-Do App (Vue)</h1>
      <button @click="toggleDarkMode" className="theme-toggle">
        <i :class="isDarkMode ? 'fas fa-sun' : 'fas fa-moon'"></i>
        {{ isDarkMode ? " Light Mode" : " Dark Mode" }}
      </button>
    </div>
    <Login v-if="!authStore.token" />
    <template v-else>
      <TodoListSelector />
      <TaskList v-if="taskStore.selectedListId" />
      <button @click="authStore.logout" className="logout-btn">Logout</button>
    </template>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import Login from "./components/Login.vue";
import TaskList from "./components/TaskList.vue";
import TodoListSelector from "./components/TodoListSelector.vue";
import { useAuthStore } from "./stores/auth";
import { useTaskStore } from "./stores/auth";
import "./components/styles/App.css";

const authStore = useAuthStore();
const taskStore = useTaskStore();
const isDarkMode = ref(localStorage.getItem("darkMode") === "true");

onMounted(() => {
  document.body.classList.toggle("dark", isDarkMode.value);
});

const toggleDarkMode = () => {
  isDarkMode.value = !isDarkMode.value;
  localStorage.setItem("darkMode", isDarkMode.value.toString());
  document.body.classList.toggle("dark", isDarkMode.value);
};
</script>
