import { createRouter, createWebHashHistory, RouteRecordRaw } from 'vue-router'
import { HuggingFaceIntegrationView, AuthorizationView, RegistrationView } from '../views'
import { useAuthStore } from '../store';
import { getTokenFromCookie } from '../utils/cookieUtils';

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'main',
    component: HuggingFaceIntegrationView,
    meta: { 
      requiresAuth: true
    }
  },
  {
    path: '/auth',
    name: 'authorization', 
    component: AuthorizationView
  },
  {
    path: '/reg',
    name: 'registration', 
    component: RegistrationView
  },
]

const router = createRouter({
  history: createWebHashHistory(),
  routes: routes
})

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();
  const requiresAuth = to.matched.some(record => record.meta.requiresAuth);
  const token = getTokenFromCookie();

  if (requiresAuth && !authStore.isAuthenticated && !token) {
    next('/auth');
  } 
  else {
    next();
  }
});

export default router;
