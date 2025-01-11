import { createPinia, defineStore } from 'pinia';

export const useAuthStore = defineStore('auth', {
  state: () => ({
    isAuthenticated: false
  }),
  getters: {
    getIsAuthenticated: (state) => state.isAuthenticated
  },
  actions: {
    setAuthenticated(isAuthenticated) {
      this.isAuthenticated = isAuthenticated;
    }
  }
});

export default createPinia();