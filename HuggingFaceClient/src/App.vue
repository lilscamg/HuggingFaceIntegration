<template>
  <!-- title  -->
  <div style="display: flex; justify-content: space-between;">
    <h1>Text processing using Hugging Face API</h1>
    <p v-if="store.isAuthenticated" @click="logout" style="cursor: pointer;">Logout</p>
  </div>
  <router-view/>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref, watch } from 'vue'
import { getTokenFromCookie } from './utils/cookieUtils'
import { useAuthStore } from './store';
import { useRouter } from 'vue-router';
import axios from 'axios';

export default defineComponent({
  name: 'App',
  setup() {
    const store = useAuthStore();
    const router = useRouter();

    onMounted(() => {
      const token = getTokenFromCookie();
      if (!!token) {
        store.setAuthenticated(true);
      }
      else {
        store.setAuthenticated(false);
      }
    });

    const logout = () => {
      axios.get(
        process.env.VUE_APP_HUGGING_FACE_BACKEND_URL + "/auth/logout",
        {
          withCredentials: true
        }
      )
      .then((response) => {
        store.setAuthenticated(false);
        router.push('/auth');
      })
    }

    return {
      logout,
      store
    }

  },
});
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

nav {
  padding: 30px;
}

nav a {
  font-weight: bold;
  color: #2c3e50;
}

nav a.router-link-exact-active {
  color: #42b983;
}
</style>
