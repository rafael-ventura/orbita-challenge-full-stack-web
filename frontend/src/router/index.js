import { createRouter, createWebHistory } from 'vue-router'
import LoginView from '../views/auth/LoginView.vue'
import StudentsView from '../views/StudentsView.vue'
import MainLayout from '../components/MainLayout.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/students'
    },
    // Rotas de autenticação
    {
      path: '/auth',
      children: [
        {
          path: '',
          redirect: '/auth/login'
        },
        {
          path: 'login',
          name: 'auth-login',
          component: LoginView
        },
        {
          path: 'register',
          name: 'auth-register',
          component: () => import('../views/auth/RegisterView.vue')
        }
      ]
    },
    // Rotas de estudantes
    {
      path: '/students',
      component: MainLayout,
      children: [
        {
          path: '',
          name: 'students-index',
          component: StudentsView,
          meta: { requiresAuth: true }
        },
        {
          path: 'register',
          name: 'students-register',
          component: () => import('../views/students/RegisterView.vue'),
          meta: { requiresAuth: true }
        },
        {
          path: 'edit/:id',
          name: 'students-edit',
          component: () => import('../views/students/EditView.vue'),
          meta: { requiresAuth: true },
          props: true
        }
      ]
    },
    // Rota para página inicial (redirecionamento baseado na autenticação)
    {
      path: '/dashboard',
      component: MainLayout,
      children: [
        {
          path: '',
          name: 'dashboard',
          redirect: '/students',
          meta: { requiresAuth: true }
        }
      ]
    },
    // Rota 404
    {
      path: '/:pathMatch(.*)*',
      name: 'not-found',
      component: () => import('../views/NotFoundView.vue')
    }
  ]
})

// Guard de navegação simplificado
router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token')
  const isAuthenticated = !!token
  
  console.log('Navegando para:', to.path, 'Autenticado:', isAuthenticated)
  
  // Rotas que não requerem autenticação
  const publicRoutes = ['/auth/login', '/auth/register']
  
  // Se a rota requer autenticação e o usuário não está autenticado
  if (to.meta.requiresAuth && !isAuthenticated) {
    console.log('Redirecionando para login - rota protegida')
    next('/auth/login')
    return
  }
  
  // Se o usuário está autenticado e tenta acessar páginas de auth
  if (isAuthenticated && publicRoutes.includes(to.path)) {
    console.log('Redirecionando para students - já autenticado')
    next('/students')
    return
  }
  
  // Se não está autenticado e está na raiz, redirecionar para login
  if (!isAuthenticated && to.path === '/') {
    console.log('Redirecionando para login - rota raiz')
    next('/auth/login')
    return
  }
  
  // Permitir navegação
  next()
})

export default router 