<template>
  <div class="login">
    <h2>Login</h2>
    <form @submit.prevent="authorize">
      <div>
        <label for="username">Username:</label>
        <input type="text" id="username" v-model="loginInputText" required />
      </div>
      <div>
        <label for="password">Password:</label>
        <input type="password" id="password" v-model="passwordInputText" required />
      </div>
      <button type="submit">Login</button>
    </form>
  </div>
  <router-link to="/reg">Don't have an account?</router-link>
  
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '../store';
import axios from 'axios';

export default defineComponent({
  name: 'AuthorizationView',
  setup() {
    const router = useRouter();
    const authStore = useAuthStore();
    
    const loginInputText = ref<String>('');
    const passwordInputText = ref<String>('');
    
    const authorize = () => {
      axios.post(
          process.env.VUE_APP_HUGGING_FACE_BACKEND_URL + "/auth/authorize",
          {
            username: loginInputText.value,
            hashedPassword: passwordInputText.value
          },
          {
            withCredentials: true
          }
        )
        .then((data) => {
          authStore.setAuthenticated(true);
          router.push('/');
        })
        .catch(error => {
          authStore.setAuthenticated(false);
          alert(error.response.data);
        })
    }

    return {
        loginInputText,
        passwordInputText,
        authorize
    };
  }
});
</script>

<style scoped>
.login {
  max-width: 400px;
  margin: 0 auto;
  padding: 20px;
  border: 1px solid #ccc;
  border-radius: 5px;
}

form div {
  margin-bottom: 15px;
}

button {
  width: 100%;
  padding: 10px;
  background-color: #4CAF50;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

button:hover {
  background-color: #45a049;
}
</style>