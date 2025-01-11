<template>
  <div class="register">
    <h2>Registration</h2>
    <form @submit.prevent="registrate">
      <div>
        <label for="username">Username:</label>
        <input type="text" id="username" v-model="loginInputText" required />
      </div>
      <div>
        <label for="password">Password:</label>
        <input type="password" id="password" v-model="passwordInputText" required />
      </div>
      <div>
        <label for="confirmPassword">Confirm Password:</label>
        <input type="password" id="confirmPassword" v-model="checkPasswordInputText" required />
      </div>
      <button type="submit">Register</button>
    </form>
  </div>
  <router-link to="/auth">Already have an account?</router-link>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import { useRouter } from 'vue-router';
import axios from 'axios';

export default defineComponent({
  name: 'RegistrationView',
  setup() {
    const router = useRouter();

    const loginInputText = ref<String>('');
    const passwordInputText = ref<String>('');
    const checkPasswordInputText = ref<String>('');
    
    const registrate = () => {
        axios.post(
          process.env.VUE_APP_HUGGING_FACE_BACKEND_URL + "/auth/registrate",
          {
            username: loginInputText.value,
            hashedPassword: passwordInputText.value,
            hashedCheckPassword: checkPasswordInputText.value
          }
        )
        .then((data) => {
          alert(data.data)
          router.push('/auth');
        })
        .catch(error => {
          alert(error.response.data);
        })
    }

    return {
        loginInputText,
        passwordInputText,
        checkPasswordInputText,
        registrate
    };
  }
});
</script>

<style scoped>
.register {
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