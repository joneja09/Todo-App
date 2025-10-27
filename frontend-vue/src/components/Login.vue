<template>
  <div className="login-container">
    <div className="login-card">
      <h2>{{ isLogin ? 'Login' : 'Register' }}</h2>
      <div className="form-group">
        <input v-model="email" placeholder="Email" />
        <input v-model="password" type="password" placeholder="Password" />
        <button @click="handleSubmit" className="primary-btn">
          {{ isLogin ? 'Login' : 'Register' }}
        </button>
        <button @click="isLogin = !isLogin" className="secondary-btn">
          {{ isLogin ? 'Switch to Register' : 'Switch to Login' }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useToast } from 'vue-toastification';
import { useAuthStore } from '../stores/auth';
import './styles/Login.css';

const toast = useToast();

const authStore = useAuthStore();
const email = ref('');
const password = ref('');
const isLogin = ref(true);

const handleSubmit = async () => {
  try {
    const loadingText = isLogin.value ? 'Signing in...' : 'Creating account...';
    const successText = isLogin.value ? 'Welcome back!' : 'Account created successfully!';
    
    toast.info(loadingText);
    
    if (isLogin.value) {
      await authStore.login(email.value, password.value);
    } else {
      await authStore.register(email.value, password.value);
    }
    
    toast.success(successText);
    email.value = '';
    password.value = '';
  } catch (error) {
    toast.error('Error: ' + (error as Error).message);
  }
};
</script>